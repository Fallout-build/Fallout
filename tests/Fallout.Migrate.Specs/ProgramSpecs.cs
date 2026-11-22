using System;
using System.IO;
using System.Threading.Tasks;
using Fallout.Common;
using Fallout.Common.IO;
using FluentAssertions;
using Xunit;

namespace Fallout.Migrate.Specs;

public class ProgramSpecs : IDisposable
{
    private const string OriginalCsproj =
        """
        <Project Sdk="Microsoft.NET.Sdk">
          <ItemGroup>
            <PackageReference Include="Nuke.Common" Version="9.0.0" />
          </ItemGroup>
        </Project>
        """;

    private readonly AbsolutePath tempDirectory;
    private readonly AbsolutePath csproj;
    private readonly string originalCsproj;

    public ProgramSpecs()
    {
        // Arrange
        tempDirectory = AbsolutePath.Temp("fallout-migrate-program-test");
        (tempDirectory / "build").CreateDirectory();
        (tempDirectory / ".nuke").CreateDirectory();
        csproj = tempDirectory / "build" / "_build.csproj";
        csproj.WriteAllText(OriginalCsproj);
        originalCsproj = csproj.ReadAllText();
    }

    [Fact]
    public async Task Unknown_option_fails_and_changes_nothing()
    {
        // Act
        var (exitCode, _) = await Run("--bogus-flag");

        // Assert
        exitCode.Should().NotBe(0);
        AssertNothingMigrated();
    }

    [Fact]
    public async Task Version_option_prints_version_and_changes_nothing()
    {
        // Act
        var (exitCode, output) = await Run("--version");

        // Assert
        exitCode.Should().Be(0, output);
        output.Trim().Should().NotBeEmpty();
        AssertNothingMigrated();
    }

    // No path argument: the tool then migrates the working directory, which is the case in the issue.
    private async Task<(int ExitCode, string Output)> Run(string option)
    {
        var originalOut = Console.Out;
        var writer = new StringWriter();
        Console.SetOut(writer);
        using var _ = tempDirectory.SwitchWorkingDirectory();
        try
        {
            var exitCode = await Program.CreateApp().RunAsync([option]);
            return (exitCode, writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    private void AssertNothingMigrated()
    {
        csproj.ReadAllText().Should().Be(originalCsproj);
        (tempDirectory / ".nuke").DirectoryExists().Should().BeTrue();
        (tempDirectory / ".fallout").DirectoryExists().Should().BeFalse();
    }

    public void Dispose()
    {
        tempDirectory.DeleteDirectory();
    }
}
