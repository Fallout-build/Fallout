using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Common.IO;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// Rewrites every <c>*.csproj</c> and <c>*.props</c> file under the repository root: <c>Nuke.*</c>
/// package/project references and central <c>PackageVersion</c> items become <c>Fallout.*</c>
/// (pinning the current Fallout version where a <c>Version</c> or <c>VersionOverride</c> attribute
/// was present), <c>Nuke*</c> MSBuild properties are renamed to <c>Fallout*</c>, version properties
/// are bumped wherever they are defined, and stale explicit
/// <c>System.Security.Cryptography.Xml</c> pins are stripped from <c>*.csproj</c> files.
/// </summary>
internal sealed class RewriteCsprojsStep : IMigrationStep
{
    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        Dictionary<AbsolutePath, string> fileContents = ReadFileContents(context, summary);

        // A variable can be used in Directory.Packages.props and defined in an imported Version.props.
        // Classify all readable files before rewriting any of them, using the same contents for both.
        VersionVariables variables = VersionVariables.Collect(fileContents.Values);

        foreach ((AbsolutePath path, string content) in fileContents)
        {
            MigrationFileOperations.ApplyRewrite(
                context,
                path,
                content,
                original => Rewrite(original, context.FalloutVersion, variables, path.HasExtension(".csproj")),
                summary);
        }

        return Task.CompletedTask;
    }

    private static Dictionary<AbsolutePath, string> ReadFileContents(MigrationContext context, Summary summary)
    {
        IEnumerable<AbsolutePath> paths = MigrationFileOperations.EnumerateFiles(context.RootDirectory, "*.csproj")
            .Concat(MigrationFileOperations.EnumerateFiles(context.RootDirectory, "*.props"));
        var fileContents = new Dictionary<AbsolutePath, string>();

        foreach (AbsolutePath path in paths)
        {
            if (MigrationFileOperations.TryRead(context, path, summary, out string content))
            {
                fileContents.Add(path, content);
            }
        }

        return fileContents;
    }

    /// <summary>
    /// Applies the ordered rewrite passes to one MSBuild file and counts their edits.
    /// </summary>
    private static RewriteResult Rewrite(string original, string falloutVersion, VersionVariables variables, bool isProjectFile)
    {
        var result = new RewriteResult(original, 0);
        result = RenamePackageItemsAndPinLiteralVersions(result, falloutVersion);
        result = RenameRemainingItemNames(result);
        result = RenameMsBuildProperties(result);
        result = RemoveTelemetryVersionProperty(result);

        if (isProjectFile)
        {
            result = RemoveCryptographyXmlPackageReference(result);
        }

        result = RedirectSharedVersionReferences(result, variables, falloutVersion);
        result = UpdateVersionProperties(result, variables.ToBump, falloutVersion);
        return result;
    }

    private static RewriteResult RenamePackageItemsAndPinLiteralVersions(RewriteResult previous, string falloutVersion)
    {
        int edits = 0;
        string content = MsBuildPatterns.PackageItem.Replace(previous.Content, match =>
        {
            string renamedItem = MsBuildPatterns.NukeItemName.Replace(match.Value, "Fallout.");
            if (renamedItem == match.Value)
            {
                return match.Value;
            }

            // Count one edit per item. Already migrated Fallout items keep their literal version.
            edits++;
            return MsBuildPatterns.LiteralVersion.Replace(renamedItem, _ => falloutVersion);
        });

        return new RewriteResult(content, previous.EditCount + edits);
    }

    private static RewriteResult RenameRemainingItemNames(RewriteResult previous) =>
        ReplaceAndCount(previous, MsBuildPatterns.NukeItemName, "Fallout.");

    private static RewriteResult RenameMsBuildProperties(RewriteResult previous) =>
        ReplaceAndCount(previous, MsBuildPatterns.NukePropertyName, "Fallout");

    private static RewriteResult RemoveTelemetryVersionProperty(RewriteResult previous) =>
        ReplaceAndCount(previous, MsBuildPatterns.TelemetryVersionProperty, string.Empty);

    private static RewriteResult RemoveCryptographyXmlPackageReference(RewriteResult previous) =>
        ReplaceAndCount(previous, MsBuildPatterns.CryptographyXmlPackageReference, string.Empty);

    private static RewriteResult ReplaceAndCount(RewriteResult previous, Regex pattern, string replacement)
    {
        int edits = 0;
        string content = pattern.Replace(previous.Content, _ =>
        {
            edits++;
            return replacement;
        });

        return new RewriteResult(content, previous.EditCount + edits);
    }

    private static RewriteResult RedirectSharedVersionReferences(
        RewriteResult previous, VersionVariables variables, string falloutVersion)
    {
        int edits = 0;
        string content = MsBuildPatterns.VariableVersionItem.Replace(previous.Content, match =>
        {
            Group variableName = MsBuildPatterns.VariableNameOf(match);
            if (!MsBuildPatterns.PackageNameOf(match).Value.StartsWith("Fallout.", StringComparison.Ordinal) ||
                !variables.Ambiguous.Contains(variableName.Value))
            {
                return match.Value;
            }

            // Runs after renaming packages. Change only the variable name in Fallout references,
            // leaving the shared property and the unrelated package references unchanged.
            edits++;
            return ReplaceGroupValue(match, variableName, VersionVariables.FalloutVersionVariable);
        });

        var result = new RewriteResult(content, previous.EditCount + edits);

        // Only a file with a redirected reference needs a property, and only if no file defines it.
        // Each such file gets its own property. A property in one project file is not visible in
        // another, and this step does not follow MSBuild imports to find a file that both import.
        return edits > 0 && !variables.DefinesFalloutVersion
            ? InsertFalloutVersionProperty(result, falloutVersion)
            : result;
    }

    /// <summary>
    /// Replaces the text one group captured and keeps the rest of the matched text as written.
    /// </summary>
    private static string ReplaceGroupValue(Match match, Group group, string replacement)
    {
        int offset = group.Index - match.Index;
        return match.Value.Remove(offset, group.Length).Insert(offset, replacement);
    }

    private static RewriteResult InsertFalloutVersionProperty(RewriteResult previous, string falloutVersion)
    {
        string content = previous.Content;
        string newLine = content.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        int propertyGroupIndex = content.IndexOf("<PropertyGroup>", StringComparison.Ordinal);
        if (propertyGroupIndex >= 0)
        {
            content = content.Insert(
                propertyGroupIndex + "<PropertyGroup>".Length,
                $"{newLine}    <FalloutVersion>{falloutVersion}</FalloutVersion>");
            return new RewriteResult(content, previous.EditCount + 1);
        }

        // A file without an unconditional PropertyGroup needs one after the opening Project tag.
        int projectTagStart = content.IndexOf("<Project", StringComparison.Ordinal);
        int projectTagEnd = projectTagStart >= 0
            ? content.IndexOf('>', projectTagStart)
            : -1;

        if (projectTagEnd < 0)
        {
            return previous;
        }

        content = content.Insert(
            projectTagEnd + 1,
            $"{newLine}  <PropertyGroup>{newLine}    <FalloutVersion>{falloutVersion}</FalloutVersion>{newLine}  </PropertyGroup>");
        return new RewriteResult(content, previous.EditCount + 1);
    }

    private static RewriteResult UpdateVersionProperties(
        RewriteResult previous, HashSet<string> variablesToBump, string falloutVersion)
    {
        int edits = 0;
        string content = previous.Content;

        foreach (string variableName in variablesToBump)
        {
            content = CreatePropertyValuePattern(variableName).Replace(content, match =>
            {
                if (match.Value == falloutVersion)
                {
                    return match.Value;
                }

                edits++;
                return falloutVersion;
            });
        }

        return new RewriteResult(content, previous.EditCount + edits);
    }

    /// <summary>
    /// Matches the text value inside a named MSBuild property element.
    /// </summary>
    /// <remarks>
    /// Lookarounds leave the opening and closing tags outside the match. Whitespace before
    /// <c>&gt;</c> is allowed, but attributes, empty values, and nested elements are not matched.
    /// Property names and values are case-sensitive. The name comes from the file, so it is escaped.
    /// </remarks>
    private static Regex CreatePropertyValuePattern(string variableName)
    {
        string name = Regex.Escape(variableName);
        return new Regex($"""(?<=<{name}\s*>)[^<]+(?=</{name}\s*>)""");
    }
}
