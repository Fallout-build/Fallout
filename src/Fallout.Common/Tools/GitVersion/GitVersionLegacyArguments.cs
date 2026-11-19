using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Fallout.Common.Tooling;

namespace Fallout.Common.Tools.GitVersion;

/// <summary>
/// GitVersion 7 takes <c>--no-fetch</c>. GitVersion 6 takes <c>-nofetch</c> or <c>/nofetch</c>.
/// The same settings object serves both, so the switch names are rewritten from the tool version.
/// </summary>
internal static class GitVersionLegacyArguments
{
    internal static IEnumerable<string> Adjust(IEnumerable<string> arguments, string toolPath)
    {
        var version = TryGetPackageVersion(toolPath);
        if (version is not { Major: < 7 })
        {
            return arguments;
        }

        return arguments.Select(ToLegacySwitch).Where(argument => argument != null);
    }

    internal static Version TryGetPackageVersion(string toolPath)
    {
        var directory = Path.GetDirectoryName(toolPath);
        while (!string.IsNullOrEmpty(directory))
        {
            if (TryParseVersion(Path.GetFileName(directory), out var version))
            {
                return version;
            }

            directory = Path.GetDirectoryName(directory);
        }

        return TryGetAssemblyVersion(toolPath);
    }

    private static string ToLegacySwitch(string argument) => argument switch
    {
        "--allow-shallow" => "-allowshallow",
        "--branch" => "-b",
        "--commit" => "-c",
        "--config" => "-config",
        "--diagnose" => "-diag",
        "--dynamic-repo-location" => "-dynamicRepoLocation",
        "--ensure-assembly-info" => "-ensureassemblyinfo",
        "--format" => "-format",
        "--log-file" => "-l",
        "--no-cache" => "-nocache",
        "--no-fetch" => "-nofetch",
        "--no-normalize" => "-nonormalize",
        "--output" => "-output",
        "--output-file" => "-outputfile",
        "--override-config" => "-overrideconfig",
        "--password" => "-p",
        "--show-config" => "-showconfig",
        "--show-variable" => "-showvariable",
        "--telemetry-opt-out" => null,
        "--update-assembly-info" => "-updateassemblyinfo",
        "--update-project-files" => "-updateprojectfiles",
        "--update-wix-version-file" => "-updatewixversionfile",
        "--url" => "-url",
        "--username" => "-u",
        "--verbosity" => "-verbosity",
        "--version" => "-version",
        _ => argument
    };

    private static bool TryParseVersion(string name, out Version version)
    {
        version = null;
        if (string.IsNullOrEmpty(name) || !char.IsDigit(name[0]))
        {
            return false;
        }

        var end = 0;
        var dots = 0;
        while (end < name.Length && (char.IsDigit(name[end]) || name[end] == '.'))
        {
            if (name[end] == '.')
            {
                dots++;
            }

            end++;
        }

        return dots >= 2 && Version.TryParse(name[..end], out version);
    }

    private static Version TryGetAssemblyVersion(string toolPath)
    {
        try
        {
            if (string.IsNullOrEmpty(toolPath) || !File.Exists(toolPath))
            {
                return null;
            }

            return AssemblyName.GetAssemblyName(toolPath).Version;
        }
        catch (Exception exception) when (exception is BadImageFormatException or IOException or UnauthorizedAccessException)
        {
            return null;
        }
    }
}

partial class GitVersionSettings
{
    protected override IEnumerable<string> AdjustArguments(IEnumerable<string> arguments)
        => GitVersionLegacyArguments.Adjust(arguments, GitVersionTasks.ToolPath(this));
}
