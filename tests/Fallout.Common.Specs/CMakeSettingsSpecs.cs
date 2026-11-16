using Fallout.Common.Tools.CMake;
using Fallout.Common.Utilities;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class CMakeSettingsSpecs
{
    [Fact]
    public void Generate_passes_nothing_that_the_build_did_not_set()
    {
        // Act
        var arguments = new CMakeGenerateSettings().GetArguments().JoinSpace();

        // Assert
        arguments.Should().BeEmpty();
    }

    [Fact]
    public void Build_passes_the_binary_directory_right_after_the_build_flag()
    {
        // Arrange
        var settings = new CMakeBuildSettings()
            .EnableVerbose()
            .SetConfiguration(CMakeConfiguration.Release)
            .SetOutputDirectory("out");

        // Act
        var arguments = settings.GetArguments().JoinSpace();

        // Assert
        arguments.Should().Be("--build out --verbose --config Release");
    }

    [Fact]
    public void Install_passes_the_binary_directory_right_after_the_install_flag()
    {
        // Arrange
        var settings = new CMakeInstallSettings()
            .SetConfiguration(CMakeConfiguration.Release)
            .SetRootDirectory("out");

        // Act
        var arguments = settings.GetArguments().JoinSpace();

        // Assert
        arguments.Should().Be("--install out --config Release");
    }
}
