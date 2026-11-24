using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.GitVersion;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

/// <summary>
/// GitVersion 6 and 7 accept different switch names for the same options.
/// </summary>
public class GitVersionLegacyArgumentSpecs
{
    [Fact]
    public void GitVersion_6_receives_the_short_switches()
    {
        // Arrange
        var settings = Settings("6.8.2")
            .SetNoFetch(true)
            .SetNoCache(true)
            .SetVerbosity(GitVersionVerbosity.Quiet);

        // Act
        var arguments = settings.GetArguments();

        // Assert
        arguments.Should().Equal("-nofetch", "-nocache", "-verbosity", "Quiet");
    }

    [Fact]
    public void GitVersion_7_keeps_the_long_switches()
    {
        // Arrange
        var settings = Settings("7.0.0", "win-x64")
            .SetNoFetch(true)
            .SetNoCache(true)
            .SetVerbosity(GitVersionVerbosity.Quiet);

        // Act
        var arguments = settings.GetArguments();

        // Assert
        arguments.Should().Equal("--no-fetch", "--no-cache", "--verbosity", "Quiet");
    }

    [Fact]
    public void GitVersion_6_uses_the_short_name_for_each_switch()
    {
        // Arrange
        var arguments = new[]
        {
            "--version",
            "--diagnose",
            "--output",
            "json",
            "--output-file",
            "out.json",
            "--show-variable",
            "SemVer",
            "--format",
            "{Major}",
            "--log-file",
            "console",
            "--config",
            "GitVersion.yml",
            "--show-config",
            "--override-config",
            "tag-prefix=v",
            "--no-cache",
            "--no-normalize",
            "--allow-shallow",
            "--update-assembly-info",
            "--ensure-assembly-info",
            "--update-project-files",
            "src",
            "--update-wix-version-file",
            "--url",
            "https://example.test/repo.git",
            "--branch",
            "develop",
            "--username",
            "user",
            "--password",
            "secret",
            "--commit",
            "abc",
            "--dynamic-repo-location",
            "repo",
            "--no-fetch",
            "--verbosity",
            "Quiet"
        };

        // Act
        var legacy = GitVersionLegacyArguments.Adjust(arguments, ToolPath("6.8.2"));

        // Assert
        legacy.Should().Equal(
            "-version",
            "-diag",
            "-output",
            "json",
            "-outputfile",
            "out.json",
            "-showvariable",
            "SemVer",
            "-format",
            "{Major}",
            "-l",
            "console",
            "-config",
            "GitVersion.yml",
            "-showconfig",
            "-overrideconfig",
            "tag-prefix=v",
            "-nocache",
            "-nonormalize",
            "-allowshallow",
            "-updateassemblyinfo",
            "-ensureassemblyinfo",
            "-updateprojectfiles",
            "src",
            "-updatewixversionfile",
            "-url",
            "https://example.test/repo.git",
            "-b",
            "develop",
            "-u",
            "user",
            "-p",
            "secret",
            "-c",
            "abc",
            "-dynamicRepoLocation",
            "repo",
            "-nofetch",
            "-verbosity",
            "Quiet");
    }

    [Fact]
    public void GitVersion_6_drops_the_telemetry_switch()
    {
        // Arrange
        var arguments = new[]
        {
            "--no-fetch",
            "--telemetry-opt-out",
            "--show-variable",
            "SemVer"
        };

        // Act
        var legacy = GitVersionLegacyArguments.Adjust(arguments, ToolPath("6.8.2"));

        // Assert
        legacy.Should().Equal("-nofetch", "-showvariable", "SemVer");
    }

    [Theory]
    [InlineData("6.8.2", 6)]
    [InlineData("6.0.5", 6)]
    [InlineData("7.0.0", 7)]
    [InlineData("7.0.0-beta.1", 7)]
    public void Package_folder_name_provides_the_tool_version(string folder, int major)
    {
        // Act
        var version = GitVersionLegacyArguments.TryGetPackageVersion(ToolPath(folder));

        // Assert
        version.Should().NotBeNull();
        version.Major.Should().Be(major);
    }

    [Fact]
    public void Unknown_tool_version_receives_the_short_switches()
    {
        // Arrange
        var arguments = new[] { "--no-fetch", "--no-cache" };

        // Act
        var legacy = GitVersionLegacyArguments.Adjust(arguments, "dotnet-gitversion");

        // Assert
        legacy.Should().Equal("-nofetch", "-nocache");
    }

    [Fact]
    public void Every_long_switch_has_a_GitVersion_6_mapping()
    {
        // Arrange
        var switches = typeof(GitVersionSettings)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(property => property.GetCustomAttribute<ArgumentAttribute>()?.Format)
            .Where(format => format != null && format.StartsWith("--", StringComparison.Ordinal))
            .Select(format => format.Split(' ')[0])
            .Distinct()
            .ToList();

        // Act
        var legacy = GitVersionLegacyArguments.Adjust(switches, ToolPath("6.8.2")).ToList();

        // Assert
        legacy.Should().NotContain(argument => argument.StartsWith("--", StringComparison.Ordinal));
        legacy.Count.Should().BeLessThan(switches.Count);
    }

    private static GitVersionSettings Settings(string version, string runtime = "any")
        => new GitVersionSettings().SetProcessToolPath(ToolPath(version, runtime));

    private static string ToolPath(string version, string runtime = "any")
        => Path.Combine(
            "packages",
            "gitversion.tool",
            version,
            "tools",
            "net10.0",
            runtime,
            "gitversion.dll");
}
