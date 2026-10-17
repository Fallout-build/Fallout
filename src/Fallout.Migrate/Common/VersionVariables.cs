using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fallout.Migrate.Common;

/// <summary>
/// The version variable names found in the files of one migration run: those used only by
/// <c>Nuke.*</c>/<c>Fallout.*</c> packages and those shared with unrelated packages.
/// </summary>
internal sealed class VersionVariables
{
    public const string FalloutVersionVariable = "FalloutVersion";

    /// <summary>
    /// Finds an opening <c>NukeVersion</c> or <c>FalloutVersion</c> property tag.
    /// </summary>
    /// <remarks>
    /// Both spellings count because the rewrite renames <c>NukeVersion</c> to <c>FalloutVersion</c>.
    /// The match is case-sensitive and permits whitespace before <c>&gt;</c>, but no attributes.
    /// </remarks>
    private static readonly Regex falloutVersionPropertyPattern =
        new("""<(?:Nuke|Fallout)Version\s*>""", RegexOptions.Compiled);

    /// <summary>
    /// Property names whose values should be updated to the current Fallout version.
    /// </summary>
    /// <remarks>
    /// Starts with <c>FalloutVersion</c> so an existing dedicated property is updated even when
    /// no package references it before the rewrite.
    /// </remarks>
    public HashSet<string> ToBump { get; } = [FalloutVersionVariable];

    /// <summary>
    /// Variable names used by both Fallout packages and unrelated packages.
    /// </summary>
    /// <remarks>
    /// Updating these properties would change unrelated package versions. Instead, Fallout package
    /// references are redirected to <c>$(FalloutVersion)</c> and the shared property value is kept.
    /// </remarks>
    public HashSet<string> Ambiguous { get; } = [];

    /// <summary>
    /// Whether any file already defines <c>FalloutVersion</c>, including its old <c>NukeVersion</c> name.
    /// </summary>
    public bool DefinesFalloutVersion { get; private set; }

    /// <summary>
    /// Classifies version variable names over all files before any of them are rewritten.
    /// </summary>
    /// <remarks>
    /// Classification uses names across the repository, without evaluating MSBuild imports or scopes.
    /// A name used by Fallout in project A and by another package in project B is treated as shared,
    /// even if the projects define separate properties. This can introduce extra reference changes,
    /// but avoids updating unrelated package versions.
    /// </remarks>
    public static VersionVariables Collect(IEnumerable<string> fileContents)
    {
        var result = new VersionVariables();
        var falloutVariables = new HashSet<string>();
        var nonFalloutVariables = new HashSet<string>();

        foreach (string content in fileContents)
        {
            foreach (Match match in MsBuildPatterns.VariableVersionItem.Matches(content))
            {
                string packageName = MsBuildPatterns.PackageNameOf(match).Value;
                bool isFalloutPackage = packageName.StartsWith("Nuke.", StringComparison.Ordinal) ||
                                        packageName.StartsWith("Fallout.", StringComparison.Ordinal);
                (isFalloutPackage ? falloutVariables : nonFalloutVariables).Add(MsBuildPatterns.VariableNameOf(match).Value);
            }

            result.DefinesFalloutVersion |= falloutVersionPropertyPattern.IsMatch(content);
        }

        foreach (string variableName in falloutVariables)
        {
            (nonFalloutVariables.Contains(variableName) ? result.Ambiguous : result.ToBump).Add(variableName);
        }

        return result;
    }
}
