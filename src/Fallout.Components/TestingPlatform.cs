using System.Collections.Generic;
using System.Linq;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.DotNet;
using Fallout.Common.Utilities;
using Fallout.Solutions;
using Serilog;

namespace Fallout.Components;

/// <summary>
/// The project settings that <see cref="ITest" /> uses when <see cref="ITest.UseTestingPlatform" /> is on.
/// </summary>
internal static class TestingPlatform
{
    /// <summary>
    /// The namespace of the <c>TestingPlatformBuilderHook</c> type of each extension package.
    /// </summary>
    /// <remarks>
    /// Each extension package adds a <c>TestingPlatformBuilderHook</c> item to the projects that reference it. The
    /// <c>TypeFullName</c> metadata of that item is in the namespace of the extension.
    /// </remarks>
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
    /// Returns the extensions of a project, from its evaluated <c>TestingPlatformBuilderHook</c> items.
    /// </summary>
    /// <remarks>
    /// These items include the extensions that come from a package reference, a transitive package or a test SDK profile.
    /// </remarks>
    public static TestingPlatformExtensions GetExtensions(Project project)
    {
        Microsoft.Build.Evaluation.Project msbuildProject = project.GetMSBuildProject();
        IEnumerable<string> hookTypeNames = msbuildProject.GetItems("TestingPlatformBuilderHook")
            .Select(x => x.GetMetadataValue("TypeFullName"));

        bool isXunitProject = msbuildProject.GetPropertyValue("XunitTestProject").EqualsOrdinalIgnoreCase(bool.TrueString);
        return GetExtensions(hookTypeNames, isXunitProject);
    }

    /// <summary>
    /// Returns the extensions whose <c>TestingPlatformBuilderHook</c> types are in <paramref name="hookTypeNames"/>.
    /// </summary>
    public static TestingPlatformExtensions GetExtensions(IEnumerable<string> hookTypeNames, bool isXunitProject)
    {
        List<string> typeNames = hookTypeNames.ToList();
        TestingPlatformExtensions extensions = builderHooks
            .Where(hook => typeNames.Any(x => x.StartsWithOrdinalIgnoreCase($"{hook.Namespace}.")))
            .Aggregate(TestingPlatformExtensions.None, (result, hook) => result | hook.Extension);

        return isXunitProject ? extensions | TestingPlatformExtensions.Xunit : extensions;
    }

    /// <summary>
    /// Adds the report, CI reporter and coverage options of one test project, each only when the project has an extension
    /// that knows the option.
    /// </summary>
    /// <remarks>
    /// When a project has more than one extension for the same job, one of them wins:
    /// <list type="bullet">
    ///     <item>The TRX report extension wins over the TRX report of xunit.v3.</item>
    ///     <item>Coverlet wins over Microsoft code coverage.</item>
    ///     <item>The Microsoft GitHub Actions report wins over GitHubActionsTestLogger.</item>
    /// </list>
    /// It logs a warning when a project can't write a TRX report, because its tests then aren't counted. It also logs one
    /// when coverage is asked for but the project has no coverage extension.
    /// </remarks>
    public static DotNetTestMtpSettings ApplyProject(
        DotNetTestMtpSettings settings,
        string projectName,
        TestingPlatformExtensions extensions,
        TestingPlatformRun run)
    {
        settings = ApplyReports(settings, projectName, extensions, run);
        return run.CollectCoverage ? ApplyCoverage(settings, projectName, extensions, run) : settings;
    }

    /// <summary>
    /// Adds the TRX report and the reporter of the CI server.
    /// </summary>
    private static DotNetTestMtpSettings ApplyReports(
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
            settings = settings.AddTestApplicationArguments(
                "--report-xunit-trx",
                "--report-xunit-trx-filename",
                $"{projectName}.trx");
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

        return settings;
    }

    /// <summary>
    /// Collects coverage as a Cobertura file named after the project.
    /// </summary>
    private static DotNetTestMtpSettings ApplyCoverage(
        DotNetTestMtpSettings settings,
        string projectName,
        TestingPlatformExtensions extensions,
        TestingPlatformRun run)
    {
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
