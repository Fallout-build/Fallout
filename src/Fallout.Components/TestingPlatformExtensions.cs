using System;

namespace Fallout.Components;

/// <summary>
/// The Microsoft.Testing.Platform extensions of a test project.
/// </summary>
/// <remarks>
/// A test application fails on an option that none of its extensions knows. So <see cref="ITest" /> passes only the options
/// of the extensions that a project has.
/// </remarks>
[Flags]
internal enum TestingPlatformExtensions
{
    /// <summary>
    /// No known extension.
    /// </summary>
    None = 0,

    /// <summary>
    /// <c>Microsoft.Testing.Extensions.TrxReport</c>, which writes TRX reports.
    /// </summary>
    TrxReport = 1 << 0,

    /// <summary>
    /// <c>Microsoft.Testing.Extensions.CodeCoverage</c>, which collects coverage with Microsoft code coverage.
    /// </summary>
    CodeCoverage = 1 << 1,

    /// <summary>
    /// <c>coverlet.MTP</c>, which collects coverage with Coverlet.
    /// </summary>
    Coverlet = 1 << 2,

    /// <summary>
    /// <c>Microsoft.Testing.Extensions.GitHubActionsReport</c>, which writes annotations and a job summary on GitHub Actions.
    /// </summary>
    GitHubActionsReport = 1 << 3,

    /// <summary>
    /// <c>GitHubActionsTestLogger</c>, which also writes annotations and a job summary on GitHub Actions.
    /// </summary>
    GitHubActionsTestLogger = 1 << 4,

    /// <summary>
    /// <c>Microsoft.Testing.Extensions.AzureDevOpsReport</c>, which writes annotations on Azure Pipelines.
    /// </summary>
    AzureDevOpsReport = 1 << 5,

    /// <summary>
    /// An xunit.v3 project, which writes TRX reports itself.
    /// </summary>
    Xunit = 1 << 6
}
