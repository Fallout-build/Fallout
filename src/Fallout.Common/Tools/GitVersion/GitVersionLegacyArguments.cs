using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
        // A path with no version, such as a global tool apphost, stays on the GitVersion 6 switches.
        // Those switches still work on GitVersion 6. GitVersion 7 is the case that needs the long names.
        if (version is { Major: >= 7 })
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
        var match = Regex.Match(name ?? string.Empty, @"^\d+\.\d+\.\d+");
        return match.Success && Version.TryParse(match.Value, out version);
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
    internal override IEnumerable<string> AdjustArguments(IEnumerable<string> arguments, string toolPath)
        => GitVersionLegacyArguments.Adjust(arguments, toolPath ?? GitVersionTasks.ToolPath(this));
}
