using System;
using System.Linq;
using System.Text.Json;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using Fallout.Common.Utilities;
using Serilog.Events;

namespace Fallout.Common.Tools.DotNet;

public class DotNetVerbosityMappingAttribute : VerbosityMappingAttribute
{
    public DotNetVerbosityMappingAttribute()
        : base(typeof(DotNetVerbosity))
    {
        Quiet = nameof(DotNetVerbosity.quiet);
        Minimal = nameof(DotNetVerbosity.minimal);
        Normal = nameof(DotNetVerbosity.minimal);
        Verbose = nameof(DotNetVerbosity.detailed);
    }
}

[LogLevelPattern(LogEventLevel.Warning, @": warning \w{2,}\d+:")]
[LogLevelPattern(LogEventLevel.Error, @": error \w{2,}\d+:")]
partial class DotNetTasks;

public partial class DotNetTasks
{
    private const string TestRunnerVariable = "DOTNET_TEST_RUNNER";

    /// <summary>
    /// Returns the test runner that <c>dotnet test</c> uses in <paramref name="directory"/>: the <c>test.runner</c> of the
    /// nearest <c>global.json</c>, or the <c>DOTNET_TEST_RUNNER</c> environment variable, which overrides it with the
    /// .NET 11 SDK and later. Without either, the runner is <see cref="DotNetTestRunner.VSTest"/>.
    /// </summary>
    public static DotNetTestRunner GetTestRunner(AbsolutePath directory)
    {
        // The .NET 10 SDK ignores the variable, so check the SDK version only when the variable is set.
        var variableRunner = ParseTestRunner(EnvironmentInfo.GetVariable(TestRunnerVariable));
        if (variableRunner != null && GetSdkMajorVersion(directory) >= 11)
        {
            return variableRunner.Value;
        }

        return GetGlobalJsonTestRunner(directory) ?? DotNetTestRunner.VSTest;
    }

    internal static DotNetTestRunner? GetGlobalJsonTestRunner(AbsolutePath directory)
    {
        var globalJsonFile = FindGlobalJsonFile(directory);
        if (globalJsonFile == null)
        {
            return null;
        }

        using var document = JsonDocument.Parse(
            globalJsonFile.ReadAllText(),
            new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            });

        var runner = GetObjectProperty(document.RootElement, "test") is { } test
            ? GetObjectProperty(test, "runner")
            : null;

        return runner is { ValueKind: JsonValueKind.String } ? ParseTestRunner(runner.Value.GetString()) : null;
    }

    internal static DotNetTestRunner? ParseTestRunner(string value)
    {
        if (string.Equals(value, "Microsoft.Testing.Platform", StringComparison.OrdinalIgnoreCase))
        {
            return DotNetTestRunner.MicrosoftTestingPlatform;
        }

        if (string.Equals(value, nameof(DotNetTestRunner.VSTest), StringComparison.OrdinalIgnoreCase))
        {
            return DotNetTestRunner.VSTest;
        }

        return null;
    }

    // dotnet reads only the nearest global.json, even when it has no test section.
    private static AbsolutePath FindGlobalJsonFile(AbsolutePath directory)
    {
        for (var current = directory; current != null; current = current.Parent)
        {
            var file = current / "global.json";
            if (file.FileExists())
            {
                return file;
            }
        }

        return null;
    }

    private static JsonElement? GetObjectProperty(JsonElement element, string name)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        return element.EnumerateObject()
            .Where(x => x.Name.EqualsOrdinalIgnoreCase(name))
            .Select(x => (JsonElement?)x.Value)
            .FirstOrDefault();
    }

    private static int GetSdkMajorVersion(AbsolutePath directory)
    {
        var version = DotNet("--version", directory, logOutput: false, logInvocation: false).StdToText();
        return int.TryParse(version.Split('.').First(), out var major) ? major : 0;
    }

    public static string EscapeMSBuild(string str)
    {
        // https://docs.microsoft.com/en-us/visualstudio/msbuild/msbuild-special-characters
        return str
            .Replace("%", "%25") // Referencing metadata
            .Replace("$", "%24") // Referencing properties
            .Replace("@", "%40") // Referencing item lists
            .Replace("'", "%27") // Conditions and other expressions
            .Replace(";", "%3B") // List separator
            .Replace("?", "%3F") // Wildcard character for file names in Include and Exclude attributes
            .Replace("*", "%2A"); // Wildcard character for use in file names in Include and Exclude attributes
    }
}
