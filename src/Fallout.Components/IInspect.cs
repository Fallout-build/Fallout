using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Fallout.Common;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.DotNet;
using Fallout.Common.Tools.ReSharper;
using Serilog;
using static Fallout.Common.Tools.DotNet.DotNetTasks;
using static Fallout.Common.Tools.ReSharper.ReSharperTasks;

namespace Fallout.Components;

/// <summary>
/// Adds an <c>Inspect</c> target that collects static analysis findings for the solution and sorts
/// them into triage buckets. It runs one <c>dotnet build</c> that writes a Roslyn SARIF error log per
/// project, then ReSharper's <c>inspectcode</c>. It merges both reports, drops findings in <c>obj/</c>
/// and generated files, keeps one finding per rule, file and line, and assigns each finding the
/// bucket of the first matching rule in <see cref="InspectRules"/>. The reports are
/// <c>inspect.sarif</c>, <c>inspect.tsv</c> and <c>inspect.md</c> in the reports directory, and one
/// count line per bucket goes to stderr. The target reports only: a failing build or tool logs a
/// warning and never fails the target.
/// </summary>
[Experimental("FALLOUT005")]
public interface IInspect : IHasSolution, IHasReports
{
    /// <summary>The merged SARIF report.</summary>
    AbsolutePath InspectSarifFile => ReportDirectory / "inspect.sarif";

    /// <summary>The merged report as tab-separated values.</summary>
    AbsolutePath InspectTsvFile => ReportDirectory / "inspect.tsv";

    /// <summary>The Markdown summary.</summary>
    AbsolutePath InspectMarkdownFile => ReportDirectory / "inspect.md";

    /// <summary>The directory for the raw reports of ReSharper and Roslyn.</summary>
    AbsolutePath InspectRawDirectory => ReportDirectory / "inspect";

    /// <summary>
    /// The rule table: the first rule whose pattern (<c>*</c> and <c>?</c> wildcards, case-insensitive)
    /// matches a finding's rule ID sets its bucket. Override it to change buckets, for example
    /// <c>InspectRuleTable.Default.Prepend(("CA1307", InspectBucket.RuntimeCandidate)).ToList()</c>.
    /// </summary>
    IReadOnlyList<(string Rule, InspectBucket Bucket)> InspectRules => InspectRuleTable.Default;

    /// <summary>Whether a file, relative to the root directory, belongs to a test project.</summary>
    bool IsInspectTestFile(string relativePath) => InspectReport.IsTestPath(relativePath);

    Target Inspect => _ => _
        .Description("Collects ReSharper and Roslyn findings and sorts them into triage buckets")
        .Produces(InspectSarifFile, InspectTsvFile, InspectMarkdownFile)
        .Executes(() =>
        {
            InspectRawDirectory.CreateOrCleanDirectory();

            // The compiler does not create the directory of its error log.
            var errorLogDirectory = InspectRawDirectory / "roslyn";
            errorLogDirectory.CreateDirectory();
            var errorLogTargetsFile = TemporaryDirectory / "inspect-errorlog.targets";
            errorLogTargetsFile.WriteAllText(ErrorLogTargets);
            RunReportStep("dotnet build", () => DotNetBuild(_ => _
                .Apply(InspectBuildSettingsBase, (errorLogDirectory, errorLogTargetsFile))
                .Apply(InspectBuildSettings)));

            var inspectCodeFile = InspectRawDirectory / "resharper.sarif";
            RunReportStep("jb inspectcode", () => ReSharperInspectCode(_ => _
                .Apply(InspectCodeSettingsBase, inspectCodeFile)
                .Apply(InspectCodeSettings)));

            var findings = errorLogDirectory.GlobFiles("*.sarif")
                .OrderBy(x => x.ToString(), StringComparer.Ordinal)
                .SelectMany(x => InspectReport.Read(x, RootDirectory, "Roslyn"))
                .Concat(inspectCodeFile.FileExists()
                    ? InspectReport.Read(inspectCodeFile, RootDirectory, "ReSharper")
                    : Array.Empty<InspectFinding>());
            var merged = InspectReport.Merge(findings, InspectRules, IsInspectTestFile);

            InspectSarifFile.WriteAllText(InspectReport.ToSarif(merged, RootDirectory));
            InspectTsvFile.WriteAllText(InspectReport.ToTsv(merged));
            InspectMarkdownFile.WriteAllText(InspectReport.ToMarkdown(merged, Solution.Name));

            foreach (var line in InspectReport.GetCountLines(merged))
            {
                Console.Error.WriteLine(line);
            }
        });

    sealed Configure<DotNetBuildSettings, (AbsolutePath ErrorLogDirectory, AbsolutePath TargetsFile)> InspectBuildSettingsBase => (_, v) => _
        .SetProjectFile(Solution)
        .EnableNoIncremental()
        .SetProperty("CustomAfterDirectoryBuildTargets", v.TargetsFile)
        .SetProperty("FalloutInspectErrorLogDirectory", v.ErrorLogDirectory)
        .SetProcessExitHandler(p => WarnOnExitCode("dotnet build", p));

    /// <summary>Changes the <c>dotnet build</c> that writes the Roslyn error logs.</summary>
    Configure<DotNetBuildSettings> InspectBuildSettings => _ => _;

    sealed Configure<ReSharperInspectCodeSettings, AbsolutePath> InspectCodeSettingsBase => (_, v) => _
        .SetTargetPath(Solution)
        .SetOutput(v)
        .SetFormat(ReSharperFormat.Sarif)
        .SetSeverity(ReSharperSeverity.HINT)
        .EnableNoSwea()
        .EnableNoBuild()
        .EnableNoUpdates()
        .SetProcessExitHandler(p => WarnOnExitCode("jb inspectcode", p));

    /// <summary>Changes the <c>jb inspectcode</c> call.</summary>
    Configure<ReSharperInspectCodeSettings> InspectCodeSettings => _ => _;

    private static void RunReportStep(string name, Action step)
    {
        try
        {
            step();
        }
        catch (Exception exception)
        {
            Log.Warning(exception, "{Step} failed; the inspect report misses its findings", name);
        }
    }

    private static void WarnOnExitCode(string name, IProcess process)
    {
        if (process.ExitCode != 0)
        {
            Log.Warning("{Step} exited with code {ExitCode}; the inspect report uses what it wrote", name, process.ExitCode);
        }
    }

    // Imported after Directory.Build.targets of every project. A single ErrorLog path for a whole
    // solution would be overwritten by each compiler call, so every project and target framework
    // writes its own file.
    private const string ErrorLogTargets =
        """
        <Project>
          <PropertyGroup Condition="'$(FalloutInspectErrorLogDirectory)' != ''">
            <ErrorLog>$(FalloutInspectErrorLogDirectory)/$(MSBuildProjectName)-$(TargetFramework)-$([System.Guid]::NewGuid().ToString('N')).sarif,version=2.1</ErrorLog>
          </PropertyGroup>
        </Project>
        """;
}
