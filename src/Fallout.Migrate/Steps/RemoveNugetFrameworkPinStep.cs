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
