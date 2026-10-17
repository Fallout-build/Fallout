using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// Bumps the repo's build orchestrator project (<c>_build.csproj</c>) to target
/// <see cref="TargetFramework"/> and pins <c>global.json</c>'s SDK version to
/// <see cref="SdkVersion"/> — but only when what's there is behind those minimums. Already
/// up-to-date or newer values (e.g. a build project already on <c>net11.0</c>) are left alone.
/// </summary>
internal sealed class BumpDotNetVersionStep : IMigrationStep
{
    /// <summary>
    /// The .NET target framework moniker to bump <c>_build.csproj</c> to when it's behind.
    /// </summary>
    private const string TargetFramework = "net10.0";

    /// <summary>
    /// The .NET SDK version to pin <c>global.json</c> to when it's behind.
    /// </summary>
    private const string SdkVersion = "10.0.100";

    /// <summary>
    /// The minimum .NET SDK version. Versions at or above this aren't touched.
    /// </summary>
    private static readonly Version minimumSupportedSdkVersion = new(10, 0, 100);

    /// <summary>
    /// The minimum .NET target framework major version, derived from <see cref="TargetFramework"/>.
    /// Monikers at or above this aren't touched. Shared with
    /// <see cref="VerifyBuildTargetFrameworkStep"/> so the minimum is only defined once.
    /// </summary>
    internal static readonly int MinimumSupportedMajor = TargetFrameworkMonikers.ExtractMajor(TargetFramework);

    /// <summary>
    /// Matches the <c>TargetFramework</c> element's raw value. Build projects don't multi-target,
    /// so <c>TargetFrameworks</c> (plural) is intentionally not matched here.
    /// </summary>
    private static readonly Regex targetFrameworkElementPattern = new(
        @"<TargetFramework>(?<value>[^<]+)</TargetFramework>",
        RegexOptions.Compiled);

    /// <summary>
    /// Matches the value of <c>global.json</c>'s <c>sdk.version</c> property.
    /// </summary>
    private static readonly Regex sdkVersionPattern = new(
        @"(?<=""sdk""\s*:\s*\{[^}]*?""version""\s*:\s*"")[^""]+",
        RegexOptions.Compiled | RegexOptions.Singleline);

    /// <summary>
    /// The roll-forward policy added to <c>global.json</c> when the SDK is pinned and no policy is set.
    /// Without one, the default <c>patch</c> policy needs a 10.0.1xx SDK, so a machine with only a
    /// later feature band (for example 10.0.4xx) fails every <c>dotnet</c> command.
    /// Fallout's own <c>global.json</c> uses <c>latestMinor</c>. <c>latestFeature</c> is used here
    /// because it allows a later feature band but never a later minor version, so it changes less
    /// in the migrated repository.
    /// </summary>
    private const string RollForward = "latestFeature";

    /// <summary>
    /// The roll-forward policies that accept only an SDK from the pinned feature band (10.0.1xx).
    /// An existing one is kept, but the user gets a warning.
    /// </summary>
    private static readonly string[] pinnedFeatureBandPolicies = ["patch", "latestPatch", "disable"];

    /// <summary>
    /// Matches a <c>rollForward</c> property inside <c>global.json</c>'s <c>sdk</c> object, and
    /// captures its value.
    /// </summary>
    private static readonly Regex sdkRollForwardPattern = new(
        @"""sdk""\s*:\s*\{[^}]*""rollForward""(?:\s*:\s*""(?<value>[^""]*)"")?",
        RegexOptions.Compiled | RegexOptions.Singleline);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        foreach (var path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "_build.csproj"))
        {
            MigrationFileOperations.ApplyRewrite(context, path, BumpTargetFramework, summary);
        }

        foreach (var path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "global.json"))
        {
            MigrationFileOperations.ApplyRewrite(
                context,
                path,
                original =>
                {
                    var result = BumpSdkVersion(original);
                    if (result.EditCount > 0 && KeptPinnedFeatureBandPolicy(result.Content) is { } policy)
                    {
                        summary.Warnings.Add(
                            $"{MigrationFileOperations.RelativePath(context.RootDirectory, path)} keeps \"rollForward\": \"{policy}\". " +
                            $"With the new SDK pin {SdkVersion}, this policy needs a 10.0.1xx SDK, so a machine with only " +
                            "a later feature band (for example 10.0.4xx) fails every dotnet command. " +
                            $"Change it to \"{RollForward}\" unless you need exactly that feature band.");
                    }

                    return result;
                },
                summary);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns the <c>rollForward</c> value of <paramref name="content"/> when it is one of
    /// <see cref="pinnedFeatureBandPolicies"/>, otherwise <c>null</c>.
    /// </summary>
    /// <param name="content">The <c>global.json</c> content after the SDK pin was bumped.</param>
    private static string KeptPinnedFeatureBandPolicy(string content)
    {
        var value = sdkRollForwardPattern.Match(content).Groups["value"];
        return value.Success && Array.Exists(pinnedFeatureBandPolicies, p => p.Equals(value.Value, StringComparison.OrdinalIgnoreCase))
            ? value.Value
            : null;
    }

    /// <summary>
    /// Rewrites a build project's <c>TargetFramework</c> element to <see cref="TargetFramework"/>
    /// when its current moniker is behind <see cref="MinimumSupportedMajor"/>.
    /// </summary>
    /// <param name="original">The original <c>_build.csproj</c> content.</param>
    /// <returns>The rewritten content and the number of edits made.</returns>
    private static RewriteResult BumpTargetFramework(string original)
    {
        Match match = targetFrameworkElementPattern.Match(original);
        if (!match.Success ||
            !TargetFrameworkMonikers.IsOlderThanMinimumSupported(match.Groups["value"].Value, MinimumSupportedMajor))
        {
            return new RewriteResult(original, 0);
        }

        string content = targetFrameworkElementPattern.Replace(
            original,
            $"<TargetFramework>{TargetFramework}</TargetFramework>",
            count: 1);

        return new RewriteResult(content, 1);
    }

    /// <summary>
    /// Rewrites <c>global.json</c>'s <c>sdk.version</c> to <see cref="SdkVersion"/> only when the
    /// current version is behind <see cref="minimumSupportedSdkVersion"/>, and adds
    /// <see cref="RollForward"/> when the <c>sdk</c> object has no <c>rollForward</c> yet.
    /// </summary>
    /// <param name="original">The original <c>global.json</c> content.</param>
    /// <returns>The rewritten content and the number of edits made.</returns>
    public static RewriteResult BumpSdkVersion(string original)
    {
        Match match = sdkVersionPattern.Match(original);
        if (!match.Success || !IsOlderThanMinimumSupportedSdk(match.Value))
        {
            return new RewriteResult(original, 0);
        }

        string content = sdkVersionPattern.Replace(original, SdkVersion, count: 1);
        if (sdkRollForwardPattern.IsMatch(content))
        {
            return new RewriteResult(content, 1);
        }

        // Insert after the closing quote of the new version value.
        int insertAt = match.Index + SdkVersion.Length + 1;
        content = content.Insert(insertAt, $",{PropertySeparator(original, match.Index)}\"rollForward\": \"{RollForward}\"");
        return new RewriteResult(content, 2);
    }

    /// <summary>
    /// Returns the text to put before a new property next to the <c>version</c> property: a line
    /// break plus the same indentation when <c>version</c> starts its own line, otherwise a space.
    /// </summary>
    /// <param name="original">The original <c>global.json</c> content.</param>
    /// <param name="versionValueIndex">The index of the <c>sdk.version</c> value.</param>
    private static string PropertySeparator(string original, int versionValueIndex)
    {
        int lineStart = original.LastIndexOf('\n', versionValueIndex) + 1;
        int keyStart = lineStart;
        while (original[keyStart] is ' ' or '\t')
        {
            keyStart++;
        }

        if (string.CompareOrdinal(original, keyStart, "\"version\"", 0, "\"version\"".Length) != 0)
        {
            return " ";
        }

        string lineBreak = original.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        return lineBreak + original[lineStart..keyStart];
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="version"/> is behind
    /// <see cref="minimumSupportedSdkVersion"/>, or can't be parsed as a version at all.
    /// </summary>
    /// <param name="version">The <c>sdk.version</c> value from <c>global.json</c>.</param>
    private static bool IsOlderThanMinimumSupportedSdk(string version)
    {
        Version parsed;
        if (!Version.TryParse(version, out parsed))
        {
            return true;
        }

        return parsed < minimumSupportedSdkVersion;
    }
}
