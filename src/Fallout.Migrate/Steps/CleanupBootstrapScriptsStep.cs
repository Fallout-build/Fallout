using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fallout.Migrate.Common;

namespace Fallout.Migrate.Steps;

/// <summary>
/// This migration step cleans up bootstrap scripts by removing unnecessary code.
/// The unnecessary code includes checks for a specific environment variable and related logic.
/// </summary>
internal partial class CleanupBootstrapScriptsStep : IMigrationStep
{
    public Task ExecuteAsync(MigrationContext context, Summary summary)
    {
        // The scripts can sit in a subdirectory (NukeScriptDirectory), so search the whole tree.
        foreach (var name in new[]
                 {
                     "build.sh",
                     "build.ps1"
                 })
        {
            foreach (var path in MigrationFileOperations.EnumerateFiles(context.RootDirectory, name))
            {
                MigrationFileOperations.ApplyRewrite(context, path, Cleanup, summary);
            }
        }

        return Task.CompletedTask;
    }

    private static RewriteResult Cleanup(string content)
    {
        var envVarToCheck = "NUKE_ENTERPRISE_TOKEN";
        if (!content.Contains(envVarToCheck))
        {
            return new RewriteResult(content, 0);
        }

        // This is just to preserve the current line ending the user has for this files
        string newline =
            GetLineFeedRegex().Match(content).Value is { Length: > 0 } value
                ? value
                : Environment.NewLine;

        var lines = GetLineFeedRegex().Split(content).ToList();

        // here, we get the index of the line that contains the environment variable check
        var indexOfEnterpriseEnvVarCheck = lines.FindIndex(line => line.Contains(envVarToCheck));

        // The generated block starts with the `if` line that checks the token. Any other first use,
        // for example `export NUKE_ENTERPRISE_TOKEN=...`, is not the generated block, so leave the
        // file alone. Otherwise an unrelated `fi`/`}` later in the file would end the removal.
        var checkLine = lines[indexOfEnterpriseEnvVarCheck].TrimStart();
        if (!checkLine.StartsWith("if ", StringComparison.OrdinalIgnoreCase) &&
            !checkLine.StartsWith("if(", StringComparison.OrdinalIgnoreCase))
        {
            return new RewriteResult(content, 0);
        }

        // here, we get the index of the line that ends the if block
        // which is fi on bash and a simple "}" in powershell
        var endOfIfBlock = lines.FindIndex(indexOfEnterpriseEnvVarCheck,
            line => line.Trim() == "}" || line.Trim() == "fi");

        // No closing `fi`/`}` after the token: not the generated block, so leave the file alone.
        if (endOfIfBlock < 0)
        {
            return new RewriteResult(content, 0);
        }

        // this is "just" to remove the empty line after the if block
        if (lines.Count > endOfIfBlock + 1 && lines[endOfIfBlock + 1].Trim() == "")
        {
            endOfIfBlock++;
        }

        lines.RemoveRange(indexOfEnterpriseEnvVarCheck, endOfIfBlock - indexOfEnterpriseEnvVarCheck + 1);

        return new RewriteResult(string.Join(newline, lines), 1);
    }

    [GeneratedRegex(@"\r\n|\n|\r")]
    private static partial Regex GetLineFeedRegex();
}
