using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Fallout.Common.Tooling;
using Fallout.Common.Utilities;
using Fallout.Common.Utilities.Collections;

namespace Fallout.Common.Tools.GitVersion;

partial class GitVersionTasks
{
    internal static string ToolPath(GitVersionSettings settings) => new GitVersionTasks().GetToolPathInternal(settings);

    protected override object GetResult<T>(ToolOptions options, IReadOnlyCollection<Output> output)
    {
        try
        {
            // GitVersion 7 writes warnings to stderr while the JSON stays on stdout.
            return output.StdToJson<GitVersion>();
        }
        catch (Exception exception)
        {
            throw new Exception($"Cannot parse {nameof(GitVersion)} output:".Concat(output.Select(x => x.Text)).JoinNewLine(),
                exception);
        }
    }
}

/// <summary>
/// Version data from GitVersion JSON output.
/// GitVersion 6 and 7 use different JSON names for the same values. Each property keeps one meaning across both versions.
/// </summary>
[JsonConverter(typeof(GitVersionJsonConverter))]
public record GitVersion(
    int Major,
    int Minor,
    int Patch,
    string PreReleaseTag,
    string PreReleaseTagWithDash,
    string PreReleaseLabel,
    string PreReleaseLabelWithDash,
    string PreReleaseNumber,
    string WeightedPreReleaseNumber,
    string BuildMetaData,
    string BuildMetaDataPadded,
    string FullBuildMetaData,
    string MajorMinorPatch,
    string SemVer,
    string LegacySemVer,
    string LegacySemVerPadded,
    string AssemblySemVer,
    string AssemblySemFileVer,
    string FullSemVer,
    string InformationalVersion,
    string BranchName,
    string EscapedBranchName,
    string Sha,
    string ShortSha,
    string NuGetVersionV2,
    string NuGetVersion,
    string NuGetPreReleaseTagV2,
    string NuGetPreReleaseTag,
    string VersionSourceSha,
    string CommitsSinceVersionSource,
    string CommitsSinceVersionSourcePadded,
    int? UncommittedChanges,
    string CommitDate,
    string VersionSourceSemVer,
    string VersionSourceIncrement,
    string VersionSourceDistance,
    string PreReleaseLabelName,
    string PreReleaseLabelNameWithDash,
    string SemVerSourceSha,
    string SemVerSourceSemVer,
    string SemVerSourceIncrement,
    string CommitCountSourceSha,
    string CommitCountSourceDistance,
    string CustomVersion);
