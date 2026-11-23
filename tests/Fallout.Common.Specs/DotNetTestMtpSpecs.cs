using System;
using System.Collections.Generic;
using System.IO;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.DotNet;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class DotNetTestMtpSpecs
{
    [Fact]
    public void Options_of_the_test_applications_come_after_one_separator()
    {
        // Arrange
        DotNetTestMtpSettings settings = new DotNetTestMtpSettings()
            .SetProjectFile("A.Specs.csproj")
            .EnableReportTrx()
            .SetConfiguration("Release")
            .AddTestApplicationArguments("--report-xunit-html")
            .SetResultsDirectory("results")
            .SetReportTrxFileName("A.Specs.trx");

        // Act
        IEnumerable<string> arguments = settings.GetArguments();

        // Assert
        arguments.Should().Equal(
            "test", "--project", "A.Specs.csproj", "--configuration", "Release", "--results-directory", "results",
            "--", "--report-trx", "--report-trx-filename", "A.Specs.trx", "--report-xunit-html");
    }

    [Fact]
    public void Without_options_of_the_test_applications_there_is_no_separator()
    {
        // Arrange
        DotNetTestMtpSettings settings = new DotNetTestMtpSettings()
            .SetSolutionFile("All.slnx")
            .EnableNoBuild();

        // Act
        IEnumerable<string> arguments = settings.GetArguments();

        // Assert
        arguments.Should().Equal("test", "--solution", "All.slnx", "--no-build");
    }

    [Fact]
    public void Exit_codes_to_ignore_are_one_argument()
    {
        // Arrange
        DotNetTestMtpSettings settings = new DotNetTestMtpSettings()
            .SetIgnoreExitCodes(8, 9);

        // Act
        IEnumerable<string> arguments = settings.GetArguments();

        // Assert
        arguments.Should().Equal("test", "--", "--ignore-exit-code", "8;9");
    }

    [Fact]
    public void The_test_runner_comes_from_the_nearest_global_json()
    {
        // Arrange
        AbsolutePath directory = (AbsolutePath.Create(Path.GetTempPath()) / $"fallout-{Guid.NewGuid():N}").CreateDirectory();
        try
        {
            (directory / "global.json").WriteAllText("""{ "test": { "runner": "microsoft.testing.platform" } }""");
            AbsolutePath projectDirectory = (directory / "src" / "A").CreateDirectory();

            // Act
            DotNetTestRunner runner = GetTestRunner(projectDirectory, variable: null);

            // Assert
            runner.Should().Be(DotNetTestRunner.MicrosoftTestingPlatform);
        }
        finally
        {
            directory.DeleteDirectory();
        }
    }

    [Fact]
    public void A_nearer_global_json_without_a_test_runner_selects_vstest()
    {
        // Arrange
        AbsolutePath directory = (AbsolutePath.Create(Path.GetTempPath()) / $"fallout-{Guid.NewGuid():N}").CreateDirectory();
        try
        {
            (directory / "global.json").WriteAllText("""{ "test": { "runner": "Microsoft.Testing.Platform" } }""");
            AbsolutePath subDirectory = (directory / "sub").CreateDirectory();
            (subDirectory / "global.json").WriteAllText("""{ "sdk": { "version": "10.0.100" } }""");

            // Act
            DotNetTestRunner runner = GetTestRunner(subDirectory, variable: null);

            // Assert
            runner.Should().Be(DotNetTestRunner.VSTest);
        }
        finally
        {
            directory.DeleteDirectory();
        }
    }

    [Theory]
    [InlineData("""{ /* a comment */ "test": { "runner": "Microsoft.Testing.Platform" } }""",
        DotNetTestRunner.MicrosoftTestingPlatform)]
    [InlineData("""{ "Test": { "Runner": "Microsoft.Testing.Platform" } }""", DotNetTestRunner.VSTest)]
    [InlineData("""{ "test": { "runner": "Microsoft.Testing.Platform", } }""", DotNetTestRunner.VSTest)]
    public void A_global_json_is_read_like_dotnet_test_reads_it(string content, DotNetTestRunner expected)
    {
        // Arrange
        AbsolutePath directory = (AbsolutePath.Create(Path.GetTempPath()) / $"fallout-{Guid.NewGuid():N}").CreateDirectory();
        try
        {
            (directory / "global.json").WriteAllText(content);

            // Act
            DotNetTestRunner runner = GetTestRunner(directory, variable: null);

            // Assert
            runner.Should().Be(expected);
        }
        finally
        {
            directory.DeleteDirectory();
        }
    }

    [Theory]
    [InlineData("vstest", DotNetTestRunner.VSTest)]
    [InlineData("NUnit", DotNetTestRunner.MicrosoftTestingPlatform)]
    public void The_environment_variable_overrides_global_json_only_with_a_known_runner(
        string variable,
        DotNetTestRunner expected)
    {
        // Arrange
        AbsolutePath directory = (AbsolutePath.Create(Path.GetTempPath()) / $"fallout-{Guid.NewGuid():N}").CreateDirectory();
        try
        {
            (directory / "global.json").WriteAllText("""{ "test": { "runner": "Microsoft.Testing.Platform" } }""");

            // Act
            DotNetTestRunner runner = GetTestRunner(directory, variable);

            // Assert
            runner.Should().Be(expected);
        }
        finally
        {
            directory.DeleteDirectory();
        }
    }

    private static DotNetTestRunner GetTestRunner(AbsolutePath directory, string variable)
    {
        string previousValue = EnvironmentInfo.GetVariable("DOTNET_TEST_RUNNER");
        EnvironmentInfo.SetVariable("DOTNET_TEST_RUNNER", variable);
        try
        {
            return DotNetTasks.GetTestRunner(directory);
        }
        finally
        {
            EnvironmentInfo.SetVariable("DOTNET_TEST_RUNNER", previousValue);
        }
    }
}
