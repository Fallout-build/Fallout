using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fallout.Common.CI;
using Fallout.Common.Execution;
using Fallout.Common.IO;
using Fallout.Common.Tooling;

namespace Fallout.Common;

public interface IFalloutBuild
{
    void ReportSummary(Configure<Dictionary<string, string>> configurator = null);

    internal IReadOnlyCollection<ExecutableTarget> ExecutableTargets { get; }

    internal IReadOnlyCollection<IBuildExtension> BuildExtensions { get; }

    internal bool IsInterceptorExecution { get; }

    internal string[] LoadedLocalProfiles { get; }

    internal bool IsOutputEnabled(DefaultOutput output);

    /// <summary>
    /// Reads a parameter by its CLI/environment name, without needing a declared member.
    /// </summary>
    internal T GetParameter<T>(string name);

    /// <summary>
    /// Stops the build after the current extension event. No target runs and the build returns <paramref name="exitCode"/>.
    /// Extensions that have not run yet for this event are skipped, and so are the build-finished extensions.
    /// </summary>
    internal void Exit(int exitCode);

    IReadOnlyCollection<ExecutableTarget> ExecutionPlan { get; }

    IReadOnlyCollection<ExecutableTarget> InvokedTargets { get; }

    IReadOnlyCollection<ExecutableTarget> SkippedTargets { get; }

    IReadOnlyCollection<ExecutableTarget> ScheduledTargets { get; }

    IReadOnlyCollection<ExecutableTarget> RunningTargets { get; }

    IReadOnlyCollection<ExecutableTarget> AbortedTargets { get; }

    IReadOnlyCollection<ExecutableTarget> FailedTargets { get; }

    IReadOnlyCollection<ExecutableTarget> SucceededTargets { get; }

    IReadOnlyCollection<ExecutableTarget> FinishedTargets { get; }

    bool IsSucceeding { get; }

    bool IsFailing { get; }

    bool IsFinished { get; }

    int? ExitCode { get; set; }

    AbsolutePath RootDirectory { get; }

    AbsolutePath TemporaryDirectory { get; }

    AbsolutePath BuildAssemblyFile { get; }

    AbsolutePath BuildAssemblyDirectory { get; }

    AbsolutePath BuildProjectDirectory { get; }

    AbsolutePath BuildProjectFile { get; }

    Verbosity Verbosity { get; }

    Host Host { get; }

    bool Plan { get; }

    bool Help { get; }

    bool NoLogo { get; }

    bool IsLocalBuild { get; }

    bool IsServerBuild { get; }

    bool Continue { get; }

    Partition Partition { get; }

    T TryGetValue<T>(Expression<Func<T>> parameterExpression) where T : class;

    T TryGetValue<T>(Expression<Func<object>> parameterExpression);
}
