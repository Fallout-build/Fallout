namespace Fallout.Common.Tools.DotNet;

/// <summary>
/// The test runner of <c>dotnet test</c>. See <a href="https://learn.microsoft.com/dotnet/core/tools/dotnet-test#choose-a-test-runner">Choose a test runner</a>.
/// </summary>
public enum DotNetTestRunner
{
    /// <summary>VSTest, the default. Run it with <see cref="DotNetTasks.DotNetTest(DotNetTestSettings)"/>.</summary>
    VSTest,

    /// <summary>Microsoft.Testing.Platform. Run it with <see cref="DotNetTasks.DotNetTestMtp(DotNetTestMtpSettings)"/>.</summary>
    MicrosoftTestingPlatform
}
