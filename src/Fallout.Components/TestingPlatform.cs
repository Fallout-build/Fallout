using System;
using System.Collections.Generic;
using System.Linq;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.DotNet;
using Fallout.Common.Utilities;
using Fallout.Solutions;
using Serilog;

namespace Fallout.Components;

/// <summary>
///     The Microsoft.Testing.Platform extensions of a test project. A test application fails on an option that none
///     of its extensions knows, so <see cref="ITest" /> passes only the options of the extensions a project has.
/// </summary>
[Flags]
internal enum TestingPlatformExtensions
{
    None = 0,
    TrxReport = 1 << 0,
    CodeCoverage = 1 << 1,
    Coverlet = 1 << 2,
    GitHubActionsReport = 1 << 3,
    GitHubActionsTestLogger = 1 << 4,
    AzureDevOpsReport = 1 << 5,

    /// <summary>An xunit.v3 project, which writes TRX reports itself.</summary>
    Xunit = 1 << 6
}

/// <summary>Where and how <see cref="ITest" /> runs the tests.</summary>
internal readonly record struct TestingPlatformRun(
    bool CollectCoverage,
    bool OnGitHubActions,
    bool OnTeamCity,
    bool OnAzurePipelines);

/// <summary>
///     The project settings that <see cref="ITest" /> uses when <see cref="ITest.UseTestingPlatform" /> is on.
/// </summary>
internal static class TestingPlatform
{
    // Each extension package adds a TestingPlatformBuilderHook item. Its TypeFullName is in the namespace of the extension.
    private static readonly (string Namespace, TestingPlatformExtensions Extension)[] builderHooks =
    [
        ("Microsoft.Testing.Extensions.TrxReport", TestingPlatformExtensions.TrxReport),
        ("Microsoft.Testing.Extensions.CodeCoverage", TestingPlatformExtensions.CodeCoverage),
        ("Coverlet.MTP", TestingPlatformExtensions.Coverlet),
        ("Microsoft.Testing.Extensions.GitHubActionsReport", TestingPlatformExtensions.GitHubActionsReport),
        ("GitHubActionsTestLogger", TestingPlatformExtensions.GitHubActionsTestLogger),
        ("Microsoft.Testing.Extensions.AzureDevOpsReport", TestingPlatformExtensions.AzureDevOpsReport)
    ];

    /// <summary>
    ///     Finds the extensions of a project from its evaluated <c>TestingPlatformBuilderHook</c> items. These include
    ///     extensions that come from a package reference, a transitive package or a test SDK profile.
    /// </summary>
    public static TestingPlatformExtensions GetExtensions(Project project)
    {
        var msbuildProject = project.GetMSBuildProject();
        var hookTypeNames = msbuildProject.GetItems("TestingPlatformBuilderHook").Select(x => x.GetMetadataValue("TypeFullName"));
        var isXunitProject = msbuildProject.GetPropertyValue("XunitTestProject").EqualsOrdinalIgnoreCase(bool.TrueString);
        return GetExtensions(hookTypeNames, isXunitProject);
    }

    public static TestingPlatformExtensions GetExtensions(IEnumerable<string> hookTypeNames, bool isXunitProject)
    {
        var typeNames = hookTypeNames.ToList();
        var extensions = builderHooks
            .Where(hook => typeNames.Any(x => x.StartsWithOrdinalIgnoreCase($"{hook.Namespace}.")))
            .Aggregate(TestingPlatformExtensions.None, (result, hook) => result | hook.Extension);

        return isXunitProject ? extensions | TestingPlatformExtensions.Xunit : extensions;
    }

    public static DotNetTestMtpSettings ApplyProject(
        DotNetTestMtpSettings settings,
        string projectName,
        TestingPlatformExtensions extensions,
        TestingPlatformRun run)
    {
        if (extensions.HasFlag(TestingPlatformExtensions.TrxReport))
        {
            settings = settings
                .EnableReportTrx()
                .SetReportTrxFileName($"{projectName}.trx");
        }
        else if (extensions.HasFlag(TestingPlatformExtensions.Xunit))
        {
            settings = settings.AddTestApplicationArguments("--report-xunit-trx", "--report-xunit-trx-filename", $"{projectName}.trx");
        }
        else
        {
            Log.Warning("Project {Project} can't write a TRX report, so its tests are not counted. " +
                        "Add the Microsoft.Testing.Extensions.TrxReport package to it", projectName);
        }

        if (run.OnGitHubActions && extensions.HasFlag(TestingPlatformExtensions.GitHubActionsReport))
        {
            settings = settings.EnableReportGitHubActions();
        }
        else if (run.OnGitHubActions && extensions.HasFlag(TestingPlatformExtensions.GitHubActionsTestLogger))
        {
            settings = settings.AddTestApplicationArguments("--report-github");
        }

        if (run.OnAzurePipelines && extensions.HasFlag(TestingPlatformExtensions.AzureDevOpsReport))
        {
            settings = settings.EnableReportAzureDevOps();
        }

        if (!run.CollectCoverage)
        {
            return settings;
        }

        if (extensions.HasFlag(TestingPlatformExtensions.Coverlet))
        {
            return settings
                .EnableCoverlet()
                .AddCoverletOutputFormats(DotNetTestCoverletOutputFormat.cobertura)
                .When(run.OnTeamCity, _ => _
                    .AddCoverletOutputFormats(DotNetTestCoverletOutputFormat.teamcity))
                .AddCoverletExcludeByFiles("**/*.Generated.cs")
                .SetCoverletFilePrefix(projectName);
        }

        if (extensions.HasFlag(TestingPlatformExtensions.CodeCoverage))
        {
            return settings
                .EnableCoverage()
                .SetCoverageOutputFormat(DotNetTestCoverageOutputFormat.cobertura)
                .SetCoverageOutput($"{projectName}.cobertura.xml");
        }

        Log.Warning("Project {Project} can't collect coverage. " +
                    "Add the coverlet.MTP or Microsoft.Testing.Extensions.CodeCoverage package to it", projectName);
        return settings;
    }
}
