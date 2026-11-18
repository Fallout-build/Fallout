using System;
using System.IO;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.DotNet;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class DotNetTestMtpSpecs : IDisposable
{
    private readonly AbsolutePath _directory =
        AbsolutePath.Create(Path.GetTempPath()) / $"fallout-test-runner-{Guid.NewGuid():N}";

    public DotNetTestMtpSpecs()
    {
        _directory.CreateDirectory();
    }

    public void Dispose()
    {
        _directory.DeleteDirectory();
    }

    [Fact]
    public void Options_of_the_test_applications_come_after_one_separator()
    {
        // Arrange
        var settings = new DotNetTestMtpSettings()
            .SetProjectFile("A.Specs.csproj")
            .EnableReportTrx()
            .SetConfiguration("Release")
            .AddTestApplicationArguments("--report-xunit-html")
            .SetResultsDirectory("results")
            .SetReportTrxFileName("A.Specs.trx");

        // Act
        var arguments = settings.GetArguments();

        // Assert
        arguments.Should().Equal(
            "test", "--project", "A.Specs.csproj", "--configuration", "Release", "--results-directory", "results",
            "--", "--report-trx", "--report-trx-filename", "A.Specs.trx", "--report-xunit-html");
    }

    [Fact]
    public void Without_options_of_the_test_applications_there_is_no_separator()
    {
        // Arrange
        var settings = new DotNetTestMtpSettings()
            .SetSolutionFile("All.slnx")
            .EnableNoBuild();

        // Act
        var arguments = settings.GetArguments();

        // Assert
        arguments.Should().Equal("test", "--solution", "All.slnx", "--no-build");
    }

    [Fact]
    public void Exit_codes_to_ignore_are_one_argument()
    {
        // Arrange
        var settings = new DotNetTestMtpSettings()
            .SetIgnoreExitCodes(8, 9);

        // Act
        var arguments = settings.GetArguments();

        // Assert
        arguments.Should().Equal("test", "--", "--ignore-exit-code", "8;9");
    }

    [Fact]
    public void The_test_runner_comes_from_the_nearest_global_json()
    {
        // Arrange
        WriteGlobalJson(_directory, """{ "test": { "runner": "Microsoft.Testing.Platform" } }""");
        var projectDirectory = (_directory / "src" / "A").CreateDirectory();

        // Act
        var runner = DotNetTasks.GetGlobalJsonTestRunner(projectDirectory);

        // Assert
        runner.Should().Be(DotNetTestRunner.MicrosoftTestingPlatform);
    }

    [Fact]
    public void A_nearer_global_json_without_a_test_runner_hides_the_one_above()
    {
        // Arrange
        WriteGlobalJson(_directory, """{ "test": { "runner": "Microsoft.Testing.Platform" } }""");
        var subDirectory = (_directory / "sub").CreateDirectory();
        WriteGlobalJson(subDirectory, """{ "sdk": { "version": "10.0.100" } }""");

        // Act
        var runner = DotNetTasks.GetGlobalJsonTestRunner(subDirectory);

        // Assert
        runner.Should().BeNull();
    }

    [Fact]
    public void A_global_json_can_have_comments_and_trailing_commas()
    {
        // Arrange
        WriteGlobalJson(_directory, """
            {
              // Run the tests on Microsoft.Testing.Platform.
              "test": { "runner": "microsoft.testing.platform", },
            }
            """);

        // Act
        var runner = DotNetTasks.GetGlobalJsonTestRunner(_directory);

        // Assert
        runner.Should().Be(DotNetTestRunner.MicrosoftTestingPlatform);
    }

    [Theory]
    [InlineData("Microsoft.Testing.Platform", DotNetTestRunner.MicrosoftTestingPlatform)]
    [InlineData("VSTest", DotNetTestRunner.VSTest)]
    [InlineData("vstest", DotNetTestRunner.VSTest)]
    public void Known_test_runner_names_are_recognized(string value, DotNetTestRunner expected)
    {
        // Act
        var runner = DotNetTasks.ParseTestRunner(value);

        // Assert
        runner.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("NUnit")]
    public void Unknown_test_runner_names_are_ignored(string value)
    {
        // Act
        var runner = DotNetTasks.ParseTestRunner(value);

        // Assert
        runner.Should().BeNull();
    }

    private static void WriteGlobalJson(AbsolutePath directory, string content)
    {
        (directory / "global.json").WriteAllText(content);
    }
}
