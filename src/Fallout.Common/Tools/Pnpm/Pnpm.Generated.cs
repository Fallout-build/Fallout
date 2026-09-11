// Generated from https://github.com/Fallout-build/Fallout/blob/develop/src/Fallout.Common/Tools/Pnpm/Pnpm.json

using Fallout.Common;
using Fallout.Common.Tooling;
using Fallout.Common.Tools;
using Fallout.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Fallout.Common.Tools.Pnpm;

/// <summary><p>pnpm is a fast, disk space efficient package manager for the Node JavaScript platform.<para/>Files inside <c>node_modules</c> are hard linked or cloned from a single content-addressable storage on disk, saving disk space and speeding up installations.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
[ExcludeFromCodeCoverage]
[PathTool(Executable = PathExecutable)]
public partial class PnpmTasks : ToolTasks, IRequirePathTool
{
    public static string PnpmPath { get => new PnpmTasks().GetToolPathInternal(); set => new PnpmTasks().SetToolPath(value); }
    public const string PathExecutable = "pnpm";
    /// <summary><p>pnpm is a fast, disk space efficient package manager for the Node JavaScript platform.<para/>Files inside <c>node_modules</c> are hard linked or cloned from a single content-addressable storage on disk, saving disk space and speeding up installations.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> Pnpm(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => new PnpmTasks().Run(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>Runs <c>pnpm clean</c> followed by <c>pnpm install --frozen-lockfile</c>. Designed for CI/CD environments.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--dir</c> via <see cref="PnpmCiSettings.Dir"/></li><li><c>--loglevel</c> via <see cref="PnpmCiSettings.LogLevel"/></li><li><c>--reporter</c> via <see cref="PnpmCiSettings.Reporter"/></li><li><c>--silent</c> via <see cref="PnpmCiSettings.Silent"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PnpmCi(PnpmCiSettings options = null) => new PnpmTasks().Run<PnpmCiSettings>(options);
    /// <inheritdoc cref="PnpmTasks.PnpmCi(Fallout.Common.Tools.Pnpm.PnpmCiSettings)"/>
    public static IReadOnlyCollection<Output> PnpmCi(Configure<PnpmCiSettings> configurator) => new PnpmTasks().Run<PnpmCiSettings>(configurator.Invoke(new PnpmCiSettings()));
    /// <inheritdoc cref="PnpmTasks.PnpmCi(Fallout.Common.Tools.Pnpm.PnpmCiSettings)"/>
    public static IEnumerable<(PnpmCiSettings Settings, IReadOnlyCollection<Output> Output)> PnpmCi(CombinatorialConfigure<PnpmCiSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PnpmCi, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Installs all dependencies of the project in the current working directory. When executed inside a workspace, installs all dependencies of all projects.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;packages&gt;</c> via <see cref="PnpmInstallSettings.Packages"/></li><li><c>--aggregate-output</c> via <see cref="PnpmInstallSettings.AggregateOutput"/></li><li><c>--child-concurrency</c> via <see cref="PnpmInstallSettings.ChildConcurrency"/></li><li><c>--dev</c> via <see cref="PnpmInstallSettings.Dev"/></li><li><c>--dir</c> via <see cref="PnpmInstallSettings.Dir"/></li><li><c>--filter</c> via <see cref="PnpmInstallSettings.Filters"/></li><li><c>--fix-lockfile</c> via <see cref="PnpmInstallSettings.FixLockfile"/></li><li><c>--force</c> via <see cref="PnpmInstallSettings.Force"/></li><li><c>--frozen-lockfile</c> via <see cref="PnpmInstallSettings.FrozenLockfile"/></li><li><c>--global</c> via <see cref="PnpmInstallSettings.Global"/></li><li><c>--ignore-scripts</c> via <see cref="PnpmInstallSettings.IgnoreScripts"/></li><li><c>--lockfile-dir</c> via <see cref="PnpmInstallSettings.LockfileDir"/></li><li><c>--lockfile-only</c> via <see cref="PnpmInstallSettings.LockfileOnly"/></li><li><c>--loglevel</c> via <see cref="PnpmInstallSettings.LogLevel"/></li><li><c>--modules-dir</c> via <see cref="PnpmInstallSettings.ModulesDir"/></li><li><c>--network-concurrency</c> via <see cref="PnpmInstallSettings.NetworkConcurrency"/></li><li><c>--no-frozen-lockfile</c> via <see cref="PnpmInstallSettings.NoFrozenLockfile"/></li><li><c>--no-lockfile</c> via <see cref="PnpmInstallSettings.NoLockfile"/></li><li><c>--no-optional</c> via <see cref="PnpmInstallSettings.NoOptional"/></li><li><c>--offline</c> via <see cref="PnpmInstallSettings.Offline"/></li><li><c>--package-import-method</c> via <see cref="PnpmInstallSettings.PackageImportMethod"/></li><li><c>--prefer-frozen-lockfile</c> via <see cref="PnpmInstallSettings.PreferFrozenLockfile"/></li><li><c>--prefer-offline</c> via <see cref="PnpmInstallSettings.PreferOffline"/></li><li><c>--prod</c> via <see cref="PnpmInstallSettings.Production"/></li><li><c>--recursive</c> via <see cref="PnpmInstallSettings.Recursive"/></li><li><c>--reporter</c> via <see cref="PnpmInstallSettings.Reporter"/></li><li><c>--shamefully-hoist</c> via <see cref="PnpmInstallSettings.ShamefullyHoist"/></li><li><c>--silent</c> via <see cref="PnpmInstallSettings.Silent"/></li><li><c>--store-dir</c> via <see cref="PnpmInstallSettings.StoreDir"/></li><li><c>--strict-peer-dependencies</c> via <see cref="PnpmInstallSettings.StrictPeerDependencies"/></li><li><c>--virtual-store-dir</c> via <see cref="PnpmInstallSettings.VirtualStoreDir"/></li><li><c>--workspace-root</c> via <see cref="PnpmInstallSettings.WorkspaceRoot"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PnpmInstall(PnpmInstallSettings options = null) => new PnpmTasks().Run<PnpmInstallSettings>(options);
    /// <inheritdoc cref="PnpmTasks.PnpmInstall(Fallout.Common.Tools.Pnpm.PnpmInstallSettings)"/>
    public static IReadOnlyCollection<Output> PnpmInstall(Configure<PnpmInstallSettings> configurator) => new PnpmTasks().Run<PnpmInstallSettings>(configurator.Invoke(new PnpmInstallSettings()));
    /// <inheritdoc cref="PnpmTasks.PnpmInstall(Fallout.Common.Tools.Pnpm.PnpmInstallSettings)"/>
    public static IEnumerable<(PnpmInstallSettings Settings, IReadOnlyCollection<Output> Output)> PnpmInstall(CombinatorialConfigure<PnpmInstallSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PnpmInstall, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Runs an arbitrary command from a package's <c>"scripts"</c> object.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;command&gt;</c> via <see cref="PnpmRunSettings.Command"/></li><li><c>--</c> via <see cref="PnpmRunSettings.Arguments"/></li><li><c>--dir</c> via <see cref="PnpmRunSettings.Dir"/></li><li><c>--filter</c> via <see cref="PnpmRunSettings.Filters"/></li><li><c>--if-present</c> via <see cref="PnpmRunSettings.IfPresent"/></li><li><c>--loglevel</c> via <see cref="PnpmRunSettings.LogLevel"/></li><li><c>--no-bail</c> via <see cref="PnpmRunSettings.NoBail"/></li><li><c>--parallel</c> via <see cref="PnpmRunSettings.Parallel"/></li><li><c>--recursive</c> via <see cref="PnpmRunSettings.Recursive"/></li><li><c>--reporter</c> via <see cref="PnpmRunSettings.Reporter"/></li><li><c>--sequential</c> via <see cref="PnpmRunSettings.Sequential"/></li><li><c>--silent</c> via <see cref="PnpmRunSettings.Silent"/></li><li><c>--stream</c> via <see cref="PnpmRunSettings.Stream"/></li><li><c>--workspace-root</c> via <see cref="PnpmRunSettings.WorkspaceRoot"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PnpmRun(PnpmRunSettings options = null) => new PnpmTasks().Run<PnpmRunSettings>(options);
    /// <inheritdoc cref="PnpmTasks.PnpmRun(Fallout.Common.Tools.Pnpm.PnpmRunSettings)"/>
    public static IReadOnlyCollection<Output> PnpmRun(Configure<PnpmRunSettings> configurator) => new PnpmTasks().Run<PnpmRunSettings>(configurator.Invoke(new PnpmRunSettings()));
    /// <inheritdoc cref="PnpmTasks.PnpmRun(Fallout.Common.Tools.Pnpm.PnpmRunSettings)"/>
    public static IEnumerable<(PnpmRunSettings Settings, IReadOnlyCollection<Output> Output)> PnpmRun(CombinatorialConfigure<PnpmRunSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PnpmRun, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Executes a shell command in scope of a project.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;arguments&gt;</c> via <see cref="PnpmExecSettings.Arguments"/></li><li><c>&lt;command&gt;</c> via <see cref="PnpmExecSettings.Command"/></li><li><c>--dir</c> via <see cref="PnpmExecSettings.Dir"/></li><li><c>--filter</c> via <see cref="PnpmExecSettings.Filters"/></li><li><c>--loglevel</c> via <see cref="PnpmExecSettings.LogLevel"/></li><li><c>--parallel</c> via <see cref="PnpmExecSettings.Parallel"/></li><li><c>--recursive</c> via <see cref="PnpmExecSettings.Recursive"/></li><li><c>--shell-mode</c> via <see cref="PnpmExecSettings.ShellMode"/></li><li><c>--silent</c> via <see cref="PnpmExecSettings.Silent"/></li><li><c>--stream</c> via <see cref="PnpmExecSettings.Stream"/></li><li><c>--workspace-root</c> via <see cref="PnpmExecSettings.WorkspaceRoot"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PnpmExec(PnpmExecSettings options = null) => new PnpmTasks().Run<PnpmExecSettings>(options);
    /// <inheritdoc cref="PnpmTasks.PnpmExec(Fallout.Common.Tools.Pnpm.PnpmExecSettings)"/>
    public static IReadOnlyCollection<Output> PnpmExec(Configure<PnpmExecSettings> configurator) => new PnpmTasks().Run<PnpmExecSettings>(configurator.Invoke(new PnpmExecSettings()));
    /// <inheritdoc cref="PnpmTasks.PnpmExec(Fallout.Common.Tools.Pnpm.PnpmExecSettings)"/>
    public static IEnumerable<(PnpmExecSettings Settings, IReadOnlyCollection<Output> Output)> PnpmExec(CombinatorialConfigure<PnpmExecSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PnpmExec, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Fetches a package from the registry without installing it as a dependency, hot loads it, and runs whatever default command binary it exposes.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;arguments&gt;</c> via <see cref="PnpmDlxSettings.Arguments"/></li><li><c>&lt;package&gt;</c> via <see cref="PnpmDlxSettings.Package"/></li><li><c>--dir</c> via <see cref="PnpmDlxSettings.Dir"/></li><li><c>--silent</c> via <see cref="PnpmDlxSettings.Silent"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PnpmDlx(PnpmDlxSettings options = null) => new PnpmTasks().Run<PnpmDlxSettings>(options);
    /// <inheritdoc cref="PnpmTasks.PnpmDlx(Fallout.Common.Tools.Pnpm.PnpmDlxSettings)"/>
    public static IReadOnlyCollection<Output> PnpmDlx(Configure<PnpmDlxSettings> configurator) => new PnpmTasks().Run<PnpmDlxSettings>(configurator.Invoke(new PnpmDlxSettings()));
    /// <inheritdoc cref="PnpmTasks.PnpmDlx(Fallout.Common.Tools.Pnpm.PnpmDlxSettings)"/>
    public static IEnumerable<(PnpmDlxSettings Settings, IReadOnlyCollection<Output> Output)> PnpmDlx(CombinatorialConfigure<PnpmDlxSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PnpmDlx, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Publishes a package to the npm registry.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;target&gt;</c> via <see cref="PnpmPublishSettings.Target"/></li><li><c>--access</c> via <see cref="PnpmPublishSettings.Access"/></li><li><c>--dir</c> via <see cref="PnpmPublishSettings.Dir"/></li><li><c>--dry-run</c> via <see cref="PnpmPublishSettings.DryRun"/></li><li><c>--force</c> via <see cref="PnpmPublishSettings.Force"/></li><li><c>--ignore-scripts</c> via <see cref="PnpmPublishSettings.IgnoreScripts"/></li><li><c>--json</c> via <see cref="PnpmPublishSettings.Json"/></li><li><c>--no-git-checks</c> via <see cref="PnpmPublishSettings.NoGitChecks"/></li><li><c>--otp</c> via <see cref="PnpmPublishSettings.Otp"/></li><li><c>--publish-branch</c> via <see cref="PnpmPublishSettings.PublishBranch"/></li><li><c>--recursive</c> via <see cref="PnpmPublishSettings.Recursive"/></li><li><c>--report-summary</c> via <see cref="PnpmPublishSettings.ReportSummary"/></li><li><c>--tag</c> via <see cref="PnpmPublishSettings.Tag"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PnpmPublish(PnpmPublishSettings options = null) => new PnpmTasks().Run<PnpmPublishSettings>(options);
    /// <inheritdoc cref="PnpmTasks.PnpmPublish(Fallout.Common.Tools.Pnpm.PnpmPublishSettings)"/>
    public static IReadOnlyCollection<Output> PnpmPublish(Configure<PnpmPublishSettings> configurator) => new PnpmTasks().Run<PnpmPublishSettings>(configurator.Invoke(new PnpmPublishSettings()));
    /// <inheritdoc cref="PnpmTasks.PnpmPublish(Fallout.Common.Tools.Pnpm.PnpmPublishSettings)"/>
    public static IEnumerable<(PnpmPublishSettings Settings, IReadOnlyCollection<Output> Output)> PnpmPublish(CombinatorialConfigure<PnpmPublishSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PnpmPublish, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Creates a tarball from a package.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--dir</c> via <see cref="PnpmPackSettings.Dir"/></li><li><c>--dry-run</c> via <see cref="PnpmPackSettings.DryRun"/></li><li><c>--filter</c> via <see cref="PnpmPackSettings.Filters"/></li><li><c>--json</c> via <see cref="PnpmPackSettings.Json"/></li><li><c>--out</c> via <see cref="PnpmPackSettings.Out"/></li><li><c>--pack-destination</c> via <see cref="PnpmPackSettings.PackDestination"/></li><li><c>--recursive</c> via <see cref="PnpmPackSettings.Recursive"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PnpmPack(PnpmPackSettings options = null) => new PnpmTasks().Run<PnpmPackSettings>(options);
    /// <inheritdoc cref="PnpmTasks.PnpmPack(Fallout.Common.Tools.Pnpm.PnpmPackSettings)"/>
    public static IReadOnlyCollection<Output> PnpmPack(Configure<PnpmPackSettings> configurator) => new PnpmTasks().Run<PnpmPackSettings>(configurator.Invoke(new PnpmPackSettings()));
    /// <inheritdoc cref="PnpmTasks.PnpmPack(Fallout.Common.Tools.Pnpm.PnpmPackSettings)"/>
    public static IEnumerable<(PnpmPackSettings Settings, IReadOnlyCollection<Output> Output)> PnpmPack(CombinatorialConfigure<PnpmPackSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PnpmPack, degreeOfParallelism, completeOnFailure);
    /// <summary><p>Checks for known security issues with the installed packages.</p><p>For more details, visit the <a href="https://pnpm.io/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--audit-level</c> via <see cref="PnpmAuditSettings.AuditLevel"/></li><li><c>--dev</c> via <see cref="PnpmAuditSettings.Dev"/></li><li><c>--dir</c> via <see cref="PnpmAuditSettings.Dir"/></li><li><c>--fix</c> via <see cref="PnpmAuditSettings.Fix"/></li><li><c>--ignore-registry-errors</c> via <see cref="PnpmAuditSettings.IgnoreRegistryErrors"/></li><li><c>--ignore-unfixable</c> via <see cref="PnpmAuditSettings.IgnoreUnfixable"/></li><li><c>--json</c> via <see cref="PnpmAuditSettings.Json"/></li><li><c>--no-optional</c> via <see cref="PnpmAuditSettings.NoOptional"/></li><li><c>--prod</c> via <see cref="PnpmAuditSettings.Prod"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> PnpmAudit(PnpmAuditSettings options = null) => new PnpmTasks().Run<PnpmAuditSettings>(options);
    /// <inheritdoc cref="PnpmTasks.PnpmAudit(Fallout.Common.Tools.Pnpm.PnpmAuditSettings)"/>
    public static IReadOnlyCollection<Output> PnpmAudit(Configure<PnpmAuditSettings> configurator) => new PnpmTasks().Run<PnpmAuditSettings>(configurator.Invoke(new PnpmAuditSettings()));
    /// <inheritdoc cref="PnpmTasks.PnpmAudit(Fallout.Common.Tools.Pnpm.PnpmAuditSettings)"/>
    public static IEnumerable<(PnpmAuditSettings Settings, IReadOnlyCollection<Output> Output)> PnpmAudit(CombinatorialConfigure<PnpmAuditSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(PnpmAudit, degreeOfParallelism, completeOnFailure);
}
#region PnpmCiSettings
/// <inheritdoc cref="PnpmTasks.PnpmCi(Fallout.Common.Tools.Pnpm.PnpmCiSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PnpmTasks), Command = nameof(PnpmTasks.PnpmCi), Arguments = "ci")]
public partial class PnpmCiSettings : ToolOptions
{
    /// <summary>Change to directory <c>&lt;dir&gt;</c>.</summary>
    [Argument(Format = "--dir {value}")] public string Dir => Get<string>(() => Dir);
    /// <summary>Controls how output is reported.</summary>
    [Argument(Format = "--reporter {value}")] public PnpmReporter Reporter => Get<PnpmReporter>(() => Reporter);
    /// <summary>What level of logs to report.</summary>
    [Argument(Format = "--loglevel {value}")] public PnpmLogLevel LogLevel => Get<PnpmLogLevel>(() => LogLevel);
    /// <summary>No output is logged to the console, not even fatal errors.</summary>
    [Argument(Format = "--silent")] public bool? Silent => Get<bool?>(() => Silent);
}
#endregion
#region PnpmInstallSettings
/// <inheritdoc cref="PnpmTasks.PnpmInstall(Fallout.Common.Tools.Pnpm.PnpmInstallSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PnpmTasks), Command = nameof(PnpmTasks.PnpmInstall), Arguments = "install")]
public partial class PnpmInstallSettings : ToolOptions
{
    /// <summary>List of packages to be installed.</summary>
    [Argument(Format = "{value}", Position = 1)] public IReadOnlyList<string> Packages => Get<List<string>>(() => Packages);
    /// <summary>Packages in <c>devDependencies</c> will not be installed.</summary>
    [Argument(Format = "--prod")] public bool? Production => Get<bool?>(() => Production);
    /// <summary>Only <c>devDependencies</c> will be installed.</summary>
    [Argument(Format = "--dev")] public bool? Dev => Get<bool?>(() => Dev);
    /// <summary>Prevents <c>optionalDependencies</c> from being installed.</summary>
    [Argument(Format = "--no-optional")] public bool? NoOptional => Get<bool?>(() => NoOptional);
    /// <summary>Don't generate a lockfile and fail if an update is needed. This setting is on by default in CI environments.</summary>
    [Argument(Format = "--frozen-lockfile")] public bool? FrozenLockfile => Get<bool?>(() => FrozenLockfile);
    /// <summary>Forces generating or updating a lockfile even in CI environments.</summary>
    [Argument(Format = "--no-frozen-lockfile")] public bool? NoFrozenLockfile => Get<bool?>(() => NoFrozenLockfile);
    /// <summary>If the available <c>pnpm-lock.yaml</c> satisfies the <c>package.json</c> then perform a headless installation.</summary>
    [Argument(Format = "--prefer-frozen-lockfile")] public bool? PreferFrozenLockfile => Get<bool?>(() => PreferFrozenLockfile);
    /// <summary>Dependencies are not downloaded. Only <c>pnpm-lock.yaml</c> is updated.</summary>
    [Argument(Format = "--lockfile-only")] public bool? LockfileOnly => Get<bool?>(() => LockfileOnly);
    /// <summary>Don't read or generate a <c>pnpm-lock.yaml</c> file.</summary>
    [Argument(Format = "--no-lockfile")] public bool? NoLockfile => Get<bool?>(() => NoLockfile);
    /// <summary>Fix broken lockfile entries automatically.</summary>
    [Argument(Format = "--fix-lockfile")] public bool? FixLockfile => Get<bool?>(() => FixLockfile);
    /// <summary>Forces reinstalling dependencies: refetch packages modified in store, recreate a lockfile and/or modules directory created by a non-compatible version of pnpm.</summary>
    [Argument(Format = "--force")] public bool? Force => Get<bool?>(() => Force);
    /// <summary>Install package globally.</summary>
    [Argument(Format = "--global")] public bool? Global => Get<bool?>(() => Global);
    /// <summary>Causes pnpm to not execute any scripts defined in the <c>package.json</c>.</summary>
    [Argument(Format = "--ignore-scripts")] public bool? IgnoreScripts => Get<bool?>(() => IgnoreScripts);
    /// <summary>Trigger an error if any required dependencies are not available in local store.</summary>
    [Argument(Format = "--offline")] public bool? Offline => Get<bool?>(() => Offline);
    /// <summary>Skip staleness checks for cached data, but request missing data from the server.</summary>
    [Argument(Format = "--prefer-offline")] public bool? PreferOffline => Get<bool?>(() => PreferOffline);
    /// <summary>The directory in which all the packages are saved on disk.</summary>
    [Argument(Format = "--store-dir {value}")] public string StoreDir => Get<string>(() => StoreDir);
    /// <summary>The directory with links to the store (default is <c>node_modules/.pnpm</c>).</summary>
    [Argument(Format = "--virtual-store-dir {value}")] public string VirtualStoreDir => Get<string>(() => VirtualStoreDir);
    /// <summary>The directory in which dependencies will be installed (instead of <c>node_modules</c>).</summary>
    [Argument(Format = "--modules-dir {value}")] public string ModulesDir => Get<string>(() => ModulesDir);
    /// <summary>The directory in which the <c>pnpm-lock.yaml</c> will be created.</summary>
    [Argument(Format = "--lockfile-dir {value}")] public string LockfileDir => Get<string>(() => LockfileDir);
    /// <summary>Change to directory <c>&lt;dir&gt;</c>.</summary>
    [Argument(Format = "--dir {value}")] public string Dir => Get<string>(() => Dir);
    /// <summary>Run installation recursively in every package found in subdirectories.</summary>
    [Argument(Format = "--recursive")] public bool? Recursive => Get<bool?>(() => Recursive);
    /// <summary>Restricts the scope to package names matching the given pattern.</summary>
    [Argument(Format = "--filter {value}")] public IReadOnlyList<string> Filters => Get<List<string>>(() => Filters);
    /// <summary>Run the command on the root workspace project.</summary>
    [Argument(Format = "--workspace-root")] public bool? WorkspaceRoot => Get<bool?>(() => WorkspaceRoot);
    /// <summary>All the subdeps will be hoisted into the root <c>node_modules</c>.</summary>
    [Argument(Format = "--shamefully-hoist")] public bool? ShamefullyHoist => Get<bool?>(() => ShamefullyHoist);
    /// <summary>Controls the number of child processes run parallelly to build node modules.</summary>
    [Argument(Format = "--child-concurrency {value}")] public int? ChildConcurrency => Get<int?>(() => ChildConcurrency);
    /// <summary>Maximum number of concurrent network requests.</summary>
    [Argument(Format = "--network-concurrency {value}")] public int? NetworkConcurrency => Get<int?>(() => NetworkConcurrency);
    /// <summary>Fail on missing or invalid peer dependencies.</summary>
    [Argument(Format = "--strict-peer-dependencies")] public bool? StrictPeerDependencies => Get<bool?>(() => StrictPeerDependencies);
    /// <summary>Method used to clone, hardlink, or copy packages from the store.</summary>
    [Argument(Format = "--package-import-method {value}")] public PnpmPackageImportMethod PackageImportMethod => Get<PnpmPackageImportMethod>(() => PackageImportMethod);
    /// <summary>Controls how output is reported.</summary>
    [Argument(Format = "--reporter {value}")] public PnpmReporter Reporter => Get<PnpmReporter>(() => Reporter);
    /// <summary>What level of logs to report.</summary>
    [Argument(Format = "--loglevel {value}")] public PnpmLogLevel LogLevel => Get<PnpmLogLevel>(() => LogLevel);
    /// <summary>No output is logged to the console, not even fatal errors.</summary>
    [Argument(Format = "--silent")] public bool? Silent => Get<bool?>(() => Silent);
    /// <summary>Aggregate output from child processes that are run in parallel.</summary>
    [Argument(Format = "--aggregate-output")] public bool? AggregateOutput => Get<bool?>(() => AggregateOutput);
}
#endregion
#region PnpmRunSettings
/// <inheritdoc cref="PnpmTasks.PnpmRun(Fallout.Common.Tools.Pnpm.PnpmRunSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PnpmTasks), Command = nameof(PnpmTasks.PnpmRun), Arguments = "run")]
public partial class PnpmRunSettings : ToolOptions
{
    /// <summary>The command to be executed.</summary>
    [Argument(Format = "{value}", Position = 1)] public string Command => Get<string>(() => Command);
    /// <summary>Arguments passed to the script.</summary>
    [Argument(Format = "-- {value}", Position = -1, Separator = " ")] public IReadOnlyList<string> Arguments => Get<List<string>>(() => Arguments);
    /// <summary>Run the defined package script in every package found in subdirectories or workspace.</summary>
    [Argument(Format = "--recursive")] public bool? Recursive => Get<bool?>(() => Recursive);
    /// <summary>Restricts the scope to package names matching the given pattern.</summary>
    [Argument(Format = "--filter {value}")] public IReadOnlyList<string> Filters => Get<List<string>>(() => Filters);
    /// <summary>Completely disregard concurrency and topological sorting, running a given script immediately in all matching packages.</summary>
    [Argument(Format = "--parallel")] public bool? Parallel => Get<bool?>(() => Parallel);
    /// <summary>Run the specified scripts one by one.</summary>
    [Argument(Format = "--sequential")] public bool? Sequential => Get<bool?>(() => Sequential);
    /// <summary>Avoid exiting with a non-zero exit code when the script is undefined.</summary>
    [Argument(Format = "--if-present")] public bool? IfPresent => Get<bool?>(() => IfPresent);
    /// <summary>The command will exit with a 0 exit code even if the script fails.</summary>
    [Argument(Format = "--no-bail")] public bool? NoBail => Get<bool?>(() => NoBail);
    /// <summary>Stream output from child processes immediately, prefixed with the originating package directory.</summary>
    [Argument(Format = "--stream")] public bool? Stream => Get<bool?>(() => Stream);
    /// <summary>Change to directory <c>&lt;dir&gt;</c>.</summary>
    [Argument(Format = "--dir {value}")] public string Dir => Get<string>(() => Dir);
    /// <summary>Run the command on the root workspace project.</summary>
    [Argument(Format = "--workspace-root")] public bool? WorkspaceRoot => Get<bool?>(() => WorkspaceRoot);
    /// <summary>Controls how output is reported.</summary>
    [Argument(Format = "--reporter {value}")] public PnpmReporter Reporter => Get<PnpmReporter>(() => Reporter);
    /// <summary>What level of logs to report.</summary>
    [Argument(Format = "--loglevel {value}")] public PnpmLogLevel LogLevel => Get<PnpmLogLevel>(() => LogLevel);
    /// <summary>No output is logged to the console, not even fatal errors.</summary>
    [Argument(Format = "--silent")] public bool? Silent => Get<bool?>(() => Silent);
}
#endregion
#region PnpmExecSettings
/// <inheritdoc cref="PnpmTasks.PnpmExec(Fallout.Common.Tools.Pnpm.PnpmExecSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PnpmTasks), Command = nameof(PnpmTasks.PnpmExec), Arguments = "exec")]
public partial class PnpmExecSettings : ToolOptions
{
    /// <summary>The command to be executed.</summary>
    [Argument(Format = "{value}", Position = 1)] public string Command => Get<string>(() => Command);
    /// <summary>Arguments passed to the command.</summary>
    [Argument(Format = "{value}", Position = -1, Separator = " ")] public IReadOnlyList<string> Arguments => Get<List<string>>(() => Arguments);
    /// <summary>Run the shell command in every package found in subdirectories or workspace.</summary>
    [Argument(Format = "--recursive")] public bool? Recursive => Get<bool?>(() => Recursive);
    /// <summary>Restricts the scope to package names matching the given pattern.</summary>
    [Argument(Format = "--filter {value}")] public IReadOnlyList<string> Filters => Get<List<string>>(() => Filters);
    /// <summary>Completely disregard concurrency and topological sorting, running a given command immediately in all matching packages.</summary>
    [Argument(Format = "--parallel")] public bool? Parallel => Get<bool?>(() => Parallel);
    /// <summary>Runs command inside of a shell (/bin/sh on UNIX and cmd.exe on Windows).</summary>
    [Argument(Format = "--shell-mode")] public bool? ShellMode => Get<bool?>(() => ShellMode);
    /// <summary>Stream output from child processes immediately, prefixed with the originating package directory.</summary>
    [Argument(Format = "--stream")] public bool? Stream => Get<bool?>(() => Stream);
    /// <summary>Change to directory <c>&lt;dir&gt;</c>.</summary>
    [Argument(Format = "--dir {value}")] public string Dir => Get<string>(() => Dir);
    /// <summary>Run the command on the root workspace project.</summary>
    [Argument(Format = "--workspace-root")] public bool? WorkspaceRoot => Get<bool?>(() => WorkspaceRoot);
    /// <summary>What level of logs to report.</summary>
    [Argument(Format = "--loglevel {value}")] public PnpmLogLevel LogLevel => Get<PnpmLogLevel>(() => LogLevel);
    /// <summary>No output is logged to the console, not even fatal errors.</summary>
    [Argument(Format = "--silent")] public bool? Silent => Get<bool?>(() => Silent);
}
#endregion
#region PnpmDlxSettings
/// <inheritdoc cref="PnpmTasks.PnpmDlx(Fallout.Common.Tools.Pnpm.PnpmDlxSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PnpmTasks), Command = nameof(PnpmTasks.PnpmDlx), Arguments = "dlx")]
public partial class PnpmDlxSettings : ToolOptions
{
    /// <summary>The package binary to execute.</summary>
    [Argument(Format = "{value}", Position = 1)] public string Package => Get<string>(() => Package);
    /// <summary>Arguments passed to the package binary.</summary>
    [Argument(Format = "{value}", Position = -1, Separator = " ")] public IReadOnlyList<string> Arguments => Get<List<string>>(() => Arguments);
    /// <summary>Change to directory <c>&lt;dir&gt;</c>.</summary>
    [Argument(Format = "--dir {value}")] public string Dir => Get<string>(() => Dir);
    /// <summary>No output is logged to the console, not even fatal errors.</summary>
    [Argument(Format = "--silent")] public bool? Silent => Get<bool?>(() => Silent);
}
#endregion
#region PnpmPublishSettings
/// <inheritdoc cref="PnpmTasks.PnpmPublish(Fallout.Common.Tools.Pnpm.PnpmPublishSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PnpmTasks), Command = nameof(PnpmTasks.PnpmPublish), Arguments = "publish")]
public partial class PnpmPublishSettings : ToolOptions
{
    /// <summary>A tarball or directory to publish.</summary>
    [Argument(Format = "{value}", Position = 1)] public string Target => Get<string>(() => Target);
    /// <summary>Registers the published package with the given tag.</summary>
    [Argument(Format = "--tag {value}")] public string Tag => Get<string>(() => Tag);
    /// <summary>Tells the registry whether this package should be published as public or restricted.</summary>
    [Argument(Format = "--access {value}")] public PnpmPublishAccess Access => Get<PnpmPublishAccess>(() => Access);
    /// <summary>Packages are proceeded to be published even if their current version is already in the registry.</summary>
    [Argument(Format = "--force")] public bool? Force => Get<bool?>(() => Force);
    /// <summary>Does everything a publish would do except actually publishing to the registry.</summary>
    [Argument(Format = "--dry-run")] public bool? DryRun => Get<bool?>(() => DryRun);
    /// <summary>Show information in JSON format.</summary>
    [Argument(Format = "--json")] public bool? Json => Get<bool?>(() => Json);
    /// <summary>Don't check if current branch is your publish branch, clean, and up to date.</summary>
    [Argument(Format = "--no-git-checks")] public bool? NoGitChecks => Get<bool?>(() => NoGitChecks);
    /// <summary>When publishing packages that require two-factor authentication, this option can specify a one-time password.</summary>
    [Argument(Format = "--otp {value}", Secret = true)] public string Otp => Get<string>(() => Otp);
    /// <summary>Sets branch name to publish.</summary>
    [Argument(Format = "--publish-branch {value}")] public string PublishBranch => Get<string>(() => PublishBranch);
    /// <summary>Publish all packages from the workspace.</summary>
    [Argument(Format = "--recursive")] public bool? Recursive => Get<bool?>(() => Recursive);
    /// <summary>Save the list of the newly published packages to <c>pnpm-publish-summary.json</c>.</summary>
    [Argument(Format = "--report-summary")] public bool? ReportSummary => Get<bool?>(() => ReportSummary);
    /// <summary>Ignores any publish related lifecycle scripts.</summary>
    [Argument(Format = "--ignore-scripts")] public bool? IgnoreScripts => Get<bool?>(() => IgnoreScripts);
    /// <summary>Change to directory <c>&lt;dir&gt;</c>.</summary>
    [Argument(Format = "--dir {value}")] public string Dir => Get<string>(() => Dir);
}
#endregion
#region PnpmPackSettings
/// <inheritdoc cref="PnpmTasks.PnpmPack(Fallout.Common.Tools.Pnpm.PnpmPackSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PnpmTasks), Command = nameof(PnpmTasks.PnpmPack), Arguments = "pack")]
public partial class PnpmPackSettings : ToolOptions
{
    /// <summary>Customizes the output path for the tarball.</summary>
    [Argument(Format = "--out {value}")] public string Out => Get<string>(() => Out);
    /// <summary>Directory in which <c>pnpm pack</c> will save tarballs.</summary>
    [Argument(Format = "--pack-destination {value}")] public string PackDestination => Get<string>(() => PackDestination);
    /// <summary>Does everything <c>pnpm pack</c> would do except actually writing the tarball to disk.</summary>
    [Argument(Format = "--dry-run")] public bool? DryRun => Get<bool?>(() => DryRun);
    /// <summary>Prints the packed tarball and contents in JSON format.</summary>
    [Argument(Format = "--json")] public bool? Json => Get<bool?>(() => Json);
    /// <summary>Pack all packages from the workspace.</summary>
    [Argument(Format = "--recursive")] public bool? Recursive => Get<bool?>(() => Recursive);
    /// <summary>Restricts the scope to package names matching the given pattern.</summary>
    [Argument(Format = "--filter {value}")] public IReadOnlyList<string> Filters => Get<List<string>>(() => Filters);
    /// <summary>Change to directory <c>&lt;dir&gt;</c>.</summary>
    [Argument(Format = "--dir {value}")] public string Dir => Get<string>(() => Dir);
}
#endregion
#region PnpmAuditSettings
/// <inheritdoc cref="PnpmTasks.PnpmAudit(Fallout.Common.Tools.Pnpm.PnpmAuditSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(PnpmTasks), Command = nameof(PnpmTasks.PnpmAudit), Arguments = "audit")]
public partial class PnpmAuditSettings : ToolOptions
{
    /// <summary>Only print advisories with severity greater than or equal to the given level.</summary>
    [Argument(Format = "--audit-level {value}")] public PnpmAuditLevel AuditLevel => Get<PnpmAuditLevel>(() => AuditLevel);
    /// <summary>Only audit <c>devDependencies</c>.</summary>
    [Argument(Format = "--dev")] public bool? Dev => Get<bool?>(() => Dev);
    /// <summary>Only audit <c>dependencies</c> and <c>optionalDependencies</c>.</summary>
    [Argument(Format = "--prod")] public bool? Prod => Get<bool?>(() => Prod);
    /// <summary>Don't audit <c>optionalDependencies</c>.</summary>
    [Argument(Format = "--no-optional")] public bool? NoOptional => Get<bool?>(() => NoOptional);
    /// <summary>Fix the audited vulnerabilities using the specified method: <c>override</c> or <c>update</c>.</summary>
    [Argument(Format = "--fix {value}")] public PnpmAuditFix Fix => Get<PnpmAuditFix>(() => Fix);
    /// <summary>Use exit code 0 if the registry responds with an error. Useful in CI.</summary>
    [Argument(Format = "--ignore-registry-errors")] public bool? IgnoreRegistryErrors => Get<bool?>(() => IgnoreRegistryErrors);
    /// <summary>Ignore all vulnerabilities for which no fix exists.</summary>
    [Argument(Format = "--ignore-unfixable")] public bool? IgnoreUnfixable => Get<bool?>(() => IgnoreUnfixable);
    /// <summary>Output audit report in JSON format.</summary>
    [Argument(Format = "--json")] public bool? Json => Get<bool?>(() => Json);
    /// <summary>Change to directory <c>&lt;dir&gt;</c>.</summary>
    [Argument(Format = "--dir {value}")] public string Dir => Get<string>(() => Dir);
}
#endregion
#region PnpmCiSettingsExtensions
/// <inheritdoc cref="PnpmTasks.PnpmCi(Fallout.Common.Tools.Pnpm.PnpmCiSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class PnpmCiSettingsExtensions
{
    #region Dir
    /// <inheritdoc cref="PnpmCiSettings.Dir"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Dir))]
    public static T SetDir<T>(this T o, string v) where T : PnpmCiSettings => o.Modify(b => b.Set(() => o.Dir, v));
    /// <inheritdoc cref="PnpmCiSettings.Dir"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Dir))]
    public static T ResetDir<T>(this T o) where T : PnpmCiSettings => o.Modify(b => b.Remove(() => o.Dir));
    #endregion
    #region Reporter
    /// <inheritdoc cref="PnpmCiSettings.Reporter"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Reporter))]
    public static T SetReporter<T>(this T o, PnpmReporter v) where T : PnpmCiSettings => o.Modify(b => b.Set(() => o.Reporter, v));
    /// <inheritdoc cref="PnpmCiSettings.Reporter"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Reporter))]
    public static T ResetReporter<T>(this T o) where T : PnpmCiSettings => o.Modify(b => b.Remove(() => o.Reporter));
    #endregion
    #region LogLevel
    /// <inheritdoc cref="PnpmCiSettings.LogLevel"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.LogLevel))]
    public static T SetLogLevel<T>(this T o, PnpmLogLevel v) where T : PnpmCiSettings => o.Modify(b => b.Set(() => o.LogLevel, v));
    /// <inheritdoc cref="PnpmCiSettings.LogLevel"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.LogLevel))]
    public static T ResetLogLevel<T>(this T o) where T : PnpmCiSettings => o.Modify(b => b.Remove(() => o.LogLevel));
    #endregion
    #region Silent
    /// <inheritdoc cref="PnpmCiSettings.Silent"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Silent))]
    public static T SetSilent<T>(this T o, bool? v) where T : PnpmCiSettings => o.Modify(b => b.Set(() => o.Silent, v));
    /// <inheritdoc cref="PnpmCiSettings.Silent"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Silent))]
    public static T ResetSilent<T>(this T o) where T : PnpmCiSettings => o.Modify(b => b.Remove(() => o.Silent));
    /// <inheritdoc cref="PnpmCiSettings.Silent"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Silent))]
    public static T EnableSilent<T>(this T o) where T : PnpmCiSettings => o.Modify(b => b.Set(() => o.Silent, true));
    /// <inheritdoc cref="PnpmCiSettings.Silent"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Silent))]
    public static T DisableSilent<T>(this T o) where T : PnpmCiSettings => o.Modify(b => b.Set(() => o.Silent, false));
    /// <inheritdoc cref="PnpmCiSettings.Silent"/>
    [Builder(Type = typeof(PnpmCiSettings), Property = nameof(PnpmCiSettings.Silent))]
    public static T ToggleSilent<T>(this T o) where T : PnpmCiSettings => o.Modify(b => b.Set(() => o.Silent, !o.Silent));
    #endregion
}
#endregion
#region PnpmInstallSettingsExtensions
/// <inheritdoc cref="PnpmTasks.PnpmInstall(Fallout.Common.Tools.Pnpm.PnpmInstallSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class PnpmInstallSettingsExtensions
{
    #region Packages
    /// <inheritdoc cref="PnpmInstallSettings.Packages"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Packages))]
    public static T SetPackages<T>(this T o, params string[] v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Packages, v));
    /// <inheritdoc cref="PnpmInstallSettings.Packages"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Packages))]
    public static T SetPackages<T>(this T o, IEnumerable<string> v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Packages, v));
    /// <inheritdoc cref="PnpmInstallSettings.Packages"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Packages))]
    public static T AddPackages<T>(this T o, params string[] v) where T : PnpmInstallSettings => o.Modify(b => b.AddCollection(() => o.Packages, v));
    /// <inheritdoc cref="PnpmInstallSettings.Packages"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Packages))]
    public static T AddPackages<T>(this T o, IEnumerable<string> v) where T : PnpmInstallSettings => o.Modify(b => b.AddCollection(() => o.Packages, v));
    /// <inheritdoc cref="PnpmInstallSettings.Packages"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Packages))]
    public static T RemovePackages<T>(this T o, params string[] v) where T : PnpmInstallSettings => o.Modify(b => b.RemoveCollection(() => o.Packages, v));
    /// <inheritdoc cref="PnpmInstallSettings.Packages"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Packages))]
    public static T RemovePackages<T>(this T o, IEnumerable<string> v) where T : PnpmInstallSettings => o.Modify(b => b.RemoveCollection(() => o.Packages, v));
    /// <inheritdoc cref="PnpmInstallSettings.Packages"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Packages))]
    public static T ClearPackages<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.ClearCollection(() => o.Packages));
    #endregion
    #region Production
    /// <inheritdoc cref="PnpmInstallSettings.Production"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Production))]
    public static T SetProduction<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Production, v));
    /// <inheritdoc cref="PnpmInstallSettings.Production"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Production))]
    public static T ResetProduction<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Production));
    /// <inheritdoc cref="PnpmInstallSettings.Production"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Production))]
    public static T EnableProduction<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Production, true));
    /// <inheritdoc cref="PnpmInstallSettings.Production"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Production))]
    public static T DisableProduction<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Production, false));
    /// <inheritdoc cref="PnpmInstallSettings.Production"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Production))]
    public static T ToggleProduction<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Production, !o.Production));
    #endregion
    #region Dev
    /// <inheritdoc cref="PnpmInstallSettings.Dev"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Dev))]
    public static T SetDev<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Dev, v));
    /// <inheritdoc cref="PnpmInstallSettings.Dev"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Dev))]
    public static T ResetDev<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Dev));
    /// <inheritdoc cref="PnpmInstallSettings.Dev"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Dev))]
    public static T EnableDev<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Dev, true));
    /// <inheritdoc cref="PnpmInstallSettings.Dev"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Dev))]
    public static T DisableDev<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Dev, false));
    /// <inheritdoc cref="PnpmInstallSettings.Dev"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Dev))]
    public static T ToggleDev<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Dev, !o.Dev));
    #endregion
    #region NoOptional
    /// <inheritdoc cref="PnpmInstallSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoOptional))]
    public static T SetNoOptional<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoOptional, v));
    /// <inheritdoc cref="PnpmInstallSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoOptional))]
    public static T ResetNoOptional<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.NoOptional));
    /// <inheritdoc cref="PnpmInstallSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoOptional))]
    public static T EnableNoOptional<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoOptional, true));
    /// <inheritdoc cref="PnpmInstallSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoOptional))]
    public static T DisableNoOptional<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoOptional, false));
    /// <inheritdoc cref="PnpmInstallSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoOptional))]
    public static T ToggleNoOptional<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoOptional, !o.NoOptional));
    #endregion
    #region FrozenLockfile
    /// <inheritdoc cref="PnpmInstallSettings.FrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FrozenLockfile))]
    public static T SetFrozenLockfile<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.FrozenLockfile, v));
    /// <inheritdoc cref="PnpmInstallSettings.FrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FrozenLockfile))]
    public static T ResetFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.FrozenLockfile));
    /// <inheritdoc cref="PnpmInstallSettings.FrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FrozenLockfile))]
    public static T EnableFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.FrozenLockfile, true));
    /// <inheritdoc cref="PnpmInstallSettings.FrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FrozenLockfile))]
    public static T DisableFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.FrozenLockfile, false));
    /// <inheritdoc cref="PnpmInstallSettings.FrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FrozenLockfile))]
    public static T ToggleFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.FrozenLockfile, !o.FrozenLockfile));
    #endregion
    #region NoFrozenLockfile
    /// <inheritdoc cref="PnpmInstallSettings.NoFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoFrozenLockfile))]
    public static T SetNoFrozenLockfile<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoFrozenLockfile, v));
    /// <inheritdoc cref="PnpmInstallSettings.NoFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoFrozenLockfile))]
    public static T ResetNoFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.NoFrozenLockfile));
    /// <inheritdoc cref="PnpmInstallSettings.NoFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoFrozenLockfile))]
    public static T EnableNoFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoFrozenLockfile, true));
    /// <inheritdoc cref="PnpmInstallSettings.NoFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoFrozenLockfile))]
    public static T DisableNoFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoFrozenLockfile, false));
    /// <inheritdoc cref="PnpmInstallSettings.NoFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoFrozenLockfile))]
    public static T ToggleNoFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoFrozenLockfile, !o.NoFrozenLockfile));
    #endregion
    #region PreferFrozenLockfile
    /// <inheritdoc cref="PnpmInstallSettings.PreferFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferFrozenLockfile))]
    public static T SetPreferFrozenLockfile<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PreferFrozenLockfile, v));
    /// <inheritdoc cref="PnpmInstallSettings.PreferFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferFrozenLockfile))]
    public static T ResetPreferFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.PreferFrozenLockfile));
    /// <inheritdoc cref="PnpmInstallSettings.PreferFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferFrozenLockfile))]
    public static T EnablePreferFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PreferFrozenLockfile, true));
    /// <inheritdoc cref="PnpmInstallSettings.PreferFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferFrozenLockfile))]
    public static T DisablePreferFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PreferFrozenLockfile, false));
    /// <inheritdoc cref="PnpmInstallSettings.PreferFrozenLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferFrozenLockfile))]
    public static T TogglePreferFrozenLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PreferFrozenLockfile, !o.PreferFrozenLockfile));
    #endregion
    #region LockfileOnly
    /// <inheritdoc cref="PnpmInstallSettings.LockfileOnly"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LockfileOnly))]
    public static T SetLockfileOnly<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.LockfileOnly, v));
    /// <inheritdoc cref="PnpmInstallSettings.LockfileOnly"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LockfileOnly))]
    public static T ResetLockfileOnly<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.LockfileOnly));
    /// <inheritdoc cref="PnpmInstallSettings.LockfileOnly"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LockfileOnly))]
    public static T EnableLockfileOnly<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.LockfileOnly, true));
    /// <inheritdoc cref="PnpmInstallSettings.LockfileOnly"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LockfileOnly))]
    public static T DisableLockfileOnly<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.LockfileOnly, false));
    /// <inheritdoc cref="PnpmInstallSettings.LockfileOnly"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LockfileOnly))]
    public static T ToggleLockfileOnly<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.LockfileOnly, !o.LockfileOnly));
    #endregion
    #region NoLockfile
    /// <inheritdoc cref="PnpmInstallSettings.NoLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoLockfile))]
    public static T SetNoLockfile<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoLockfile, v));
    /// <inheritdoc cref="PnpmInstallSettings.NoLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoLockfile))]
    public static T ResetNoLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.NoLockfile));
    /// <inheritdoc cref="PnpmInstallSettings.NoLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoLockfile))]
    public static T EnableNoLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoLockfile, true));
    /// <inheritdoc cref="PnpmInstallSettings.NoLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoLockfile))]
    public static T DisableNoLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoLockfile, false));
    /// <inheritdoc cref="PnpmInstallSettings.NoLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NoLockfile))]
    public static T ToggleNoLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NoLockfile, !o.NoLockfile));
    #endregion
    #region FixLockfile
    /// <inheritdoc cref="PnpmInstallSettings.FixLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FixLockfile))]
    public static T SetFixLockfile<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.FixLockfile, v));
    /// <inheritdoc cref="PnpmInstallSettings.FixLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FixLockfile))]
    public static T ResetFixLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.FixLockfile));
    /// <inheritdoc cref="PnpmInstallSettings.FixLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FixLockfile))]
    public static T EnableFixLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.FixLockfile, true));
    /// <inheritdoc cref="PnpmInstallSettings.FixLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FixLockfile))]
    public static T DisableFixLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.FixLockfile, false));
    /// <inheritdoc cref="PnpmInstallSettings.FixLockfile"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.FixLockfile))]
    public static T ToggleFixLockfile<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.FixLockfile, !o.FixLockfile));
    #endregion
    #region Force
    /// <inheritdoc cref="PnpmInstallSettings.Force"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Force))]
    public static T SetForce<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Force, v));
    /// <inheritdoc cref="PnpmInstallSettings.Force"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Force))]
    public static T ResetForce<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Force));
    /// <inheritdoc cref="PnpmInstallSettings.Force"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Force))]
    public static T EnableForce<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Force, true));
    /// <inheritdoc cref="PnpmInstallSettings.Force"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Force))]
    public static T DisableForce<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Force, false));
    /// <inheritdoc cref="PnpmInstallSettings.Force"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Force))]
    public static T ToggleForce<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Force, !o.Force));
    #endregion
    #region Global
    /// <inheritdoc cref="PnpmInstallSettings.Global"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Global))]
    public static T SetGlobal<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Global, v));
    /// <inheritdoc cref="PnpmInstallSettings.Global"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Global))]
    public static T ResetGlobal<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Global));
    /// <inheritdoc cref="PnpmInstallSettings.Global"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Global))]
    public static T EnableGlobal<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Global, true));
    /// <inheritdoc cref="PnpmInstallSettings.Global"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Global))]
    public static T DisableGlobal<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Global, false));
    /// <inheritdoc cref="PnpmInstallSettings.Global"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Global))]
    public static T ToggleGlobal<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Global, !o.Global));
    #endregion
    #region IgnoreScripts
    /// <inheritdoc cref="PnpmInstallSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.IgnoreScripts))]
    public static T SetIgnoreScripts<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.IgnoreScripts, v));
    /// <inheritdoc cref="PnpmInstallSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.IgnoreScripts))]
    public static T ResetIgnoreScripts<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.IgnoreScripts));
    /// <inheritdoc cref="PnpmInstallSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.IgnoreScripts))]
    public static T EnableIgnoreScripts<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.IgnoreScripts, true));
    /// <inheritdoc cref="PnpmInstallSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.IgnoreScripts))]
    public static T DisableIgnoreScripts<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.IgnoreScripts, false));
    /// <inheritdoc cref="PnpmInstallSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.IgnoreScripts))]
    public static T ToggleIgnoreScripts<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.IgnoreScripts, !o.IgnoreScripts));
    #endregion
    #region Offline
    /// <inheritdoc cref="PnpmInstallSettings.Offline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Offline))]
    public static T SetOffline<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Offline, v));
    /// <inheritdoc cref="PnpmInstallSettings.Offline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Offline))]
    public static T ResetOffline<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Offline));
    /// <inheritdoc cref="PnpmInstallSettings.Offline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Offline))]
    public static T EnableOffline<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Offline, true));
    /// <inheritdoc cref="PnpmInstallSettings.Offline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Offline))]
    public static T DisableOffline<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Offline, false));
    /// <inheritdoc cref="PnpmInstallSettings.Offline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Offline))]
    public static T ToggleOffline<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Offline, !o.Offline));
    #endregion
    #region PreferOffline
    /// <inheritdoc cref="PnpmInstallSettings.PreferOffline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferOffline))]
    public static T SetPreferOffline<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PreferOffline, v));
    /// <inheritdoc cref="PnpmInstallSettings.PreferOffline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferOffline))]
    public static T ResetPreferOffline<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.PreferOffline));
    /// <inheritdoc cref="PnpmInstallSettings.PreferOffline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferOffline))]
    public static T EnablePreferOffline<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PreferOffline, true));
    /// <inheritdoc cref="PnpmInstallSettings.PreferOffline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferOffline))]
    public static T DisablePreferOffline<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PreferOffline, false));
    /// <inheritdoc cref="PnpmInstallSettings.PreferOffline"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PreferOffline))]
    public static T TogglePreferOffline<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PreferOffline, !o.PreferOffline));
    #endregion
    #region StoreDir
    /// <inheritdoc cref="PnpmInstallSettings.StoreDir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.StoreDir))]
    public static T SetStoreDir<T>(this T o, string v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.StoreDir, v));
    /// <inheritdoc cref="PnpmInstallSettings.StoreDir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.StoreDir))]
    public static T ResetStoreDir<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.StoreDir));
    #endregion
    #region VirtualStoreDir
    /// <inheritdoc cref="PnpmInstallSettings.VirtualStoreDir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.VirtualStoreDir))]
    public static T SetVirtualStoreDir<T>(this T o, string v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.VirtualStoreDir, v));
    /// <inheritdoc cref="PnpmInstallSettings.VirtualStoreDir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.VirtualStoreDir))]
    public static T ResetVirtualStoreDir<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.VirtualStoreDir));
    #endregion
    #region ModulesDir
    /// <inheritdoc cref="PnpmInstallSettings.ModulesDir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ModulesDir))]
    public static T SetModulesDir<T>(this T o, string v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.ModulesDir, v));
    /// <inheritdoc cref="PnpmInstallSettings.ModulesDir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ModulesDir))]
    public static T ResetModulesDir<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.ModulesDir));
    #endregion
    #region LockfileDir
    /// <inheritdoc cref="PnpmInstallSettings.LockfileDir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LockfileDir))]
    public static T SetLockfileDir<T>(this T o, string v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.LockfileDir, v));
    /// <inheritdoc cref="PnpmInstallSettings.LockfileDir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LockfileDir))]
    public static T ResetLockfileDir<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.LockfileDir));
    #endregion
    #region Dir
    /// <inheritdoc cref="PnpmInstallSettings.Dir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Dir))]
    public static T SetDir<T>(this T o, string v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Dir, v));
    /// <inheritdoc cref="PnpmInstallSettings.Dir"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Dir))]
    public static T ResetDir<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Dir));
    #endregion
    #region Recursive
    /// <inheritdoc cref="PnpmInstallSettings.Recursive"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Recursive))]
    public static T SetRecursive<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Recursive, v));
    /// <inheritdoc cref="PnpmInstallSettings.Recursive"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Recursive))]
    public static T ResetRecursive<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Recursive));
    /// <inheritdoc cref="PnpmInstallSettings.Recursive"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Recursive))]
    public static T EnableRecursive<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Recursive, true));
    /// <inheritdoc cref="PnpmInstallSettings.Recursive"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Recursive))]
    public static T DisableRecursive<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Recursive, false));
    /// <inheritdoc cref="PnpmInstallSettings.Recursive"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Recursive))]
    public static T ToggleRecursive<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Recursive, !o.Recursive));
    #endregion
    #region Filters
    /// <inheritdoc cref="PnpmInstallSettings.Filters"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Filters))]
    public static T SetFilters<T>(this T o, params string[] v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Filters, v));
    /// <inheritdoc cref="PnpmInstallSettings.Filters"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Filters))]
    public static T SetFilters<T>(this T o, IEnumerable<string> v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Filters, v));
    /// <inheritdoc cref="PnpmInstallSettings.Filters"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Filters))]
    public static T AddFilters<T>(this T o, params string[] v) where T : PnpmInstallSettings => o.Modify(b => b.AddCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmInstallSettings.Filters"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Filters))]
    public static T AddFilters<T>(this T o, IEnumerable<string> v) where T : PnpmInstallSettings => o.Modify(b => b.AddCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmInstallSettings.Filters"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Filters))]
    public static T RemoveFilters<T>(this T o, params string[] v) where T : PnpmInstallSettings => o.Modify(b => b.RemoveCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmInstallSettings.Filters"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Filters))]
    public static T RemoveFilters<T>(this T o, IEnumerable<string> v) where T : PnpmInstallSettings => o.Modify(b => b.RemoveCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmInstallSettings.Filters"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Filters))]
    public static T ClearFilters<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.ClearCollection(() => o.Filters));
    #endregion
    #region WorkspaceRoot
    /// <inheritdoc cref="PnpmInstallSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.WorkspaceRoot))]
    public static T SetWorkspaceRoot<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, v));
    /// <inheritdoc cref="PnpmInstallSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.WorkspaceRoot))]
    public static T ResetWorkspaceRoot<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.WorkspaceRoot));
    /// <inheritdoc cref="PnpmInstallSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.WorkspaceRoot))]
    public static T EnableWorkspaceRoot<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, true));
    /// <inheritdoc cref="PnpmInstallSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.WorkspaceRoot))]
    public static T DisableWorkspaceRoot<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, false));
    /// <inheritdoc cref="PnpmInstallSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.WorkspaceRoot))]
    public static T ToggleWorkspaceRoot<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, !o.WorkspaceRoot));
    #endregion
    #region ShamefullyHoist
    /// <inheritdoc cref="PnpmInstallSettings.ShamefullyHoist"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ShamefullyHoist))]
    public static T SetShamefullyHoist<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.ShamefullyHoist, v));
    /// <inheritdoc cref="PnpmInstallSettings.ShamefullyHoist"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ShamefullyHoist))]
    public static T ResetShamefullyHoist<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.ShamefullyHoist));
    /// <inheritdoc cref="PnpmInstallSettings.ShamefullyHoist"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ShamefullyHoist))]
    public static T EnableShamefullyHoist<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.ShamefullyHoist, true));
    /// <inheritdoc cref="PnpmInstallSettings.ShamefullyHoist"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ShamefullyHoist))]
    public static T DisableShamefullyHoist<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.ShamefullyHoist, false));
    /// <inheritdoc cref="PnpmInstallSettings.ShamefullyHoist"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ShamefullyHoist))]
    public static T ToggleShamefullyHoist<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.ShamefullyHoist, !o.ShamefullyHoist));
    #endregion
    #region ChildConcurrency
    /// <inheritdoc cref="PnpmInstallSettings.ChildConcurrency"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ChildConcurrency))]
    public static T SetChildConcurrency<T>(this T o, int? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.ChildConcurrency, v));
    /// <inheritdoc cref="PnpmInstallSettings.ChildConcurrency"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.ChildConcurrency))]
    public static T ResetChildConcurrency<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.ChildConcurrency));
    #endregion
    #region NetworkConcurrency
    /// <inheritdoc cref="PnpmInstallSettings.NetworkConcurrency"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NetworkConcurrency))]
    public static T SetNetworkConcurrency<T>(this T o, int? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.NetworkConcurrency, v));
    /// <inheritdoc cref="PnpmInstallSettings.NetworkConcurrency"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.NetworkConcurrency))]
    public static T ResetNetworkConcurrency<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.NetworkConcurrency));
    #endregion
    #region StrictPeerDependencies
    /// <inheritdoc cref="PnpmInstallSettings.StrictPeerDependencies"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.StrictPeerDependencies))]
    public static T SetStrictPeerDependencies<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.StrictPeerDependencies, v));
    /// <inheritdoc cref="PnpmInstallSettings.StrictPeerDependencies"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.StrictPeerDependencies))]
    public static T ResetStrictPeerDependencies<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.StrictPeerDependencies));
    /// <inheritdoc cref="PnpmInstallSettings.StrictPeerDependencies"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.StrictPeerDependencies))]
    public static T EnableStrictPeerDependencies<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.StrictPeerDependencies, true));
    /// <inheritdoc cref="PnpmInstallSettings.StrictPeerDependencies"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.StrictPeerDependencies))]
    public static T DisableStrictPeerDependencies<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.StrictPeerDependencies, false));
    /// <inheritdoc cref="PnpmInstallSettings.StrictPeerDependencies"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.StrictPeerDependencies))]
    public static T ToggleStrictPeerDependencies<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.StrictPeerDependencies, !o.StrictPeerDependencies));
    #endregion
    #region PackageImportMethod
    /// <inheritdoc cref="PnpmInstallSettings.PackageImportMethod"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PackageImportMethod))]
    public static T SetPackageImportMethod<T>(this T o, PnpmPackageImportMethod v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.PackageImportMethod, v));
    /// <inheritdoc cref="PnpmInstallSettings.PackageImportMethod"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.PackageImportMethod))]
    public static T ResetPackageImportMethod<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.PackageImportMethod));
    #endregion
    #region Reporter
    /// <inheritdoc cref="PnpmInstallSettings.Reporter"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Reporter))]
    public static T SetReporter<T>(this T o, PnpmReporter v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Reporter, v));
    /// <inheritdoc cref="PnpmInstallSettings.Reporter"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Reporter))]
    public static T ResetReporter<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Reporter));
    #endregion
    #region LogLevel
    /// <inheritdoc cref="PnpmInstallSettings.LogLevel"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LogLevel))]
    public static T SetLogLevel<T>(this T o, PnpmLogLevel v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.LogLevel, v));
    /// <inheritdoc cref="PnpmInstallSettings.LogLevel"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.LogLevel))]
    public static T ResetLogLevel<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.LogLevel));
    #endregion
    #region Silent
    /// <inheritdoc cref="PnpmInstallSettings.Silent"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Silent))]
    public static T SetSilent<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Silent, v));
    /// <inheritdoc cref="PnpmInstallSettings.Silent"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Silent))]
    public static T ResetSilent<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.Silent));
    /// <inheritdoc cref="PnpmInstallSettings.Silent"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Silent))]
    public static T EnableSilent<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Silent, true));
    /// <inheritdoc cref="PnpmInstallSettings.Silent"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Silent))]
    public static T DisableSilent<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Silent, false));
    /// <inheritdoc cref="PnpmInstallSettings.Silent"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.Silent))]
    public static T ToggleSilent<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.Silent, !o.Silent));
    #endregion
    #region AggregateOutput
    /// <inheritdoc cref="PnpmInstallSettings.AggregateOutput"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.AggregateOutput))]
    public static T SetAggregateOutput<T>(this T o, bool? v) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.AggregateOutput, v));
    /// <inheritdoc cref="PnpmInstallSettings.AggregateOutput"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.AggregateOutput))]
    public static T ResetAggregateOutput<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Remove(() => o.AggregateOutput));
    /// <inheritdoc cref="PnpmInstallSettings.AggregateOutput"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.AggregateOutput))]
    public static T EnableAggregateOutput<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.AggregateOutput, true));
    /// <inheritdoc cref="PnpmInstallSettings.AggregateOutput"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.AggregateOutput))]
    public static T DisableAggregateOutput<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.AggregateOutput, false));
    /// <inheritdoc cref="PnpmInstallSettings.AggregateOutput"/>
    [Builder(Type = typeof(PnpmInstallSettings), Property = nameof(PnpmInstallSettings.AggregateOutput))]
    public static T ToggleAggregateOutput<T>(this T o) where T : PnpmInstallSettings => o.Modify(b => b.Set(() => o.AggregateOutput, !o.AggregateOutput));
    #endregion
}
#endregion
#region PnpmRunSettingsExtensions
/// <inheritdoc cref="PnpmTasks.PnpmRun(Fallout.Common.Tools.Pnpm.PnpmRunSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class PnpmRunSettingsExtensions
{
    #region Command
    /// <inheritdoc cref="PnpmRunSettings.Command"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Command))]
    public static T SetCommand<T>(this T o, string v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Command, v));
    /// <inheritdoc cref="PnpmRunSettings.Command"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Command))]
    public static T ResetCommand<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.Command));
    #endregion
    #region Arguments
    /// <inheritdoc cref="PnpmRunSettings.Arguments"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Arguments))]
    public static T SetArguments<T>(this T o, params string[] v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmRunSettings.Arguments"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Arguments))]
    public static T SetArguments<T>(this T o, IEnumerable<string> v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmRunSettings.Arguments"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Arguments))]
    public static T AddArguments<T>(this T o, params string[] v) where T : PnpmRunSettings => o.Modify(b => b.AddCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmRunSettings.Arguments"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Arguments))]
    public static T AddArguments<T>(this T o, IEnumerable<string> v) where T : PnpmRunSettings => o.Modify(b => b.AddCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmRunSettings.Arguments"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Arguments))]
    public static T RemoveArguments<T>(this T o, params string[] v) where T : PnpmRunSettings => o.Modify(b => b.RemoveCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmRunSettings.Arguments"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Arguments))]
    public static T RemoveArguments<T>(this T o, IEnumerable<string> v) where T : PnpmRunSettings => o.Modify(b => b.RemoveCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmRunSettings.Arguments"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Arguments))]
    public static T ClearArguments<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.ClearCollection(() => o.Arguments));
    #endregion
    #region Recursive
    /// <inheritdoc cref="PnpmRunSettings.Recursive"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Recursive))]
    public static T SetRecursive<T>(this T o, bool? v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Recursive, v));
    /// <inheritdoc cref="PnpmRunSettings.Recursive"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Recursive))]
    public static T ResetRecursive<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.Recursive));
    /// <inheritdoc cref="PnpmRunSettings.Recursive"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Recursive))]
    public static T EnableRecursive<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Recursive, true));
    /// <inheritdoc cref="PnpmRunSettings.Recursive"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Recursive))]
    public static T DisableRecursive<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Recursive, false));
    /// <inheritdoc cref="PnpmRunSettings.Recursive"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Recursive))]
    public static T ToggleRecursive<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Recursive, !o.Recursive));
    #endregion
    #region Filters
    /// <inheritdoc cref="PnpmRunSettings.Filters"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Filters))]
    public static T SetFilters<T>(this T o, params string[] v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Filters, v));
    /// <inheritdoc cref="PnpmRunSettings.Filters"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Filters))]
    public static T SetFilters<T>(this T o, IEnumerable<string> v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Filters, v));
    /// <inheritdoc cref="PnpmRunSettings.Filters"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Filters))]
    public static T AddFilters<T>(this T o, params string[] v) where T : PnpmRunSettings => o.Modify(b => b.AddCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmRunSettings.Filters"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Filters))]
    public static T AddFilters<T>(this T o, IEnumerable<string> v) where T : PnpmRunSettings => o.Modify(b => b.AddCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmRunSettings.Filters"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Filters))]
    public static T RemoveFilters<T>(this T o, params string[] v) where T : PnpmRunSettings => o.Modify(b => b.RemoveCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmRunSettings.Filters"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Filters))]
    public static T RemoveFilters<T>(this T o, IEnumerable<string> v) where T : PnpmRunSettings => o.Modify(b => b.RemoveCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmRunSettings.Filters"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Filters))]
    public static T ClearFilters<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.ClearCollection(() => o.Filters));
    #endregion
    #region Parallel
    /// <inheritdoc cref="PnpmRunSettings.Parallel"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Parallel))]
    public static T SetParallel<T>(this T o, bool? v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Parallel, v));
    /// <inheritdoc cref="PnpmRunSettings.Parallel"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Parallel))]
    public static T ResetParallel<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.Parallel));
    /// <inheritdoc cref="PnpmRunSettings.Parallel"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Parallel))]
    public static T EnableParallel<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Parallel, true));
    /// <inheritdoc cref="PnpmRunSettings.Parallel"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Parallel))]
    public static T DisableParallel<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Parallel, false));
    /// <inheritdoc cref="PnpmRunSettings.Parallel"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Parallel))]
    public static T ToggleParallel<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Parallel, !o.Parallel));
    #endregion
    #region Sequential
    /// <inheritdoc cref="PnpmRunSettings.Sequential"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Sequential))]
    public static T SetSequential<T>(this T o, bool? v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Sequential, v));
    /// <inheritdoc cref="PnpmRunSettings.Sequential"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Sequential))]
    public static T ResetSequential<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.Sequential));
    /// <inheritdoc cref="PnpmRunSettings.Sequential"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Sequential))]
    public static T EnableSequential<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Sequential, true));
    /// <inheritdoc cref="PnpmRunSettings.Sequential"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Sequential))]
    public static T DisableSequential<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Sequential, false));
    /// <inheritdoc cref="PnpmRunSettings.Sequential"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Sequential))]
    public static T ToggleSequential<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Sequential, !o.Sequential));
    #endregion
    #region IfPresent
    /// <inheritdoc cref="PnpmRunSettings.IfPresent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.IfPresent))]
    public static T SetIfPresent<T>(this T o, bool? v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.IfPresent, v));
    /// <inheritdoc cref="PnpmRunSettings.IfPresent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.IfPresent))]
    public static T ResetIfPresent<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.IfPresent));
    /// <inheritdoc cref="PnpmRunSettings.IfPresent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.IfPresent))]
    public static T EnableIfPresent<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.IfPresent, true));
    /// <inheritdoc cref="PnpmRunSettings.IfPresent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.IfPresent))]
    public static T DisableIfPresent<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.IfPresent, false));
    /// <inheritdoc cref="PnpmRunSettings.IfPresent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.IfPresent))]
    public static T ToggleIfPresent<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.IfPresent, !o.IfPresent));
    #endregion
    #region NoBail
    /// <inheritdoc cref="PnpmRunSettings.NoBail"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.NoBail))]
    public static T SetNoBail<T>(this T o, bool? v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.NoBail, v));
    /// <inheritdoc cref="PnpmRunSettings.NoBail"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.NoBail))]
    public static T ResetNoBail<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.NoBail));
    /// <inheritdoc cref="PnpmRunSettings.NoBail"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.NoBail))]
    public static T EnableNoBail<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.NoBail, true));
    /// <inheritdoc cref="PnpmRunSettings.NoBail"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.NoBail))]
    public static T DisableNoBail<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.NoBail, false));
    /// <inheritdoc cref="PnpmRunSettings.NoBail"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.NoBail))]
    public static T ToggleNoBail<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.NoBail, !o.NoBail));
    #endregion
    #region Stream
    /// <inheritdoc cref="PnpmRunSettings.Stream"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Stream))]
    public static T SetStream<T>(this T o, bool? v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Stream, v));
    /// <inheritdoc cref="PnpmRunSettings.Stream"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Stream))]
    public static T ResetStream<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.Stream));
    /// <inheritdoc cref="PnpmRunSettings.Stream"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Stream))]
    public static T EnableStream<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Stream, true));
    /// <inheritdoc cref="PnpmRunSettings.Stream"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Stream))]
    public static T DisableStream<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Stream, false));
    /// <inheritdoc cref="PnpmRunSettings.Stream"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Stream))]
    public static T ToggleStream<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Stream, !o.Stream));
    #endregion
    #region Dir
    /// <inheritdoc cref="PnpmRunSettings.Dir"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Dir))]
    public static T SetDir<T>(this T o, string v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Dir, v));
    /// <inheritdoc cref="PnpmRunSettings.Dir"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Dir))]
    public static T ResetDir<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.Dir));
    #endregion
    #region WorkspaceRoot
    /// <inheritdoc cref="PnpmRunSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.WorkspaceRoot))]
    public static T SetWorkspaceRoot<T>(this T o, bool? v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, v));
    /// <inheritdoc cref="PnpmRunSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.WorkspaceRoot))]
    public static T ResetWorkspaceRoot<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.WorkspaceRoot));
    /// <inheritdoc cref="PnpmRunSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.WorkspaceRoot))]
    public static T EnableWorkspaceRoot<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, true));
    /// <inheritdoc cref="PnpmRunSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.WorkspaceRoot))]
    public static T DisableWorkspaceRoot<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, false));
    /// <inheritdoc cref="PnpmRunSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.WorkspaceRoot))]
    public static T ToggleWorkspaceRoot<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, !o.WorkspaceRoot));
    #endregion
    #region Reporter
    /// <inheritdoc cref="PnpmRunSettings.Reporter"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Reporter))]
    public static T SetReporter<T>(this T o, PnpmReporter v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Reporter, v));
    /// <inheritdoc cref="PnpmRunSettings.Reporter"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Reporter))]
    public static T ResetReporter<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.Reporter));
    #endregion
    #region LogLevel
    /// <inheritdoc cref="PnpmRunSettings.LogLevel"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.LogLevel))]
    public static T SetLogLevel<T>(this T o, PnpmLogLevel v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.LogLevel, v));
    /// <inheritdoc cref="PnpmRunSettings.LogLevel"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.LogLevel))]
    public static T ResetLogLevel<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.LogLevel));
    #endregion
    #region Silent
    /// <inheritdoc cref="PnpmRunSettings.Silent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Silent))]
    public static T SetSilent<T>(this T o, bool? v) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Silent, v));
    /// <inheritdoc cref="PnpmRunSettings.Silent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Silent))]
    public static T ResetSilent<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Remove(() => o.Silent));
    /// <inheritdoc cref="PnpmRunSettings.Silent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Silent))]
    public static T EnableSilent<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Silent, true));
    /// <inheritdoc cref="PnpmRunSettings.Silent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Silent))]
    public static T DisableSilent<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Silent, false));
    /// <inheritdoc cref="PnpmRunSettings.Silent"/>
    [Builder(Type = typeof(PnpmRunSettings), Property = nameof(PnpmRunSettings.Silent))]
    public static T ToggleSilent<T>(this T o) where T : PnpmRunSettings => o.Modify(b => b.Set(() => o.Silent, !o.Silent));
    #endregion
}
#endregion
#region PnpmExecSettingsExtensions
/// <inheritdoc cref="PnpmTasks.PnpmExec(Fallout.Common.Tools.Pnpm.PnpmExecSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class PnpmExecSettingsExtensions
{
    #region Command
    /// <inheritdoc cref="PnpmExecSettings.Command"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Command))]
    public static T SetCommand<T>(this T o, string v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Command, v));
    /// <inheritdoc cref="PnpmExecSettings.Command"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Command))]
    public static T ResetCommand<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.Command));
    #endregion
    #region Arguments
    /// <inheritdoc cref="PnpmExecSettings.Arguments"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Arguments))]
    public static T SetArguments<T>(this T o, params string[] v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmExecSettings.Arguments"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Arguments))]
    public static T SetArguments<T>(this T o, IEnumerable<string> v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmExecSettings.Arguments"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Arguments))]
    public static T AddArguments<T>(this T o, params string[] v) where T : PnpmExecSettings => o.Modify(b => b.AddCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmExecSettings.Arguments"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Arguments))]
    public static T AddArguments<T>(this T o, IEnumerable<string> v) where T : PnpmExecSettings => o.Modify(b => b.AddCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmExecSettings.Arguments"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Arguments))]
    public static T RemoveArguments<T>(this T o, params string[] v) where T : PnpmExecSettings => o.Modify(b => b.RemoveCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmExecSettings.Arguments"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Arguments))]
    public static T RemoveArguments<T>(this T o, IEnumerable<string> v) where T : PnpmExecSettings => o.Modify(b => b.RemoveCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmExecSettings.Arguments"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Arguments))]
    public static T ClearArguments<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.ClearCollection(() => o.Arguments));
    #endregion
    #region Recursive
    /// <inheritdoc cref="PnpmExecSettings.Recursive"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Recursive))]
    public static T SetRecursive<T>(this T o, bool? v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Recursive, v));
    /// <inheritdoc cref="PnpmExecSettings.Recursive"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Recursive))]
    public static T ResetRecursive<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.Recursive));
    /// <inheritdoc cref="PnpmExecSettings.Recursive"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Recursive))]
    public static T EnableRecursive<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Recursive, true));
    /// <inheritdoc cref="PnpmExecSettings.Recursive"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Recursive))]
    public static T DisableRecursive<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Recursive, false));
    /// <inheritdoc cref="PnpmExecSettings.Recursive"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Recursive))]
    public static T ToggleRecursive<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Recursive, !o.Recursive));
    #endregion
    #region Filters
    /// <inheritdoc cref="PnpmExecSettings.Filters"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Filters))]
    public static T SetFilters<T>(this T o, params string[] v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Filters, v));
    /// <inheritdoc cref="PnpmExecSettings.Filters"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Filters))]
    public static T SetFilters<T>(this T o, IEnumerable<string> v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Filters, v));
    /// <inheritdoc cref="PnpmExecSettings.Filters"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Filters))]
    public static T AddFilters<T>(this T o, params string[] v) where T : PnpmExecSettings => o.Modify(b => b.AddCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmExecSettings.Filters"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Filters))]
    public static T AddFilters<T>(this T o, IEnumerable<string> v) where T : PnpmExecSettings => o.Modify(b => b.AddCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmExecSettings.Filters"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Filters))]
    public static T RemoveFilters<T>(this T o, params string[] v) where T : PnpmExecSettings => o.Modify(b => b.RemoveCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmExecSettings.Filters"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Filters))]
    public static T RemoveFilters<T>(this T o, IEnumerable<string> v) where T : PnpmExecSettings => o.Modify(b => b.RemoveCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmExecSettings.Filters"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Filters))]
    public static T ClearFilters<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.ClearCollection(() => o.Filters));
    #endregion
    #region Parallel
    /// <inheritdoc cref="PnpmExecSettings.Parallel"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Parallel))]
    public static T SetParallel<T>(this T o, bool? v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Parallel, v));
    /// <inheritdoc cref="PnpmExecSettings.Parallel"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Parallel))]
    public static T ResetParallel<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.Parallel));
    /// <inheritdoc cref="PnpmExecSettings.Parallel"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Parallel))]
    public static T EnableParallel<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Parallel, true));
    /// <inheritdoc cref="PnpmExecSettings.Parallel"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Parallel))]
    public static T DisableParallel<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Parallel, false));
    /// <inheritdoc cref="PnpmExecSettings.Parallel"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Parallel))]
    public static T ToggleParallel<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Parallel, !o.Parallel));
    #endregion
    #region ShellMode
    /// <inheritdoc cref="PnpmExecSettings.ShellMode"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.ShellMode))]
    public static T SetShellMode<T>(this T o, bool? v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.ShellMode, v));
    /// <inheritdoc cref="PnpmExecSettings.ShellMode"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.ShellMode))]
    public static T ResetShellMode<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.ShellMode));
    /// <inheritdoc cref="PnpmExecSettings.ShellMode"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.ShellMode))]
    public static T EnableShellMode<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.ShellMode, true));
    /// <inheritdoc cref="PnpmExecSettings.ShellMode"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.ShellMode))]
    public static T DisableShellMode<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.ShellMode, false));
    /// <inheritdoc cref="PnpmExecSettings.ShellMode"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.ShellMode))]
    public static T ToggleShellMode<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.ShellMode, !o.ShellMode));
    #endregion
    #region Stream
    /// <inheritdoc cref="PnpmExecSettings.Stream"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Stream))]
    public static T SetStream<T>(this T o, bool? v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Stream, v));
    /// <inheritdoc cref="PnpmExecSettings.Stream"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Stream))]
    public static T ResetStream<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.Stream));
    /// <inheritdoc cref="PnpmExecSettings.Stream"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Stream))]
    public static T EnableStream<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Stream, true));
    /// <inheritdoc cref="PnpmExecSettings.Stream"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Stream))]
    public static T DisableStream<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Stream, false));
    /// <inheritdoc cref="PnpmExecSettings.Stream"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Stream))]
    public static T ToggleStream<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Stream, !o.Stream));
    #endregion
    #region Dir
    /// <inheritdoc cref="PnpmExecSettings.Dir"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Dir))]
    public static T SetDir<T>(this T o, string v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Dir, v));
    /// <inheritdoc cref="PnpmExecSettings.Dir"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Dir))]
    public static T ResetDir<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.Dir));
    #endregion
    #region WorkspaceRoot
    /// <inheritdoc cref="PnpmExecSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.WorkspaceRoot))]
    public static T SetWorkspaceRoot<T>(this T o, bool? v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, v));
    /// <inheritdoc cref="PnpmExecSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.WorkspaceRoot))]
    public static T ResetWorkspaceRoot<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.WorkspaceRoot));
    /// <inheritdoc cref="PnpmExecSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.WorkspaceRoot))]
    public static T EnableWorkspaceRoot<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, true));
    /// <inheritdoc cref="PnpmExecSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.WorkspaceRoot))]
    public static T DisableWorkspaceRoot<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, false));
    /// <inheritdoc cref="PnpmExecSettings.WorkspaceRoot"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.WorkspaceRoot))]
    public static T ToggleWorkspaceRoot<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.WorkspaceRoot, !o.WorkspaceRoot));
    #endregion
    #region LogLevel
    /// <inheritdoc cref="PnpmExecSettings.LogLevel"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.LogLevel))]
    public static T SetLogLevel<T>(this T o, PnpmLogLevel v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.LogLevel, v));
    /// <inheritdoc cref="PnpmExecSettings.LogLevel"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.LogLevel))]
    public static T ResetLogLevel<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.LogLevel));
    #endregion
    #region Silent
    /// <inheritdoc cref="PnpmExecSettings.Silent"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Silent))]
    public static T SetSilent<T>(this T o, bool? v) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Silent, v));
    /// <inheritdoc cref="PnpmExecSettings.Silent"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Silent))]
    public static T ResetSilent<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Remove(() => o.Silent));
    /// <inheritdoc cref="PnpmExecSettings.Silent"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Silent))]
    public static T EnableSilent<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Silent, true));
    /// <inheritdoc cref="PnpmExecSettings.Silent"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Silent))]
    public static T DisableSilent<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Silent, false));
    /// <inheritdoc cref="PnpmExecSettings.Silent"/>
    [Builder(Type = typeof(PnpmExecSettings), Property = nameof(PnpmExecSettings.Silent))]
    public static T ToggleSilent<T>(this T o) where T : PnpmExecSettings => o.Modify(b => b.Set(() => o.Silent, !o.Silent));
    #endregion
}
#endregion
#region PnpmDlxSettingsExtensions
/// <inheritdoc cref="PnpmTasks.PnpmDlx(Fallout.Common.Tools.Pnpm.PnpmDlxSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class PnpmDlxSettingsExtensions
{
    #region Package
    /// <inheritdoc cref="PnpmDlxSettings.Package"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Package))]
    public static T SetPackage<T>(this T o, string v) where T : PnpmDlxSettings => o.Modify(b => b.Set(() => o.Package, v));
    /// <inheritdoc cref="PnpmDlxSettings.Package"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Package))]
    public static T ResetPackage<T>(this T o) where T : PnpmDlxSettings => o.Modify(b => b.Remove(() => o.Package));
    #endregion
    #region Arguments
    /// <inheritdoc cref="PnpmDlxSettings.Arguments"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Arguments))]
    public static T SetArguments<T>(this T o, params string[] v) where T : PnpmDlxSettings => o.Modify(b => b.Set(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmDlxSettings.Arguments"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Arguments))]
    public static T SetArguments<T>(this T o, IEnumerable<string> v) where T : PnpmDlxSettings => o.Modify(b => b.Set(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmDlxSettings.Arguments"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Arguments))]
    public static T AddArguments<T>(this T o, params string[] v) where T : PnpmDlxSettings => o.Modify(b => b.AddCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmDlxSettings.Arguments"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Arguments))]
    public static T AddArguments<T>(this T o, IEnumerable<string> v) where T : PnpmDlxSettings => o.Modify(b => b.AddCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmDlxSettings.Arguments"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Arguments))]
    public static T RemoveArguments<T>(this T o, params string[] v) where T : PnpmDlxSettings => o.Modify(b => b.RemoveCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmDlxSettings.Arguments"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Arguments))]
    public static T RemoveArguments<T>(this T o, IEnumerable<string> v) where T : PnpmDlxSettings => o.Modify(b => b.RemoveCollection(() => o.Arguments, v));
    /// <inheritdoc cref="PnpmDlxSettings.Arguments"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Arguments))]
    public static T ClearArguments<T>(this T o) where T : PnpmDlxSettings => o.Modify(b => b.ClearCollection(() => o.Arguments));
    #endregion
    #region Dir
    /// <inheritdoc cref="PnpmDlxSettings.Dir"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Dir))]
    public static T SetDir<T>(this T o, string v) where T : PnpmDlxSettings => o.Modify(b => b.Set(() => o.Dir, v));
    /// <inheritdoc cref="PnpmDlxSettings.Dir"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Dir))]
    public static T ResetDir<T>(this T o) where T : PnpmDlxSettings => o.Modify(b => b.Remove(() => o.Dir));
    #endregion
    #region Silent
    /// <inheritdoc cref="PnpmDlxSettings.Silent"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Silent))]
    public static T SetSilent<T>(this T o, bool? v) where T : PnpmDlxSettings => o.Modify(b => b.Set(() => o.Silent, v));
    /// <inheritdoc cref="PnpmDlxSettings.Silent"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Silent))]
    public static T ResetSilent<T>(this T o) where T : PnpmDlxSettings => o.Modify(b => b.Remove(() => o.Silent));
    /// <inheritdoc cref="PnpmDlxSettings.Silent"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Silent))]
    public static T EnableSilent<T>(this T o) where T : PnpmDlxSettings => o.Modify(b => b.Set(() => o.Silent, true));
    /// <inheritdoc cref="PnpmDlxSettings.Silent"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Silent))]
    public static T DisableSilent<T>(this T o) where T : PnpmDlxSettings => o.Modify(b => b.Set(() => o.Silent, false));
    /// <inheritdoc cref="PnpmDlxSettings.Silent"/>
    [Builder(Type = typeof(PnpmDlxSettings), Property = nameof(PnpmDlxSettings.Silent))]
    public static T ToggleSilent<T>(this T o) where T : PnpmDlxSettings => o.Modify(b => b.Set(() => o.Silent, !o.Silent));
    #endregion
}
#endregion
#region PnpmPublishSettingsExtensions
/// <inheritdoc cref="PnpmTasks.PnpmPublish(Fallout.Common.Tools.Pnpm.PnpmPublishSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class PnpmPublishSettingsExtensions
{
    #region Target
    /// <inheritdoc cref="PnpmPublishSettings.Target"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Target))]
    public static T SetTarget<T>(this T o, string v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Target, v));
    /// <inheritdoc cref="PnpmPublishSettings.Target"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Target))]
    public static T ResetTarget<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.Target));
    #endregion
    #region Tag
    /// <inheritdoc cref="PnpmPublishSettings.Tag"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Tag))]
    public static T SetTag<T>(this T o, string v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Tag, v));
    /// <inheritdoc cref="PnpmPublishSettings.Tag"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Tag))]
    public static T ResetTag<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.Tag));
    #endregion
    #region Access
    /// <inheritdoc cref="PnpmPublishSettings.Access"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Access))]
    public static T SetAccess<T>(this T o, PnpmPublishAccess v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Access, v));
    /// <inheritdoc cref="PnpmPublishSettings.Access"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Access))]
    public static T ResetAccess<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.Access));
    #endregion
    #region Force
    /// <inheritdoc cref="PnpmPublishSettings.Force"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Force))]
    public static T SetForce<T>(this T o, bool? v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Force, v));
    /// <inheritdoc cref="PnpmPublishSettings.Force"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Force))]
    public static T ResetForce<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.Force));
    /// <inheritdoc cref="PnpmPublishSettings.Force"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Force))]
    public static T EnableForce<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Force, true));
    /// <inheritdoc cref="PnpmPublishSettings.Force"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Force))]
    public static T DisableForce<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Force, false));
    /// <inheritdoc cref="PnpmPublishSettings.Force"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Force))]
    public static T ToggleForce<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Force, !o.Force));
    #endregion
    #region DryRun
    /// <inheritdoc cref="PnpmPublishSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.DryRun))]
    public static T SetDryRun<T>(this T o, bool? v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.DryRun, v));
    /// <inheritdoc cref="PnpmPublishSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.DryRun))]
    public static T ResetDryRun<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.DryRun));
    /// <inheritdoc cref="PnpmPublishSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.DryRun))]
    public static T EnableDryRun<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.DryRun, true));
    /// <inheritdoc cref="PnpmPublishSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.DryRun))]
    public static T DisableDryRun<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.DryRun, false));
    /// <inheritdoc cref="PnpmPublishSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.DryRun))]
    public static T ToggleDryRun<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.DryRun, !o.DryRun));
    #endregion
    #region Json
    /// <inheritdoc cref="PnpmPublishSettings.Json"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Json))]
    public static T SetJson<T>(this T o, bool? v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Json, v));
    /// <inheritdoc cref="PnpmPublishSettings.Json"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Json))]
    public static T ResetJson<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.Json));
    /// <inheritdoc cref="PnpmPublishSettings.Json"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Json))]
    public static T EnableJson<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Json, true));
    /// <inheritdoc cref="PnpmPublishSettings.Json"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Json))]
    public static T DisableJson<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Json, false));
    /// <inheritdoc cref="PnpmPublishSettings.Json"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Json))]
    public static T ToggleJson<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Json, !o.Json));
    #endregion
    #region NoGitChecks
    /// <inheritdoc cref="PnpmPublishSettings.NoGitChecks"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.NoGitChecks))]
    public static T SetNoGitChecks<T>(this T o, bool? v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.NoGitChecks, v));
    /// <inheritdoc cref="PnpmPublishSettings.NoGitChecks"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.NoGitChecks))]
    public static T ResetNoGitChecks<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.NoGitChecks));
    /// <inheritdoc cref="PnpmPublishSettings.NoGitChecks"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.NoGitChecks))]
    public static T EnableNoGitChecks<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.NoGitChecks, true));
    /// <inheritdoc cref="PnpmPublishSettings.NoGitChecks"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.NoGitChecks))]
    public static T DisableNoGitChecks<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.NoGitChecks, false));
    /// <inheritdoc cref="PnpmPublishSettings.NoGitChecks"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.NoGitChecks))]
    public static T ToggleNoGitChecks<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.NoGitChecks, !o.NoGitChecks));
    #endregion
    #region Otp
    /// <inheritdoc cref="PnpmPublishSettings.Otp"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Otp))]
    public static T SetOtp<T>(this T o, [Secret] string v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Otp, v));
    /// <inheritdoc cref="PnpmPublishSettings.Otp"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Otp))]
    public static T ResetOtp<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.Otp));
    #endregion
    #region PublishBranch
    /// <inheritdoc cref="PnpmPublishSettings.PublishBranch"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.PublishBranch))]
    public static T SetPublishBranch<T>(this T o, string v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.PublishBranch, v));
    /// <inheritdoc cref="PnpmPublishSettings.PublishBranch"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.PublishBranch))]
    public static T ResetPublishBranch<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.PublishBranch));
    #endregion
    #region Recursive
    /// <inheritdoc cref="PnpmPublishSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Recursive))]
    public static T SetRecursive<T>(this T o, bool? v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Recursive, v));
    /// <inheritdoc cref="PnpmPublishSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Recursive))]
    public static T ResetRecursive<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.Recursive));
    /// <inheritdoc cref="PnpmPublishSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Recursive))]
    public static T EnableRecursive<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Recursive, true));
    /// <inheritdoc cref="PnpmPublishSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Recursive))]
    public static T DisableRecursive<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Recursive, false));
    /// <inheritdoc cref="PnpmPublishSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Recursive))]
    public static T ToggleRecursive<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Recursive, !o.Recursive));
    #endregion
    #region ReportSummary
    /// <inheritdoc cref="PnpmPublishSettings.ReportSummary"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.ReportSummary))]
    public static T SetReportSummary<T>(this T o, bool? v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.ReportSummary, v));
    /// <inheritdoc cref="PnpmPublishSettings.ReportSummary"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.ReportSummary))]
    public static T ResetReportSummary<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.ReportSummary));
    /// <inheritdoc cref="PnpmPublishSettings.ReportSummary"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.ReportSummary))]
    public static T EnableReportSummary<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.ReportSummary, true));
    /// <inheritdoc cref="PnpmPublishSettings.ReportSummary"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.ReportSummary))]
    public static T DisableReportSummary<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.ReportSummary, false));
    /// <inheritdoc cref="PnpmPublishSettings.ReportSummary"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.ReportSummary))]
    public static T ToggleReportSummary<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.ReportSummary, !o.ReportSummary));
    #endregion
    #region IgnoreScripts
    /// <inheritdoc cref="PnpmPublishSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.IgnoreScripts))]
    public static T SetIgnoreScripts<T>(this T o, bool? v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.IgnoreScripts, v));
    /// <inheritdoc cref="PnpmPublishSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.IgnoreScripts))]
    public static T ResetIgnoreScripts<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.IgnoreScripts));
    /// <inheritdoc cref="PnpmPublishSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.IgnoreScripts))]
    public static T EnableIgnoreScripts<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.IgnoreScripts, true));
    /// <inheritdoc cref="PnpmPublishSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.IgnoreScripts))]
    public static T DisableIgnoreScripts<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.IgnoreScripts, false));
    /// <inheritdoc cref="PnpmPublishSettings.IgnoreScripts"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.IgnoreScripts))]
    public static T ToggleIgnoreScripts<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.IgnoreScripts, !o.IgnoreScripts));
    #endregion
    #region Dir
    /// <inheritdoc cref="PnpmPublishSettings.Dir"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Dir))]
    public static T SetDir<T>(this T o, string v) where T : PnpmPublishSettings => o.Modify(b => b.Set(() => o.Dir, v));
    /// <inheritdoc cref="PnpmPublishSettings.Dir"/>
    [Builder(Type = typeof(PnpmPublishSettings), Property = nameof(PnpmPublishSettings.Dir))]
    public static T ResetDir<T>(this T o) where T : PnpmPublishSettings => o.Modify(b => b.Remove(() => o.Dir));
    #endregion
}
#endregion
#region PnpmPackSettingsExtensions
/// <inheritdoc cref="PnpmTasks.PnpmPack(Fallout.Common.Tools.Pnpm.PnpmPackSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class PnpmPackSettingsExtensions
{
    #region Out
    /// <inheritdoc cref="PnpmPackSettings.Out"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Out))]
    public static T SetOut<T>(this T o, string v) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Out, v));
    /// <inheritdoc cref="PnpmPackSettings.Out"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Out))]
    public static T ResetOut<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Remove(() => o.Out));
    #endregion
    #region PackDestination
    /// <inheritdoc cref="PnpmPackSettings.PackDestination"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.PackDestination))]
    public static T SetPackDestination<T>(this T o, string v) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.PackDestination, v));
    /// <inheritdoc cref="PnpmPackSettings.PackDestination"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.PackDestination))]
    public static T ResetPackDestination<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Remove(() => o.PackDestination));
    #endregion
    #region DryRun
    /// <inheritdoc cref="PnpmPackSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.DryRun))]
    public static T SetDryRun<T>(this T o, bool? v) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.DryRun, v));
    /// <inheritdoc cref="PnpmPackSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.DryRun))]
    public static T ResetDryRun<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Remove(() => o.DryRun));
    /// <inheritdoc cref="PnpmPackSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.DryRun))]
    public static T EnableDryRun<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.DryRun, true));
    /// <inheritdoc cref="PnpmPackSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.DryRun))]
    public static T DisableDryRun<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.DryRun, false));
    /// <inheritdoc cref="PnpmPackSettings.DryRun"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.DryRun))]
    public static T ToggleDryRun<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.DryRun, !o.DryRun));
    #endregion
    #region Json
    /// <inheritdoc cref="PnpmPackSettings.Json"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Json))]
    public static T SetJson<T>(this T o, bool? v) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Json, v));
    /// <inheritdoc cref="PnpmPackSettings.Json"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Json))]
    public static T ResetJson<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Remove(() => o.Json));
    /// <inheritdoc cref="PnpmPackSettings.Json"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Json))]
    public static T EnableJson<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Json, true));
    /// <inheritdoc cref="PnpmPackSettings.Json"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Json))]
    public static T DisableJson<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Json, false));
    /// <inheritdoc cref="PnpmPackSettings.Json"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Json))]
    public static T ToggleJson<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Json, !o.Json));
    #endregion
    #region Recursive
    /// <inheritdoc cref="PnpmPackSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Recursive))]
    public static T SetRecursive<T>(this T o, bool? v) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Recursive, v));
    /// <inheritdoc cref="PnpmPackSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Recursive))]
    public static T ResetRecursive<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Remove(() => o.Recursive));
    /// <inheritdoc cref="PnpmPackSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Recursive))]
    public static T EnableRecursive<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Recursive, true));
    /// <inheritdoc cref="PnpmPackSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Recursive))]
    public static T DisableRecursive<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Recursive, false));
    /// <inheritdoc cref="PnpmPackSettings.Recursive"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Recursive))]
    public static T ToggleRecursive<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Recursive, !o.Recursive));
    #endregion
    #region Filters
    /// <inheritdoc cref="PnpmPackSettings.Filters"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Filters))]
    public static T SetFilters<T>(this T o, params string[] v) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Filters, v));
    /// <inheritdoc cref="PnpmPackSettings.Filters"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Filters))]
    public static T SetFilters<T>(this T o, IEnumerable<string> v) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Filters, v));
    /// <inheritdoc cref="PnpmPackSettings.Filters"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Filters))]
    public static T AddFilters<T>(this T o, params string[] v) where T : PnpmPackSettings => o.Modify(b => b.AddCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmPackSettings.Filters"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Filters))]
    public static T AddFilters<T>(this T o, IEnumerable<string> v) where T : PnpmPackSettings => o.Modify(b => b.AddCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmPackSettings.Filters"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Filters))]
    public static T RemoveFilters<T>(this T o, params string[] v) where T : PnpmPackSettings => o.Modify(b => b.RemoveCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmPackSettings.Filters"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Filters))]
    public static T RemoveFilters<T>(this T o, IEnumerable<string> v) where T : PnpmPackSettings => o.Modify(b => b.RemoveCollection(() => o.Filters, v));
    /// <inheritdoc cref="PnpmPackSettings.Filters"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Filters))]
    public static T ClearFilters<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.ClearCollection(() => o.Filters));
    #endregion
    #region Dir
    /// <inheritdoc cref="PnpmPackSettings.Dir"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Dir))]
    public static T SetDir<T>(this T o, string v) where T : PnpmPackSettings => o.Modify(b => b.Set(() => o.Dir, v));
    /// <inheritdoc cref="PnpmPackSettings.Dir"/>
    [Builder(Type = typeof(PnpmPackSettings), Property = nameof(PnpmPackSettings.Dir))]
    public static T ResetDir<T>(this T o) where T : PnpmPackSettings => o.Modify(b => b.Remove(() => o.Dir));
    #endregion
}
#endregion
#region PnpmAuditSettingsExtensions
/// <inheritdoc cref="PnpmTasks.PnpmAudit(Fallout.Common.Tools.Pnpm.PnpmAuditSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class PnpmAuditSettingsExtensions
{
    #region AuditLevel
    /// <inheritdoc cref="PnpmAuditSettings.AuditLevel"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.AuditLevel))]
    public static T SetAuditLevel<T>(this T o, PnpmAuditLevel v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.AuditLevel, v));
    /// <inheritdoc cref="PnpmAuditSettings.AuditLevel"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.AuditLevel))]
    public static T ResetAuditLevel<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.AuditLevel));
    #endregion
    #region Dev
    /// <inheritdoc cref="PnpmAuditSettings.Dev"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Dev))]
    public static T SetDev<T>(this T o, bool? v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Dev, v));
    /// <inheritdoc cref="PnpmAuditSettings.Dev"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Dev))]
    public static T ResetDev<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.Dev));
    /// <inheritdoc cref="PnpmAuditSettings.Dev"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Dev))]
    public static T EnableDev<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Dev, true));
    /// <inheritdoc cref="PnpmAuditSettings.Dev"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Dev))]
    public static T DisableDev<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Dev, false));
    /// <inheritdoc cref="PnpmAuditSettings.Dev"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Dev))]
    public static T ToggleDev<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Dev, !o.Dev));
    #endregion
    #region Prod
    /// <inheritdoc cref="PnpmAuditSettings.Prod"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Prod))]
    public static T SetProd<T>(this T o, bool? v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Prod, v));
    /// <inheritdoc cref="PnpmAuditSettings.Prod"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Prod))]
    public static T ResetProd<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.Prod));
    /// <inheritdoc cref="PnpmAuditSettings.Prod"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Prod))]
    public static T EnableProd<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Prod, true));
    /// <inheritdoc cref="PnpmAuditSettings.Prod"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Prod))]
    public static T DisableProd<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Prod, false));
    /// <inheritdoc cref="PnpmAuditSettings.Prod"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Prod))]
    public static T ToggleProd<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Prod, !o.Prod));
    #endregion
    #region NoOptional
    /// <inheritdoc cref="PnpmAuditSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.NoOptional))]
    public static T SetNoOptional<T>(this T o, bool? v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.NoOptional, v));
    /// <inheritdoc cref="PnpmAuditSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.NoOptional))]
    public static T ResetNoOptional<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.NoOptional));
    /// <inheritdoc cref="PnpmAuditSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.NoOptional))]
    public static T EnableNoOptional<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.NoOptional, true));
    /// <inheritdoc cref="PnpmAuditSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.NoOptional))]
    public static T DisableNoOptional<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.NoOptional, false));
    /// <inheritdoc cref="PnpmAuditSettings.NoOptional"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.NoOptional))]
    public static T ToggleNoOptional<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.NoOptional, !o.NoOptional));
    #endregion
    #region Fix
    /// <inheritdoc cref="PnpmAuditSettings.Fix"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Fix))]
    public static T SetFix<T>(this T o, PnpmAuditFix v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Fix, v));
    /// <inheritdoc cref="PnpmAuditSettings.Fix"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Fix))]
    public static T ResetFix<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.Fix));
    #endregion
    #region IgnoreRegistryErrors
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreRegistryErrors"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreRegistryErrors))]
    public static T SetIgnoreRegistryErrors<T>(this T o, bool? v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.IgnoreRegistryErrors, v));
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreRegistryErrors"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreRegistryErrors))]
    public static T ResetIgnoreRegistryErrors<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.IgnoreRegistryErrors));
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreRegistryErrors"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreRegistryErrors))]
    public static T EnableIgnoreRegistryErrors<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.IgnoreRegistryErrors, true));
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreRegistryErrors"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreRegistryErrors))]
    public static T DisableIgnoreRegistryErrors<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.IgnoreRegistryErrors, false));
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreRegistryErrors"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreRegistryErrors))]
    public static T ToggleIgnoreRegistryErrors<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.IgnoreRegistryErrors, !o.IgnoreRegistryErrors));
    #endregion
    #region IgnoreUnfixable
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreUnfixable"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreUnfixable))]
    public static T SetIgnoreUnfixable<T>(this T o, bool? v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.IgnoreUnfixable, v));
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreUnfixable"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreUnfixable))]
    public static T ResetIgnoreUnfixable<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.IgnoreUnfixable));
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreUnfixable"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreUnfixable))]
    public static T EnableIgnoreUnfixable<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.IgnoreUnfixable, true));
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreUnfixable"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreUnfixable))]
    public static T DisableIgnoreUnfixable<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.IgnoreUnfixable, false));
    /// <inheritdoc cref="PnpmAuditSettings.IgnoreUnfixable"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.IgnoreUnfixable))]
    public static T ToggleIgnoreUnfixable<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.IgnoreUnfixable, !o.IgnoreUnfixable));
    #endregion
    #region Json
    /// <inheritdoc cref="PnpmAuditSettings.Json"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Json))]
    public static T SetJson<T>(this T o, bool? v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Json, v));
    /// <inheritdoc cref="PnpmAuditSettings.Json"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Json))]
    public static T ResetJson<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.Json));
    /// <inheritdoc cref="PnpmAuditSettings.Json"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Json))]
    public static T EnableJson<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Json, true));
    /// <inheritdoc cref="PnpmAuditSettings.Json"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Json))]
    public static T DisableJson<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Json, false));
    /// <inheritdoc cref="PnpmAuditSettings.Json"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Json))]
    public static T ToggleJson<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Json, !o.Json));
    #endregion
    #region Dir
    /// <inheritdoc cref="PnpmAuditSettings.Dir"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Dir))]
    public static T SetDir<T>(this T o, string v) where T : PnpmAuditSettings => o.Modify(b => b.Set(() => o.Dir, v));
    /// <inheritdoc cref="PnpmAuditSettings.Dir"/>
    [Builder(Type = typeof(PnpmAuditSettings), Property = nameof(PnpmAuditSettings.Dir))]
    public static T ResetDir<T>(this T o) where T : PnpmAuditSettings => o.Modify(b => b.Remove(() => o.Dir));
    #endregion
}
#endregion
#region PnpmLogLevel
/// <summary>Used within <see cref="PnpmTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<PnpmLogLevel>))]
public partial class PnpmLogLevel : Enumeration
{
    public static PnpmLogLevel debug = (PnpmLogLevel) "debug";
    public static PnpmLogLevel info = (PnpmLogLevel) "info";
    public static PnpmLogLevel warn = (PnpmLogLevel) "warn";
    public static PnpmLogLevel error = (PnpmLogLevel) "error";
    public static implicit operator PnpmLogLevel(string value)
    {
        return new PnpmLogLevel { Value = value };
    }
}
#endregion
#region PnpmReporter
/// <summary>Used within <see cref="PnpmTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<PnpmReporter>))]
public partial class PnpmReporter : Enumeration
{
    public static PnpmReporter append_only = (PnpmReporter) "append-only";
    public static PnpmReporter default_ = (PnpmReporter) "default";
    public static PnpmReporter ndjson = (PnpmReporter) "ndjson";
    public static PnpmReporter silent = (PnpmReporter) "silent";
    public static implicit operator PnpmReporter(string value)
    {
        return new PnpmReporter { Value = value };
    }
}
#endregion
#region PnpmPackageImportMethod
/// <summary>Used within <see cref="PnpmTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<PnpmPackageImportMethod>))]
public partial class PnpmPackageImportMethod : Enumeration
{
    public static PnpmPackageImportMethod auto = (PnpmPackageImportMethod) "auto";
    public static PnpmPackageImportMethod clone = (PnpmPackageImportMethod) "clone";
    public static PnpmPackageImportMethod copy = (PnpmPackageImportMethod) "copy";
    public static PnpmPackageImportMethod hardlink = (PnpmPackageImportMethod) "hardlink";
    public static implicit operator PnpmPackageImportMethod(string value)
    {
        return new PnpmPackageImportMethod { Value = value };
    }
}
#endregion
#region PnpmPublishAccess
/// <summary>Used within <see cref="PnpmTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<PnpmPublishAccess>))]
public partial class PnpmPublishAccess : Enumeration
{
    public static PnpmPublishAccess public_ = (PnpmPublishAccess) "public";
    public static PnpmPublishAccess restricted = (PnpmPublishAccess) "restricted";
    public static implicit operator PnpmPublishAccess(string value)
    {
        return new PnpmPublishAccess { Value = value };
    }
}
#endregion
#region PnpmAuditLevel
/// <summary>Used within <see cref="PnpmTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<PnpmAuditLevel>))]
public partial class PnpmAuditLevel : Enumeration
{
    public static PnpmAuditLevel info = (PnpmAuditLevel) "info";
    public static PnpmAuditLevel low = (PnpmAuditLevel) "low";
    public static PnpmAuditLevel moderate = (PnpmAuditLevel) "moderate";
    public static PnpmAuditLevel high = (PnpmAuditLevel) "high";
    public static PnpmAuditLevel critical = (PnpmAuditLevel) "critical";
    public static implicit operator PnpmAuditLevel(string value)
    {
        return new PnpmAuditLevel { Value = value };
    }
}
#endregion
#region PnpmAuditFix
/// <summary>Used within <see cref="PnpmTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<PnpmAuditFix>))]
public partial class PnpmAuditFix : Enumeration
{
    public static PnpmAuditFix override_ = (PnpmAuditFix) "override";
    public static PnpmAuditFix update = (PnpmAuditFix) "update";
    public static implicit operator PnpmAuditFix(string value)
    {
        return new PnpmAuditFix { Value = value };
    }
}
#endregion
