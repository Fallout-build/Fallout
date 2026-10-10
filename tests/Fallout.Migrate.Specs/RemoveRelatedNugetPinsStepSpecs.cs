using System;
using System.IO;
using System.Threading.Tasks;
using Fallout.Common.IO;
using Fallout.Migrate.Common;
using Fallout.Migrate.Steps;
using FluentAssertions;
using Xunit;

namespace Fallout.Migrate.Specs;

public class RemoveRelatedNugetPinsStepSpecs : IDisposable
{
    private readonly AbsolutePath tempDirectory;
    private readonly Summary summary = new();

    public RemoveRelatedNugetPinsStepSpecs()
    {
        tempDirectory = AbsolutePath.Temp("fallout-remove-related-nuget-pins");
    }

    [Theory]
    [InlineData("NuGet.Protocol", "11.0.0")]
    [InlineData("NuGet.Packaging", "11.0.0-preview.1")]
    [InlineData("NuGet.Resolver", "12.0.0")]
    public async Task Explicit_related_nuget_pin_is_removed_for_v11_or_later(
        string packageName,
        string falloutVersion)
    {
        // Arrange
        var project = tempDirectory / "Library.csproj";
        project.WriteAllText(
            $"""
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="{packageName}" Version="7.9.0" />
              </ItemGroup>
            </Project>
            """, eofLineBreak: false);
        var context = CreateContext(falloutVersion);

        // Act
        await new RemoveRelatedNugetPinsStep().ExecuteAsync(context, summary);

        // Assert
        project.ReadAllText().Should().Be(
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
              </ItemGroup>
            </Project>
            """);
        summary.FilesChanged.Should().Be(1);
        summary.EditCount.Should().Be(1);
    }

    [Theory]
    [InlineData("10.9.0")]
    [InlineData(null)]
    public async Task Explicit_related_nuget_pin_is_left_unchanged_before_v11(string falloutVersion)
    {
        // Arrange
        var project = tempDirectory / "Library.csproj";
        const string input =
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="NuGet.Protocol" Version="7.9.0" />
              </ItemGroup>
            </Project>
            """;

        project.WriteAllText(input, eofLineBreak: false);
        var context = CreateContext(falloutVersion);

        // Act
        await new RemoveRelatedNugetPinsStep().ExecuteAsync(context, summary);

        // Assert
        project.ReadAllText().Should().Be(input);
        summary.EditCount.Should().Be(0);
    }

    private MigrationContext CreateContext(string falloutVersion) =>
        new(tempDirectory, dryRun: false, TextWriter.Null)
        {
            FalloutVersion = falloutVersion
        };

    public void Dispose()
    {
        tempDirectory.DeleteDirectory();
    }
}