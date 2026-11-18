using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class BuildAssemblyFileSpecs
{
    [Fact]
    public void An_entry_assembly_without_a_build_class_has_no_build_assembly_file()
    {
        // Arrange
        // A Microsoft.Testing.Platform test project is its own entry assembly, and it has no build class.
        var testProject = typeof(object).Assembly;

        // Act
        var buildAssemblyFile = FalloutBuild.GetBuildAssemblyFile(testProject, invokedLocation: "MyProject.Tests.dll");

        // Assert
        buildAssemblyFile.Should().BeNull();
    }

    [Fact]
    public void An_entry_assembly_with_a_build_class_is_the_build_assembly_file()
    {
        // Arrange
        var buildAssembly = typeof(BuildAssemblyFileSpecs).Assembly;

        // Act
        var buildAssemblyFile = FalloutBuild.GetBuildAssemblyFile(buildAssembly, invokedLocation: "other.dll");

        // Assert
        buildAssemblyFile.ToString().Should().Be(buildAssembly.Location);
    }
}
