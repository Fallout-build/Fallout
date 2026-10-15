using System;
using System.IO;
using System.Threading.Tasks;
using Fallout.Common.IO;
using Fallout.Migrate.Common;
using Fallout.Migrate.Steps;
using FluentAssertions;
using Xunit;

namespace Fallout.Migrate.Specs;

public class RemoveNugetFrameworkPinStepSpecs : IDisposable
{
    private const string TestFalloutVersion = "10.5.0";

    private readonly AbsolutePath tempDirectory;
    private readonly MigrationContext context;
    private readonly Summary summary = new();

    public RemoveNugetFrameworkPinStepSpecs()
    {
        tempDirectory = AbsolutePath.Temp("fallout-remove-nuget-framework-pin");
        context = new MigrationContext(tempDirectory, dryRun: false, TextWriter.Null)
        {
            FalloutVersion = TestFalloutVersion
        };
    }

    [Fact]
    public async Task Explicit_nuget_framework_pin_is_removed()
    {
        // Arrange
        var project = tempDirectory / "Library.csproj";
        project.WriteAllText(
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Version="7.9.0" PrivateAssets="all" Include="NuGet.Frameworks" />
                <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
              </ItemGroup>
            </Project>
            """, eofLineBreak: false);

        // Act
        await new RemoveNugetFrameworkPinStep().ExecuteAsync(context, summary);

        // Assert
        project.ReadAllText().Should().Be(
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
              </ItemGroup>
            </Project>
            """);

        summary.FilesChanged.Should().Be(1);
        summary.EditCount.Should().Be(1);
    }

    [Fact]
    public async Task Explicit_nuget_framework_pin_is_left_alone_for_version_prior_10_5()
    {
        // Arrange
        var project = tempDirectory / "_build.csproj";
        var input =
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Version="7.9.0" PrivateAssets="all" Include="NuGet.Frameworks" />
                <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
              </ItemGroup>
            </Project>
            """;
        project.WriteAllText(input, eofLineBreak: false);
        context.FalloutVersion = "10.4.0";

        // Act
        await new RemoveNugetFrameworkPinStep().ExecuteAsync(context, summary);

        // Assert
        project.ReadAllText().Should().Be(input);

        summary.FilesChanged.Should().Be(0);
        summary.EditCount.Should().Be(0);
    }


    [Fact]
    public async Task Nuget_framework_reference_without_version_is_left_unchanged()
    {
        // Arrange
        var project = tempDirectory / "Library.csproj";
        const string input =
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="NuGet.Frameworks" />
              </ItemGroup>
            </Project>
            """;

        project.WriteAllText(input, eofLineBreak: false);

        // Act
        await new RemoveNugetFrameworkPinStep().ExecuteAsync(context, summary);

        // Assert
        project.ReadAllText().Should().Be(input);
        summary.EditCount.Should().Be(0);
    }

    [Fact]
    public async Task Nuget_framework_reference_is_left_unchanged_in_non_build_projects()
    {
        // Arrange
        var project = tempDirectory / "Library.csproj";
        const string input =
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <ItemGroup>
                <PackageReference Include="NuGet.Frameworks" Version="7.9.0"/>
              </ItemGroup>
            </Project>
            """;

        project.WriteAllText(input, eofLineBreak: false);

        // Act
        await new RemoveNugetFrameworkPinStep().ExecuteAsync(context, summary);

        // Assert
        project.ReadAllText().Should().Be(input);
        summary.EditCount.Should().Be(0);
    }

    [Fact]
    public async Task Missing_nuget_framework_pin_is_left_unchanged()
    {
        // Arrange
        var project = tempDirectory / "build" / "_build.csproj";
        const string input =
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
            </Project>
            """;

        project.WriteAllText(input, eofLineBreak: false);

        // Act
        await new RemoveNugetFrameworkPinStep().ExecuteAsync(context, summary);

        // Assert
        project.ReadAllText().Should().Be(input);
        summary.EditCount.Should().Be(0);
    }

    public void Dispose()
    {
        tempDirectory.DeleteDirectory();
    }
}
