using System;
using System.Collections.Generic;
using System.Linq;
using Fallout.Common.IO;
using FluentAssertions;
using Xunit;

namespace Fallout.Components.Specs;

public class InspectReportSpecs : IDisposable
{
    private readonly AbsolutePath root;
    private readonly AbsolutePath reSharperSarif;
    private readonly AbsolutePath roslynSarif;

    public InspectReportSpecs()
    {
        root = AbsolutePath.Temp("inspect-report");
        reSharperSarif = root / "resharper.sarif";
        roslynSarif = root / "roslyn.sarif";

        // ReSharper writes paths relative to the solution directory through a uriBaseId.
        reSharperSarif.WriteAllText($$"""
            {
              "version": "2.1.0",
              "runs": [{
                "tool": { "driver": { "name": "InspectCode" } },
                "originalUriBaseIds": { "solutionDir": { "uri": "{{new Uri(root + "/").AbsoluteUri}}" } },
                "results": [
                  {{Result("SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", "note", "src/Store/Metrics.cs", 307, baseId: true)}},
                  {{Result("PossibleMultipleEnumeration", "warning", "src/Analysis/Engine.cs", 171, column: 65, baseId: true)}},
                  {{Result("PossibleMultipleEnumeration", "warning", "src/Analysis/Engine.cs", 171, column: 98, baseId: true)}},
                  {{Result("RedundantNameQualifier", "warning", "tests/Store.Tests/MetricSpecs.cs", 12, baseId: true)}},
                  {{Result("CA1822", "warning", "src/Build.cs", 96, baseId: true)}},
                  {{Result("InconsistentNaming", "note", "src/Store/Metrics.Generated.cs", 3, baseId: true)}}
                ]
              }]
            }
            """);

        // Roslyn writes absolute file URIs and keeps suppressed diagnostics in the log.
        roslynSarif.WriteAllText($$"""
            {
              "version": "2.1.0",
              "runs": [{
                "tool": { "driver": { "name": "Microsoft (R) Visual C# Compiler" } },
                "results": [
                  {{Result("CA2100", "note", FileUri("src/Store/Store.cs"), 85)}},
                  {{Result("CA1822", "warning", FileUri("src/Build.cs"), 96)}},
                  {{Result("CA1307", "note", FileUri("src/Store/Store.cs"), 40, suppressed: true)}},
                  {{Result("IDE0005", "note", FileUri("artifacts/obj/Store/GlobalUsings.cs"), 1)}},
                  {{Result("CA1002", "note", "file:///outside/the/root/Other.cs", 5)}},
                  { "ruleId": "AD0001", "level": "warning", "message": { "text": "Analyzer threw" } }
                ]
              }]
            }
            """);
    }

    public void Dispose() => root.DeleteDirectory();

    [Fact]
    public void Reading_drops_suppressed_generated_obj_outside_and_locationless_results()
    {
        // Act
        var findings = InspectReport.Read(roslynSarif, root, "Roslyn");

        // Assert
        findings.Select(x => (x.RuleId, x.File, x.Line)).Should().Equal(
            ("CA2100", "src/Store/Store.cs", 85),
            ("CA1822", "src/Build.cs", 96));
    }

    [Fact]
    public void Reading_resolves_paths_relative_to_the_uri_base()
    {
        // Act
        var findings = InspectReport.Read(reSharperSarif, root, "ReSharper");

        // Assert
        findings.Should().HaveCount(5);
        findings[0].File.Should().Be("src/Store/Metrics.cs");
        findings[1].Column.Should().Be(65);
        findings.Should().OnlyContain(x => x.Tool == "ReSharper");
    }

    [Fact]
    public void Merging_keeps_one_finding_per_rule_file_and_line_across_tools()
    {
        // Arrange
        var findings = InspectReport.Read(roslynSarif, root, "Roslyn")
            .Concat(InspectReport.Read(reSharperSarif, root, "ReSharper"));

        // Act
        var merged = InspectReport.Merge(findings, InspectRuleTable.Default);

        // Assert
        merged.Should().HaveCount(5);
        merged.Should().ContainSingle(x => x.RuleId == "PossibleMultipleEnumeration");
        merged.Should().ContainSingle(x => x.RuleId == "CA1822").Which.Tool.Should().Be("Roslyn");
    }

    [Fact]
    public void Merging_sorts_findings_into_buckets_and_flags_test_files()
    {
        // Arrange
        var findings = InspectReport.Read(roslynSarif, root, "Roslyn")
            .Concat(InspectReport.Read(reSharperSarif, root, "ReSharper"));

        // Act
        var merged = InspectReport.Merge(findings, InspectRuleTable.Default);

        // Assert
        merged.Select(x => (x.RuleId, x.Bucket, x.IsTest)).Should().Equal(
            ("PossibleMultipleEnumeration", InspectBucket.RuntimeCandidate, false),
            ("SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault", InspectBucket.RuntimeCandidate, false),
            ("CA2100", InspectBucket.Review, false),
            ("CA1822", InspectBucket.Mechanical, false),
            ("RedundantNameQualifier", InspectBucket.Mechanical, true));
    }

    [Fact]
    public void The_first_matching_rule_sets_the_bucket()
    {
        // Arrange
        var rules = new List<(string Rule, InspectBucket Bucket)>
        {
            ("CA1307", InspectBucket.RuntimeCandidate),
            ("CA*", InspectBucket.Mechanical),
        };

        // Act / Assert
        InspectReport.Classify("CA1307", rules).Should().Be(InspectBucket.RuntimeCandidate);
        InspectReport.Classify("ca1002", rules).Should().Be(InspectBucket.Mechanical);
        InspectReport.Classify("SomeNewRule", rules).Should().Be(InspectBucket.Unclassified);
    }

    [Fact]
    public void Count_lines_name_every_bucket_with_its_test_share()
    {
        // Arrange
        var merged = InspectReport.Merge(InspectReport.Read(reSharperSarif, root, "ReSharper"), InspectRuleTable.Default);

        // Act
        var lines = InspectReport.GetCountLines(merged);

        // Assert
        lines.Should().Equal(
            "inspect: runtime-candidate 2 (tests 0)",
            "inspect: review 0 (tests 0)",
            "inspect: mechanical 2 (tests 1)",
            "inspect: unclassified 0 (tests 0)");
    }

    [Fact]
    public void Tsv_report_has_a_header_and_one_row_per_finding()
    {
        // Arrange
        var merged = InspectReport.Merge(InspectReport.Read(roslynSarif, root, "Roslyn"), InspectRuleTable.Default);

        // Act
        var rows = InspectReport.ToTsv(merged).TrimEnd('\n').Split('\n');

        // Assert
        rows.Should().Equal(
            "bucket\ttest\trule\tlevel\ttool\tfile\tline\tcolumn\tmessage",
            "review\t\tCA2100\tnote\tRoslyn\tsrc/Store/Store.cs\t85\t1\tCA2100 message",
            "mechanical\t\tCA1822\twarning\tRoslyn\tsrc/Build.cs\t96\t1\tCA1822 message");
    }

    private string FileUri(string relativePath) => new Uri(root / relativePath).AbsoluteUri;

    private static string Result(string ruleId, string level, string uri, int line, int column = 1, bool baseId = false, bool suppressed = false)
        => $$"""
            {
              "ruleId": "{{ruleId}}",
              "level": "{{level}}",
              "message": { "text": "{{ruleId}} message" },
              "locations": [{ "physicalLocation": {
                "artifactLocation": { "uri": "{{uri}}"{{(baseId ? ", \"uriBaseId\": \"solutionDir\"" : "")}} },
                "region": { "startLine": {{line}}, "startColumn": {{column}} }
              } }]{{(suppressed ? ", \"suppressions\": [{ \"kind\": \"inSource\" }]" : "")}}
            }
            """;
}
