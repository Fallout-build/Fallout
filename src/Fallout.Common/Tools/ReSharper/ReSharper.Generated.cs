// Generated from https://github.com/Fallout-build/Fallout/blob/develop/src/Fallout.Common/Tools/ReSharper/ReSharper.json

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

namespace Fallout.Common.Tools.ReSharper;

/// <summary><p>The ReSharper command line tools run ReSharper's code inspections and code cleanup outside the IDE. They ship as the <c>jb</c> global .NET tool (package <c>JetBrains.ReSharper.GlobalTools</c>).<para/>The tool path is resolved in this order: the <c>RESHARPER_EXE</c> environment variable, a <c>jb</c> executable on the PATH (for example, after <c>dotnet tool install --global JetBrains.ReSharper.GlobalTools</c>), and then the <c>JetBrains.ReSharper.GlobalTools</c> package in the NuGet packages folder (add it as a <c>PackageDownload</c> item to the build project).</p><p>For more details, visit the <a href="https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html">official website</a>.</p></summary>
[ExcludeFromCodeCoverage]
[NuGetTool(Id = PackageId, Executable = PackageExecutable)]
public partial class ReSharperTasks : ToolTasks, IRequireNuGetPackage
{
    public static string ReSharperPath { get => new ReSharperTasks().GetToolPathInternal(); set => new ReSharperTasks().SetToolPath(value); }
    public const string PackageId = "JetBrains.ReSharper.GlobalTools";
    public const string PackageExecutable = "JetBrains.CommandLine.Products.dll";
    /// <summary><p>The ReSharper command line tools run ReSharper's code inspections and code cleanup outside the IDE. They ship as the <c>jb</c> global .NET tool (package <c>JetBrains.ReSharper.GlobalTools</c>).<para/>The tool path is resolved in this order: the <c>RESHARPER_EXE</c> environment variable, a <c>jb</c> executable on the PATH (for example, after <c>dotnet tool install --global JetBrains.ReSharper.GlobalTools</c>), and then the <c>JetBrains.ReSharper.GlobalTools</c> package in the NuGet packages folder (add it as a <c>PackageDownload</c> item to the build project).</p><p>For more details, visit the <a href="https://www.jetbrains.com/help/resharper/ReSharper_Command_Line_Tools.html">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> ReSharper(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => new ReSharperTasks().Run(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>InspectCode runs ReSharper's code inspections on a solution or project and writes the issues it finds to a report. The default report format is SARIF.</p><p>For more details, visit the <a href="https://www.jetbrains.com/help/resharper/InspectCode.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;targetPath&gt;</c> via <see cref="ReSharperInspectCodeSettings.TargetPath"/></li><li><c>--absolute-paths</c> via <see cref="ReSharperInspectCodeSettings.AbsolutePaths"/></li><li><c>--build</c> via <see cref="ReSharperInspectCodeSettings.Build"/></li><li><c>--caches-home</c> via <see cref="ReSharperInspectCodeSettings.CachesHome"/></li><li><c>--config</c> via <see cref="ReSharperInspectCodeSettings.ConfigFile"/></li><li><c>--config-create</c> via <see cref="ReSharperInspectCodeSettings.CreateConfigFile"/></li><li><c>--debug</c> via <see cref="ReSharperInspectCodeSettings.Debug"/></li><li><c>--disable-settings-layers</c> via <see cref="ReSharperInspectCodeSettings.DisableSettingsLayers"/></li><li><c>--dotnetcore</c> via <see cref="ReSharperInspectCodeSettings.DotNetCore"/></li><li><c>--dotnetcoresdk</c> via <see cref="ReSharperInspectCodeSettings.DotNetCoreSdk"/></li><li><c>--dumpIssuesTypes</c> via <see cref="ReSharperInspectCodeSettings.DumpIssuesTypes"/></li><li><c>--exclude</c> via <see cref="ReSharperInspectCodeSettings.Excludes"/></li><li><c>--eXtensions</c> via <see cref="ReSharperInspectCodeSettings.Extensions"/></li><li><c>--format</c> via <see cref="ReSharperInspectCodeSettings.Format"/></li><li><c>--include</c> via <see cref="ReSharperInspectCodeSettings.Includes"/></li><li><c>--jobs</c> via <see cref="ReSharperInspectCodeSettings.Jobs"/></li><li><c>--LogFile</c> via <see cref="ReSharperInspectCodeSettings.LogFile"/></li><li><c>--LogFolder</c> via <see cref="ReSharperInspectCodeSettings.LogFolder"/></li><li><c>--LogLevel</c> via <see cref="ReSharperInspectCodeSettings.LogLevel"/></li><li><c>--measure</c> via <see cref="ReSharperInspectCodeSettings.Measure"/></li><li><c>--mono</c> via <see cref="ReSharperInspectCodeSettings.MonoPath"/></li><li><c>--no-build</c> via <see cref="ReSharperInspectCodeSettings.NoBuild"/></li><li><c>--no-buildin-settings</c> via <see cref="ReSharperInspectCodeSettings.NoBuiltinSettings"/></li><li><c>--no-swea</c> via <see cref="ReSharperInspectCodeSettings.NoSwea"/></li><li><c>--no-updates</c> via <see cref="ReSharperInspectCodeSettings.NoUpdates"/></li><li><c>--output</c> via <see cref="ReSharperInspectCodeSettings.Output"/></li><li><c>--project</c> via <see cref="ReSharperInspectCodeSettings.Project"/></li><li><c>--properties</c> via <see cref="ReSharperInspectCodeSettings.Properties"/></li><li><c>--settings</c> via <see cref="ReSharperInspectCodeSettings.Settings"/></li><li><c>--severity</c> via <see cref="ReSharperInspectCodeSettings.Severity"/></li><li><c>--source</c> via <see cref="ReSharperInspectCodeSettings.Sources"/></li><li><c>--stdout</c> via <see cref="ReSharperInspectCodeSettings.Stdout"/></li><li><c>--swea</c> via <see cref="ReSharperInspectCodeSettings.Swea"/></li><li><c>--target</c> via <see cref="ReSharperInspectCodeSettings.Target"/></li><li><c>--target-framework</c> via <see cref="ReSharperInspectCodeSettings.TargetFramework"/></li><li><c>--targets-for-items</c> via <see cref="ReSharperInspectCodeSettings.ItemTargets"/></li><li><c>--targets-for-references</c> via <see cref="ReSharperInspectCodeSettings.ReferenceTargets"/></li><li><c>--toolset</c> via <see cref="ReSharperInspectCodeSettings.Toolset"/></li><li><c>--toolset-path</c> via <see cref="ReSharperInspectCodeSettings.ToolsetPath"/></li><li><c>--verbosity</c> via <see cref="ReSharperInspectCodeSettings.Verbosity"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> ReSharperInspectCode(ReSharperInspectCodeSettings options = null) => new ReSharperTasks().Run<ReSharperInspectCodeSettings>(options);
    /// <inheritdoc cref="ReSharperTasks.ReSharperInspectCode(Fallout.Common.Tools.ReSharper.ReSharperInspectCodeSettings)"/>
    public static IReadOnlyCollection<Output> ReSharperInspectCode(Configure<ReSharperInspectCodeSettings> configurator) => new ReSharperTasks().Run<ReSharperInspectCodeSettings>(configurator.Invoke(new ReSharperInspectCodeSettings()));
    /// <inheritdoc cref="ReSharperTasks.ReSharperInspectCode(Fallout.Common.Tools.ReSharper.ReSharperInspectCodeSettings)"/>
    public static IEnumerable<(ReSharperInspectCodeSettings Settings, IReadOnlyCollection<Output> Output)> ReSharperInspectCode(CombinatorialConfigure<ReSharperInspectCodeSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(ReSharperInspectCode, degreeOfParallelism, completeOnFailure);
    /// <summary><p>CleanupCode runs a code cleanup profile on a solution or project, so that formatting and code style issues are fixed across the code base.</p><p>For more details, visit the <a href="https://www.jetbrains.com/help/resharper/CleanupCode.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;targetPath&gt;</c> via <see cref="ReSharperCleanupCodeSettings.TargetPath"/></li><li><c>--build</c> via <see cref="ReSharperCleanupCodeSettings.Build"/></li><li><c>--caches-home</c> via <see cref="ReSharperCleanupCodeSettings.CachesHome"/></li><li><c>--config</c> via <see cref="ReSharperCleanupCodeSettings.ConfigFile"/></li><li><c>--config-create</c> via <see cref="ReSharperCleanupCodeSettings.CreateConfigFile"/></li><li><c>--debug</c> via <see cref="ReSharperCleanupCodeSettings.Debug"/></li><li><c>--disable-settings-layers</c> via <see cref="ReSharperCleanupCodeSettings.DisableSettingsLayers"/></li><li><c>--dotnetcore</c> via <see cref="ReSharperCleanupCodeSettings.DotNetCore"/></li><li><c>--dotnetcoresdk</c> via <see cref="ReSharperCleanupCodeSettings.DotNetCoreSdk"/></li><li><c>--exclude</c> via <see cref="ReSharperCleanupCodeSettings.Excludes"/></li><li><c>--eXtensions</c> via <see cref="ReSharperCleanupCodeSettings.Extensions"/></li><li><c>--include</c> via <see cref="ReSharperCleanupCodeSettings.Includes"/></li><li><c>--LogFile</c> via <see cref="ReSharperCleanupCodeSettings.LogFile"/></li><li><c>--LogFolder</c> via <see cref="ReSharperCleanupCodeSettings.LogFolder"/></li><li><c>--LogLevel</c> via <see cref="ReSharperCleanupCodeSettings.LogLevel"/></li><li><c>--measure</c> via <see cref="ReSharperCleanupCodeSettings.Measure"/></li><li><c>--mono</c> via <see cref="ReSharperCleanupCodeSettings.MonoPath"/></li><li><c>--no-build</c> via <see cref="ReSharperCleanupCodeSettings.NoBuild"/></li><li><c>--no-buildin-settings</c> via <see cref="ReSharperCleanupCodeSettings.NoBuiltinSettings"/></li><li><c>--no-updates</c> via <see cref="ReSharperCleanupCodeSettings.NoUpdates"/></li><li><c>--profile</c> via <see cref="ReSharperCleanupCodeSettings.Profile"/></li><li><c>--properties</c> via <see cref="ReSharperCleanupCodeSettings.Properties"/></li><li><c>--settings</c> via <see cref="ReSharperCleanupCodeSettings.Settings"/></li><li><c>--source</c> via <see cref="ReSharperCleanupCodeSettings.Sources"/></li><li><c>--target</c> via <see cref="ReSharperCleanupCodeSettings.Target"/></li><li><c>--targets-for-items</c> via <see cref="ReSharperCleanupCodeSettings.ItemTargets"/></li><li><c>--targets-for-references</c> via <see cref="ReSharperCleanupCodeSettings.ReferenceTargets"/></li><li><c>--toolset</c> via <see cref="ReSharperCleanupCodeSettings.Toolset"/></li><li><c>--toolset-path</c> via <see cref="ReSharperCleanupCodeSettings.ToolsetPath"/></li><li><c>--verbosity</c> via <see cref="ReSharperCleanupCodeSettings.Verbosity"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> ReSharperCleanupCode(ReSharperCleanupCodeSettings options = null) => new ReSharperTasks().Run<ReSharperCleanupCodeSettings>(options);
    /// <inheritdoc cref="ReSharperTasks.ReSharperCleanupCode(Fallout.Common.Tools.ReSharper.ReSharperCleanupCodeSettings)"/>
    public static IReadOnlyCollection<Output> ReSharperCleanupCode(Configure<ReSharperCleanupCodeSettings> configurator) => new ReSharperTasks().Run<ReSharperCleanupCodeSettings>(configurator.Invoke(new ReSharperCleanupCodeSettings()));
    /// <inheritdoc cref="ReSharperTasks.ReSharperCleanupCode(Fallout.Common.Tools.ReSharper.ReSharperCleanupCodeSettings)"/>
    public static IEnumerable<(ReSharperCleanupCodeSettings Settings, IReadOnlyCollection<Output> Output)> ReSharperCleanupCode(CombinatorialConfigure<ReSharperCleanupCodeSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(ReSharperCleanupCode, degreeOfParallelism, completeOnFailure);
}
#region ReSharperInspectCodeSettings
/// <inheritdoc cref="ReSharperTasks.ReSharperInspectCode(Fallout.Common.Tools.ReSharper.ReSharperInspectCodeSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(ReSharperTasks), Command = nameof(ReSharperTasks.ReSharperInspectCode), Arguments = "inspectcode")]
public partial class ReSharperInspectCodeSettings : ToolOptions
{
    /// <summary>The solution or project file to inspect.</summary>
    [Argument(Format = "{value}", Position = 1)] public string TargetPath => Get<string>(() => TargetPath);
    /// <summary>The report file. Use <c>-</c> to write the report to stdout.</summary>
    [Argument(Format = "--output={value}")] public string Output => Get<string>(() => Output);
    /// <summary>Write the report to stdout.</summary>
    [Argument(Format = "--stdout")] public bool? Stdout => Get<bool?>(() => Stdout);
    /// <summary>The report format. The default is <c>Sarif</c>.</summary>
    [Argument(Format = "--format={value}")] public ReSharperFormat Format => Get<ReSharperFormat>(() => Format);
    /// <summary>Run up to this many jobs in parallel. <c>0</c> means as many as possible, which is the default.</summary>
    [Argument(Format = "--jobs={value}")] public int? Jobs => Get<int?>(() => Jobs);
    /// <summary>Write absolute paths to the report. By default, paths are relative to the solution file.</summary>
    [Argument(Format = "--absolute-paths")] public bool? AbsolutePaths => Get<bool?>(() => AbsolutePaths);
    /// <summary>Turn off solution-wide analysis.</summary>
    [Argument(Format = "--no-swea")] public bool? NoSwea => Get<bool?>(() => NoSwea);
    /// <summary>Turn on solution-wide analysis.</summary>
    [Argument(Format = "--swea")] public bool? Swea => Get<bool?>(() => Swea);
    /// <summary>Inspect only the projects that match this wildcard, for example <c>--project=*Billing</c>. By default, all projects in the solution are inspected.</summary>
    [Argument(Format = "--project={value}")] public string Project => Get<string>(() => Project);
    /// <summary>Inspect for one target framework only, for example <c>net10.0</c>. By default, all frameworks are inspected.</summary>
    [Argument(Format = "--target-framework={value}")] public string TargetFramework => Get<string>(() => TargetFramework);
    /// <summary>The lowest severity to report. The default is <c>SUGGESTION</c>.</summary>
    [Argument(Format = "--severity={value}")] public ReSharperSeverity Severity => Get<ReSharperSeverity>(() => Severity);
    /// <summary>Write all known <a href="https://www.jetbrains.com/help/resharper/Code_Analysis__Code_Inspections.html">code inspections</a> to the report instead of inspecting code. Use it without a target path.</summary>
    [Argument(Format = "--dumpIssuesTypes")] public bool? DumpIssuesTypes => Get<bool?>(() => DumpIssuesTypes);
    /// <summary>A <c>.DotSettings</c> file whose settings override all other settings layers. By default, the <a href="https://www.jetbrains.com/help/resharper/Sharing_Configuration_Options.html#solution-team-shared-layer">team-shared layer</a> of the solution is used if it exists.</summary>
    [Argument(Format = "--settings={value}")] public string Settings => Get<string>(() => Settings);
    /// <summary>The <a href="https://www.jetbrains.com/help/resharper/Sharing_Configuration_Options.html#layers">settings layers</a> to turn off.</summary>
    [Argument(Format = "--disable-settings-layers={value}", Separator = ";")] public IReadOnlyList<ReSharperSettingsLayers> DisableSettingsLayers => Get<List<ReSharperSettingsLayers>>(() => DisableSettingsLayers);
    /// <summary>Turn off all global, solution and project settings layers. Same as <c>--disable-settings-layers=GlobalAll;GlobalPerProduct;SolutionShared;SolutionPersonal;ProjectShared;ProjectPersonal</c>.</summary>
    [Argument(Format = "--no-buildin-settings")] public bool? NoBuiltinSettings => Get<bool?>(() => NoBuiltinSettings);
    /// <summary>ReSharper extensions to install and use. Available extensions are listed in the <a href="https://plugins.jetbrains.com/resharper">plugin repository</a>.</summary>
    [Argument(Format = "--eXtensions={value}", Separator = ";")] public IReadOnlyList<string> Extensions => Get<List<string>>(() => Extensions);
    /// <summary>The sources to install extensions from.</summary>
    [Argument(Format = "--source={value}", Separator = ";")] public IReadOnlyList<string> Sources => Get<List<string>>(() => Sources);
    /// <summary>The directory for ReSharper's caches.</summary>
    [Argument(Format = "--caches-home={value}")] public string CachesHome => Get<string>(() => CachesHome);
    /// <summary>Save the current parameters to this configuration file.</summary>
    [Argument(Format = "--config-create={value}")] public string CreateConfigFile => Get<string>(() => CreateConfigFile);
    /// <summary>Load the parameters from this configuration file.</summary>
    [Argument(Format = "--config={value}")] public string ConfigFile => Get<string>(() => ConfigFile);
    /// <summary>How much information to write to the console. The default is <c>INFO</c>.</summary>
    [Argument(Format = "--verbosity={value}")] public ReSharperVerbosity Verbosity => Get<ReSharperVerbosity>(() => Verbosity);
    /// <summary>Show debugging messages.</summary>
    [Argument(Format = "--debug")] public bool? Debug => Get<bool?>(() => Debug);
    /// <summary>Don't check for updates.</summary>
    [Argument(Format = "--no-updates")] public bool? NoUpdates => Get<bool?>(() => NoUpdates);
    /// <summary>The logging level.</summary>
    [Argument(Format = "--LogLevel={value}")] public ReSharperVerbosity LogLevel => Get<ReSharperVerbosity>(() => LogLevel);
    /// <summary>The log file.</summary>
    [Argument(Format = "--LogFile={value}")] public string LogFile => Get<string>(() => LogFile);
    /// <summary>The log folder.</summary>
    [Argument(Format = "--LogFolder={value}")] public string LogFolder => Get<string>(() => LogFolder);
    /// <summary>Measure the tool's own performance.</summary>
    [Argument(Format = "--measure={value}")] public ReSharperMeasure Measure => Get<ReSharperMeasure>(() => Measure);
    /// <summary>MSBuild properties that apply to all analyzed projects.</summary>
    [Argument(Format = "--properties:{key}={value}")] public IReadOnlyDictionary<string, string> Properties => Get<Dictionary<string, string>>(() => Properties);
    /// <summary>The MSBuild toolset version, for example <c>17.0</c>. By default, the highest available version is used.</summary>
    [Argument(Format = "--toolset={value}")] public string Toolset => Get<string>(() => Toolset);
    /// <summary>The path to the MSBuild executable or DLL.</summary>
    [Argument(Format = "--toolset-path={value}")] public string ToolsetPath => Get<string>(() => ToolsetPath);
    /// <summary>The path to the <c>dotnet</c> executable. By default, it is detected automatically.</summary>
    [Argument(Format = "--dotnetcore={value}")] public string DotNetCore => Get<string>(() => DotNetCore);
    /// <summary>The .NET SDK version that provides MSBuild, for example <c>10.0.100</c>.</summary>
    [Argument(Format = "--dotnetcoresdk={value}")] public string DotNetCoreSdk => Get<string>(() => DotNetCoreSdk);
    /// <summary>The path to Mono. Leave it empty to ignore Mono.</summary>
    [Argument(Format = "--mono={value}")] public string MonoPath => Get<string>(() => MonoPath);
    /// <summary>MSBuild targets that are run to get the referenced assemblies of projects.</summary>
    [Argument(Format = "--targets-for-references={value}", Separator = ";")] public IReadOnlyList<string> ReferenceTargets => Get<List<string>>(() => ReferenceTargets);
    /// <summary>MSBuild targets that are run to get other items of projects, for example <c>Compile</c> items.</summary>
    [Argument(Format = "--targets-for-items={value}", Separator = ";")] public IReadOnlyList<string> ItemTargets => Get<List<string>>(() => ItemTargets);
    /// <summary>Build the solution before processing. This is the default.</summary>
    [Argument(Format = "--build")] public bool? Build => Get<bool?>(() => Build);
    /// <summary>Don't build the solution before processing. Use it when the solution is already built.</summary>
    [Argument(Format = "--no-build")] public bool? NoBuild => Get<bool?>(() => NoBuild);
    /// <summary>The MSBuild target to run before processing. The default is <c>Build</c>.</summary>
    [Argument(Format = "--target={value}")] public string Target => Get<string>(() => Target);
    /// <summary>Process only the files that match these relative paths or wildcards (<c>*</c> and <c>**</c>).</summary>
    [Argument(Format = "--include={value}", Separator = ";")] public IReadOnlyList<string> Includes => Get<List<string>>(() => Includes);
    /// <summary>Skip the files that match these relative paths or wildcards (<c>*</c> and <c>**</c>). <c>--exclude</c> wins over <c>--include</c>.</summary>
    [Argument(Format = "--exclude={value}", Separator = ";")] public IReadOnlyList<string> Excludes => Get<List<string>>(() => Excludes);
}
#endregion
#region ReSharperCleanupCodeSettings
/// <inheritdoc cref="ReSharperTasks.ReSharperCleanupCode(Fallout.Common.Tools.ReSharper.ReSharperCleanupCodeSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(ReSharperTasks), Command = nameof(ReSharperTasks.ReSharperCleanupCode), Arguments = "cleanupcode")]
public partial class ReSharperCleanupCodeSettings : ToolOptions
{
    /// <summary>The solution or project file to clean up.</summary>
    [Argument(Format = "{value}", Position = 1)] public string TargetPath => Get<string>(() => TargetPath);
    /// <summary>The name of the code cleanup profile to run. By default, the <a href="https://www.jetbrains.com/help/resharper/Code_Cleanup__Index.html#profiles">Built-in: Full Cleanup</a> profile is used.</summary>
    [Argument(Format = "--profile={value}")] public string Profile => Get<string>(() => Profile);
    /// <summary>A <c>.DotSettings</c> file whose settings override all other settings layers. By default, the <a href="https://www.jetbrains.com/help/resharper/Sharing_Configuration_Options.html#solution-team-shared-layer">team-shared layer</a> of the solution is used if it exists.</summary>
    [Argument(Format = "--settings={value}")] public string Settings => Get<string>(() => Settings);
    /// <summary>The <a href="https://www.jetbrains.com/help/resharper/Sharing_Configuration_Options.html#layers">settings layers</a> to turn off.</summary>
    [Argument(Format = "--disable-settings-layers={value}", Separator = ";")] public IReadOnlyList<ReSharperSettingsLayers> DisableSettingsLayers => Get<List<ReSharperSettingsLayers>>(() => DisableSettingsLayers);
    /// <summary>Turn off all global, solution and project settings layers. Same as <c>--disable-settings-layers=GlobalAll;GlobalPerProduct;SolutionShared;SolutionPersonal;ProjectShared;ProjectPersonal</c>.</summary>
    [Argument(Format = "--no-buildin-settings")] public bool? NoBuiltinSettings => Get<bool?>(() => NoBuiltinSettings);
    /// <summary>ReSharper extensions to install and use. Available extensions are listed in the <a href="https://plugins.jetbrains.com/resharper">plugin repository</a>.</summary>
    [Argument(Format = "--eXtensions={value}", Separator = ";")] public IReadOnlyList<string> Extensions => Get<List<string>>(() => Extensions);
    /// <summary>The sources to install extensions from.</summary>
    [Argument(Format = "--source={value}", Separator = ";")] public IReadOnlyList<string> Sources => Get<List<string>>(() => Sources);
    /// <summary>The directory for ReSharper's caches.</summary>
    [Argument(Format = "--caches-home={value}")] public string CachesHome => Get<string>(() => CachesHome);
    /// <summary>Save the current parameters to this configuration file.</summary>
    [Argument(Format = "--config-create={value}")] public string CreateConfigFile => Get<string>(() => CreateConfigFile);
    /// <summary>Load the parameters from this configuration file.</summary>
    [Argument(Format = "--config={value}")] public string ConfigFile => Get<string>(() => ConfigFile);
    /// <summary>How much information to write to the console. The default is <c>INFO</c>.</summary>
    [Argument(Format = "--verbosity={value}")] public ReSharperVerbosity Verbosity => Get<ReSharperVerbosity>(() => Verbosity);
    /// <summary>Show debugging messages.</summary>
    [Argument(Format = "--debug")] public bool? Debug => Get<bool?>(() => Debug);
    /// <summary>Don't check for updates.</summary>
    [Argument(Format = "--no-updates")] public bool? NoUpdates => Get<bool?>(() => NoUpdates);
    /// <summary>The logging level.</summary>
    [Argument(Format = "--LogLevel={value}")] public ReSharperVerbosity LogLevel => Get<ReSharperVerbosity>(() => LogLevel);
    /// <summary>The log file.</summary>
    [Argument(Format = "--LogFile={value}")] public string LogFile => Get<string>(() => LogFile);
    /// <summary>The log folder.</summary>
    [Argument(Format = "--LogFolder={value}")] public string LogFolder => Get<string>(() => LogFolder);
    /// <summary>Measure the tool's own performance.</summary>
    [Argument(Format = "--measure={value}")] public ReSharperMeasure Measure => Get<ReSharperMeasure>(() => Measure);
    /// <summary>MSBuild properties that apply to all analyzed projects.</summary>
    [Argument(Format = "--properties:{key}={value}")] public IReadOnlyDictionary<string, string> Properties => Get<Dictionary<string, string>>(() => Properties);
    /// <summary>The MSBuild toolset version, for example <c>17.0</c>. By default, the highest available version is used.</summary>
    [Argument(Format = "--toolset={value}")] public string Toolset => Get<string>(() => Toolset);
    /// <summary>The path to the MSBuild executable or DLL.</summary>
    [Argument(Format = "--toolset-path={value}")] public string ToolsetPath => Get<string>(() => ToolsetPath);
    /// <summary>The path to the <c>dotnet</c> executable. By default, it is detected automatically.</summary>
    [Argument(Format = "--dotnetcore={value}")] public string DotNetCore => Get<string>(() => DotNetCore);
    /// <summary>The .NET SDK version that provides MSBuild, for example <c>10.0.100</c>.</summary>
    [Argument(Format = "--dotnetcoresdk={value}")] public string DotNetCoreSdk => Get<string>(() => DotNetCoreSdk);
    /// <summary>The path to Mono. Leave it empty to ignore Mono.</summary>
    [Argument(Format = "--mono={value}")] public string MonoPath => Get<string>(() => MonoPath);
    /// <summary>MSBuild targets that are run to get the referenced assemblies of projects.</summary>
    [Argument(Format = "--targets-for-references={value}", Separator = ";")] public IReadOnlyList<string> ReferenceTargets => Get<List<string>>(() => ReferenceTargets);
    /// <summary>MSBuild targets that are run to get other items of projects, for example <c>Compile</c> items.</summary>
    [Argument(Format = "--targets-for-items={value}", Separator = ";")] public IReadOnlyList<string> ItemTargets => Get<List<string>>(() => ItemTargets);
    /// <summary>Build the solution before processing. This is the default.</summary>
    [Argument(Format = "--build")] public bool? Build => Get<bool?>(() => Build);
    /// <summary>Don't build the solution before processing. Use it when the solution is already built.</summary>
    [Argument(Format = "--no-build")] public bool? NoBuild => Get<bool?>(() => NoBuild);
    /// <summary>The MSBuild target to run before processing. The default is <c>Build</c>.</summary>
    [Argument(Format = "--target={value}")] public string Target => Get<string>(() => Target);
    /// <summary>Process only the files that match these relative paths or wildcards (<c>*</c> and <c>**</c>).</summary>
    [Argument(Format = "--include={value}", Separator = ";")] public IReadOnlyList<string> Includes => Get<List<string>>(() => Includes);
    /// <summary>Skip the files that match these relative paths or wildcards (<c>*</c> and <c>**</c>). <c>--exclude</c> wins over <c>--include</c>.</summary>
    [Argument(Format = "--exclude={value}", Separator = ";")] public IReadOnlyList<string> Excludes => Get<List<string>>(() => Excludes);
}
#endregion
#region ReSharperInspectCodeSettingsExtensions
/// <inheritdoc cref="ReSharperTasks.ReSharperInspectCode(Fallout.Common.Tools.ReSharper.ReSharperInspectCodeSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class ReSharperInspectCodeSettingsExtensions
{
    #region TargetPath
    /// <inheritdoc cref="ReSharperInspectCodeSettings.TargetPath"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.TargetPath))]
    public static T SetTargetPath<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.TargetPath, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.TargetPath"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.TargetPath))]
    public static T ResetTargetPath<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.TargetPath));
    #endregion
    #region Output
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Output"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Output))]
    public static T SetOutput<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Output, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Output"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Output))]
    public static T ResetOutput<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Output));
    #endregion
    #region Stdout
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Stdout"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Stdout))]
    public static T SetStdout<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Stdout, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Stdout"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Stdout))]
    public static T ResetStdout<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Stdout));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Stdout"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Stdout))]
    public static T EnableStdout<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Stdout, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Stdout"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Stdout))]
    public static T DisableStdout<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Stdout, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Stdout"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Stdout))]
    public static T ToggleStdout<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Stdout, !o.Stdout));
    #endregion
    #region Format
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Format"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Format))]
    public static T SetFormat<T>(this T o, ReSharperFormat v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Format, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Format"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Format))]
    public static T ResetFormat<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Format));
    #endregion
    #region Jobs
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Jobs"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Jobs))]
    public static T SetJobs<T>(this T o, int? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Jobs, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Jobs"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Jobs))]
    public static T ResetJobs<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Jobs));
    #endregion
    #region AbsolutePaths
    /// <inheritdoc cref="ReSharperInspectCodeSettings.AbsolutePaths"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.AbsolutePaths))]
    public static T SetAbsolutePaths<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.AbsolutePaths, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.AbsolutePaths"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.AbsolutePaths))]
    public static T ResetAbsolutePaths<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.AbsolutePaths));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.AbsolutePaths"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.AbsolutePaths))]
    public static T EnableAbsolutePaths<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.AbsolutePaths, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.AbsolutePaths"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.AbsolutePaths))]
    public static T DisableAbsolutePaths<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.AbsolutePaths, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.AbsolutePaths"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.AbsolutePaths))]
    public static T ToggleAbsolutePaths<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.AbsolutePaths, !o.AbsolutePaths));
    #endregion
    #region NoSwea
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoSwea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoSwea))]
    public static T SetNoSwea<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoSwea, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoSwea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoSwea))]
    public static T ResetNoSwea<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.NoSwea));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoSwea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoSwea))]
    public static T EnableNoSwea<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoSwea, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoSwea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoSwea))]
    public static T DisableNoSwea<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoSwea, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoSwea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoSwea))]
    public static T ToggleNoSwea<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoSwea, !o.NoSwea));
    #endregion
    #region Swea
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Swea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Swea))]
    public static T SetSwea<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Swea, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Swea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Swea))]
    public static T ResetSwea<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Swea));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Swea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Swea))]
    public static T EnableSwea<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Swea, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Swea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Swea))]
    public static T DisableSwea<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Swea, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Swea"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Swea))]
    public static T ToggleSwea<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Swea, !o.Swea));
    #endregion
    #region Project
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Project"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Project))]
    public static T SetProject<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Project, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Project"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Project))]
    public static T ResetProject<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Project));
    #endregion
    #region TargetFramework
    /// <inheritdoc cref="ReSharperInspectCodeSettings.TargetFramework"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.TargetFramework))]
    public static T SetTargetFramework<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.TargetFramework, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.TargetFramework"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.TargetFramework))]
    public static T ResetTargetFramework<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.TargetFramework));
    #endregion
    #region Severity
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Severity"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Severity))]
    public static T SetSeverity<T>(this T o, ReSharperSeverity v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Severity, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Severity"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Severity))]
    public static T ResetSeverity<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Severity));
    #endregion
    #region DumpIssuesTypes
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DumpIssuesTypes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DumpIssuesTypes))]
    public static T SetDumpIssuesTypes<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.DumpIssuesTypes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DumpIssuesTypes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DumpIssuesTypes))]
    public static T ResetDumpIssuesTypes<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.DumpIssuesTypes));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DumpIssuesTypes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DumpIssuesTypes))]
    public static T EnableDumpIssuesTypes<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.DumpIssuesTypes, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DumpIssuesTypes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DumpIssuesTypes))]
    public static T DisableDumpIssuesTypes<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.DumpIssuesTypes, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DumpIssuesTypes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DumpIssuesTypes))]
    public static T ToggleDumpIssuesTypes<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.DumpIssuesTypes, !o.DumpIssuesTypes));
    #endregion
    #region Settings
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Settings"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Settings))]
    public static T SetSettings<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Settings, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Settings"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Settings))]
    public static T ResetSettings<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Settings));
    #endregion
    #region DisableSettingsLayers
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DisableSettingsLayers))]
    public static T SetDisableSettingsLayers<T>(this T o, params ReSharperSettingsLayers[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DisableSettingsLayers))]
    public static T SetDisableSettingsLayers<T>(this T o, IEnumerable<ReSharperSettingsLayers> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DisableSettingsLayers))]
    public static T AddDisableSettingsLayers<T>(this T o, params ReSharperSettingsLayers[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DisableSettingsLayers))]
    public static T AddDisableSettingsLayers<T>(this T o, IEnumerable<ReSharperSettingsLayers> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DisableSettingsLayers))]
    public static T RemoveDisableSettingsLayers<T>(this T o, params ReSharperSettingsLayers[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DisableSettingsLayers))]
    public static T RemoveDisableSettingsLayers<T>(this T o, IEnumerable<ReSharperSettingsLayers> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DisableSettingsLayers))]
    public static T ClearDisableSettingsLayers<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.ClearCollection(() => o.DisableSettingsLayers));
    #endregion
    #region NoBuiltinSettings
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuiltinSettings))]
    public static T SetNoBuiltinSettings<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoBuiltinSettings, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuiltinSettings))]
    public static T ResetNoBuiltinSettings<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.NoBuiltinSettings));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuiltinSettings))]
    public static T EnableNoBuiltinSettings<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoBuiltinSettings, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuiltinSettings))]
    public static T DisableNoBuiltinSettings<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoBuiltinSettings, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuiltinSettings))]
    public static T ToggleNoBuiltinSettings<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoBuiltinSettings, !o.NoBuiltinSettings));
    #endregion
    #region Extensions
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Extensions))]
    public static T SetExtensions<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Extensions))]
    public static T SetExtensions<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Extensions))]
    public static T AddExtensions<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Extensions))]
    public static T AddExtensions<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Extensions))]
    public static T RemoveExtensions<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Extensions))]
    public static T RemoveExtensions<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Extensions))]
    public static T ClearExtensions<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.ClearCollection(() => o.Extensions));
    #endregion
    #region Sources
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Sources))]
    public static T SetSources<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Sources))]
    public static T SetSources<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Sources))]
    public static T AddSources<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Sources))]
    public static T AddSources<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Sources))]
    public static T RemoveSources<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Sources))]
    public static T RemoveSources<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Sources))]
    public static T ClearSources<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.ClearCollection(() => o.Sources));
    #endregion
    #region CachesHome
    /// <inheritdoc cref="ReSharperInspectCodeSettings.CachesHome"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.CachesHome))]
    public static T SetCachesHome<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.CachesHome, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.CachesHome"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.CachesHome))]
    public static T ResetCachesHome<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.CachesHome));
    #endregion
    #region CreateConfigFile
    /// <inheritdoc cref="ReSharperInspectCodeSettings.CreateConfigFile"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.CreateConfigFile))]
    public static T SetCreateConfigFile<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.CreateConfigFile, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.CreateConfigFile"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.CreateConfigFile))]
    public static T ResetCreateConfigFile<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.CreateConfigFile));
    #endregion
    #region ConfigFile
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ConfigFile"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ConfigFile))]
    public static T SetConfigFile<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.ConfigFile, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ConfigFile"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ConfigFile))]
    public static T ResetConfigFile<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.ConfigFile));
    #endregion
    #region Verbosity
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Verbosity"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Verbosity))]
    public static T SetVerbosity<T>(this T o, ReSharperVerbosity v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Verbosity, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Verbosity"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Verbosity))]
    public static T ResetVerbosity<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Verbosity));
    #endregion
    #region Debug
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Debug))]
    public static T SetDebug<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Debug, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Debug))]
    public static T ResetDebug<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Debug));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Debug))]
    public static T EnableDebug<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Debug, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Debug))]
    public static T DisableDebug<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Debug, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Debug))]
    public static T ToggleDebug<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Debug, !o.Debug));
    #endregion
    #region NoUpdates
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoUpdates))]
    public static T SetNoUpdates<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoUpdates, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoUpdates))]
    public static T ResetNoUpdates<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.NoUpdates));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoUpdates))]
    public static T EnableNoUpdates<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoUpdates, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoUpdates))]
    public static T DisableNoUpdates<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoUpdates, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoUpdates))]
    public static T ToggleNoUpdates<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoUpdates, !o.NoUpdates));
    #endregion
    #region LogLevel
    /// <inheritdoc cref="ReSharperInspectCodeSettings.LogLevel"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.LogLevel))]
    public static T SetLogLevel<T>(this T o, ReSharperVerbosity v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.LogLevel, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.LogLevel"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.LogLevel))]
    public static T ResetLogLevel<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.LogLevel));
    #endregion
    #region LogFile
    /// <inheritdoc cref="ReSharperInspectCodeSettings.LogFile"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.LogFile))]
    public static T SetLogFile<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.LogFile, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.LogFile"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.LogFile))]
    public static T ResetLogFile<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.LogFile));
    #endregion
    #region LogFolder
    /// <inheritdoc cref="ReSharperInspectCodeSettings.LogFolder"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.LogFolder))]
    public static T SetLogFolder<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.LogFolder, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.LogFolder"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.LogFolder))]
    public static T ResetLogFolder<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.LogFolder));
    #endregion
    #region Measure
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Measure"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Measure))]
    public static T SetMeasure<T>(this T o, ReSharperMeasure v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Measure, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Measure"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Measure))]
    public static T ResetMeasure<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Measure));
    #endregion
    #region Properties
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Properties))]
    public static T SetProperties<T>(this T o, IDictionary<string, string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Properties, v.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase)));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Properties))]
    public static T SetProperty<T>(this T o, string k, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.SetDictionary(() => o.Properties, k, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Properties))]
    public static T AddProperty<T>(this T o, string k, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddDictionary(() => o.Properties, k, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Properties))]
    public static T RemoveProperty<T>(this T o, string k) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveDictionary(() => o.Properties, k));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Properties))]
    public static T ClearProperties<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.ClearDictionary(() => o.Properties));
    #endregion
    #region Toolset
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Toolset"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Toolset))]
    public static T SetToolset<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Toolset, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Toolset"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Toolset))]
    public static T ResetToolset<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Toolset));
    #endregion
    #region ToolsetPath
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ToolsetPath"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ToolsetPath))]
    public static T SetToolsetPath<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.ToolsetPath, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ToolsetPath"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ToolsetPath))]
    public static T ResetToolsetPath<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.ToolsetPath));
    #endregion
    #region DotNetCore
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DotNetCore"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DotNetCore))]
    public static T SetDotNetCore<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.DotNetCore, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DotNetCore"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DotNetCore))]
    public static T ResetDotNetCore<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.DotNetCore));
    #endregion
    #region DotNetCoreSdk
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DotNetCoreSdk"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DotNetCoreSdk))]
    public static T SetDotNetCoreSdk<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.DotNetCoreSdk, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.DotNetCoreSdk"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.DotNetCoreSdk))]
    public static T ResetDotNetCoreSdk<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.DotNetCoreSdk));
    #endregion
    #region MonoPath
    /// <inheritdoc cref="ReSharperInspectCodeSettings.MonoPath"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.MonoPath))]
    public static T SetMonoPath<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.MonoPath, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.MonoPath"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.MonoPath))]
    public static T ResetMonoPath<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.MonoPath));
    #endregion
    #region ReferenceTargets
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ReferenceTargets))]
    public static T SetReferenceTargets<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ReferenceTargets))]
    public static T SetReferenceTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ReferenceTargets))]
    public static T AddReferenceTargets<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ReferenceTargets))]
    public static T AddReferenceTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ReferenceTargets))]
    public static T RemoveReferenceTargets<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ReferenceTargets))]
    public static T RemoveReferenceTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ReferenceTargets))]
    public static T ClearReferenceTargets<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.ClearCollection(() => o.ReferenceTargets));
    #endregion
    #region ItemTargets
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ItemTargets))]
    public static T SetItemTargets<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ItemTargets))]
    public static T SetItemTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ItemTargets))]
    public static T AddItemTargets<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ItemTargets))]
    public static T AddItemTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ItemTargets))]
    public static T RemoveItemTargets<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ItemTargets))]
    public static T RemoveItemTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.ItemTargets))]
    public static T ClearItemTargets<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.ClearCollection(() => o.ItemTargets));
    #endregion
    #region Build
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Build))]
    public static T SetBuild<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Build, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Build))]
    public static T ResetBuild<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Build));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Build))]
    public static T EnableBuild<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Build, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Build))]
    public static T DisableBuild<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Build, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Build))]
    public static T ToggleBuild<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Build, !o.Build));
    #endregion
    #region NoBuild
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuild))]
    public static T SetNoBuild<T>(this T o, bool? v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoBuild, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuild))]
    public static T ResetNoBuild<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.NoBuild));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuild))]
    public static T EnableNoBuild<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoBuild, true));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuild))]
    public static T DisableNoBuild<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoBuild, false));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.NoBuild))]
    public static T ToggleNoBuild<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.NoBuild, !o.NoBuild));
    #endregion
    #region Target
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Target"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Target))]
    public static T SetTarget<T>(this T o, string v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Target, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Target"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Target))]
    public static T ResetTarget<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Remove(() => o.Target));
    #endregion
    #region Includes
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Includes))]
    public static T SetIncludes<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Includes))]
    public static T SetIncludes<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Includes))]
    public static T AddIncludes<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Includes))]
    public static T AddIncludes<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Includes))]
    public static T RemoveIncludes<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Includes))]
    public static T RemoveIncludes<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Includes))]
    public static T ClearIncludes<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.ClearCollection(() => o.Includes));
    #endregion
    #region Excludes
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Excludes))]
    public static T SetExcludes<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Excludes))]
    public static T SetExcludes<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.Set(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Excludes))]
    public static T AddExcludes<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Excludes))]
    public static T AddExcludes<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.AddCollection(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Excludes))]
    public static T RemoveExcludes<T>(this T o, params string[] v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Excludes))]
    public static T RemoveExcludes<T>(this T o, IEnumerable<string> v) where T : ReSharperInspectCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperInspectCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperInspectCodeSettings), Property = nameof(ReSharperInspectCodeSettings.Excludes))]
    public static T ClearExcludes<T>(this T o) where T : ReSharperInspectCodeSettings => o.Modify(b => b.ClearCollection(() => o.Excludes));
    #endregion
}
#endregion
#region ReSharperCleanupCodeSettingsExtensions
/// <inheritdoc cref="ReSharperTasks.ReSharperCleanupCode(Fallout.Common.Tools.ReSharper.ReSharperCleanupCodeSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class ReSharperCleanupCodeSettingsExtensions
{
    #region TargetPath
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.TargetPath"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.TargetPath))]
    public static T SetTargetPath<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.TargetPath, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.TargetPath"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.TargetPath))]
    public static T ResetTargetPath<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.TargetPath));
    #endregion
    #region Profile
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Profile"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Profile))]
    public static T SetProfile<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Profile, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Profile"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Profile))]
    public static T ResetProfile<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.Profile));
    #endregion
    #region Settings
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Settings"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Settings))]
    public static T SetSettings<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Settings, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Settings"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Settings))]
    public static T ResetSettings<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.Settings));
    #endregion
    #region DisableSettingsLayers
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DisableSettingsLayers))]
    public static T SetDisableSettingsLayers<T>(this T o, params ReSharperSettingsLayers[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DisableSettingsLayers))]
    public static T SetDisableSettingsLayers<T>(this T o, IEnumerable<ReSharperSettingsLayers> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DisableSettingsLayers))]
    public static T AddDisableSettingsLayers<T>(this T o, params ReSharperSettingsLayers[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DisableSettingsLayers))]
    public static T AddDisableSettingsLayers<T>(this T o, IEnumerable<ReSharperSettingsLayers> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DisableSettingsLayers))]
    public static T RemoveDisableSettingsLayers<T>(this T o, params ReSharperSettingsLayers[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DisableSettingsLayers))]
    public static T RemoveDisableSettingsLayers<T>(this T o, IEnumerable<ReSharperSettingsLayers> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.DisableSettingsLayers, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DisableSettingsLayers"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DisableSettingsLayers))]
    public static T ClearDisableSettingsLayers<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.ClearCollection(() => o.DisableSettingsLayers));
    #endregion
    #region NoBuiltinSettings
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuiltinSettings))]
    public static T SetNoBuiltinSettings<T>(this T o, bool? v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoBuiltinSettings, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuiltinSettings))]
    public static T ResetNoBuiltinSettings<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.NoBuiltinSettings));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuiltinSettings))]
    public static T EnableNoBuiltinSettings<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoBuiltinSettings, true));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuiltinSettings))]
    public static T DisableNoBuiltinSettings<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoBuiltinSettings, false));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuiltinSettings"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuiltinSettings))]
    public static T ToggleNoBuiltinSettings<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoBuiltinSettings, !o.NoBuiltinSettings));
    #endregion
    #region Extensions
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Extensions))]
    public static T SetExtensions<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Extensions))]
    public static T SetExtensions<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Extensions))]
    public static T AddExtensions<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Extensions))]
    public static T AddExtensions<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Extensions))]
    public static T RemoveExtensions<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Extensions))]
    public static T RemoveExtensions<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Extensions, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Extensions"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Extensions))]
    public static T ClearExtensions<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.ClearCollection(() => o.Extensions));
    #endregion
    #region Sources
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Sources))]
    public static T SetSources<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Sources))]
    public static T SetSources<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Sources))]
    public static T AddSources<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Sources))]
    public static T AddSources<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Sources))]
    public static T RemoveSources<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Sources))]
    public static T RemoveSources<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Sources, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Sources"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Sources))]
    public static T ClearSources<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.ClearCollection(() => o.Sources));
    #endregion
    #region CachesHome
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.CachesHome"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.CachesHome))]
    public static T SetCachesHome<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.CachesHome, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.CachesHome"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.CachesHome))]
    public static T ResetCachesHome<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.CachesHome));
    #endregion
    #region CreateConfigFile
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.CreateConfigFile"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.CreateConfigFile))]
    public static T SetCreateConfigFile<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.CreateConfigFile, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.CreateConfigFile"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.CreateConfigFile))]
    public static T ResetCreateConfigFile<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.CreateConfigFile));
    #endregion
    #region ConfigFile
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ConfigFile"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ConfigFile))]
    public static T SetConfigFile<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.ConfigFile, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ConfigFile"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ConfigFile))]
    public static T ResetConfigFile<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.ConfigFile));
    #endregion
    #region Verbosity
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Verbosity"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Verbosity))]
    public static T SetVerbosity<T>(this T o, ReSharperVerbosity v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Verbosity, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Verbosity"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Verbosity))]
    public static T ResetVerbosity<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.Verbosity));
    #endregion
    #region Debug
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Debug))]
    public static T SetDebug<T>(this T o, bool? v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Debug, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Debug))]
    public static T ResetDebug<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.Debug));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Debug))]
    public static T EnableDebug<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Debug, true));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Debug))]
    public static T DisableDebug<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Debug, false));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Debug"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Debug))]
    public static T ToggleDebug<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Debug, !o.Debug));
    #endregion
    #region NoUpdates
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoUpdates))]
    public static T SetNoUpdates<T>(this T o, bool? v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoUpdates, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoUpdates))]
    public static T ResetNoUpdates<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.NoUpdates));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoUpdates))]
    public static T EnableNoUpdates<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoUpdates, true));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoUpdates))]
    public static T DisableNoUpdates<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoUpdates, false));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoUpdates"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoUpdates))]
    public static T ToggleNoUpdates<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoUpdates, !o.NoUpdates));
    #endregion
    #region LogLevel
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.LogLevel"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.LogLevel))]
    public static T SetLogLevel<T>(this T o, ReSharperVerbosity v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.LogLevel, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.LogLevel"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.LogLevel))]
    public static T ResetLogLevel<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.LogLevel));
    #endregion
    #region LogFile
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.LogFile"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.LogFile))]
    public static T SetLogFile<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.LogFile, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.LogFile"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.LogFile))]
    public static T ResetLogFile<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.LogFile));
    #endregion
    #region LogFolder
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.LogFolder"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.LogFolder))]
    public static T SetLogFolder<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.LogFolder, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.LogFolder"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.LogFolder))]
    public static T ResetLogFolder<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.LogFolder));
    #endregion
    #region Measure
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Measure"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Measure))]
    public static T SetMeasure<T>(this T o, ReSharperMeasure v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Measure, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Measure"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Measure))]
    public static T ResetMeasure<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.Measure));
    #endregion
    #region Properties
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Properties))]
    public static T SetProperties<T>(this T o, IDictionary<string, string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Properties, v.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase)));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Properties))]
    public static T SetProperty<T>(this T o, string k, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.SetDictionary(() => o.Properties, k, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Properties))]
    public static T AddProperty<T>(this T o, string k, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddDictionary(() => o.Properties, k, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Properties))]
    public static T RemoveProperty<T>(this T o, string k) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveDictionary(() => o.Properties, k));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Properties"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Properties))]
    public static T ClearProperties<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.ClearDictionary(() => o.Properties));
    #endregion
    #region Toolset
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Toolset"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Toolset))]
    public static T SetToolset<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Toolset, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Toolset"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Toolset))]
    public static T ResetToolset<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.Toolset));
    #endregion
    #region ToolsetPath
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ToolsetPath"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ToolsetPath))]
    public static T SetToolsetPath<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.ToolsetPath, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ToolsetPath"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ToolsetPath))]
    public static T ResetToolsetPath<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.ToolsetPath));
    #endregion
    #region DotNetCore
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DotNetCore"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DotNetCore))]
    public static T SetDotNetCore<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.DotNetCore, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DotNetCore"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DotNetCore))]
    public static T ResetDotNetCore<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.DotNetCore));
    #endregion
    #region DotNetCoreSdk
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DotNetCoreSdk"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DotNetCoreSdk))]
    public static T SetDotNetCoreSdk<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.DotNetCoreSdk, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.DotNetCoreSdk"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.DotNetCoreSdk))]
    public static T ResetDotNetCoreSdk<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.DotNetCoreSdk));
    #endregion
    #region MonoPath
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.MonoPath"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.MonoPath))]
    public static T SetMonoPath<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.MonoPath, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.MonoPath"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.MonoPath))]
    public static T ResetMonoPath<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.MonoPath));
    #endregion
    #region ReferenceTargets
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ReferenceTargets))]
    public static T SetReferenceTargets<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ReferenceTargets))]
    public static T SetReferenceTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ReferenceTargets))]
    public static T AddReferenceTargets<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ReferenceTargets))]
    public static T AddReferenceTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ReferenceTargets))]
    public static T RemoveReferenceTargets<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ReferenceTargets))]
    public static T RemoveReferenceTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.ReferenceTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ReferenceTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ReferenceTargets))]
    public static T ClearReferenceTargets<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.ClearCollection(() => o.ReferenceTargets));
    #endregion
    #region ItemTargets
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ItemTargets))]
    public static T SetItemTargets<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ItemTargets))]
    public static T SetItemTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ItemTargets))]
    public static T AddItemTargets<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ItemTargets))]
    public static T AddItemTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ItemTargets))]
    public static T RemoveItemTargets<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ItemTargets))]
    public static T RemoveItemTargets<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.ItemTargets, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.ItemTargets"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.ItemTargets))]
    public static T ClearItemTargets<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.ClearCollection(() => o.ItemTargets));
    #endregion
    #region Build
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Build))]
    public static T SetBuild<T>(this T o, bool? v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Build, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Build))]
    public static T ResetBuild<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.Build));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Build))]
    public static T EnableBuild<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Build, true));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Build))]
    public static T DisableBuild<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Build, false));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Build"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Build))]
    public static T ToggleBuild<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Build, !o.Build));
    #endregion
    #region NoBuild
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuild))]
    public static T SetNoBuild<T>(this T o, bool? v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoBuild, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuild))]
    public static T ResetNoBuild<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.NoBuild));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuild))]
    public static T EnableNoBuild<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoBuild, true));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuild))]
    public static T DisableNoBuild<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoBuild, false));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.NoBuild"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.NoBuild))]
    public static T ToggleNoBuild<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.NoBuild, !o.NoBuild));
    #endregion
    #region Target
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Target"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Target))]
    public static T SetTarget<T>(this T o, string v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Target, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Target"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Target))]
    public static T ResetTarget<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Remove(() => o.Target));
    #endregion
    #region Includes
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Includes))]
    public static T SetIncludes<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Includes))]
    public static T SetIncludes<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Includes))]
    public static T AddIncludes<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Includes))]
    public static T AddIncludes<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Includes))]
    public static T RemoveIncludes<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Includes))]
    public static T RemoveIncludes<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Includes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Includes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Includes))]
    public static T ClearIncludes<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.ClearCollection(() => o.Includes));
    #endregion
    #region Excludes
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Excludes))]
    public static T SetExcludes<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Excludes))]
    public static T SetExcludes<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.Set(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Excludes))]
    public static T AddExcludes<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Excludes))]
    public static T AddExcludes<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.AddCollection(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Excludes))]
    public static T RemoveExcludes<T>(this T o, params string[] v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Excludes))]
    public static T RemoveExcludes<T>(this T o, IEnumerable<string> v) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.RemoveCollection(() => o.Excludes, v));
    /// <inheritdoc cref="ReSharperCleanupCodeSettings.Excludes"/>
    [Builder(Type = typeof(ReSharperCleanupCodeSettings), Property = nameof(ReSharperCleanupCodeSettings.Excludes))]
    public static T ClearExcludes<T>(this T o) where T : ReSharperCleanupCodeSettings => o.Modify(b => b.ClearCollection(() => o.Excludes));
    #endregion
}
#endregion
#region ReSharperSettingsLayers
/// <summary>Used within <see cref="ReSharperTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<ReSharperSettingsLayers>))]
public partial class ReSharperSettingsLayers : Enumeration
{
    public static ReSharperSettingsLayers GlobalAll = (ReSharperSettingsLayers) "GlobalAll";
    public static ReSharperSettingsLayers GlobalPerProduct = (ReSharperSettingsLayers) "GlobalPerProduct";
    public static ReSharperSettingsLayers SolutionShared = (ReSharperSettingsLayers) "SolutionShared";
    public static ReSharperSettingsLayers SolutionPersonal = (ReSharperSettingsLayers) "SolutionPersonal";
    public static ReSharperSettingsLayers ProjectShared = (ReSharperSettingsLayers) "ProjectShared";
    public static ReSharperSettingsLayers ProjectPersonal = (ReSharperSettingsLayers) "ProjectPersonal";
    public static implicit operator ReSharperSettingsLayers(string value)
    {
        return new ReSharperSettingsLayers { Value = value };
    }
}
#endregion
#region ReSharperFormat
/// <summary>Used within <see cref="ReSharperTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<ReSharperFormat>))]
public partial class ReSharperFormat : Enumeration
{
    public static ReSharperFormat Xml = (ReSharperFormat) "Xml";
    public static ReSharperFormat Html = (ReSharperFormat) "Html";
    public static ReSharperFormat Text = (ReSharperFormat) "Text";
    public static ReSharperFormat Sarif = (ReSharperFormat) "Sarif";
    public static implicit operator ReSharperFormat(string value)
    {
        return new ReSharperFormat { Value = value };
    }
}
#endregion
#region ReSharperVerbosity
/// <summary>Used within <see cref="ReSharperTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<ReSharperVerbosity>))]
public partial class ReSharperVerbosity : Enumeration
{
    public static ReSharperVerbosity OFF = (ReSharperVerbosity) "OFF";
    public static ReSharperVerbosity FATAL = (ReSharperVerbosity) "FATAL";
    public static ReSharperVerbosity ERROR = (ReSharperVerbosity) "ERROR";
    public static ReSharperVerbosity WARN = (ReSharperVerbosity) "WARN";
    public static ReSharperVerbosity INFO = (ReSharperVerbosity) "INFO";
    public static ReSharperVerbosity VERBOSE = (ReSharperVerbosity) "VERBOSE";
    public static ReSharperVerbosity TRACE = (ReSharperVerbosity) "TRACE";
    public static implicit operator ReSharperVerbosity(string value)
    {
        return new ReSharperVerbosity { Value = value };
    }
}
#endregion
#region ReSharperSeverity
/// <summary>Used within <see cref="ReSharperTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<ReSharperSeverity>))]
public partial class ReSharperSeverity : Enumeration
{
    public static ReSharperSeverity INFO = (ReSharperSeverity) "INFO";
    public static ReSharperSeverity HINT = (ReSharperSeverity) "HINT";
    public static ReSharperSeverity SUGGESTION = (ReSharperSeverity) "SUGGESTION";
    public static ReSharperSeverity WARNING = (ReSharperSeverity) "WARNING";
    public static ReSharperSeverity ERROR = (ReSharperSeverity) "ERROR";
    public static implicit operator ReSharperSeverity(string value)
    {
        return new ReSharperSeverity { Value = value };
    }
}
#endregion
#region ReSharperMeasure
/// <summary>Used within <see cref="ReSharperTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<ReSharperMeasure>))]
public partial class ReSharperMeasure : Enumeration
{
    public static ReSharperMeasure MEMORY = (ReSharperMeasure) "MEMORY";
    public static ReSharperMeasure SAMPLING = (ReSharperMeasure) "SAMPLING";
    public static ReSharperMeasure TIMELINE = (ReSharperMeasure) "TIMELINE";
    public static implicit operator ReSharperMeasure(string value)
    {
        return new ReSharperMeasure { Value = value };
    }
}
#endregion
