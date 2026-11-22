using System;
using System.Collections.Generic;
using System.IO;
using Fallout.Build.Execution.Extensions;
using Fallout.Build.Utilities;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class HandlePlanRequestsAttributeSpecs
{
    [Fact]
    public void Shows_the_plan_in_the_browser_by_default()
    {
        // Arrange
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryDirectory);

        var build = new FakeBuildBuilder()
            .WithTemporaryDirectory(temporaryDirectory)
            .WithPlan()
            .Build();

        var fileOpener = A.Fake<IFileOpener>();
        var sut = new HandlePlanRequestsAttribute { Build = build, FileOpener = fileOpener };

        try
        {
            // Act
            sut.OnBuildInitialized([], []);

            // Assert
            A.CallTo(() => fileOpener.Open(A<AbsolutePath>.That.Matches(x => x.Name == "execution-plan.html")))
                .MustHaveHappenedOnceExactly();
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Fact]
    public void Writes_the_plan_without_opening_it_when_the_browser_must_stay_closed()
    {
        // Arrange
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryDirectory);

        var build = new FakeBuildBuilder()
            .WithTemporaryDirectory(temporaryDirectory)
            .WithPlan()
            .WithBrowserClosed()
            .Build();

        var fileOpener = A.Fake<IFileOpener>();
        var sut = new HandlePlanRequestsAttribute { Build = build, FileOpener = fileOpener };

        try
        {
            // Act
            sut.OnBuildInitialized([], []);

            // Assert
            File.Exists(Path.Combine(temporaryDirectory, "execution-plan.html")).Should().BeTrue();
            A.CallTo(() => fileOpener.Open(A<AbsolutePath>._)).MustNotHaveHappened();
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Fact]
    public void Stops_the_build_successfully_after_showing_the_plan()
    {
        // Arrange
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryDirectory);

        var build = new FakeBuildBuilder()
            .WithTemporaryDirectory(temporaryDirectory)
            .WithPlan()
            .Build();

        var sut = new HandlePlanRequestsAttribute { Build = build, FileOpener = A.Fake<IFileOpener>() };

        try
        {
            // Act
            sut.OnBuildInitialized([], []);

            // Assert
            A.CallTo(() => build.Exit(0)).MustHaveHappenedOnceExactly();
        }
        finally
        {
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    [Fact]
    public void Does_nothing_when_no_plan_is_requested()
    {
        // Arrange
        var build = new FakeBuildBuilder().Build();

        var fileOpener = A.Fake<IFileOpener>();
        var sut = new HandlePlanRequestsAttribute { Build = build, FileOpener = fileOpener };

        // Act
        sut.OnBuildInitialized([], []);

        // Assert
        A.CallTo(() => fileOpener.Open(A<AbsolutePath>._)).MustNotHaveHappened();
        A.CallTo(() => build.Exit(A<int>._)).MustNotHaveHappened();
    }

    /// <summary>
    /// Builds a fake build with no targets, no plan request and the browser allowed to open.
    /// A spec changes only what it is about.
    /// </summary>
    private class FakeBuildBuilder
    {
        private bool _plan;
        private bool _browserClosed;
        private string _temporaryDirectory = Path.GetTempPath();

        public FakeBuildBuilder WithPlan()
        {
            _plan = true;
            return this;
        }

        public FakeBuildBuilder WithBrowserClosed()
        {
            _browserClosed = true;
            return this;
        }

        public FakeBuildBuilder WithTemporaryDirectory(string path)
        {
            _temporaryDirectory = path;
            return this;
        }

        public IFalloutBuild Build()
        {
            var build = A.Fake<IFalloutBuild>();
            A.CallTo(() => build.Plan).Returns(_plan);
            A.CallTo(() => build.TemporaryDirectory).Returns((AbsolutePath) _temporaryDirectory);
            A.CallTo(() => build.ExecutableTargets).Returns(new List<ExecutableTarget>());
            A.CallTo(() => build.GetParameter<bool>(Constants.PlanNoOpenParameterName)).Returns(_browserClosed);

            return build;
        }
    }
}
