using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// Removes explicit <c>NuGet.Frameworks</c> package pins from project files. The pins are no longer needed.
/// </summary>
internal sealed class RemoveNugetFrameworkPinStep : IMigrationStep
{
    /// <summary>
    /// Matches an entire single-line, self-closing
    /// <c>PackageReference</c> element for <c>NuGet.Frameworks</c>
    /// that explicitly declares a non-empty <c>Version</c> attribute,
    /// including its indentation and trailing line ending.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Intended for removing explicitly versioned
    /// <c>NuGet.Frameworks</c> package references from MSBuild XML
    /// files such as <c>.csproj</c>, <c>.props</c>, and <c>.targets</c>.
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
    /// </code>
    /// <para>
    /// <b>Limitations:</b>
    /// The expression is case-sensitive, supports double-quoted
    /// attributes only, and does not match multiline elements.
    /// It does not validate XML structure or NuGet version syntax
    /// and may match package references inside XML comments.
    /// </para>
    /// </remarks>
    private static readonly Regex explicitPinPattern = new(
        @"^[ \t]*<PackageReference\b(?=[^\r\n>]*[ \t]Include=""NuGet\.Frameworks"")(?=[^\r\n>]*[ \t]Version=""[^""]+"")[^\r\n>]*?/>[ \t]*(?:\r?\n|$)",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        Version.TryParse(context.FalloutVersion, out var versionGate);

        if (versionGate < Version.Parse("10.5.0"))
        {
            return Task.CompletedTask;
        }

        foreach (var path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "_build.csproj"))
        {
            MigrationFileOperations.ApplyRewrite(context, path, Rewrite, summary);
        }

        return Task.CompletedTask;
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
