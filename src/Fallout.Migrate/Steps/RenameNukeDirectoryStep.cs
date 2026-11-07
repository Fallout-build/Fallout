using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Common.IO;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// Renames the repository's <c>.nuke/</c> directory to <c>.fallout/</c>, or records a warning if
/// both already exist and need a manual merge. The <c>build.schema.json</c> inside is rewritten
/// first, so the moved file names its base definition <c>FalloutBuild</c>.
/// </summary>
internal sealed class RenameNukeDirectoryStep : IMigrationStep
{
    // NUKE's schema has the layout Fallout reads; only the base definition is named
    // NukeBuild. CompletionUtility (fallout completion and :secrets) looks up FalloutBuild,
    // so rename the definition key and its $ref. The next build rewrites the whole file.
    // The lookbehind matches any quoted "NukeBuild", not only these two. That is safe because
    // the schema is machine-generated, and NukeBuild appears only as the key and in the $ref.
    private static readonly Regex nukeBuildDefinition =
        new(@"(?<=""|#/definitions/)NukeBuild(?="")", RegexOptions.Compiled);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        var legacy = context.RootDirectory / ".nuke";
        var canonical = context.RootDirectory / ".fallout";

        // Rewrite the schema in the directory that ends up as .fallout/: .nuke/ when it is moved,
        // otherwise .fallout/. When both exist, .nuke/ is left for the manual merge. This also
        // repairs a repository that an earlier fallout-migrate moved without rewriting.
        var moveLegacy = legacy.DirectoryExists() && !canonical.DirectoryExists();
        var schema = (moveLegacy ? legacy : canonical) / "build.schema.json";
        if (schema.FileExists())
        {
            MigrationFileOperations.ApplyRewrite(context, schema, RewriteSchema, summary);
        }

        if (!legacy.DirectoryExists())
        {
            return Task.CompletedTask;
        }

        if (canonical.DirectoryExists())
        {
            summary.Warnings.Add(
                "Both .nuke/ and .fallout/ exist. Skipped rename; merge their contents manually.");

            return Task.CompletedTask;
        }

        context.Log.WriteLine(
            $"rename {MigrationFileOperations.RelativePath(context.RootDirectory, legacy)} -> {MigrationFileOperations.RelativePath(context.RootDirectory, canonical)}");

        if (!context.DryRun)
        {
            // We purposely use the .NET directory move as this is atomic. Fallout's own
            // Move/MoveDirectory moves them one by one.
            Directory.Move(legacy, canonical);
        }

        summary.DirectoriesRenamed++;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Renames the <c>NukeBuild</c> definition and its <c>$ref</c> in <paramref name="original"/>.
    /// </summary>
    /// <param name="original">The original <c>build.schema.json</c> content.</param>
    /// <returns>The rewritten content and the number of edits made.</returns>
    private static RewriteResult RewriteSchema(string original)
    {
        var edits = 0;
        var content = nukeBuildDefinition.Replace(original, _ =>
        {
            edits++;
            return "FalloutBuild";
        });

        return new RewriteResult(content, edits);
    }
}
