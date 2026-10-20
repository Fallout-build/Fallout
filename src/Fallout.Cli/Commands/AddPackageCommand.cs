using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fallout.Common;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.DotNet;
using Fallout.Solutions;
using Microsoft.Build.Evaluation;

namespace Fallout.Cli.Commands;

/// <summary>
/// <c>fallout :add-package</c>: adds (or upgrades) a NuGet package reference in the build project.
/// </summary>
internal sealed class AddPackageCommand(IPackageManager packages) : IFalloutCommand
{
    public string Name => "add-package";

    public async Task<int> ExecuteAsync(string[] args, AbsolutePath rootDirectory, AbsolutePath buildScript)
    {
        ToolBanner.Print();
        Logging.Configure();
        ProjectModelTasks.Initialize();

        var packageId = args.ElementAt(0);
        var packageVersion =
            (EnvironmentInfo.GetNamedArgument<string>("version") ??
             args.ElementAtOrDefault(1) ??
             await NuGetVersionResolver.GetLatestVersion(packageId, includePrereleases: false) ??
             NuGetPackageResolver.GetGlobalInstalledPackage(packageId, version: null, packagesConfigFile: null)?.Version
                 .ToString())
            .NotNull("packageVersion != null");

        var buildProjectFile = FindBuildProject(rootDirectory);
        Host.Information($"Installing {packageId}/{packageVersion} to {buildProjectFile} ...");
        packages.AddOrReplacePackage(packageId, packageVersion, PackageManager.DownloadType, buildProjectFile);
        DotNetTasks.DotNet($"restore {buildProjectFile}");

        var installedPackage = NuGetPackageResolver.GetGlobalInstalledPackage(packageId, packageVersion, packagesConfigFile: null)
            .NotNull("installedPackage != null");

        var hasToolsDirectory = installedPackage.Directory.GlobDirectories("tools").Any();
        if (!hasToolsDirectory)
        {
            packages.AddOrReplacePackage(packageId, packageVersion, PackageManager.ReferenceType, buildProjectFile);
        }

        Host.Information($"Done installing {packageId}/{packageVersion} to {buildProjectFile}");
        return 0;
    }

    internal static AbsolutePath FindBuildProject(AbsolutePath rootDirectory)
    {
        var buildProject = rootDirectory.GlobFiles("**/*.csproj")
            .Where(x => HasMatchingRootDirectory(x, rootDirectory))
            .OrderBy(x => x.ToString().Length)
            .FirstOrDefault();

        Assert.True(buildProject != null,
            $"Could not find a build project with a FalloutRootDirectory property pointing to '{rootDirectory}'.");

        return buildProject;
    }

    private static bool HasMatchingRootDirectory(AbsolutePath projectFile, AbsolutePath rootDirectory)
    {
        ProjectProperty rootDirectoryProperty;
        try
        {
            rootDirectoryProperty = ProjectModelTasks.ParseProject(projectFile).NotNull()
                .GetProperty("FalloutRootDirectory");
        }
        catch
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(rootDirectoryProperty?.EvaluatedValue))
        {
            return false;
        }

        var configuredRootDirectory = rootDirectoryProperty.EvaluatedValue;
        var resolvedRootDirectory = Path.IsPathRooted(configuredRootDirectory)
            ? (AbsolutePath)configuredRootDirectory
            : projectFile.Parent / configuredRootDirectory;

        return resolvedRootDirectory == rootDirectory;
    }
}
