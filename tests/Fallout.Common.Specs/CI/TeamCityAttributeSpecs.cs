using System;
using System.IO;
using Fallout.Common.CI.TeamCity;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs.CI;

public sealed class TeamCityAttributeSpecs : IDisposable
{
    private readonly AbsolutePath root = AbsolutePath.Temp("teamcity-line-breaks").CreateDirectory();

    private AbsolutePath PomFile => root / ".teamcity" / "pom.xml";

    public void Dispose()
    {
        root.DeleteDirectory();
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Regenerating_keeps_the_line_breaks_of_the_existing_pom_file(string lineBreak)
    {
        // Arrange
        PomFile.Parent.CreateDirectory();
        File.WriteAllText(PomFile, $"<project>{lineBreak}</project>{lineBreak}");
        var sut = new TeamCityAttribute { Build = new RootedBuild(root) };

        // Act
        sut.Generate(Array.Empty<ExecutableTarget>());

        // Assert
        var text = File.ReadAllText(PomFile);
        text.Should().Contain("<modelVersion>").And.Be(text.ReplaceLineEndings(lineBreak));
    }

    private sealed class RootedBuild(AbsolutePath root) : FalloutBuild, IFalloutBuild
    {
        AbsolutePath IFalloutBuild.RootDirectory => root;
    }
}
