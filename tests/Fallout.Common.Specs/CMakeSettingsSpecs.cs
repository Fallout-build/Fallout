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
    public void Generate_passes_the_preset_together_with_explicit_overrides()
    {
        // Arrange
        var settings = new CMakeGenerateSettings()
            .SetPreset("ninja")
            .SetGenerator(CMakeGenerator.Ninja_Multi_Config)
            .SetOutputDirectory("out/ninja")
            .AddDefine("MY_LIBRARY=lib");

        // Act
        var arguments = settings.GetArguments().JoinSpace();

        // Assert
        arguments.Should().Be("--preset ninja -G \"Ninja Multi-Config\" -B out/ninja -D MY_LIBRARY=lib");
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
    public void Build_passes_the_preset_instead_of_a_binary_directory()
    {
        // Arrange
        var settings = new CMakeBuildSettings()
            .SetPreset("ninja")
            .SetTarget("install");

        // Act
        var arguments = settings.GetArguments().JoinSpace();

        // Assert
        arguments.Should().Be("--build --preset ninja --target install");
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
