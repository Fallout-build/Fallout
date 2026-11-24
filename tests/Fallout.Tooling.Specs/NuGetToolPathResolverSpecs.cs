using System;
using System.IO;
using System.Runtime.InteropServices;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

[Collection(ToolPathResolverStateCollection.Name)]
public class NuGetToolPathResolverSpecs : IDisposable
{
    private readonly string _embeddedPackagesDirectory = NuGetToolPathResolver.EmbeddedPackagesDirectory;
    private readonly string _nuGetPackagesConfigFile = NuGetToolPathResolver.NuGetPackagesConfigFile;
    private readonly string _nuGetAssetsConfigFile = NuGetToolPathResolver.NuGetAssetsConfigFile;
    private readonly string _paketPackagesConfigFile = NuGetToolPathResolver.PaketPackagesConfigFile;
    private readonly AbsolutePath _root = AbsolutePath.Temp("fallout-nuget-tool-path");

    public NuGetToolPathResolverSpecs()
    {
        NuGetToolPathResolver.EmbeddedPackagesDirectory = _root;
        NuGetToolPathResolver.NuGetPackagesConfigFile = null;
        NuGetToolPathResolver.NuGetAssetsConfigFile = null;
        NuGetToolPathResolver.PaketPackagesConfigFile = null;
    }

    [Fact]
    public void GitVersion_7_runtime_package_resolves_the_requested_framework()
    {
        // Arrange
        var runtimeIdentifier = RuntimeInformation.RuntimeIdentifier;
        var runtimePackageId = "gitversion.tool." + runtimeIdentifier;
        Write(
            _root / "gitversion.tool" / "7.0.0" / "tools" / "any" / "any" / "DotnetToolSettings.xml",
            $"""
             <?xml version="1.0" encoding="utf-8"?>
             <DotNetCliTool Version="2">
               <Commands>
                 <Command Name="dotnet-gitversion" EntryPoint="gitversion.dll" Runner="dotnet" />
               </Commands>
               <RuntimeIdentifierPackages>
                 <RuntimeIdentifierPackage RuntimeIdentifier="{runtimeIdentifier}" Id="GitVersion.Tool.{runtimeIdentifier}" />
               </RuntimeIdentifierPackages>
             </DotNetCliTool>
             """);
        Write(_root / runtimePackageId / "7.0.0" / "tools" / "net10.0" / runtimeIdentifier / "gitversion.dll", "");
        Write(_root / runtimePackageId / "7.0.0" / "tools" / "net11.0" / runtimeIdentifier / "gitversion.dll", "");

        // Act
        var path = NuGetToolPathResolver.GetPackageExecutable("gitversion.tool", "gitversion.dll", framework: "net10.0");

        // Assert
        path.Should().StartWith(_root);
        path.Should().Contain(Path.Combine("net10.0", runtimeIdentifier));
        path.Should().NotContain("net11.0");
        path.Should().EndWith("gitversion.dll");
    }

    [Fact]
    public void GitVersion_7_executable_is_found_in_the_dotnet_tool_store()
    {
        // Arrange
        var runtimeIdentifier = RuntimeInformation.RuntimeIdentifier;
        var previousCliHome = Environment.GetEnvironmentVariable("DOTNET_CLI_HOME");
        var cliHome = _root / "cli-home";
        Environment.SetEnvironmentVariable("DOTNET_CLI_HOME", cliHome);
        try
        {
            Write(
                _root / "gitversion.tool" / "7.0.0" / "tools" / "any" / "any" / "DotnetToolSettings.xml",
                $"""
                 <?xml version="1.0" encoding="utf-8"?>
                 <DotNetCliTool Version="2">
                   <Commands>
                     <Command Name="dotnet-gitversion" EntryPoint="gitversion.exe" Runner="executable" />
                   </Commands>
                   <RuntimeIdentifierPackages>
                     <RuntimeIdentifierPackage RuntimeIdentifier="{runtimeIdentifier}" Id="GitVersion.Tool.{runtimeIdentifier}" />
                   </RuntimeIdentifierPackages>
                 </DotNetCliTool>
                 """);
            var storePackage = cliHome / ".dotnet" / "tools" / ".store" / "gitversion.tool" / "7.0.0" /
                               ("gitversion.tool." + runtimeIdentifier) / "7.0.0";
            Write(storePackage / "tools" / "net10.0" / runtimeIdentifier / "gitversion.dll", "");
            Write(storePackage / "tools" / "net11.0" / runtimeIdentifier / "gitversion.dll", "");

            // Act
            var path = NuGetToolPathResolver.GetPackageExecutable(
                "gitversion.tool",
                "gitversion.dll",
                framework: "net10.0");

            // Assert
            path.Should().StartWith(cliHome);
            path.Should().Contain(Path.Combine("tools", "net10.0", runtimeIdentifier));
            path.Should().NotContain("net11.0");
        }
        finally
        {
            Environment.SetEnvironmentVariable("DOTNET_CLI_HOME", previousCliHome);
        }
    }

    [Fact]
    public void Framework_folder_is_read_from_the_parent_of_a_runtime_identifier_folder()
    {
        // Arrange
        Write(_root / "sample.tool" / "tools" / "net8.0" / "any" / "sample.dll", "");
        Write(_root / "sample.tool" / "tools" / "net10.0" / "win-x64" / "sample.dll", "");
        Write(_root / "sample.tool" / "tools" / "net11.0" / "win-x64" / "sample.dll", "");

        // Act
        var net8 = NuGetToolPathResolver.GetPackageExecutable("sample.tool", "sample.dll", framework: "net8.0");
        var net10 = NuGetToolPathResolver.GetPackageExecutable("sample.tool", "sample.dll", framework: "net10.0");

        // Assert
        net8.Should().Contain(Path.Combine("net8.0", "any"));
        net10.Should().Contain(Path.Combine("net10.0", "win-x64"));
        net10.Should().NotContain("net11.0");
    }

    [Fact]
    public void Current_runtime_is_chosen_when_another_runtime_sits_beside_it()
    {
        // Arrange
        var runtimeIdentifier = RuntimeInformation.RuntimeIdentifier;
        var otherRuntime = runtimeIdentifier.Equals("win-x64", StringComparison.OrdinalIgnoreCase)
            ? "linux-x64"
            : "win-x64";
        Write(_root / "sample.tool" / "tools" / "net10.0" / otherRuntime / "sample.dll", "");
        Write(_root / "sample.tool" / "tools" / "net10.0" / runtimeIdentifier / "sample.dll", "");

        // Act
        var path = NuGetToolPathResolver.GetPackageExecutable("sample.tool", "sample.dll", framework: "net10.0");

        // Assert
        path.Should().Contain(Path.Combine("net10.0", runtimeIdentifier));
        path.Should().NotContain(otherRuntime);
    }

    [Fact]
    public void Distro_runtime_falls_back_to_the_portable_linux_package()
    {
        // Arrange
        var available = new[]
        {
            new NuGetToolPathResolver.RuntimeIdentifierPackage("linux-x64", "GitVersion.Tool.linux-x64"),
            new NuGetToolPathResolver.RuntimeIdentifierPackage("win-x64", "GitVersion.Tool.win-x64")
        };

        // Act
        var match = NuGetToolPathResolver.MatchRuntimeIdentifier(available, "ubuntu.24.04-x64");

        // Assert
        match.Should().NotBeNull();
        match.Value.Id.Should().Be("GitVersion.Tool.linux-x64");
    }

    public void Dispose()
    {
        NuGetToolPathResolver.EmbeddedPackagesDirectory = _embeddedPackagesDirectory;
        NuGetToolPathResolver.NuGetPackagesConfigFile = _nuGetPackagesConfigFile;
        NuGetToolPathResolver.NuGetAssetsConfigFile = _nuGetAssetsConfigFile;
        NuGetToolPathResolver.PaketPackagesConfigFile = _paketPackagesConfigFile;
        _root.DeleteDirectory();
    }

    private static void Write(AbsolutePath path, string content)
    {
        Directory.CreateDirectory(path.Parent);
        File.WriteAllText(path, content);
    }
}
