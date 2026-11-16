using Fallout.Common.Tools.CMake;
using Fallout.Common.Tools.CTest;
using Fallout.Common.Utilities;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class CTestSettingsSpecs
{
    [Fact]
    public void Passes_nothing_that_the_build_did_not_set()
    {
        // Act
        var arguments = new CTestSettings().GetArguments().JoinSpace();

        // Assert
        arguments.Should().BeEmpty();
    }

    [Fact]
    public void Passes_the_preset_together_with_explicit_overrides()
    {
        // Arrange
        var settings = new CTestSettings()
            .SetPreset("ninja")
            .SetConfiguration(CMakeConfiguration.Release)
            .SetParallel(4)
            .EnableOutputOnFailure();

        // Act
        var arguments = settings.GetArguments().JoinSpace();

        // Assert
        arguments.Should().Be("--preset ninja --build-config Release --parallel 4 --output-on-failure");
    }

    [Fact]
    public void Passes_each_label_regex_as_a_separate_option()
    {
        // Arrange
        var settings = new CTestSettings()
            .SetTestDirectory("out")
            .AddLabelRegexes("fast", "unit");

        // Act
        var arguments = settings.GetArguments().JoinSpace();

        // Assert
        arguments.Should().Be("--test-dir out --label-regex fast --label-regex unit");
    }

    [Fact]
    public void Passes_the_no_tests_action_joined_to_its_option()
    {
        // Arrange
        var settings = new CTestSettings()
            .SetNoTests(CTestNoTestsAction.error);

        // Act
        var arguments = settings.GetArguments().JoinSpace();

        // Assert
        arguments.Should().Be("--no-tests=error");
    }
}
