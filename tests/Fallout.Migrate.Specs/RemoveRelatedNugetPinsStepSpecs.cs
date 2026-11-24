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
        var project = tempDirectory / "_build.csproj";
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
        var project = tempDirectory / "_build.csproj";
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

    [Fact]
    public async Task Central_related_nuget_pin_is_removed_for_v11()
    {
        // Arrange
        var props = tempDirectory / "Directory.Packages.props";
        props.WriteAllText(
            """
            <Project>
              <ItemGroup>
                <PackageVersion Include="NuGet.Protocol" Version="7.9.0" />
                <PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
              </ItemGroup>
            </Project>
            """, eofLineBreak: false);

        var project = tempDirectory / "_build.csproj";
        project.WriteAllText(
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="NuGet.Protocol" />
                <PackageReference Include="Newtonsoft.Json" />
              </ItemGroup>
            </Project>
            """, eofLineBreak: false);
        var context = CreateContext("11.0.0");

        // Act
        await new RemoveRelatedNugetPinsStep().ExecuteAsync(context, summary);

        // Assert
        props.ReadAllText().Should().Be(
            """
            <Project>
              <ItemGroup>
                <PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
              </ItemGroup>
            </Project>
            """);
        project.ReadAllText().Should().Be(
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="Newtonsoft.Json" />
              </ItemGroup>
            </Project>
            """);
        summary.FilesChanged.Should().Be(2);
        summary.EditCount.Should().Be(2);
    }

    [Fact]
    public async Task Central_pin_is_kept_for_a_package_another_project_references()
    {
        // Arrange
        var props = tempDirectory / "Directory.Packages.props";
        props.WriteAllText(
            """
            <Project>
              <ItemGroup>
                <PackageVersion Include="NuGet.Protocol" Version="7.9.0" />
                <PackageVersion Include="NuGet.Packaging" Version="7.9.0" />
              </ItemGroup>
            </Project>
            """, eofLineBreak: false);

        var library = tempDirectory / "Library.csproj";
        const string libraryInput =
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="NuGet.Packaging" />
              </ItemGroup>
            </Project>
            """;
        library.WriteAllText(libraryInput, eofLineBreak: false);
        var context = CreateContext("11.0.0");

        // Act
        await new RemoveRelatedNugetPinsStep().ExecuteAsync(context, summary);

        // Assert
        props.ReadAllText().Should().Be(
            """
            <Project>
              <ItemGroup>
                <PackageVersion Include="NuGet.Packaging" Version="7.9.0" />
              </ItemGroup>
            </Project>
            """);
        library.ReadAllText().Should().Be(libraryInput);
        summary.FilesChanged.Should().Be(1);
        summary.EditCount.Should().Be(1);
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