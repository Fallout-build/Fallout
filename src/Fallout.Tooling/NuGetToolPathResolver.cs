using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Xml.Linq;
using Fallout.Common.Utilities;
using Fallout.Common.Utilities.Collections;
using Serilog;

namespace Fallout.Common.Tooling;

public static class NuGetToolPathResolver
{
    public static string EmbeddedPackagesDirectory;
    public static string NuGetPackagesConfigFile;
    public static string NuGetAssetsConfigFile;
    public static string PaketPackagesConfigFile;

    /// <summary>Resets the per-run package-location config so a subsequent build in the same process starts from defaults. FT-1 / #306.</summary>
    public static void Reset()
    {
        EmbeddedPackagesDirectory = null;
        NuGetPackagesConfigFile = null;
        NuGetAssetsConfigFile = null;
        PaketPackagesConfigFile = null;
    }

    public static string GetPackageExecutable(string packageId, string packageExecutable, string version = null,
        string framework = null)
    {
        Assert.True(packageId != null && packageExecutable != null);

        var packageDirectory = GetPackageDirectory(packageId.Split(new[]
        {
            '|'
        }, StringSplitOptions.RemoveEmptyEntries), version);

        var packageExecutables = packageExecutable.Split(new[]
        {
            '|'
        }, StringSplitOptions.RemoveEmptyEntries);
#if !NETSTANDARD2_0
        var enumerationOptions = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            MatchCasing = MatchCasing.CaseInsensitive
        };
#endif
        List<string> FindExecutables(string directory)
        {
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            {
                return new List<string>();
            }

            return packageExecutables
#if !NETSTANDARD2_0
                .SelectMany(x => Directory.GetFiles(directory, x, enumerationOptions))
#else
                .SelectMany(x => Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)
                    .Where(y => Path.GetFileName(y).EqualsOrdinalIgnoreCase(x)))
#endif
                .ToList();
        }

        var searchedDirectories = new List<string> { packageDirectory };
        var packageExecutablePaths = FindExecutables(packageDirectory);
        if (packageExecutablePaths.Count == 0)
        {
            foreach (var runtimeDirectory in FindRuntimeIdentifierPackageDirectories(packageDirectory))
            {
                searchedDirectories.Add(runtimeDirectory);
                packageExecutablePaths.AddRange(FindExecutables(runtimeDirectory));
            }
        }

        Assert.NotEmpty(packageExecutablePaths,
            $"Could not find {packageExecutables.Select(x => x.SingleQuote()).JoinCommaOr()} inside {searchedDirectories.Select(x => x.SingleQuote()).JoinCommaOr()}");

        if (packageExecutablePaths.Count == 1 && framework == null)
        {
            return packageExecutablePaths.Single();
        }

        // GitVersion 7 puts the executable in tools/<tfm>/<rid>/, for example tools/net10.0/win-x64/.
        // Older packages use tools/<tfm>/any/.
        static string GetFramework(string file)
        {
            var directory = new FileInfo(file).Directory.NotNull();
            if (directory.Name.EqualsOrdinalIgnoreCase("any"))
            {
                return directory.Parent.NotNull().Name;
            }

            var parent = directory.Parent;
            if (parent != null &&
                IsTargetFrameworkMoniker(parent.Name) &&
                !IsTargetFrameworkMoniker(directory.Name))
            {
                return parent.Name;
            }

            return directory.Name;
        }

        var frameworks = packageExecutablePaths.ToLookup(GetFramework, x => x, StringComparer.OrdinalIgnoreCase);
        var runtimeIdentifier = CurrentRuntimeIdentifier();

        // TODO: filter dll for .NET Framework
        string GetPackageExecutable(IEnumerable<string> executables)
            => executables
                .OrderByDescending(x => PathContainsDirectory(x, runtimeIdentifier))
                .ThenByDescending(x => x.EndsWithOrdinalIgnoreCase(".dll"))
                .ThenByDescending(x => EnvironmentInfo.IsWin && x.EndsWithOrdinalIgnoreCase(".exe"))
                .ThenByDescending(x => EnvironmentInfo.IsUnix && x.EndsWithOrdinalIgnoreCase(".sh")).ToList()
                .First();

        Assert.True(frameworks.Count > 0);
        if (frameworks.Count == 1)
        {
            return GetPackageExecutable(frameworks.Single());
        }

        framework ??= (Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()
            ?.FrameworkDisplayName?.Replace(".NET ", "net")).NotNull();

        var sortedFrameworks = frameworks.Select(x => x.Key)
            .OrderBy(x => x == framework)
            .ThenBy(x => x.Contains("."))
            .ThenBy(x => !x.StartsWith("netcore"))
            .ThenBy(x => x)
            .Reverse();

        return GetPackageExecutable(frameworks[sortedFrameworks.First()]);
    }

    private static string GetPackageDirectory(string[] packageIds, string version)
    {
        try
        {
            return packageIds
                .SelectMany(x =>
                    new Func<string>[]
                    {
                        () => EmbeddedPackagesDirectory != null
                            ? Path.Combine(EmbeddedPackagesDirectory, x)
                            : null,
                        () => NuGetAssetsConfigFile != null
                            ? NuGetPackageResolver.GetLocalInstalledPackage(x, NuGetAssetsConfigFile, version)?.Directory
                            : null,
                        () => NuGetPackagesConfigFile != null
                            ? NuGetPackageResolver.GetLocalInstalledPackage(x, NuGetPackagesConfigFile, version)?.Directory
                            : null,
                        () => PaketPackagesConfigFile != null
                            ? PaketPackageResolver.GetLocalInstalledPackageDirectory(x, PaketPackagesConfigFile)
                            : null
                    })
                .Select(x => x.Invoke())
                .FirstOrDefault(Directory.Exists)
                .NotNull(
                    new[]
                        {
                            $"Could not find package {packageIds.Select(x => x.SingleQuote()).JoinCommaOr()} " +
                            $"{(version != null ? $"({version}) " : string.Empty)}" +
                            "using:"
                        }
                        .Concat(
                            new[]
                            {
                                EmbeddedPackagesDirectory != null
                                    ? $"Embedded packages directory at '{EmbeddedPackagesDirectory}'"
                                    : null,
                                NuGetAssetsConfigFile != null
                                    ? $"Project assets file '{NuGetAssetsConfigFile}'"
                                    : null,
                                NuGetPackagesConfigFile != null
                                    ? $"NuGet packages config '{NuGetPackagesConfigFile}'"
                                    : null,
                                PaketPackagesConfigFile != null
                                    ? $"Paket packages config '{PaketPackagesConfigFile}'"
                                    : null
                            }.WhereNotNull().Select(x => $" - {x}")).JoinNewLine());
        }
        catch (Exception exception)
        {
            if (NuGetPackagesConfigFile != null && !NuGetPackagesConfigFile.EndsWithOrdinalIgnoreCase(".csproj"))
            {
                throw;
            }

            var packageCombinations =
                from packageId in packageIds
                from packageVersion in
                    new[]
                    {
                        NuGetVersionResolver.GetLatestVersion(packageId, includePrereleases: false).GetAwaiter().GetResult(),
                        NuGetVersionResolver.GetLatestVersion(packageId, includePrereleases: true).GetAwaiter().GetResult(),
                        NuGetPackageResolver.GetGlobalInstalledPackage(packageId, version: null, packagesConfigFile: null)
                            ?.Version.ToString()
                    }
                where packageVersion != null
                select (Id: packageId, Version: packageVersion);

            Assert.Fail(
                new[]
                    {
                        "Missing package reference/download.",
                        "Run one of the following commands:"
                    }.Concat(packageCombinations.Distinct().Select(x => $"  - fallout :add-package {x.Id} --version {x.Version}"))
                    .JoinNewLine(),
                exception);

            throw new Exception("Not reachable");
        }
    }

    private static bool IsTargetFrameworkMoniker(string name)
    {
        return name.StartsWith("net", StringComparison.OrdinalIgnoreCase);
    }

    private static bool PathContainsDirectory(string path, string directoryName)
    {
        return path.Split(new[]
            {
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar
            }, StringSplitOptions.RemoveEmptyEntries)
            .Any(x => x.EqualsOrdinalIgnoreCase(directoryName));
    }

    private static string CurrentRuntimeIdentifier()
    {
#if NET5_0_OR_GREATER
        return RuntimeInformation.RuntimeIdentifier;
#else
        var architecture = RuntimeInformation.OSArchitecture == Architecture.Arm64 ? "arm64" : "x64";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "win-" + architecture;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "osx-" + architecture;
        }

        return "linux-" + architecture;
#endif
    }

    /// <summary>
    /// GitVersion 7 publishes a metadata package plus one package per runtime identifier.
    /// The executable lives in the runtime package, under <c>tools/&lt;tfm&gt;/&lt;rid&gt;</c>.
    /// </summary>
    private static List<string> FindRuntimeIdentifierPackageDirectories(string packageDirectory)
    {
        var results = new List<string>();
        if (string.IsNullOrEmpty(packageDirectory) || !Directory.Exists(packageDirectory))
        {
            return results;
        }

        string settingsFile;
        try
        {
            settingsFile = Directory
                .EnumerateFiles(packageDirectory, "DotnetToolSettings.xml", SearchOption.AllDirectories)
                .FirstOrDefault(HasRuntimeIdentifierPackages);
        }
        catch (IOException)
        {
            return results;
        }
        catch (UnauthorizedAccessException)
        {
            return results;
        }

        if (settingsFile == null)
        {
            return results;
        }

        var document = XDocument.Load(settingsFile);
        var runtimeIdentifier = CurrentRuntimeIdentifier();
        var package = document.Descendants("RuntimeIdentifierPackage")
            .Select(x => new
            {
                RuntimeIdentifier = (string)x.Attribute("RuntimeIdentifier"),
                Id = (string)x.Attribute("Id")
            })
            .FirstOrDefault(x =>
                !string.IsNullOrEmpty(x.Id) &&
                !string.IsNullOrEmpty(x.RuntimeIdentifier) &&
                x.RuntimeIdentifier.EqualsOrdinalIgnoreCase(runtimeIdentifier));
        if (package == null)
        {
            return results;
        }

        var packageRoot = GetToolPackageRoot(settingsFile);
        if (packageRoot?.Parent?.Parent == null)
        {
            return results;
        }

        var version = packageRoot.Name;
        var packageId = packageRoot.Parent.Name;
        var runtimePackageId = package.Id.ToLowerInvariant();
        var candidates = new List<string>
        {
            Path.Combine(packageRoot.Parent.Parent.FullName, runtimePackageId, version)
        };

        foreach (var store in GetDotnetToolStores())
        {
            candidates.Add(Path.Combine(store, packageId, version, runtimePackageId, version));
        }

        foreach (var candidate in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (Directory.Exists(candidate))
            {
                results.Add(candidate);
            }
        }

        if (results.Count == 0)
        {
            var downloaded = TryDownloadRuntimeIdentifierPackage(packageRoot, package.Id, version);
            if (downloaded != null)
            {
                results.Add(downloaded);
            }
        }

        return results;
    }

    private static bool HasRuntimeIdentifierPackages(string settingsFile)
    {
        try
        {
            return XDocument.Load(settingsFile).Descendants("RuntimeIdentifierPackage").Any();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Xml.XmlException)
        {
            return false;
        }
    }

    private static DirectoryInfo GetToolPackageRoot(string settingsFile)
    {
        var directory = new FileInfo(settingsFile).Directory;
        while (directory != null && !directory.Name.EqualsOrdinalIgnoreCase("tools"))
        {
            directory = directory.Parent;
        }

        return directory?.Parent;
    }

    private static IEnumerable<string> GetDotnetToolStores()
    {
        var cliHome = Environment.GetEnvironmentVariable("DOTNET_CLI_HOME");
        if (!string.IsNullOrEmpty(cliHome))
        {
            yield return Path.Combine(cliHome, "tools", ".store");
        }

        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (!string.IsNullOrEmpty(home))
        {
            yield return Path.Combine(home, ".dotnet", "tools", ".store");
        }
    }

    private static string TryDownloadRuntimeIdentifierPackage(DirectoryInfo packageRoot, string packageId, string version)
    {
        var globalPackages = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".nuget",
            "packages");
        if (!packageRoot.FullName.StartsWith(globalPackages, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (version.IndexOfAny("0123456789".ToCharArray()) < 0)
        {
            return null;
        }

        var target = Path.Combine(globalPackages, packageId.ToLowerInvariant(), version);
        if (Directory.Exists(target))
        {
            return target;
        }

        var temp = Path.Combine(Path.GetTempPath(), "fallout-tool-" + Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(temp);
            var projectFile = Path.Combine(temp, "tool.csproj");
            File.WriteAllText(projectFile,
                "<Project Sdk=\"Microsoft.NET.Sdk\">\n" +
                "  <PropertyGroup>\n" +
                "    <TargetFramework>net10.0</TargetFramework>\n" +
                "  </PropertyGroup>\n" +
                "  <ItemGroup>\n" +
                $"    <PackageDownload Include=\"{packageId}\" Version=\"[{version}]\" />\n" +
                "  </ItemGroup>\n" +
                "</Project>\n");

            Log.Information("Installing {PackageId} ({Version}) for the current runtime...", packageId, version);
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"restore \"{projectFile}\" --nologo --verbosity quiet",
                WorkingDirectory = temp,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            if (process == null)
            {
                return null;
            }

            if (!process.WaitForExit(120000))
            {
                try
                {
                    process.Kill();
                }
                catch (Exception)
                {
                    // The restore process already exited.
                }

                return null;
            }

            if (process.ExitCode != 0)
            {
                return null;
            }
        }
        catch (Exception exception)
        {
            Log.Warning(exception, "Could not install {PackageId} ({Version})", packageId, version);
            return null;
        }
        finally
        {
            try
            {
                if (Directory.Exists(temp))
                {
                    Directory.Delete(temp, recursive: true);
                }
            }
            catch (Exception)
            {
                // Leaving the temp project behind does not affect the tool path.
            }
        }

        return Directory.Exists(target) ? target : null;
    }
}
