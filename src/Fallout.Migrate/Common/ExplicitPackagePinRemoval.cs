using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fallout.Common.IO;

namespace Fallout.Migrate.Common;

/// <summary>
/// Removes explicit package pins from <c>_build.csproj</c> and from <c>Directory.Packages.props</c>.
/// </summary>
/// <remarks>
/// <para>
/// Each regex must capture the package id in a group named <c>package</c>.
/// </para>
/// <para>
/// A versioned pin in <c>_build.csproj</c> is always removed.
/// The same pin in <c>Directory.Packages.props</c> is kept when another project references that package.
/// An unversioned <c>PackageReference</c> in <c>_build.csproj</c> is removed only when its central pin is removed.
/// Other project files are left unchanged.
/// </para>
/// </remarks>
internal static class ExplicitPackagePinRemoval
{
    /// <summary>
    /// Applies <paramref name="versionedPin"/> and <paramref name="unversionedBuildReference"/>
    /// to the build project and to central package management.
    /// </summary>
    /// <param name="context">The current migration context.</param>
    /// <param name="summary">The summary to update with the outcome.</param>
    /// <param name="versionedPin">Matches one self-closing, versioned pin line. Captures <c>package</c>.</param>
    /// <param name="unversionedBuildReference">
    /// Matches one self-closing <c>PackageReference</c> line without a version. Captures <c>package</c>.
    /// </param>
    /// <param name="referenceOutsideBuild">
    /// Matches a <c>PackageReference</c> in any project. Captures <c>package</c>.
    /// Used only to decide whether a central pin is still required.
    /// </param>
    public static void Remove(
        MigrationContext context,
        Summary summary,
        Regex versionedPin,
        Regex unversionedBuildReference,
        Regex referenceOutsideBuild)
    {
        // Read other projects before rewriting, so a library reference still counts.
        var usedOutsideBuild = FindPackagesUsedOutsideBuild(context, summary, referenceOutsideBuild);
        var removedCentralPins = new HashSet<string>(StringComparer.Ordinal);

        foreach (AbsolutePath path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "Directory.Packages.props"))
        {
            MigrationFileOperations.ApplyRewrite(
                context,
                path,
                original => RemoveCentralPins(original, versionedPin, usedOutsideBuild, removedCentralPins),
                summary);
        }

        foreach (AbsolutePath path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "_build.csproj"))
        {
            MigrationFileOperations.ApplyRewrite(
                context,
                path,
                original => RemoveBuildPins(original, versionedPin, unversionedBuildReference, removedCentralPins),
                summary);
        }
    }

    private static HashSet<string> FindPackagesUsedOutsideBuild(
        MigrationContext context,
        Summary summary,
        Regex referenceOutsideBuild)
    {
        var usedOutsideBuild = new HashSet<string>(StringComparer.Ordinal);

        foreach (AbsolutePath path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "*.csproj"))
        {
            if (path.Name.Equals("_build.csproj", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!MigrationFileOperations.TryRead(context, path, summary, out string content))
            {
                continue;
            }

            foreach (Match match in referenceOutsideBuild.Matches(content))
            {
                usedOutsideBuild.Add(match.Groups["package"].Value);
            }
        }

        return usedOutsideBuild;
    }

    private static RewriteResult RemoveCentralPins(
        string original,
        Regex versionedPin,
        HashSet<string> usedOutsideBuild,
        HashSet<string> removedCentralPins)
    {
        var edits = 0;
        string content = versionedPin.Replace(original, match =>
        {
            string package = match.Groups["package"].Value;
            if (usedOutsideBuild.Contains(package))
            {
                return match.Value;
            }

            removedCentralPins.Add(package);
            edits++;
            return string.Empty;
        });

        return new RewriteResult(content, edits);
    }

    private static RewriteResult RemoveBuildPins(
        string original,
        Regex versionedPin,
        Regex unversionedBuildReference,
        HashSet<string> removedCentralPins)
    {
        var edits = 0;
        string content = versionedPin.Replace(original, _ =>
        {
            edits++;
            return string.Empty;
        });

        content = unversionedBuildReference.Replace(content, match =>
        {
            if (!removedCentralPins.Contains(match.Groups["package"].Value))
            {
                return match.Value;
            }

            edits++;
            return string.Empty;
        });

        return new RewriteResult(content, edits);
    }
}
