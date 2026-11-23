using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fallout.Common;
using Fallout.Common.CI.AzurePipelines;
using Fallout.Common.CI.GitHubActions;
using Fallout.Common.CI.TeamCity;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.Coverlet;
using Fallout.Common.Tools.DotNet;
using Fallout.Common.Utilities;
using Fallout.Common.Utilities.Collections;
using Fallout.Solutions;
using static Fallout.Common.Tools.DotNet.DotNetTasks;

namespace Fallout.Components;

public interface ITest : ICompile, IHasArtifacts
{
    AbsolutePath TestResultDirectory => ArtifactsDirectory / "test-results";

    int TestDegreeOfParallelism => 1;

    /// <summary>
    ///     Runs the test projects on Microsoft.Testing.Platform with
    ///     <see cref="DotNetTasks.DotNetTestMtp(DotNetTestMtpSettings)" /> instead of VSTest.
    /// </summary>
    /// <remarks>
    ///     The default follows the test runner of <c>dotnet test</c> (<see cref="DotNetTasks.GetTestRunner" />): the
    ///     <c>DOTNET_TEST_RUNNER</c> environment variable of the .NET 11 SDK, or the <c>test.runner</c> of the nearest
    ///     <c>global.json</c>, such as <c>"test": { "runner": "Microsoft.Testing.Platform" }</c>.
    /// </remarks>
    bool UseTestingPlatform => GetTestRunner(RootDirectory) == DotNetTestRunner.MicrosoftTestingPlatform;

    Target Test => _ => _
        .DependsOn(Compile)
        .Produces(TestResultDirectory / "*.trx")
        .Produces(TestResultDirectory / "*.xml")
        .Executes(() =>
        {
            try
            {
                if (UseTestingPlatform)
                {
                    RunTestingPlatform();
                }
                else
                {
                    RunVsTest();
                }
            }
            finally
            {
                ReportTestResults();
                ReportTestCount();
            }
        });

    private void RunVsTest()
    {
        DotNetTest(_ => _
                .Apply(TestSettingsBase)
                .Apply(TestSettings)
                .CombineWith(TestProjects, (_, v) => _
                    .Apply(TestProjectSettingsBase, v)
                    .Apply(TestProjectSettings, v)),
            completeOnFailure: true,
            degreeOfParallelism: TestDegreeOfParallelism);
    }

    private void RunTestingPlatform()
    {
        // coverlet.MTP adds a timestamp to its file names, so remove the files of earlier runs.
        TestResultDirectory.GlobFiles("*coverage.cobertura.*.xml").DeleteFiles();

        TestingPlatformRun run = new(
            CollectCoverage,
            OnGitHubActions: GitHubActions.Instance is not null,
            OnTeamCity: TeamCity.Instance is not null,
            OnAzurePipelines: AzurePipelines.Instance is not null);

        DotNetTestMtp(_ => _
                .SetConfiguration(Configuration)
                .SetNoBuild(SucceededTargets.Contains(Compile))
                .ResetVerbosity()
                .SetResultsDirectory(TestResultDirectory)
                .Apply(TestMtpSettings)
                .CombineWith(TestProjects, (_, v) => TestingPlatform
                    .ApplyProject(_.SetProjectFile(v), v.Name, TestingPlatform.GetExtensions(v), run)
                    .Apply(TestMtpProjectSettings, v)),
            completeOnFailure: true,
            degreeOfParallelism: TestDegreeOfParallelism);
    }

    private bool CollectCoverage =>
        InvokedTargets.Contains((this as IReportCoverage)?.ReportCoverage) || IsServerBuild;

    void ReportTestResults()
    {
        // TeamCity has no Microsoft.Testing.Platform reporter yet, so it imports the TRX files instead.
        var importToTeamCity = TeamCity.Instance is not null && UseTestingPlatform;

        TestResultDirectory.GlobFiles("*.trx").ForEach(x =>
        {
            AzurePipelines.Instance?.PublishTestResults(
                type: AzurePipelinesTestResultsType.VSTest,
                title: $"{Path.GetFileNameWithoutExtension(x)} ({AzurePipelines.Instance.StageDisplayName})",
                files: new string[]
                {
                    x
                });

            if (importToTeamCity)
            {
                TeamCity.Instance.ImportData(TeamCityImportType.vstest, x);
            }
        });
    }

    void ReportTestCount()
    {
        IEnumerable<string> GetOutcomes(AbsolutePath file)
            => XmlTasks.XmlPeek(
                file,
                "/xn:TestRun/xn:Results/xn:UnitTestResult/@outcome",
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"));

        var resultFiles = TestResultDirectory.GlobFiles("*.trx");
        var outcomes = resultFiles.SelectMany(GetOutcomes).ToList();
        var passedTests = outcomes.Count(x => x == "Passed");
        var failedTests = outcomes.Count(x => x == "Failed");
        var skippedTests = outcomes.Count(x => x == "NotExecuted");

        ReportSummary(_ => _
            .When(failedTests > 0, _ => _
                .AddPair("Failed", failedTests.ToString()))
            .AddPair("Passed", passedTests.ToString())
            .When(skippedTests > 0, _ => _
                .AddPair("Skipped", skippedTests.ToString())));
    }

    sealed Configure<DotNetTestSettings> TestSettingsBase => _ => _
        .SetConfiguration(Configuration)
        .SetNoBuild(SucceededTargets.Contains(Compile))
        .ResetVerbosity()
        .SetResultsDirectory(TestResultDirectory)
        .When(CollectCoverage, _ => _
            .EnableCollectCoverage()
            .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
            .SetExcludeByFile("*.Generated.cs")
            .When(TeamCity.Instance is not null, _ => _
                .SetCoverletOutputFormat($"\\\"{CoverletOutputFormat.cobertura},{CoverletOutputFormat.teamcity}\\\""))
            .When(IsServerBuild, _ => _
                .EnableUseSourceLink()));

    sealed Configure<DotNetTestSettings, Project> TestProjectSettingsBase => (_, v) => _
        .SetProjectFile(v)
        // https://github.com/Tyrrrz/GitHubActionsTestLogger
        .When(GitHubActions.Instance is not null && v.HasPackageReference("GitHubActionsTestLogger"), _ => _
            .AddLoggers("GitHubActions;report-warnings=false"))
        // https://github.com/JetBrains/TeamCity.VSTest.TestAdapter
        .When(TeamCity.Instance is not null && v.HasPackageReference("TeamCity.VSTest.TestAdapter"), _ => _
            .AddLoggers("TeamCity")
            // https://github.com/xunit/visualstudio.xunit/pull/108
            .AddRunSetting("RunConfiguration.NoAutoReporters", bool.TrueString))
        .AddLoggers($"trx;LogFileName={v.Name}.trx")
        .When(CollectCoverage, _ => _
            .SetCoverletOutput(TestResultDirectory / $"{v.Name}.xml"));

    Configure<DotNetTestSettings> TestSettings => _ => _;

    Configure<DotNetTestSettings, Project> TestProjectSettings => (_, v) => _;

    /// <summary>
    ///     Settings for all test projects when <see cref="UseTestingPlatform" /> is on.
    /// </summary>
    Configure<DotNetTestMtpSettings> TestMtpSettings => _ => _;

    /// <summary>
    ///     Settings for one test project when <see cref="UseTestingPlatform" /> is on.
    /// </summary>
    Configure<DotNetTestMtpSettings, Project> TestMtpProjectSettings => (_, v) => _;

    IEnumerable<Project> TestProjects { get; }
}
