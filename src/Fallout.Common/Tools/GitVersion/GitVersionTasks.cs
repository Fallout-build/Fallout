using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Fallout.Common.Tooling;
using Fallout.Common.Utilities;
using Fallout.Common.Utilities.Collections;

namespace Fallout.Common.Tools.GitVersion;

partial class GitVersionTasks
{
    internal static string ToolPath(GitVersionSettings settings) => new GitVersionTasks().GetToolPathInternal(settings);

    internal static GitVersion ReadToolOutput(IReadOnlyCollection<Output> output)
        => (GitVersion)new GitVersionTasks().GetResult<GitVersion>(new GitVersionSettings(), output);

    protected override object GetResult<T>(ToolOptions options, IReadOnlyCollection<Output> output)
    {
        try
        {
            // GitVersion 7 writes warnings to stderr while the JSON stays on stdout.
            var json = output.Where(x => x.Type == OutputType.Std).Select(x => x.Text).JoinNewLine();
            return JsonSerializer.Deserialize<GitVersion>(json, GitVersionJsonConverter.Options);
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
/// <param name="Major">Major version number.</param>
/// <param name="Minor">Minor version number.</param>
/// <param name="Patch">Patch version number.</param>
/// <param name="PreReleaseTag">The full pre-release label, such as <c>beta.1</c>. GitVersion 7 calls this JSON field <c>PreReleaseLabel</c>.</param>
/// <param name="PreReleaseTagWithDash">The full pre-release label with a leading dash. GitVersion 7 calls this JSON field <c>PreReleaseLabelWithDash</c>.</param>
/// <param name="PreReleaseLabel">The pre-release name without the number, such as <c>beta</c>. GitVersion 7 calls this JSON field <c>PreReleaseLabelName</c>.</param>
/// <param name="PreReleaseLabelWithDash">The pre-release name with a leading dash. GitVersion 7 calls this JSON field <c>PreReleaseLabelNameWithDash</c>.</param>
/// <param name="PreReleaseNumber">The number at the end of the pre-release label.</param>
/// <param name="WeightedPreReleaseNumber">The padded pre-release number.</param>
/// <param name="BuildMetaData">Build metadata after the plus sign.</param>
/// <param name="BuildMetaDataPadded">Padded build metadata.</param>
/// <param name="FullBuildMetaData">Build metadata including branch and commit.</param>
/// <param name="MajorMinorPatch">Major, minor, and patch joined by dots.</param>
/// <param name="SemVer">Semantic version without build metadata.</param>
/// <param name="LegacySemVer">Legacy semantic version format.</param>
/// <param name="LegacySemVerPadded">Padded legacy semantic version.</param>
/// <param name="AssemblySemVer">Assembly version.</param>
/// <param name="AssemblySemFileVer">Assembly file version.</param>
/// <param name="FullSemVer">Semantic version including build metadata.</param>
/// <param name="InformationalVersion">Informational version string.</param>
/// <param name="BranchName">Git branch name.</param>
/// <param name="EscapedBranchName">Branch name safe for a file name.</param>
/// <param name="Sha">Full commit hash.</param>
/// <param name="ShortSha">Short commit hash.</param>
/// <param name="NuGetVersionV2">NuGet version. GitVersion 6 leaves this empty, so it falls back to <c>SemVer</c>.</param>
/// <param name="NuGetVersion">Older NuGet version field.</param>
/// <param name="NuGetPreReleaseTagV2">NuGet pre-release tag.</param>
/// <param name="NuGetPreReleaseTag">Older NuGet pre-release tag.</param>
/// <param name="VersionSourceSha">Commit that the version was calculated from.</param>
/// <param name="CommitsSinceVersionSource">Commits since the version source. GitVersion 7 calls this JSON field <c>VersionSourceDistance</c>.</param>
/// <param name="CommitsSinceVersionSourcePadded">Padded commit distance.</param>
/// <param name="UncommittedChanges">Count of uncommitted changes.</param>
/// <param name="CommitDate">Commit date.</param>
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
    string CommitDate)
{
    public string VersionSourceSemVer { get; init; }

    public string VersionSourceIncrement { get; init; }

    public string SemVerSourceSha { get; init; }

    public string SemVerSourceSemVer { get; init; }

    public string SemVerSourceIncrement { get; init; }

    public string CommitCountSourceSha { get; init; }

    public string CommitCountSourceDistance { get; init; }

    public string CustomVersion { get; init; }
}
