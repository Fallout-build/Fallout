using Fallout.Common;
using Fallout.Common.IO;
using FluentAssertions;
using Xunit;

namespace Fallout.Components.Specs;

/// <summary>
/// This project has no build class, like the test project of a library of build components. Microsoft.Testing.Platform
/// runs it as its own entry assembly.
/// </summary>
public class TestHostSpecs
{
    [Fact]
    public void A_test_project_without_a_build_class_has_no_build_assembly()
    {
        // Act
        AbsolutePath buildAssemblyFile = FalloutBuild.BuildAssemblyFile;

        // Assert
        buildAssemblyFile.Should().BeNull();
    }
}
