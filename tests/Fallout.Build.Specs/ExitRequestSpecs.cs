using System.Collections.Generic;
using Fallout.Common.Execution;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class ExitRequestSpecs
{
    private static readonly List<string> Calls = [];

    [Fact]
    public void Extensions_that_have_not_run_yet_are_skipped_after_an_exit_request()
    {
        // Arrange
        Calls.Clear();
        var build = new ExitingBuild();

        // Act
        build.ExecuteExtension<ITestExtension>(x => x.Run());

        // Assert
        Calls.Should().Equal("exiting");
        build.ExitRequested.Should().BeTrue();
    }

    [Fact]
    public void The_exit_code_is_kept_after_an_exit_request()
    {
        // Arrange
        var build = new ExitingBuild();

        // Act
        ((IFalloutBuild) build).Exit(7);

        // Assert
        build.ExitCode.Should().Be(7);
    }

    [Fact]
    public void All_extensions_run_without_an_exit_request()
    {
        // Arrange
        Calls.Clear();
        var build = new RunningBuild();

        // Act
        build.ExecuteExtension<ITestExtension>(x => x.Run());

        // Assert
        Calls.Should().Equal("first", "second");
    }

    private interface ITestExtension : IBuildExtension
    {
        void Run();
    }

    [ExitingExtension(Priority = 2)]
    [SecondExtension(Priority = 1)]
    private class ExitingBuild : FalloutBuild
    {
    }

    [FirstExtension(Priority = 2)]
    [SecondExtension(Priority = 1)]
    private class RunningBuild : FalloutBuild
    {
    }

    private class ExitingExtensionAttribute : BuildExtensionAttributeBase, ITestExtension
    {
        public void Run()
        {
            Calls.Add("exiting");
            Build.Exit(0);
        }
    }

    private class FirstExtensionAttribute : BuildExtensionAttributeBase, ITestExtension
    {
        public void Run()
        {
            Calls.Add("first");
        }
    }

    private class SecondExtensionAttribute : BuildExtensionAttributeBase, ITestExtension
    {
        public void Run()
        {
            Calls.Add("second");
        }
    }
}
