using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
    /// Removes explicit <c>NuGet.Frameworks</c> pins from <c>_build.csproj</c> and from
    /// <c>Directory.Packages.props</c> when migrating to Fallout 10.5 or later.
    /// A central pin is kept when another project still references the package.
    /// </summary>
internal sealed class RemoveNugetFrameworkPinStep : IMigrationStep
{
    /// <summary>
    /// Matches an entire single-line, self-closing
    /// <c>PackageReference</c> or <c>PackageVersion</c> element for <c>NuGet.Frameworks</c>
    /// that explicitly declares a non-empty <c>Version</c> or <c>VersionOverride</c> attribute,
    /// including its indentation and trailing line ending.
    /// The <c>package</c> group is the package id.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Intended for removing explicitly versioned
    /// <c>NuGet.Frameworks</c> pins from <c>_build.csproj</c> and from
    /// <c>Directory.Packages.props</c>.
    /// </para>
    /// <para>
    /// The <c>Include</c> and <c>Version</c> attributes may appear
    /// in any order. Additional attributes, such as
    /// <c>PrivateAssets</c>, are allowed before, between, or after
    /// the required attributes.
    /// </para>
    /// <para>
    /// Matching requirements:
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       The element must begin on a line containing only optional
    ///       leading spaces or tabs.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element must contain
    ///       <c>Include="NuGet.Frameworks"</c>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element must contain a non-empty
    ///       <c>Version="..."</c> or <c>VersionOverride="..."</c> attribute.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element must be self-closing and contained on one line.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// The expression consumes the entire matching line, including
    /// its indentation, optional trailing whitespace, and line
    /// ending (if present). Replacing matches with an empty string
    /// therefore removes the package reference without leaving
    /// an empty line.
    /// </para>
    /// <para>
    /// <b>Example:</b>
    /// </para>
    /// <code>
    /// &lt;PackageReference Version="7.9.0" PrivateAssets="all" Include="NuGet.Frameworks" /&gt;
    /// &lt;PackageVersion Include="NuGet.Frameworks" Version="7.9.0" /&gt;
    /// </code>
    /// <para>
    /// <b>Limitations:</b>
    /// The expression is case-sensitive, supports double-quoted
    /// attributes only, and does not match multiline elements.
    /// It does not validate XML structure or NuGet version syntax
    /// and may match package references inside XML comments.
    /// </para>
    /// </remarks>
    private static readonly Regex versionedPin = new(
        @"^[ \t]*<(?:PackageReference|PackageVersion)\b(?=[^\r\n>]*[ \t]Include=""(?<package>NuGet\.Frameworks)"")(?=[^\r\n>]*[ \t]Version(?:Override)?=""[^""]+"")[^\r\n>]*?/>[ \t]*(?:\r?\n|$)",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <summary>
    /// Matches a self-closing <c>PackageReference</c> for <c>NuGet.Frameworks</c> that has no version.
    /// The <c>package</c> group is the package id.
    /// </summary>
    /// <remarks>
    /// Used only for the build project, and only after the matching central pin was removed.
    /// A reference with no central pin is left in place.
    /// </remarks>
    private static readonly Regex unversionedBuildReference = new(
        @"^[ \t]*<PackageReference\b(?=[^\r\n>]*[ \t]Include=""(?<package>NuGet\.Frameworks)"")(?![^\r\n>]*[ \t]Version(?:Override)?="")[^\r\n>]*?/>[ \t]*(?:\r?\n|$)",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <summary>
    /// Matches a <c>PackageReference</c> for <c>NuGet.Frameworks</c> in any project.
    /// The <c>package</c> group is the package id.
    /// </summary>
    /// <remarks>
    /// This does not remove the reference. It only records that another project still needs a central pin.
    /// </remarks>
    private static readonly Regex referenceOutsideBuild = new(
        @"<PackageReference\b(?=[^>]*[ \t](?:Include|Update)=""(?<package>NuGet\.Frameworks)"")[^>]*>",
        RegexOptions.Compiled);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        // #677 (NuGet versions aligned with the SDK) shipped in 10.5.0.
        // Keep the pin when the target is older, or when the version cannot be parsed.
        Version.TryParse(context.FalloutVersion, out Version versionGate);

        if (versionGate is null || versionGate < Version.Parse("10.5.0"))
        {
            return Task.CompletedTask;
        }

        ExplicitPackagePinRemoval.Remove(
            context,
            summary,
            versionedPin,
            unversionedBuildReference,
            referenceOutsideBuild);

        return Task.CompletedTask;
    }
}
