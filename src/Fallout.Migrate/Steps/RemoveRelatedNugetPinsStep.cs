using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// Removes explicit <c>NuGet.Protocol</c>, <c>NuGet.Packaging</c>, and <c>NuGet.Resolver</c>
/// package pins from project files when migrating to Fallout 11 or later.
/// </summary>
internal sealed class RemoveRelatedNugetPinsStep : IMigrationStep
{
    /// <summary>
    /// The minimum Fallout major version that requires this step. On migrations below that version ths step is skipped.
    /// </summary>
    private const int MinimumFalloutMajor = 11;

    /// <summary>
    /// Matches an entire single-line, self-closing
    /// <c>PackageReference</c> element for <c>NuGet.Protocol</c>,
    /// <c>NuGet.Packaging</c>, or <c>NuGet.Resolver</c> that explicitly
    /// declares a non-empty <c>Version</c> attribute, including its
    /// indentation and trailing line ending.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Intended for removing explicitly versioned NuGet package
    /// references from MSBuild XML files such as <c>.csproj</c>,
    /// <c>.props</c>, and <c>.targets</c>.
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
    ///       The element must contain an <c>Include</c> attribute
    ///       identifying one of the following packages:
    ///       <c>NuGet.Protocol</c>, <c>NuGet.Packaging</c>,
    ///       or <c>NuGet.Resolver</c>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element must contain a non-empty
    ///       <c>Version="..."</c> attribute.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element must be self-closing and contained on one line.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// Two positive lookaheads independently verify the package name
    /// and version attribute. Their positions allow either attribute
    /// to appear first, with arbitrary additional attributes between
    /// them.
    /// </para>
    /// <para>
    /// The expression consumes the entire matching line, including
    /// its indentation, optional trailing whitespace, and line
    /// ending (if present). Replacing matches with an empty string
    /// therefore removes the package reference without leaving
    /// an empty line.
    /// </para>
    /// <para>
    /// <b>Matching examples:</b>
    /// </para>
    /// <code>
    /// &lt;PackageReference Include="NuGet.Protocol" Version="6.0.0" /&gt;
    /// &lt;PackageReference Version="7.9.0" Include="NuGet.Packaging" /&gt;
    /// &lt;PackageReference Version="7.9.0" PrivateAssets="all" Include="NuGet.Resolver" /&gt;
    /// &lt;PackageReference Include="NuGet.Protocol" PrivateAssets="all" Version="6.0.0" /&gt;
    /// </code>
    /// <para>
    /// <b>Limitations:</b>
    /// Matching is case-sensitive and supports double-quoted
    /// attributes only. Multiline elements are not supported.
    /// The expression does not validate XML structure or NuGet
    /// version syntax and may match elements inside XML comments.
    /// </para>
    /// </remarks>
    private static readonly Regex explicitPinPattern = new(
        @"^[ \t]*<PackageReference\b(?=[^\r\n>]*[ \t]Include=""NuGet\.(?:Protocol|Packaging|Resolver)"")(?=[^\r\n>]*[ \t]Version=""[^""]+"")[^\r\n>]*?/>[ \t]*(?:\r?\n|$)",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        if (!RunsForMinimumRequiredVersion(context.FalloutVersion))
        {
            return Task.CompletedTask;
        }

        foreach (var path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "_build.csproj"))
        {
            MigrationFileOperations.ApplyRewrite(context, path, Rewrite, summary);
        }

        return Task.CompletedTask;
    }

    /// <returns>
    /// <see langword="true"/> if the major version can be parsed as a
    /// non-negative integer and is greater than or equal to
    /// <see cref="MinimumFalloutMajor"/>; otherwise, <see langword="false"/>.
    /// Returns <see langword="false"/> for <see langword="null"/>,
    /// empty, or unparseable version strings.
    /// </returns>
    /// <remarks>
    /// The major version is parsed using <see cref="NumberStyles.None"/>
    /// and <see cref="CultureInfo.InvariantCulture"/>.
    /// Minor, patch, and subsequent version segments are ignored and
    /// are not validated.
    /// </remarks>
    private static bool RunsForMinimumRequiredVersion(string version)
    {
        int separatorIndex = version?.IndexOf('.') ?? -1;
        string majorSegment = separatorIndex == -1 ? version : version[..separatorIndex];

        return int.TryParse(majorSegment, NumberStyles.None, CultureInfo.InvariantCulture, out int major) &&
               major >= MinimumFalloutMajor;
    }

    private static RewriteResult Rewrite(string original)
    {
        var edits = 0;
        var content = explicitPinPattern.Replace(original, _ =>
        {
            edits++;
            return string.Empty;
        });

        return new RewriteResult(content, edits);
    }
}
