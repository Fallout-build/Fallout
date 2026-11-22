using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fallout.Build.Utilities;
using Fallout.Common;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using Fallout.Common.Utilities;
using Fallout.Common.Utilities.Collections;

namespace Fallout.Build.Execution.Extensions;

/// <summary>
/// Handles the <c>--plan</c> switch. Writes the execution plan as an HTML file, opens it, and stops the build.
/// </summary>
internal class HandlePlanRequestsAttribute : BuildExtensionAttributeBase, IOnBuildInitialized
{
    private const string HtmlFileName = "execution-plan.html";

    private AbsolutePath HtmlFile => Build.TemporaryDirectory / HtmlFileName;

    /// <summary>
    /// Opens the plan file.
    /// </summary>
    internal IFileOpener FileOpener { get; init; } = new ShellFileOpener();

    /// <summary>
    /// Shows the plan when <c>--plan</c> is set, then stops the build with exit code 0.
    /// </summary>
    public void OnBuildInitialized(
        IReadOnlyCollection<ExecutableTarget> executableTargets,
        IReadOnlyCollection<ExecutableTarget> executionPlan)
    {
        if (Build.Plan)
        {
            ShowPlan();
            Build.Exit(exitCode: 0);
        }
    }

    /// <summary>
    /// Writes the plan file to the temporary directory. Opens it unless <c>--plan-no-open</c> is set.
    /// </summary>
    public void ShowPlan()
    {
        var resourceText = ResourceUtility.GetResourceAllText<HandlePlanRequestsAttribute>(HtmlFileName);
        var contents = resourceText
            .Replace("__GRAPH__", GetGraphDefinition())
            .Replace("__EVENTS__", GetEvents())
            .Replace("__DOCS_URL__", Constants.FalloutDocsUrl);

        HtmlFile.WriteAllText(contents);

        // IDE integrations pass --plan-no-open and show the file themselves
        if (!Build.GetParameter<bool>(Constants.PlanNoOpenParameterName))
        {
            FileOpener.Open(HtmlFile);
        }
    }

    // ReSharper disable once CognitiveComplexity
    private string GetGraphDefinition()
    {
        var builder = new StringBuilder();
        foreach (var executableTarget in Build.ExecutableTargets)
        {
            var dependencies = Build.ExecutableTargets.Where(x => x.AllDependencies.Contains(executableTarget)).ToList();
            if (dependencies.Count == 0)
            {
                builder.AppendLine(executableTarget.Name);
            }
            else
            {
                foreach (var dependency in dependencies)
                {
                    if (dependency.ExecutionDependencies.Contains(executableTarget))
                    {
                        builder.AppendLine($"{executableTarget.Name} --> {dependency.Name}");
                    }
                    else if (dependency.OrderDependencies.Contains(executableTarget))
                    {
                        builder.AppendLine($"{executableTarget.Name} -.-> {dependency.Name}");
                    }
                    else if (dependency.TriggerDependencies.Contains(executableTarget))
                    {
                        builder.AppendLine($"{executableTarget.Name} ==> {dependency.Name}");
                    }
                }
            }

            if (!executableTarget.Listed)
            {
                builder.AppendLine($"class {executableTarget.Name} unlisted");
            }
        }

        return builder.ToString();
    }

    private string GetEvents()
    {
        var executableTargets = Build.ExecutableTargets;
        var builder = new StringBuilder();

        // When not hovering anything, highlight the default plan
        var defaultPlan = executableTargets.Where(x => x.IsDefault)
            .SelectMany(x => ExecutionPlanner.GetExecutionPlan(executableTargets, new[]
            {
                x.Name
            }))
            .Distinct().ToList();

        defaultPlan.ForEach(x => builder.AppendLine($@"  $(""#{x.Name}"").addClass('highlight');"));

        foreach (var executableTarget in executableTargets)
        {
            var executionPlan = ExecutionPlanner.GetExecutionPlan(executableTargets, new[]
            {
                executableTarget.Name
            });

            builder
                .AppendLine($@"  $(""#{executableTarget.Name}"").hover(")
                .AppendLine("    function() {");

            executableTargets.ForEach(x => builder.AppendLine($@"        $(""#{x.Name}"").removeClass('highlight');"));
            executionPlan.ForEach(x => builder.AppendLine($@"        $(""#{x.Name}"").addClass('highlight');"));
            builder
                .AppendLine("    },")
                .AppendLine("    function() {");

            executionPlan.ForEach(x => builder.AppendLine($@"        $(""#{x.Name}"").removeClass('highlight');"));
            defaultPlan.ForEach(x => builder.AppendLine($@"        $(""#{x.Name}"").addClass('highlight');"));
            builder
                .AppendLine("    });");
        }

        return builder.ToString();
    }
}
