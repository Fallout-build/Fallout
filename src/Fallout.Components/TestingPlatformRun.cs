namespace Fallout.Components;

/// <summary>
/// Where and how <see cref="ITest" /> runs the tests on Microsoft.Testing.Platform.
/// </summary>
/// <param name="CollectCoverage">
/// Whether to collect code coverage.
/// </param>
/// <param name="OnGitHubActions">
/// Whether the build runs on GitHub Actions.
/// </param>
/// <param name="OnTeamCity">
/// Whether the build runs on TeamCity.
/// </param>
/// <param name="OnAzurePipelines">
/// Whether the build runs on Azure Pipelines.
/// </param>
internal readonly record struct TestingPlatformRun(
    bool CollectCoverage,
    bool OnGitHubActions,
    bool OnTeamCity,
    bool OnAzurePipelines);
