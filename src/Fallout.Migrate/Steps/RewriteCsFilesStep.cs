using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// Rewrites every <c>*.cs</c> file under the repository root: <c>Nuke.*</c> namespace prefixes become
/// <c>Fallout.</c>, the bare <c>NukeBuild</c>/<c>INukeBuild</c> types become
/// <c>FalloutBuild</c>/<c>IFalloutBuild</c>, the <c>IHaz*</c> component interfaces become
/// <c>IHas*</c>, and the solution-model namespace (which moved out of
/// <c>*.Common.ProjectModel</c> in v11) becomes <c>Fallout.Solutions</c>.
/// </summary>
internal sealed class RewriteCsFilesStep : IMigrationStep
{
    // The solution types moved from `(Nuke|Fallout).Common.ProjectModel` to the
    // dedicated `Fallout.Solutions` namespace in v11 (#248 + onion layering).
    // Run this BEFORE the generic prefix swap so a NUKE-era reference lands on
    // the canonical v11 namespace in one edit instead of the now-dead
    // `Fallout.Common.ProjectModel`. Matching both source prefixes also fixes
    // already-partially-migrated code. Mirrors the codefix mapping from #253.
    // The leading `(?<![\w.])` lookbehind keeps the namespace inside another dotted name
    // (`Acme.Nuke.Common.ProjectModel`) as it is, the same as in namespacePrefix below.
    private static readonly Regex projectModelNamespace =
        new(@"(?<![\w.])(?:Nuke|Fallout)\.Common\.ProjectModel\b", RegexOptions.Compiled);

    // Anchored prefix swap: `Nuke.` → `Fallout.`. Covers using directives,
    // attribute references, qualified type names, namespace declarations.
    // The leading `(?<![\w.])` lookbehind keeps a `Nuke` segment inside another
    // dotted name (`Acme.Nuke.Tools`) as it is.
    // The trailing `(?=[A-Z])` lookahead avoids matching `Nuke.json` filenames
    // or other lowercase tails the prefix audit deliberately preserved.
    private static readonly Regex namespacePrefix =
        new(@"(?<![\w.])Nuke\.(?=[A-Z])", RegexOptions.Compiled);

    // Bare type renames done in the Fallout rebrand (#59).
    private static readonly Regex nukeBuildType = new(@"\bNukeBuild\b", RegexOptions.Compiled);
    private static readonly Regex iNukeBuildType = new(@"\bINukeBuild\b", RegexOptions.Compiled);

    // NUKE's IHaz* component interfaces are IHas* in Fallout.Components. The [Obsolete]
    // IHaz* aliases ship only in the Nuke.Components shim, which the namespace swap above
    // moves the code away from. Limited to the shipped names, so a user-defined IHaz*
    // interface keeps its name.
    private static readonly Regex iHazComponentType = new(
        @"\bIHaz(?=(?:Artifacts|Changelog|Configuration|GitRepository|GitVersion|NerdbankGitVersioning|Reports|Solution|TwitterCredentials)\b)",
        RegexOptions.Compiled);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        foreach (var path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "*.cs"))
        {
            MigrationFileOperations.ApplyRewrite(context, path, Rewrite, summary);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Rewrites <paramref name="original"/> C# source, replacing <c>Nuke.*</c> references and the
    /// bare NUKE build types with their Fallout equivalents.
    /// </summary>
    /// <param name="original">The original <c>.cs</c> file content.</param>
    /// <returns>The rewritten content and the number of edits made.</returns>
    private static RewriteResult Rewrite(string original)
    {
        var edits = 0;

        var content = projectModelNamespace.Replace(original, _ =>
        {
            edits++;
            return "Fallout.Solutions";
        });

        content = namespacePrefix.Replace(content, _ =>
        {
            edits++;
            return "Fallout.";
        });

        content = iNukeBuildType.Replace(content, _ =>
        {
            edits++;
            return "IFalloutBuild";
        });

        content = nukeBuildType.Replace(content, _ =>
        {
            edits++;
            return "FalloutBuild";
        });

        content = iHazComponentType.Replace(content, _ =>
        {
            edits++;
            return "IHas";
        });

        return new RewriteResult(content, edits);
    }
}
