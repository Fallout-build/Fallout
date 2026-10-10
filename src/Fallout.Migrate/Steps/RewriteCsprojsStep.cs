using System;
using System.Collections.Generic;
using System.IO;
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
    // Package items: PackageReference, central PackageVersion (Directory.Packages.props, #492)
    // and PackageDownload. A Nuke.X item becomes Fallout.X, and a literal Version or
    // VersionOverride on it is pinned to the current Fallout version, whatever the attribute
    // order. NUKE-era pins (e.g. `Version="10.1.0"`) don't exist as Fallout.* packages and
    // produce NU1603 ("not found, falling back to next-higher") which `WarningsAsErrors` in the
    // migrated project escalates. Pinning in the same pass avoids a broken post-migrate build
    // (#217). MSBuild variables (`$(...)`) are handled by HandleMsBuildVariable below.
    private static readonly Regex packageItemPattern = new(
        @"<(?:PackageReference|PackageVersion|PackageDownload)\b[^>]*>",
        RegexOptions.Compiled);

    // The `Nuke.` prefix of an Include, Update or Remove value: `Include="Nuke.X"` → `Include="Fallout.X"`.
    // Pass 1 applies it to package items. Pass 2 applies it to every item that pass 1 didn't touch.
    private static readonly Regex nukeItemNamePattern =
        new(@"(?<=\b(?:Include|Update|Remove)="")Nuke\.(?=[A-Z])", RegexOptions.Compiled);

    // The literal value of a Version or VersionOverride attribute. PackageDownload needs an exact
    // range (`[10.1.0]`), so the brackets stay outside the match. A version range such as
    // `[10.1.0,)` or `(10.1.0,11.0.0)` doesn't match and is kept as written: replacing only
    // one bound would leave an invalid range.
    private static readonly Regex literalVersionPattern = new(
        @"(?<=\bVersion(?:Override)?=""\[?)(?!\$\()[^""\[\],()]+(?=\]?"")",
        RegexOptions.Compiled);

    // A PackageReference or central PackageVersion whose version is an MSBuild variable:
    // Version="$(MyVar)". The Include and Version attributes can come in either order.
    // Used to classify the variables (both Nuke.* and Fallout.* spellings count, because the files
    // are classified before they are rewritten) and to redirect an ambiguous one.
    private static readonly Regex variableVersionItemPattern = new(
        @"<(?:PackageReference|PackageVersion)\b(?=[^>]*\bInclude=""(?<include>[^""]+)"")(?=[^>]*\bVersion=""\$\((?<variable>[^)]+)\)"")[^>]*>",
        RegexOptions.Compiled);

    // MSBuild element/property names that begin with `Nuke` followed by an uppercase
    // letter (e.g. <NukeRootDirectory>...). Limited to known consumer-facing names from
    // P3.5b so we don't rewrite unrelated user-defined identifiers that happen to start
    // with the literal "Nuke".
    private static readonly Regex msBuildPropertyPattern = new(
        @"\bNuke(?=" +
        "(?:Version|RootDirectory|ScriptDirectory|BaseDirectory|BaseNamespace|" +
        "UseNestedNamespaces|RepositoryUrl|UpdateReferences|ContinueOnError|TaskTimeout|" +
        "Timeout|TasksEnabled|DefaultExcludes|ExcludeBoot|ExcludeConfig|ExcludeLogs|" +
        "ExcludeDirectoryBuild|ExcludeCi|SpecificationFiles|ExternalFiles|TasksAssembly|" +
        "TasksDirectory)\\b)",
        RegexOptions.Compiled);

    // Strip the telemetry-version property entirely — telemetry was removed from Fallout
    // (ADR-0010), so a migrated project must not carry a dead <FalloutTelemetryVersion>.
    // Matches the whole element line (either legacy Nuke* or already-Fallout* spelling).
    private static readonly Regex telemetryVersionPropertyPattern = new(
        @"^[ \t]*<(?<tag>(?:Nuke|Fallout)TelemetryVersion)>.*?</\k<tag>>[ \t]*\r?\n?",
        RegexOptions.Compiled | RegexOptions.Multiline);

    // Strip explicit `System.Security.Cryptography.Xml` PackageReferences. NUKE-era projects
    // often pinned this directly at an older major (e.g. 9.x). Fallout.Common 10.2.12+ transitively
    // requires a newer version (10.0.6+) and the conflict trips NU1605 ("Detected package
    // downgrade"). Removing the explicit pin lets the transitive version win, which is what the
    // migrated project wants (#217). Matches a self-closing element with optional surrounding
    // indentation + trailing newline. Applied to *.csproj only: a pin in a *.props file such as
    // a root Directory.Build.props applies to every project in the repository, not just the build.
    private static readonly Regex cryptographyXmlPackageRefPattern = new(
        @"^[ \t]*<PackageReference\s+Include=""System\.Security\.Cryptography\.Xml""[^/]*/>[ \t]*\r?\n?",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        var files = MigrationFileOperations.EnumerateFiles(context.RootDirectory, "*.csproj")
            .Concat(MigrationFileOperations.EnumerateFiles(context.RootDirectory, "*.props"))
            .ToList();

        // A version variable can be used in one file and defined in another, for example
        // `$(NukeVersion)` in Directory.Packages.props and `<NukeVersion>` in Version.props (#492).
        // So the variables are classified over all files first, then every file is rewritten.
        var variables = VersionVariables.Collect(files.Select(ReadOrEmpty));

        foreach (var path in files)
        {
            MigrationFileOperations.ApplyRewrite(
                context,
                path,
                content => Rewrite(
                    content,
                    context.FalloutVersion,
                    variables,
                    isProjectFile: path.ToString().EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)),
                summary);
        }

        return Task.CompletedTask;

        static string ReadOrEmpty(AbsolutePath path)
        {
            // An unreadable file is reported by ApplyRewrite; it contributes nothing here.
            try
            {
                return path.ReadAllText();
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Rewrites <paramref name="original"/> content, replacing <c>Nuke.*</c> references and MSBuild
    /// properties with their <c>Fallout.*</c> equivalents and stripping stale pins.
    /// </summary>
    /// <param name="original">The original <c>.csproj</c> or <c>.props</c> file content.</param>
    /// <param name="falloutVersion">The Fallout version to pin into rewritten versioned references.</param>
    /// <param name="variables">The version variables classified over all files.</param>
    /// <param name="isProjectFile"><c>true</c> for a <c>.csproj</c>, <c>false</c> for a <c>.props</c> file.</param>
    /// <returns>The rewritten content and the number of edits made.</returns>
    private static RewriteResult Rewrite(string original, string falloutVersion, VersionVariables variables, bool isProjectFile)
    {
        var edits = 0;
        var content = original;

        // Pass 1 — package items: rename a Nuke.X Include and pin its literal Version/VersionOverride.
        // One edit per item, however many attributes changed. An item that is already Fallout.X
        // keeps its pin.
        content = packageItemPattern.Replace(content, m =>
        {
            var item = nukeItemNamePattern.Replace(m.Value, "Fallout.");
            if (item == m.Value)
            {
                return m.Value;
            }

            edits++;
            return literalVersionPattern.Replace(item, _ => falloutVersion);
        });

        // Pass 2 — namespace-only rewrites for anything Pass 1 didn't consume (other item types,
        // MSBuild properties).
        content = nukeItemNamePattern.Replace(content, _ =>
        {
            edits++;
            return "Fallout.";
        });

        content = msBuildPropertyPattern.Replace(content, _ =>
        {
            edits++;
            return "Fallout";
        });

        // Pass 3 — strip the telemetry-version property (feature removed in ADR-0010).
        content = telemetryVersionPropertyPattern.Replace(content, _ =>
        {
            edits++;
            return string.Empty;
        });

        // Pass 4 — strip the stale System.Security.Cryptography.Xml direct pin (project files only).
        if (isProjectFile)
        {
            content = cryptographyXmlPackageRefPattern.Replace(content, _ =>
            {
                edits++;
                return string.Empty;
            });
        }

        return HandleMsBuildVariable(falloutVersion, content, edits, variables);
    }

    // Pass 5 — decouple the variables ambiguously shared with non-Fallout packages via a dedicated
    // $(FalloutVersion) property, and bump every variable that's exclusively Fallout's to the
    // current Fallout version, in whichever file defines it.
    private static RewriteResult HandleMsBuildVariable(string falloutVersion, string content, int edits, VersionVariables variables)
    {
        (content, int redirectEdits) =
            RedirectAmbiguousVariablesToFalloutVersion(content, variables.Ambiguous, VersionVariables.FalloutVersionVariable);

        edits += redirectEdits;

        // Only a file with a redirected reference gets the property. Every other file stays unchanged.
        if (redirectEdits > 0 && !variables.DefinesFalloutVersion)
        {
            content = EnsureFalloutVersionPropertyExists(content, VersionVariables.FalloutVersionVariable, falloutVersion, ref edits);
        }

        (content, int bumpEdits) = BumpVariableProperties(content, variables.ToBump, falloutVersion);
        edits += bumpEdits;

        return new RewriteResult(content, edits);
    }

    /// <summary>
    /// The version variables found in the files of one migration run: those used only by Nuke.*/Fallout.*
    /// packages (bumped), those also shared with an unrelated package (ambiguous), and whether any
    /// file already defines the dedicated <c>FalloutVersion</c> property.
    /// </summary>
    private sealed class VersionVariables
    {
        public const string FalloutVersionVariable = "FalloutVersion";

        // The property is renamed from NukeVersion by msBuildPropertyPattern, so both spellings count.
        private static readonly Regex falloutVersionPropertyPattern =
            new(@"<(?:Nuke|Fallout)Version\s*>", RegexOptions.Compiled);

        public HashSet<string> ToBump { get; } = [FalloutVersionVariable];
        public HashSet<string> Ambiguous { get; } = [];
        public bool DefinesFalloutVersion { get; private set; }

        // A variable also shared with a non-Fallout package is ambiguous: bumping it directly would
        // change that unrelated package's version too, so it's decoupled instead — the Fallout
        // reference is redirected to a dedicated $(FalloutVersion) property.
        //
        // Ambiguity is decided by variable name over all files. A per-project property used by a
        // Fallout package in project A and by an unrelated package in project B is ambiguous, so
        // A is decoupled even though A's own definition isn't shared. The result is still correct,
        // it only has more edits than needed.
        public static VersionVariables Collect(IEnumerable<string> contents)
        {
            var result = new VersionVariables();
            var falloutVariables = new HashSet<string>();
            var nonFalloutVariables = new HashSet<string>();

            foreach (var content in contents)
            {
                foreach (Match match in variableVersionItemPattern.Matches(content))
                {
                    var include = match.Groups["include"].Value;
                    var isFalloutPackage = include.StartsWith("Nuke.", StringComparison.Ordinal) ||
                                           include.StartsWith("Fallout.", StringComparison.Ordinal);
                    (isFalloutPackage ? falloutVariables : nonFalloutVariables).Add(match.Groups["variable"].Value);
                }

                result.DefinesFalloutVersion |= falloutVersionPropertyPattern.IsMatch(content);
            }

            foreach (var variable in falloutVariables)
            {
                (nonFalloutVariables.Contains(variable) ? result.Ambiguous : result.ToBump).Add(variable);
            }

            return result;
        }
    }

    private static (string content, int edits) RedirectAmbiguousVariablesToFalloutVersion(
        string content, HashSet<string> ambiguousVariables, string falloutVersionVariable)
    {
        var edits = 0;

        // Runs after the rename, so only Fallout.* items are redirected. Only the variable name
        // inside `$(...)` is swapped. The rest of the item stays as written.
        content = variableVersionItemPattern.Replace(content, m =>
        {
            var variable = m.Groups["variable"];
            if (!m.Groups["include"].Value.StartsWith("Fallout.", StringComparison.Ordinal) ||
                !ambiguousVariables.Contains(variable.Value))
            {
                return m.Value;
            }

            edits++;
            var offset = variable.Index - m.Index;
            return m.Value.Remove(offset, variable.Length).Insert(offset, falloutVersionVariable);
        });

        return (content, edits);
    }

    private static string EnsureFalloutVersionPropertyExists(
        string content, string falloutVersionVariable, string falloutVersion, ref int edits)
    {
        if (content.Contains($"<{falloutVersionVariable}>"))
        {
            return content;
        }

        var propertyGroupIndex = content.IndexOf("<PropertyGroup>", StringComparison.Ordinal);
        if (propertyGroupIndex >= 0)
        {
            edits++;
            return content.Insert(
                propertyGroupIndex + "<PropertyGroup>".Length,
                $"\n    <{falloutVersionVariable}>{falloutVersion}</{falloutVersionVariable}>");
        }

        // No PropertyGroup exists at all (e.g. a project relying solely on Directory.Build.props
        // for properties) — synthesize one right after the opening <Project> tag so the newly
        // introduced $(FalloutVersion) reference has somewhere to resolve from.
        var projectTagStart = content.IndexOf("<Project", StringComparison.Ordinal);
        var projectTagEnd = projectTagStart >= 0
            ? content.IndexOf('>', projectTagStart)
            : -1;

        if (projectTagEnd < 0)
        {
            return content;
        }

        edits++;
        return content.Insert(
            projectTagEnd + 1,
            $"\n  <PropertyGroup>\n    <{falloutVersionVariable}>{falloutVersion}</{falloutVersionVariable}>\n  </PropertyGroup>");
    }

    private static (string content, int edits) BumpVariableProperties(string content, HashSet<string> variablesToBump,
        string falloutVersion)
    {
        var edits = 0;

        foreach (var variable in variablesToBump)
        {
            // Matches the text content of the <variable>...</variable> property element itself
            // (via lookbehind/lookahead, so the tags aren't part of the match and stay intact).
            var pattern = $@"(?<=<{variable}\s*>)[^<]+(?=</{variable}\s*>)";
            content = Regex.Replace(content,
                pattern,
                m =>
                {
                    if (m.Value == falloutVersion)
                    {
                        return m.Value;
                    }

                    edits++;
                    return falloutVersion;
                });
        }

        return (content, edits);
    }
}
