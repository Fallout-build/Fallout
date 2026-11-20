using System.Collections.Generic;
using Fallout.Build.Execution;
using Fallout.Build.Execution.Extensions;
using Fallout.Common.Utilities.Collections;
using FluentAssertions;
using Xunit;

namespace Fallout.Common.Specs;

public class HandlePlanRequestsAttributeSpecs
{
    private static ParameterService GetService(string[] commandLineArguments, IDictionary<string, string> environmentVariables = null)
    {
        environmentVariables ??= new Dictionary<string, string>();

        return new ParameterService(
            () => new ArgumentParser(commandLineArguments),
            () => environmentVariables.AsReadOnly());
    }

    [Fact]
    public void Plan_opens_the_browser_by_default()
    {
        HandlePlanRequestsAttribute.ShouldOpenBrowser(GetService(["--plan"])).Should().BeTrue();
    }

    [Fact]
    public void Plan_no_open_switch_keeps_the_browser_closed()
    {
        HandlePlanRequestsAttribute.ShouldOpenBrowser(GetService(["--plan", "--plan-no-open"])).Should().BeFalse();
    }

    [Fact]
    public void Plan_no_open_environment_variable_keeps_the_browser_closed()
    {
        var service = GetService(["--plan"], new Dictionary<string, string> { { "PLAN_NO_OPEN", "true" } });

        HandlePlanRequestsAttribute.ShouldOpenBrowser(service).Should().BeFalse();
    }
}
