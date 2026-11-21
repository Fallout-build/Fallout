using System;
using System.Collections.Generic;
using System.IO;
using Fallout.Build.Execution;
using Fallout.Build.Execution.Extensions;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using Fallout.Common.Utilities.Collections;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class HandlePlanRequestsAttributeSpecs : IDisposable
{
    private readonly string _temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
    private readonly IFalloutBuild _build = A.Fake<IFalloutBuild>();
    private readonly IFileOpener _fileOpener = A.Fake<IFileOpener>();

    public HandlePlanRequestsAttributeSpecs()
    {
        Directory.CreateDirectory(_temporaryDirectory);

        A.CallTo(() => _build.TemporaryDirectory).Returns((AbsolutePath) _temporaryDirectory);
        A.CallTo(() => _build.ExecutableTargets).Returns(new List<ExecutableTarget>());
    }

    public void Dispose()
    {
        Directory.Delete(_temporaryDirectory, recursive: true);
    }

    private HandlePlanRequestsAttribute CreateSut(bool plan, bool noOpen = false)
    {
        A.CallTo(() => _build.Plan).Returns(plan);
        A.CallTo(() => _build.GetParameter<bool>(Constants.PlanNoOpenParameterName)).Returns(noOpen);

        return new HandlePlanRequestsAttribute
        {
            Build = _build,
            FileOpener = _fileOpener
        };
    }

    [Fact]
    public void Plan_opens_the_generated_file_by_default()
    {
        // Arrange
        var sut = CreateSut(plan: true);

        // Act
        sut.OnBuildInitialized([], []);

        // Assert
        A.CallTo(() => _fileOpener.Open(A<AbsolutePath>.That.Matches(x => x.Name == "execution-plan.html")))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Plan_no_open_writes_the_plan_but_does_not_open_it()
    {
        // Arrange
        var sut = CreateSut(plan: true, noOpen: true);

        // Act
        sut.OnBuildInitialized([], []);

        // Assert
        File.Exists(Path.Combine(_temporaryDirectory, "execution-plan.html")).Should().BeTrue();
        A.CallTo(() => _fileOpener.Open(A<AbsolutePath>._)).MustNotHaveHappened();
    }

    [Fact]
    public void Plan_stops_the_build_with_a_success_exit_code()
    {
        // Arrange
        var sut = CreateSut(plan: true);

        // Act
        sut.OnBuildInitialized([], []);

        // Assert
        A.CallTo(() => _build.Exit(0)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Nothing_happens_without_the_plan_switch()
    {
        // Arrange
        var sut = CreateSut(plan: false);

        // Act
        sut.OnBuildInitialized([], []);

        // Assert
        Directory.GetFiles(_temporaryDirectory).Should().BeEmpty();
        A.CallTo(() => _fileOpener.Open(A<AbsolutePath>._)).MustNotHaveHappened();
        A.CallTo(() => _build.Exit(A<int>._)).MustNotHaveHappened();
    }

    private static ParameterService GetService(string[] commandLineArguments, IDictionary<string, string> environmentVariables = null)
    {
        environmentVariables ??= new Dictionary<string, string>();

        return new ParameterService(
            () => new ArgumentParser(commandLineArguments),
            () => environmentVariables.AsReadOnly());
    }

    private static bool GetPlanNoOpen(ParameterService service)
    {
        return (bool) service.GetParameter(Constants.PlanNoOpenParameterName, typeof(bool), separator: null);
    }

    [Fact]
    public void Plan_no_open_is_off_by_default()
    {
        GetPlanNoOpen(GetService(["--plan"])).Should().BeFalse();
    }

    [Fact]
    public void Plan_no_open_switch_turns_it_on()
    {
        GetPlanNoOpen(GetService(["--plan", "--plan-no-open"])).Should().BeTrue();
    }

    [Fact]
    public void Plan_no_open_environment_variable_turns_it_on()
    {
        var service = GetService(["--plan"], new Dictionary<string, string> { { "PLAN_NO_OPEN", "true" } });

        GetPlanNoOpen(service).Should().BeTrue();
    }
}
