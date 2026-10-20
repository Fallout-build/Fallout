using System;
using System.IO;
using Fallout.Cli.Commands;
using Fallout.Common.IO;
using FluentAssertions;
using Xunit;

namespace Fallout.Cli.Specs.Commands;

public class AddPackageCommandSpecs
{
    [Fact]
    public void Project_with_root_directory_property_is_selected()
    {
        // Arrange
        using var root = TempRoot.Create();
        root.WriteProject("src/app.csproj", "<Project />");
        var expected = root.WriteProject("build/custom.csproj",
            "<Project><PropertyGroup><FalloutRootDirectory>..</FalloutRootDirectory></PropertyGroup></Project>");

        // Act
        var actual = AddPackageCommand.FindBuildProject(root.Path);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Missing_root_directory_property_gives_clear_error()
    {
        // Arrange
        using var root = TempRoot.Create();
        root.WriteProject("build/build.csproj",
            "<Project><!-- <FalloutRootDirectory>..</FalloutRootDirectory> --></Project>");

        // Act
        var action = () => AddPackageCommand.FindBuildProject(root.Path);

        // Assert
        action.Should().Throw<Exception>()
            .WithMessage("*Could not find a build project*FalloutRootDirectory*");
    }

    [Fact]
    public void Project_with_shortest_path_is_selected()
    {
        // Arrange
        using var root = TempRoot.Create();
        root.WriteProject("build/nested/first.csproj",
            "<Project><PropertyGroup><FalloutRootDirectory>../..</FalloutRootDirectory></PropertyGroup></Project>");
        var expected = root.WriteProject("build/second.csproj",
            "<Project><PropertyGroup><FalloutRootDirectory>..</FalloutRootDirectory></PropertyGroup></Project>");

        // Act
        var actual = AddPackageCommand.FindBuildProject(root.Path);

        // Assert
        actual.Should().Be(expected);
    }

    private sealed class TempRoot : IDisposable
    {
        public AbsolutePath Path { get; }

        private TempRoot(AbsolutePath path) => Path = path;

        public static TempRoot Create()
        {
            var path = (AbsolutePath)System.IO.Path.Combine(
                System.IO.Path.GetTempPath(), "fallout-add-package-" + Guid.NewGuid().ToString("N"));
            path.CreateDirectory();
            return new TempRoot(path);
        }

        public AbsolutePath WriteProject(string relativePath, string content)
        {
            var project = Path / relativePath;
            project.Parent.CreateDirectory();
            File.WriteAllText(project, content);
            return project;
        }

        public void Dispose()
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path, recursive: true);
            }
        }
    }
}
