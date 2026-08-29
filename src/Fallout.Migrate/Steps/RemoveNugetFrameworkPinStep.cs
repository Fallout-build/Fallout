using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// Removes explicit <c>NuGet.Framework</c> package pins from project files. The pin was a temporary
/// workaround for .NET SDK 10.0.400 and is no longer needed.
/// </summary>
internal sealed class RemoveNugetFrameworkPinStep : IMigrationStep
{
    private static readonly Regex explicitPinPattern = new(
        @"^[ \t]*<PackageReference\s+(?=[^>\r\n]*\bInclude=""NuGet\.Framework"")(?=[^>\r\n]*\bVersion=""[^""]+"")[^>\r\n]*/>[ \t]*\r?\n?",
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
