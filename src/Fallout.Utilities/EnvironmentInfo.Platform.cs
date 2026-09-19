using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Fallout.Common.Utilities;

namespace Fallout.Common;

public enum PlatformFamily
{
    Unknown,
    Windows,
    Linux,
    OSX
}

partial class EnvironmentInfo
{
    /// <summary>
    /// Indicates whether the operating-system is arm64.
    /// </summary>
    public static bool IsArm64 => RuntimeInformation.OSArchitecture == Architecture.Arm64;

    /// <summary>
    /// Indicates whether the operating-system is 64bit.
    /// </summary>
    public static bool Is64Bit => RuntimeInformation.OSArchitecture == Architecture.X64 ||
                                  RuntimeInformation.OSArchitecture == Architecture.Arm64;

    /// <summary>
    /// Indicates whether the operating-system is 32bit.
    /// </summary>
    public static bool Is32Bit => !Is64Bit;

    /// <summary>
    /// Indicates whether the operating-system is UNIX.
    /// </summary>
    public static bool IsUnix => Platform == PlatformFamily.Linux ||
                                 Platform == PlatformFamily.OSX;

    /// <summary>
    /// Indicates whether the operating-system is Windows.
    /// </summary>
    public static bool IsWin => !IsUnix;

    /// <summary>
    /// Indicates whether the operating-system is Linux.
    /// </summary>
    public static bool IsLinux => Platform == PlatformFamily.Linux;

    /// <summary>
    /// Indicates whether the operating-system is OSX.
    /// </summary>
    public static bool IsOsx => Platform == PlatformFamily.OSX;

    /// <summary>
    /// Indicates whether the current process is running under Windows Subsystem for Linux.
    /// </summary>
    public static bool IsWsl
    {
        get
        {
            if (!IsLinux)
            {
                return false;
            }

            try
            {
                var version = File.ReadAllText("/proc/version");
                return version.ContainsOrdinalIgnoreCase("Microsoft");
            }
            catch (IOException)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Indicates the target framework of the current process.
    /// </summary>
    public static FrameworkName Framework
        => new(Assembly.GetEntryAssembly().NotNull().GetCustomAttribute<TargetFrameworkAttribute>().NotNull().FrameworkName);

    private static readonly TimeSpan dotNetSdkVersionTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Asks the <c>dotnet</c> host that launched the current process which .NET SDK it resolved
    /// (e.g. <c>10.0.401</c>), or returns <c>null</c> when that can't be determined within
    /// <see cref="dotNetSdkVersionTimeout"/>.
    /// </summary>
    public static string GetDotNetSdkVersion()
    {
        string dotnetPath = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? Environment.GetEnvironmentVariable("DOTNET_EXE")
            ?? "dotnet";

        var startInfo = new ProcessStartInfo(dotnetPath, "--version")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        // Suppress the SDK's first-run welcome message and telemetry prompt so neither ends up in stdout.
        startInfo.EnvironmentVariables["DOTNET_NOLOGO"] = "1";
        startInfo.EnvironmentVariables["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";

        try
        {
            using Process process = Process.Start(startInfo).NotNull();
            Task<string> outputTask = process.StandardOutput.ReadToEndAsync();

            if (!process.WaitForExit((int)dotNetSdkVersionTimeout.TotalMilliseconds))
            {
                process.Kill();
                return null;
            }

            string output = outputTask.GetAwaiter().GetResult().Trim();
            return process.ExitCode == 0 && output.Length > 0 ? output : null;
        }
        catch (Exception ex) when (ex is Win32Exception or InvalidOperationException)
        {
            return null;
        }
    }

    /// <summary>
    /// Indicates the operating-system platform.
    /// </summary>
    public static PlatformFamily Platform
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return PlatformFamily.OSX;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return PlatformFamily.Linux;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return PlatformFamily.Windows;
            }

            return PlatformFamily.Unknown;
        }
    }
}
