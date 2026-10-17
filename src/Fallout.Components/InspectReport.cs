#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Fallout.Components;

/// <summary>
/// The triage bucket of an <see cref="InspectFinding"/>. Part of the experimental
/// <see cref="IInspect"/> surface (<c>FALLOUT005</c>).
/// </summary>
[Experimental("FALLOUT005")]
public enum InspectBucket
{
    /// <summary>The finding can change runtime behavior: unhandled enum values, <c>Parse</c> calls, closures, cancellation, multiple enumeration, float equality.</summary>
    RuntimeCandidate,

    /// <summary>The finding needs a person or agent to judge it: SQL built from strings, nullable-contract guards, resource handling.</summary>
    Review,

    /// <summary>The finding is a style or cleanup issue that <c>jb cleanupcode</c> or a code fix can apply.</summary>
    Mechanical,

    /// <summary>No rule in the rule table matches the finding.</summary>
    Unclassified,
}

/// <summary>
/// One finding of <see cref="IInspect"/>, read from a SARIF file and assigned to an <see cref="InspectBucket"/>.
/// </summary>
[Experimental("FALLOUT005")]
public sealed class InspectFinding
{
    /// <summary>The tool that reported the finding, for example <c>ReSharper</c> or <c>Roslyn</c>.</summary>
    public required string Tool { get; init; }

    /// <summary>The rule ID, for example <c>CA2100</c> or <c>PossibleMultipleEnumeration</c>.</summary>
    public required string RuleId { get; init; }

    /// <summary>The SARIF level: <c>error</c>, <c>warning</c>, <c>note</c> or <c>none</c>.</summary>
    public required string Level { get; init; }

    /// <summary>The message of the finding.</summary>
    public required string Message { get; init; }

    /// <summary>The file, relative to the root directory, with <c>/</c> separators.</summary>
    public required string File { get; init; }

    /// <summary>The 1-based start line.</summary>
    public int Line { get; init; }

    /// <summary>The 1-based start column, or 0 when the tool did not report one.</summary>
    public int Column { get; init; }

    /// <summary>The bucket from the rule table.</summary>
    public InspectBucket Bucket { get; init; } = InspectBucket.Unclassified;

    /// <summary>True when the file belongs to a test project.</summary>
    public bool IsTest { get; init; }
}

/// <summary>
/// Reads SARIF 2.1 files from ReSharper's <c>inspectcode</c> and the Roslyn compiler, merges and
/// dedupes their findings, assigns buckets, and writes the <c>inspect.sarif</c>, <c>inspect.tsv</c>
/// and <c>inspect.md</c> reports of <see cref="IInspect"/>. It has no tooling dependency, so it
/// can be tested on its own.
/// </summary>
[Experimental("FALLOUT005")]
public static class InspectReport
{
    /// <summary>The buckets in report order.</summary>
    public static IReadOnlyList<InspectBucket> Buckets { get; } = new[]
    {
        InspectBucket.RuntimeCandidate,
        InspectBucket.Review,
        InspectBucket.Mechanical,
        InspectBucket.Unclassified,
    };

    /// <summary>The name of a bucket in the reports, for example <c>runtime-candidate</c>.</summary>
    public static string GetName(InspectBucket bucket)
        => bucket switch
        {
            InspectBucket.RuntimeCandidate => "runtime-candidate",
            InspectBucket.Review => "review",
            InspectBucket.Mechanical => "mechanical",
            InspectBucket.Unclassified => "unclassified",
            _ => throw new ArgumentOutOfRangeException(nameof(bucket), bucket, message: null),
        };

    /// <summary>
    /// Reads the results of a SARIF 2.1 file. Results that are suppressed in source, have no file
    /// location, lie outside <paramref name="rootDirectory"/>, or sit in <c>obj/</c> or a generated
    /// file are dropped. The returned findings are not classified yet.
    /// </summary>
    public static IReadOnlyList<InspectFinding> Read(string sarifFile, string rootDirectory, string tool)
    {
        using var stream = System.IO.File.OpenRead(sarifFile);
        var log = JsonNode.Parse(stream, documentOptions: new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip });
        var findings = new List<InspectFinding>();
        foreach (var run in log?["runs"]?.AsArray() ?? new JsonArray())
        {
            var baseUris = ReadBaseUris(run?["originalUriBaseIds"]);
            foreach (var result in run?["results"]?.AsArray() ?? new JsonArray())
            {
                if (result?["suppressions"] is JsonArray { Count: > 0 })
                {
                    continue;
                }

                var physicalLocation = result?["locations"]?.AsArray().FirstOrDefault()?["physicalLocation"];
                var artifactLocation = physicalLocation?["artifactLocation"];
                var uri = artifactLocation?["uri"]?.GetValue<string>();
                if (uri == null)
                {
                    continue;
                }

                var file = ToRelativePath(ResolvePath(uri, artifactLocation?["uriBaseId"]?.GetValue<string>(), baseUris, rootDirectory), rootDirectory);
                if (file == null || IsExcluded(file))
                {
                    continue;
                }

                var region = physicalLocation?["region"];
                findings.Add(new InspectFinding
                {
                    Tool = tool,
                    RuleId = result?["ruleId"]?.GetValue<string>() ?? "unknown",
                    Level = result?["level"]?.GetValue<string>() ?? "warning",
                    Message = NormalizeMessage(result?["message"]?["text"]?.GetValue<string>()),
                    File = file,
                    Line = region?["startLine"]?.GetValue<int>() ?? 0,
                    Column = region?["startColumn"]?.GetValue<int>() ?? 0,
                });
            }
        }

        return findings;
    }

    /// <summary>
    /// Keeps the first finding for each rule, file and line, assigns each finding the bucket of the
    /// first matching rule in <paramref name="rules"/> (<c>*</c> and <c>?</c> wildcards, case-insensitive),
    /// flags test files with <paramref name="isTestFile"/>, and sorts by bucket, file and line.
    /// </summary>
    public static IReadOnlyList<InspectFinding> Merge(
        IEnumerable<InspectFinding> findings,
        IReadOnlyList<(string Rule, InspectBucket Bucket)> rules,
        Func<string, bool>? isTestFile = null)
    {
        isTestFile ??= IsTestPath;
        var seen = new HashSet<(string, string, int)>();
        return findings
            .Where(x => seen.Add((x.RuleId.ToUpperInvariant(), x.File, x.Line)))
            .Select(x => new InspectFinding
            {
                Tool = x.Tool,
                RuleId = x.RuleId,
                Level = x.Level,
                Message = x.Message,
                File = x.File,
                Line = x.Line,
                Column = x.Column,
                Bucket = Classify(x.RuleId, rules),
                IsTest = isTestFile(x.File),
            })
            .OrderBy(x => x.Bucket)
            .ThenBy(x => x.File, StringComparer.Ordinal)
            .ThenBy(x => x.Line)
            .ThenBy(x => x.Column)
            .ThenBy(x => x.RuleId, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>The bucket of the first rule in <paramref name="rules"/> that matches <paramref name="ruleId"/>.</summary>
    public static InspectBucket Classify(string ruleId, IReadOnlyList<(string Rule, InspectBucket Bucket)> rules)
    {
        foreach (var (rule, bucket) in rules)
        {
            if (PublishPackageRouter.GlobMatches(rule, ruleId))
            {
                return bucket;
            }
        }

        return InspectBucket.Unclassified;
    }

    /// <summary>
    /// True when a directory in <paramref name="relativePath"/> is a test folder (<c>test</c>,
    /// <c>tests</c>, <c>specs</c>) or a test project (a name that ends with <c>Tests</c>, <c>Test</c>,
    /// <c>Specs</c> or <c>Spec</c>).
    /// </summary>
    public static bool IsTestPath(string relativePath)
    {
        var directories = relativePath.Split('/');
        return directories.Take(directories.Length - 1).Any(x =>
            x.Equals("test", StringComparison.OrdinalIgnoreCase) ||
            x.Equals("tests", StringComparison.OrdinalIgnoreCase) ||
            x.Equals("specs", StringComparison.OrdinalIgnoreCase) ||
            x.EndsWith("Tests", StringComparison.Ordinal) ||
            x.EndsWith(".Test", StringComparison.Ordinal) ||
            x.EndsWith("Specs", StringComparison.Ordinal) ||
            x.EndsWith(".Spec", StringComparison.Ordinal));
    }

    /// <summary>One line per bucket, for example <c>inspect: runtime-candidate 12 (tests 3)</c>.</summary>
    public static IEnumerable<string> GetCountLines(IReadOnlyList<InspectFinding> findings)
        => Buckets.Select(bucket =>
        {
            var inBucket = findings.Where(x => x.Bucket == bucket).ToList();
            return $"inspect: {GetName(bucket)} {inBucket.Count} (tests {inBucket.Count(x => x.IsTest)})";
        });

    /// <summary>Writes the findings as a SARIF 2.1 log with one run. Each result carries its bucket, tool and test flag as properties.</summary>
    public static string ToSarif(IReadOnlyList<InspectFinding> findings, string rootDirectory)
    {
        var rootUri = new Uri(Path.TrimEndingDirectorySeparator(Path.GetFullPath(rootDirectory)) + Path.DirectorySeparatorChar).AbsoluteUri;
        var results = new JsonArray();
        foreach (var finding in findings)
        {
            var region = new JsonObject { ["startLine"] = Math.Max(finding.Line, 1) };
            if (finding.Column > 0)
            {
                region["startColumn"] = finding.Column;
            }

            results.Add(new JsonObject
            {
                ["ruleId"] = finding.RuleId,
                ["level"] = finding.Level,
                ["message"] = new JsonObject { ["text"] = finding.Message },
                ["locations"] = new JsonArray(new JsonObject
                {
                    ["physicalLocation"] = new JsonObject
                    {
                        ["artifactLocation"] = new JsonObject
                        {
                            ["uri"] = string.Join('/', finding.File.Split('/').Select(Uri.EscapeDataString)),
                            ["uriBaseId"] = "ROOT",
                        },
                        ["region"] = region,
                    },
                }),
                ["properties"] = new JsonObject
                {
                    ["bucket"] = GetName(finding.Bucket),
                    ["tool"] = finding.Tool,
                    ["isTest"] = finding.IsTest,
                },
            });
        }

        var log = new JsonObject
        {
            ["$schema"] = "https://json.schemastore.org/sarif-2.1.0.json",
            ["version"] = "2.1.0",
            ["runs"] = new JsonArray(new JsonObject
            {
                ["tool"] = new JsonObject
                {
                    ["driver"] = new JsonObject
                    {
                        ["name"] = "Fallout.Inspect",
                        ["informationUri"] = "https://github.com/Fallout-build/Fallout",
                    },
                },
                ["originalUriBaseIds"] = new JsonObject
                {
                    ["ROOT"] = new JsonObject { ["uri"] = rootUri },
                },
                ["results"] = results,
            }),
        };

        return log.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>Writes the findings as tab-separated values with a header row.</summary>
    public static string ToTsv(IReadOnlyList<InspectFinding> findings)
    {
        var builder = new StringBuilder();
        builder.Append("bucket\ttest\trule\tlevel\ttool\tfile\tline\tcolumn\tmessage\n");
        foreach (var x in findings)
        {
            builder
                .Append(GetName(x.Bucket)).Append('\t')
                .Append(x.IsTest ? "test" : "").Append('\t')
                .Append(x.RuleId).Append('\t')
                .Append(x.Level).Append('\t')
                .Append(x.Tool).Append('\t')
                .Append(x.File).Append('\t')
                .Append(x.Line.ToString(CultureInfo.InvariantCulture)).Append('\t')
                .Append(x.Column.ToString(CultureInfo.InvariantCulture)).Append('\t')
                .Append(x.Message)
                .Append('\n');
        }

        return builder.ToString();
    }

    /// <summary>
    /// Writes a Markdown summary: one count row per bucket, then the findings of the runtime-candidate,
    /// review and unclassified buckets, and the rule counts of the mechanical bucket.
    /// </summary>
    public static string ToMarkdown(IReadOnlyList<InspectFinding> findings, string title)
    {
        var builder = new StringBuilder();
        builder.Append("# Inspect: ").Append(title).Append("\n\n");
        builder.Append("| Bucket | Findings | In tests |\n|---|---:|---:|\n");
        foreach (var bucket in Buckets)
        {
            var inBucket = findings.Where(x => x.Bucket == bucket).ToList();
            builder.Append("| ").Append(GetName(bucket)).Append(" | ").Append(inBucket.Count).Append(" | ").Append(inBucket.Count(x => x.IsTest)).Append(" |\n");
        }

        foreach (var bucket in Buckets)
        {
            var inBucket = findings.Where(x => x.Bucket == bucket).ToList();
            if (inBucket.Count == 0)
            {
                continue;
            }

            builder.Append("\n## ").Append(GetName(bucket)).Append("\n\n");
            builder.Append("| Rule | Findings |\n|---|---:|\n");
            foreach (var rule in inBucket.GroupBy(x => x.RuleId).OrderByDescending(x => x.Count()).ThenBy(x => x.Key, StringComparer.Ordinal))
            {
                builder.Append("| ").Append(rule.Key).Append(" | ").Append(rule.Count()).Append(" |\n");
            }

            if (bucket == InspectBucket.Mechanical)
            {
                continue;
            }

            builder.Append('\n');
            foreach (var x in inBucket)
            {
                builder
                    .Append("- `").Append(x.File).Append(':').Append(x.Line).Append("` ")
                    .Append(x.RuleId)
                    .Append(x.IsTest ? " (test)" : "")
                    .Append(": ").Append(x.Message.Replace("|", "\\|", StringComparison.Ordinal))
                    .Append('\n');
            }
        }

        return builder.ToString();
    }

    private static Dictionary<string, string> ReadBaseUris(JsonNode? originalUriBaseIds)
    {
        var baseUris = new Dictionary<string, string>(StringComparer.Ordinal);
        if (originalUriBaseIds is JsonObject baseIds)
        {
            foreach (var (name, value) in baseIds)
            {
                if (value?["uri"]?.GetValue<string>() is { } uri)
                {
                    baseUris[name] = uri;
                }
            }
        }

        return baseUris;
    }

    private static string? ResolvePath(string uri, string? uriBaseId, IReadOnlyDictionary<string, string> baseUris, string rootDirectory)
    {
        if (Uri.TryCreate(uri, UriKind.Absolute, out var absolute))
        {
            return absolute.IsFile ? absolute.LocalPath : null;
        }

        var relative = Uri.UnescapeDataString(uri);
        if (uriBaseId != null &&
            baseUris.TryGetValue(uriBaseId, out var baseUri) &&
            Uri.TryCreate(baseUri, UriKind.Absolute, out var baseAbsolute) &&
            baseAbsolute.IsFile)
        {
            return Path.Combine(baseAbsolute.LocalPath, relative);
        }

        return Path.Combine(rootDirectory, relative);
    }

    private static string? ToRelativePath(string? path, string rootDirectory)
    {
        if (path == null)
        {
            return null;
        }

        var relative = Path.GetRelativePath(Path.GetFullPath(rootDirectory), Path.GetFullPath(path));
        if (relative == "." || relative == ".." || relative.StartsWith("../", StringComparison.Ordinal) || relative.StartsWith("..\\", StringComparison.Ordinal) || Path.IsPathRooted(relative))
        {
            return null;
        }

        return relative.Replace('\\', '/');
    }

    private static bool IsExcluded(string relativePath)
    {
        var segments = relativePath.Split('/');
        var fileName = segments[^1];
        return segments.Take(segments.Length - 1).Any(x => x.Equals("obj", StringComparison.OrdinalIgnoreCase)) ||
               fileName.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase) ||
               fileName.EndsWith(".g.i.cs", StringComparison.OrdinalIgnoreCase) ||
               fileName.EndsWith(".generated.cs", StringComparison.OrdinalIgnoreCase) ||
               fileName.EndsWith(".designer.cs", StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeMessage(string? message)
        => string.Join(' ', (message ?? "").Split(new[] { '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)).Trim();
}
