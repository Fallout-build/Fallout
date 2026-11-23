using System;
using System.Collections.Generic;
using System.Linq;
using Fallout.Common;
using Fallout.Common.IO;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.DotNet;
using Fallout.Common.Utilities;
using FluentAssertions;
using Xunit;

namespace Fallout.Components.Specs;

/// <summary>
/// Runs the <see cref="ITest.Test" /> target on the solutions in <c>tests/TestSolutions</c>, one for each test runner of
/// <c>dotnet test</c>.
/// </summary>
/// <remarks>
/// The target only passes when it picks the runner that the <c>global.json</c> of the solution selects, because each runner
/// rejects the options of the other. See <c>tests/TestSolutions/README.md</c>.
/// </remarks>
public class TestSolutionSpecs
{
    [Theory]
    [InlineData("VSTest")]
    [InlineData("MTP")]
    public void The_test_target_runs_the_tests_and_writes_a_trx_report(string solution)
    {
        // Arrange
        AbsolutePath testSolutionsDirectory = FindTestSolutionsDirectory();
        AbsolutePath solutionDirectory = testSolutionsDirectory / solution;
        (solutionDirectory / "artifacts").DeleteDirectory();

        // Act
        IProcess process = RunTestTarget(testSolutionsDirectory, solutionDirectory);

        // Assert
        process.ExitCode.Should().Be(0, process.Output.Select(x => x.Text).JoinNewLine());
        AbsolutePath trxFile = solutionDirectory / "artifacts" / "test-results" / "Sample.Specs.trx";
        GetOutcomes(trxFile).Should().BeEquivalentTo("Passed", "NotExecuted");
    }

    /// <summary>
    /// Runs the build of the test solutions in the folder of one solution.
    /// </summary>
    /// <remarks>
    /// The build goes to <c>tests/TestSolutions/.build</c>, so it doesn't overwrite the outputs of the Fallout projects.
    /// The <c>Terminal</c> host keeps the build from acting as a CI build when the specs run on one.
    /// </remarks>
    private static IProcess RunTestTarget(AbsolutePath testSolutionsDirectory, AbsolutePath solutionDirectory)
    {
        IProcess process = ProcessTasks.StartProcess(
            DotNetTasks.DotNetPath,
            $"run --project {testSolutionsDirectory / "build"} --artifacts-path {testSolutionsDirectory / ".build"}" +
            $" -- Test --host Terminal",
            workingDirectory: solutionDirectory,
            environmentVariables: GetEnvironmentWithoutTestHostVariables(),
            timeout: (int)TimeSpan.FromMinutes(10).TotalMilliseconds,
            logOutput: false,
            logInvocation: false);

        process.WaitForExit().Should().BeTrue("the build of the test solution should end within 10 minutes");
        return process;
    }

    /// <summary>
    /// Returns the environment of this process without the variables of the test host that runs these specs.
    /// </summary>
    /// <remarks>
    /// <c>dotnet test</c> sets these variables for its test applications. They would change how the <c>dotnet test</c> of
    /// the test solution runs. <c>DOTNET_TEST_RUNNER</c> would override the <c>global.json</c> of the solution.
    /// </remarks>
    private static Dictionary<string, string> GetEnvironmentWithoutTestHostVariables()
    {
        string[] prefixes = ["TESTINGPLATFORM_", "DOTNET_CLI_TEST", "VSTEST_", "DOTNET_TEST_RUNNER"];
        return EnvironmentInfo.Variables
            .Where(x => !prefixes.Any(prefix => x.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            .ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Returns the outcome of each test in a TRX report.
    /// </summary>
    private static IEnumerable<string> GetOutcomes(AbsolutePath trxFile)
    {
        return XmlTasks.XmlPeek(
            trxFile,
            "/xn:TestRun/xn:Results/xn:UnitTestResult/@outcome",
            ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"));
    }

    /// <summary>
    /// Returns <c>tests/TestSolutions</c> of the repository that contains these specs.
    /// </summary>
    private static AbsolutePath FindTestSolutionsDirectory()
    {
        AbsolutePath baseDirectory = AbsolutePath.Create(AppContext.BaseDirectory);
        for (AbsolutePath directory = baseDirectory; directory is not null; directory = directory.Parent)
        {
            AbsolutePath testSolutionsDirectory = directory / "tests" / "TestSolutions";
            if (testSolutionsDirectory.DirectoryExists())
            {
                return testSolutionsDirectory;
            }
        }

        throw new InvalidOperationException($"No tests/TestSolutions directory above {baseDirectory}.");
    }
}
