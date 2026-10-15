using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// Removes explicit <c>NuGet.Framework</c>, <c>NuGet.Protocol</c>, <c>NuGet.Packaging</c>, and
/// <c>NuGet.Resolver</c> package pins from project files. The pins are no longer needed.
/// </summary>
internal sealed class RemoveNugetFrameworkPinStep : IMigrationStep
{
    /// <summary>
    /// Matches a single-line, self-closing NuGet package reference for
    /// <c>NuGet.Frameworks</c> that explicitly specifies a version.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This regular expression is intended to identify explicitly version-pinned
    /// <c>NuGet.Frameworks</c> package references in XML-based MSBuild project
    /// files, such as <c>.csproj</c>, <c>.props</c>, or <c>.targets</c>.
    /// </para>
    ///
    /// <para>
    /// A match must satisfy all of the following conditions:
    /// </para>
    /// <list type="number">
    ///   <item>
    ///     <description>
    ///       The element begins at the start of a line, optionally preceded
    ///       by spaces or tab characters.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element starts with the exact, case-sensitive text
    ///       <c>&lt;PackageReference</c>, followed by at least one
    ///       whitespace character.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element contains the exact attribute
    ///       <c>Include="NuGet.Frameworks"</c>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element contains a <c>Version</c> attribute whose value
    ///       consists of one or more characters other than a double quote.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Both required attributes occur on the same line, before
    ///       the first greater-than character.
    ///       Their order does not matter.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       The element ends with the exact sequence <c>/&gt;</c>,
    ///       optionally followed by spaces or tabs.
    ///     </description>
    ///   </item>
    /// </list>
    ///
    /// <para>
    /// The match also consumes an optional line ending (<c>\n</c> or
    /// <c>\r\n</c>) immediately following the element.
    /// </para>
    ///
    /// <para>
    /// The pattern uses two positive lookaheads to verify the presence
    /// of the required attributes without enforcing their order.
    /// </para>
    ///
    /// <para>
    /// Matching is case-sensitive. Only self-closing, single-line elements
    /// are supported. This is a textual pattern, not a full XML parser.
    /// </para>
    ///
    /// <para>
    /// <b>Examples that match:</b>
    /// </para>
    /// <code>
    /// &lt;PackageReference Include="NuGet.Frameworks" Version="6.0.0" /&gt;
    /// &lt;PackageReference Version="6.0.0" Include="NuGet.Frameworks"/&gt;
    ///     &lt;PackageReference Include="NuGet.Frameworks" Version="7.0.0" /&gt;
    /// &lt;PackageReference Include="NuGet.Frameworks" Version="[6.0.0]" /&gt;
    /// </code>
    ///
    /// <para>
    /// <b>Examples that do not match:</b>
    /// </para>
    /// <code>
    /// &lt;PackageReference Include="Other.Package" Version="6.0.0" /&gt;
    /// &lt;PackageReference Include="NuGet.Frameworks" /&gt;
    /// &lt;PackageReference Include="NuGet.Frameworks" Version="6.0.0"&gt;
    /// &lt;PackageReference Include="NuGet.Frameworks"
    ///                   Version="6.0.0" /&gt;
    /// </code>
    ///
    /// <para>
    /// <b>Important limitations:</b>
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       Does not support multiline package reference elements.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Does not match references using single-quoted attributes.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Does not match versions specified using child
    ///       <c>&lt;Version&gt;</c> elements.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Does not validate that a version is a valid NuGet version.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Does not require the matched element to be the only
    ///       content on the line after the optional trailing whitespace.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Does not validate XML syntax or distinguish XML markup
    ///       from text inside comments.
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    private static readonly Regex explicitPinPattern = new(
        @"^[ \t]*<PackageReference\s+(?=[^>\r\n]*\bInclude=""NuGet\.Frameworks"")(?=[^>\r\n]*\bVersion=""[^""]+"")[^>\r\n]*/>[ \t]*\r?\n?",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        foreach (var path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "*.csproj"))
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
