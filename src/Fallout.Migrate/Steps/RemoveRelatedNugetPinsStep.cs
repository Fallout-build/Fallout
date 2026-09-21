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
    private const int MinimumFalloutMajor = 11;

    private static readonly Regex explicitPinPattern = new(
        @"^[ \t]*<PackageReference\s+(?=[^>\r\n]*\bInclude=""NuGet\.(?:Protocol|Packaging|Resolver)"")(?=[^>\r\n]*\bVersion=""[^""]+"")[^>\r\n]*/>[ \t]*\r?\n?",
        RegexOptions.Compiled | RegexOptions.Multiline);

    /// <inheritdoc />
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        if (!RunsFor(context.FalloutVersion))
        {
            return Task.CompletedTask;
        }

        foreach (var path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, "*.csproj"))
        {
            MigrationFileOperations.ApplyRewrite(context, path, Rewrite, summary);
        }

        return Task.CompletedTask;
    }

    private static bool RunsFor(string version)
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