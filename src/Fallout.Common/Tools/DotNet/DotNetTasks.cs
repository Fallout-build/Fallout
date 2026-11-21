using System;
using System.Text.Json;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
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
    /// <summary>
    /// Returns the test runner that <c>dotnet test</c> uses in <paramref name="directory"/>, so a build can choose between
    /// <see cref="DotNetTest(DotNetTestSettings)"/> and <see cref="DotNetTestMtp(DotNetTestMtpSettings)"/>.
    /// </summary>
    /// <remarks>
    /// The <c>DOTNET_TEST_RUNNER</c> environment variable wins, as with the .NET 11 SDK and later, which read it.
    /// Otherwise the <c>test.runner</c> of the nearest <c>global.json</c> decides. Without either, the runner is
    /// <see cref="DotNetTestRunner.VSTest"/>.
    /// </remarks>
    public static DotNetTestRunner GetTestRunner(AbsolutePath directory)
    {
        DotNetTestRunner? variableRunner = ParseTestRunner(EnvironmentInfo.GetVariable("DOTNET_TEST_RUNNER"));
        return variableRunner ?? FindGlobalJsonTestRunner(directory) ?? DotNetTestRunner.VSTest;
    }

    /// <summary>
    /// Returns the test runner that a <c>test.runner</c> or <c>DOTNET_TEST_RUNNER</c> value names, or <c>null</c> for an
    /// unknown or missing value.
    /// </summary>
    /// <remarks>
    /// The names are case-insensitive, as in <c>dotnet test</c>.
    /// </remarks>
    private static DotNetTestRunner? ParseTestRunner(string value)
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

    /// <summary>
    /// Returns the <c>test.runner</c> of the nearest <c>global.json</c> in or above <paramref name="directory"/>, or
    /// <c>null</c> when that file selects no runner.
    /// </summary>
    /// <remarks>
    /// This reads the file the way <c>dotnet test</c> does. Only the nearest <c>global.json</c> counts, even when a file
    /// further up has a runner. The property names are case-sensitive, and comments are allowed. A file that isn't valid
    /// JSON, such as one with a trailing comma, selects no runner, because <c>dotnet test</c> then uses VSTest.
    /// </remarks>
    private static DotNetTestRunner? FindGlobalJsonTestRunner(AbsolutePath directory)
    {
        AbsolutePath globalJsonFile = FindGlobalJsonFile(directory);
        if (globalJsonFile is null)
        {
            return null;
        }

        try
        {
            using JsonDocument document = JsonDocument.Parse(
                globalJsonFile.ReadAllText(),
                new JsonDocumentOptions
                {
                    CommentHandling = JsonCommentHandling.Skip
                });

            JsonElement root = document.RootElement;
            return root.ValueKind is JsonValueKind.Object
                   && root.TryGetProperty("test", out JsonElement test)
                   && test.ValueKind is JsonValueKind.Object
                   && test.TryGetProperty("runner", out JsonElement runner)
                   && runner.ValueKind is JsonValueKind.String
                ? ParseTestRunner(runner.GetString())
                : null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the nearest <c>global.json</c> in or above <paramref name="directory"/>, or <c>null</c> when there is none.
    /// </summary>
    private static AbsolutePath FindGlobalJsonFile(AbsolutePath directory)
    {
        for (AbsolutePath current = directory; current is not null; current = current.Parent)
        {
            AbsolutePath file = current / "global.json";
            if (file.FileExists())
            {
                return file;
            }
        }

        return null;
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
