using System.Text.RegularExpressions;

namespace Fallout.Migrate.Common;

/// <summary>
/// Text patterns used to migrate MSBuild files without changing their XML formatting.
/// </summary>
/// <remarks>
/// These patterns are case-sensitive and match the supported MSBuild spellings, not arbitrary XML.
/// Attribute patterns expect double quotes with no whitespace around the equals sign.
/// </remarks>
internal static class MsBuildPatterns
{
    /// <summary>
    /// Finds the opening tag of a NuGet package element: <c>PackageReference</c>,
    /// <c>PackageVersion</c>, or <c>PackageDownload</c>.
    /// </summary>
    /// <remarks>
    /// The match is the whole opening tag, including all attributes. It does not capture the package
    /// name or version separately. A <c>&gt;</c> inside an attribute value ends the match early.
    /// </remarks>
    public static readonly Regex PackageItem = new(
        """<(?:PackageReference|PackageVersion|PackageDownload)\b[^>]*>""",
        RegexOptions.Compiled);

    /// <summary>
    /// Finds the <c>Nuke.</c> prefix in an <c>Include</c>, <c>Update</c>, or <c>Remove</c> attribute.
    /// </summary>
    /// <remarks>
    /// Only the prefix is matched, and it must be followed by an uppercase letter.
    /// The attribute name and the rest of the item name stay unchanged.
    /// </remarks>
    public static readonly Regex NukeItemName =
        new("""(?<=\b(?:Include|Update|Remove)=")Nuke\.(?=[A-Z])""", RegexOptions.Compiled);

    /// <summary>
    /// Finds a literal version in a <c>Version</c> or <c>VersionOverride</c> attribute.
    /// </summary>
    /// <remarks>
    /// Quotes and optional exact-version brackets are outside the match, so <c>[10.1.0]</c>
    /// keeps its brackets. MSBuild variable references and ranges containing commas or parentheses
    /// are excluded. Replacing only one bound of a range would leave an invalid version.
    /// </remarks>
    public static readonly Regex LiteralVersion = new(
        """(?<=\bVersion(?:Override)?="\[?)(?!\$\()[^"\[\],()]+(?=\]?")""",
        RegexOptions.Compiled);

    /// <summary>
    /// Finds a <c>PackageReference</c> or <c>PackageVersion</c> opening tag whose
    /// <c>Version</c> is a single MSBuild variable reference, such as <c>$(ToolsVersion)</c>.
    /// </summary>
    /// <remarks>
    /// The whole tag is matched. The <c>include</c> group captures the package name, and the
    /// <c>variable</c> group captures the name inside <c>$(...)</c>. Attributes may appear in either
    /// order. This does not match <c>VersionOverride</c> or <c>PackageDownload</c>.
    /// A <c>&gt;</c> inside an attribute value ends the tag match early.
    /// </remarks>
    public static readonly Regex VariableVersionItem = new(
        """<(?:PackageReference|PackageVersion)\b(?=[^>]*\bInclude="(?<include>[^"]+)")(?=[^>]*\bVersion="\$\((?<variable>[^)]+)\)")[^>]*>""",
        RegexOptions.Compiled);

    /// <summary>The package name in a <see cref="VariableVersionItem"/> match.</summary>
    public static Group PackageNameOf(Match variableVersionItem) => variableVersionItem.Groups["include"];

    /// <summary>The version variable name, without <c>$(...)</c>, in a <see cref="VariableVersionItem"/> match.</summary>
    public static Group VariableNameOf(Match variableVersionItem) => variableVersionItem.Groups["variable"];

    /// <summary>
    /// The MSBuild properties Fallout renamed from <c>Nuke*</c> to <c>Fallout*</c>, without their prefix.
    /// </summary>
    private static readonly string[] RenamedNukePropertySuffixes =
    [
        "Version", "RootDirectory", "ScriptDirectory", "BaseDirectory", "BaseNamespace",
        "UseNestedNamespaces", "RepositoryUrl", "UpdateReferences", "ContinueOnError", "TaskTimeout",
        "Timeout", "TasksEnabled", "DefaultExcludes", "ExcludeBoot", "ExcludeConfig", "ExcludeLogs",
        "ExcludeDirectoryBuild", "ExcludeCi", "SpecificationFiles", "ExternalFiles", "TasksAssembly",
        "TasksDirectory"
    ];

    /// <summary>
    /// Finds the <c>Nuke</c> prefix of known MSBuild properties that were renamed in Fallout.
    /// </summary>
    /// <remarks>
    /// Only the prefix is matched. <see cref="RenamedNukePropertySuffixes"/> limits the rename to known
    /// names, including both element names and references such as <c>$(NukeVersion)</c>.
    /// Unrelated user-defined names such as <c>NukeCustomProperty</c> are excluded.
    /// </remarks>
    public static readonly Regex NukePropertyName = new(
        $"""\bNuke(?=(?:{string.Join('|', RenamedNukePropertySuffixes)})\b)""",
        RegexOptions.Compiled);

    /// <summary>
    /// Finds a complete <c>NukeTelemetryVersion</c> or <c>FalloutTelemetryVersion</c> property line.
    /// </summary>
    /// <remarks>
    /// The <c>tag</c> group requires matching opening and closing names. Indentation and a trailing
    /// newline are included. The value must be on one line. Telemetry was removed from Fallout,
    /// so the whole property is deleted rather than renamed.
    /// </remarks>
    public static readonly Regex TelemetryVersionProperty = new(
        """^[ \t]*<(?<tag>(?:Nuke|Fallout)TelemetryVersion)>.*?</\k<tag>>[ \t]*\r?\n?""",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <summary>
    /// Finds a self-closing <c>System.Security.Cryptography.Xml</c> package reference line.
    /// </summary>
    /// <remarks>
    /// Attributes may appear in any order. Indentation and a trailing newline are included.
    /// The migration removes this direct pin only from project files, allowing Fallout's transitive
    /// dependency version to win. A pin in a shared props file is kept because it affects other projects.
    /// </remarks>
    public static readonly Regex CryptographyXmlPackageReference = new(
        """^[ \t]*<PackageReference\b(?=[^>]*\bInclude="System\.Security\.Cryptography\.Xml")[^/]*/>[ \t]*\r?\n?""",
        RegexOptions.Compiled | RegexOptions.Multiline);
}
