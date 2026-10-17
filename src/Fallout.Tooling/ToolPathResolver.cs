using System;
using System.IO;
using System.Linq;
using Fallout.Common.Utilities;

namespace Fallout.Common.Tooling;

public static class ToolPathResolver
{
    public static string TryGetEnvironmentExecutable(string environmentExecutable)
    {
        var environmentExecutablePath = EnvironmentInfo.GetVariable(environmentExecutable);
        if (environmentExecutablePath == null)
        {
            return null;
        }

        Assert.FileExists(environmentExecutablePath,
            $"Path '{environmentExecutablePath}' from environment variable '{environmentExecutable}' does not exist");

        return environmentExecutablePath;
    }

    public static string GetPathExecutable(string pathExecutable)
    {
        return TryGetPathExecutable(pathExecutable)
            .NotNull($"Could not find '{pathExecutable}' on the PATH.");
    }

    /// <summary>
    /// Returns the full path of <paramref name="pathExecutable"/> when it exists as a file or
    /// is found on the PATH (via <c>which</c> or <c>where</c>), otherwise <c>null</c>.
    /// </summary>
    public static string TryGetPathExecutable(string pathExecutable)
    {
        if (File.Exists(pathExecutable))
        {
            return Path.GetFullPath(pathExecutable);
        }

        var locateExecutable = EnvironmentInfo.IsWin
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "where.exe")
            : "/usr/bin/which";

        if (!File.Exists(locateExecutable))
        {
            string GetExecutableFullPath(string path)
                => Path.Combine(path,
                    Path.GetExtension(pathExecutable).IsNullOrEmpty() && EnvironmentInfo.IsWin
                        ? $"{pathExecutable}.exe"
                        : pathExecutable);

            var environmentVariable = Environment.GetEnvironmentVariable("PATH").NotNullOrEmpty("PATH variable not available");
            var paths = environmentVariable.Split(new[]
            {
                Path.PathSeparator
            }, StringSplitOptions.RemoveEmptyEntries);

            return paths.Select(GetExecutableFullPath).FirstOrDefault(File.Exists);
        }

        var locateProcess = ProcessTasks.StartProcess(
            locateExecutable,
            pathExecutable,
            logOutput: false,
            logInvocation: false);

        locateProcess.AssertWaitForExit();

        return locateProcess.Output
            .Select(x => x.Text)
            .Where(x => (EnvironmentInfo.IsWin && Path.HasExtension(x)) || EnvironmentInfo.IsUnix)
            .FirstOrDefault(File.Exists);
    }
}
