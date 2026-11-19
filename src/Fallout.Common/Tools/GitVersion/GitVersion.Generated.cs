// Generated from https://github.com/Fallout-build/Fallout/blob/develop/src/Fallout.Common/Tools/GitVersion/GitVersion.json

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

namespace Fallout.Common.Tools.GitVersion;

/// <summary><p>GitVersion is a tool to help you achieve Semantic Versioning on your project.</p><p>For more details, visit the <a href="https://gitversion.net/docs/">official website</a>.</p></summary>
[ExcludeFromCodeCoverage]
[NuGetTool(Id = PackageId, Executable = PackageExecutable)]
public partial class GitVersionTasks : ToolTasks, IRequireNuGetPackage
{
    public static string GitVersionPath { get => new GitVersionTasks().GetToolPathInternal(); set => new GitVersionTasks().SetToolPath(value); }
    public const string PackageId = "GitVersion.Tool";
    public const string PackageExecutable = "GitVersion.dll|GitVersion.exe";
    /// <summary><p>GitVersion is a tool to help you achieve Semantic Versioning on your project.</p><p>For more details, visit the <a href="https://gitversion.net/docs/">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> GitVersion(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => new GitVersionTasks().Run(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>GitVersion is a tool to help you achieve Semantic Versioning on your project.</p><p>For more details, visit the <a href="https://gitversion.net/docs/">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>&lt;targetPath&gt;</c> via <see cref="GitVersionSettings.TargetPath"/></li><li><c>--allow-shallow</c> via <see cref="GitVersionSettings.AllowShallow"/></li><li><c>--branch</c> via <see cref="GitVersionSettings.Branch"/></li><li><c>--commit</c> via <see cref="GitVersionSettings.Commit"/></li><li><c>--config</c> via <see cref="GitVersionSettings.Config"/></li><li><c>--diagnose</c> via <see cref="GitVersionSettings.Diagnostics"/></li><li><c>--dynamic-repo-location</c> via <see cref="GitVersionSettings.DynamicRepositoryLocation"/></li><li><c>--ensure-assembly-info</c> via <see cref="GitVersionSettings.EnsureAssemblyInfo"/></li><li><c>--format</c> via <see cref="GitVersionSettings.Format"/></li><li><c>--log-file</c> via <see cref="GitVersionSettings.LogFile"/></li><li><c>--no-cache</c> via <see cref="GitVersionSettings.NoCache"/></li><li><c>--no-fetch</c> via <see cref="GitVersionSettings.NoFetch"/></li><li><c>--no-normalize</c> via <see cref="GitVersionSettings.NoNormalize"/></li><li><c>--output</c> via <see cref="GitVersionSettings.Output"/></li><li><c>--output-file</c> via <see cref="GitVersionSettings.OutputFile"/></li><li><c>--override-config</c> via <see cref="GitVersionSettings.ConfigurationOverride"/></li><li><c>--password</c> via <see cref="GitVersionSettings.Password"/></li><li><c>--show-config</c> via <see cref="GitVersionSettings.ShowConfig"/></li><li><c>--show-variable</c> via <see cref="GitVersionSettings.ShowVariable"/></li><li><c>--telemetry-opt-out</c> via <see cref="GitVersionSettings.TelemetryOptOut"/></li><li><c>--update-assembly-info</c> via <see cref="GitVersionSettings.UpdateAssemblyInfo"/></li><li><c>--update-assembly-info</c> via <see cref="GitVersionSettings.UpdateAssemblyInfoFileNames"/></li><li><c>--update-project-files</c> via <see cref="GitVersionSettings.UpdateProjectFiles"/></li><li><c>--update-wix-version-file</c> via <see cref="GitVersionSettings.UpdateWixVersionFile"/></li><li><c>--url</c> via <see cref="GitVersionSettings.Url"/></li><li><c>--username</c> via <see cref="GitVersionSettings.Username"/></li><li><c>--verbosity</c> via <see cref="GitVersionSettings.Verbosity"/></li><li><c>--version</c> via <see cref="GitVersionSettings.Version"/></li></ul></remarks>
    public static (GitVersion Result, IReadOnlyCollection<Output> Output) GitVersion(GitVersionSettings options = null) => new GitVersionTasks().Run<GitVersionSettings, GitVersion>(options);
    /// <inheritdoc cref="GitVersionTasks.GitVersion(Fallout.Common.Tools.GitVersion.GitVersionSettings)"/>
    public static (GitVersion Result, IReadOnlyCollection<Output> Output) GitVersion(Configure<GitVersionSettings> configurator) => new GitVersionTasks().Run<GitVersionSettings, GitVersion>(configurator.Invoke(new GitVersionSettings()));
    /// <inheritdoc cref="GitVersionTasks.GitVersion(Fallout.Common.Tools.GitVersion.GitVersionSettings)"/>
    public static IEnumerable<(GitVersionSettings Settings, GitVersion Result, IReadOnlyCollection<Output> Output)> GitVersion(CombinatorialConfigure<GitVersionSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(GitVersion, degreeOfParallelism, completeOnFailure);
}
#region GitVersionSettings
/// <inheritdoc cref="GitVersionTasks.GitVersion(Fallout.Common.Tools.GitVersion.GitVersionSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(GitVersionTasks), Command = nameof(GitVersionTasks.GitVersion))]
public partial class GitVersionSettings : ToolOptions, IToolOptionsWithFramework
{
    /// <summary>The directory containing .git. If not defined current directory is used. (Must be first argument).</summary>
    [Argument(Format = "{value}", Position = 1)] public string TargetPath => Get<string>(() => TargetPath);
    /// <summary>Displays the version of GitVersion.</summary>
    [Argument(Format = "--version")] public bool? Version => Get<bool?>(() => Version);
    /// <summary>Runs GitVersion with additional diagnostic information. Also needs <c>--log-file</c> (requires git.exe to be installed).</summary>
    [Argument(Format = "--diagnose")] public bool? Diagnostics => Get<bool?>(() => Diagnostics);
    /// <summary>Determines the output to the console. Can be <c>json</c>, <c>file</c>, <c>buildserver</c>, or <c>dotenv</c>. The default is <c>json</c>.</summary>
    [Argument(Format = "--output {value}")] public GitVersionOutput Output => Get<GitVersionOutput>(() => Output);
    /// <summary>Path to the output file. Used together with <c>--output file</c>.</summary>
    [Argument(Format = "--output-file {value}")] public string OutputFile => Get<string>(() => OutputFile);
    /// <summary>Used together with <c>--output json</c>. Prints one variable. For example, <c>--output json --show-variable SemVer</c> prints <c>1.2.3+beta.4</c>.</summary>
    [Argument(Format = "--show-variable {value}")] public string ShowVariable => Get<string>(() => ShowVariable);
    /// <summary>Used together with <c>--output json</c>. Prints a format string that contains version variables. For example, <c>--format {Major}.{Minor}</c> prints <c>1.2</c>.</summary>
    [Argument(Format = "--format {value}")] public string Format => Get<string>(() => Format);
    /// <summary>Path to the log file. Use <c>console</c> to write the log to stderr.</summary>
    [Argument(Format = "--log-file {value}")] public string LogFile => Get<string>(() => LogFile);
    /// <summary>Path to the config file. The default is <c>GitVersion.yml</c>, <c>GitVersion.yaml</c>, <c>.GitVersion.yml</c>, or <c>.GitVersion.yaml</c>.</summary>
    [Argument(Format = "--config {value}")] public string Config => Get<string>(() => Config);
    /// <summary>Prints the effective GitVersion config in YAML format.</summary>
    [Argument(Format = "--show-config")] public bool? ShowConfig => Get<bool?>(() => ShowConfig);
    /// <summary>Overrides GitVersion config values inline. For example, <c>--override-config workflow=GitHubFlow/v1</c>.</summary>
    [Argument(Format = "--override-config {key}={value}")] public IReadOnlyDictionary<string, object> ConfigurationOverride => Get<Dictionary<string, object>>(() => ConfigurationOverride);
    /// <summary>Bypasses the cache. The result is not written to the cache.</summary>
    [Argument(Format = "--no-cache")] public bool? NoCache => Get<bool?>(() => NoCache);
    /// <summary>Disables the normalize step on a build server.</summary>
    [Argument(Format = "--no-normalize")] public bool? NoNormalize => Get<bool?>(() => NoNormalize);
    /// <summary>Allows GitVersion to run on a shallow clone. The clone must still contain the history needed to calculate the version.</summary>
    [Argument(Format = "--allow-shallow")] public bool? AllowShallow => Get<bool?>(() => AllowShallow);
    /// <summary>Recursively searches for <c>AssemblyInfo.cs</c> files in the git repo and updates them.</summary>
    [Argument(Format = "--update-assembly-info")] public bool? UpdateAssemblyInfo => Get<bool?>(() => UpdateAssemblyInfo);
    /// <summary>Names of AssemblyInfo files to update.</summary>
    [Argument(Format = "--update-assembly-info {value}", Separator = " ")] public IReadOnlyList<string> UpdateAssemblyInfoFileNames => Get<List<string>>(() => UpdateAssemblyInfoFileNames);
    /// <summary>Creates a missing assembly info file named by <c>--update-assembly-info</c>. The file gets AssemblyFileVersion, AssemblyVersion, and AssemblyInformationalVersion.</summary>
    [Argument(Format = "--ensure-assembly-info")] public bool? EnsureAssemblyInfo => Get<bool?>(() => EnsureAssemblyInfo);
    /// <summary>Project files (<c>.csproj</c>, <c>.vbproj</c>, <c>.fsproj</c>, <c>.sqlproj</c>) to update. Only SDK-style projects are supported.</summary>
    [Argument(Format = "--update-project-files {value}", Separator = " ")] public IReadOnlyList<string> UpdateProjectFiles => Get<List<string>>(() => UpdateProjectFiles);
    /// <summary>Writes the GitVersion variables to <c>GitVersion_WixVersion.wxi</c>.</summary>
    [Argument(Format = "--update-wix-version-file")] public bool? UpdateWixVersionFile => Get<bool?>(() => UpdateWixVersionFile);
    /// <summary>Url to the remote git repository.</summary>
    [Argument(Format = "--url {value}")] public string Url => Get<string>(() => Url);
    /// <summary>Branch to version. This is used on its own for a local branch, or together with <c>--url</c> for a remote repository.</summary>
    [Argument(Format = "--branch {value}")] public string Branch => Get<string>(() => Branch);
    /// <summary>Username when authentication is required.</summary>
    [Argument(Format = "--username {value}")] public string Username => Get<string>(() => Username);
    /// <summary>Password when authentication is required.</summary>
    [Argument(Format = "--password {value}", Secret = true)] public string Password => Get<string>(() => Password);
    /// <summary>The commit id to check. If not specified, the latest commit on the specified branch is used.</summary>
    [Argument(Format = "--commit {value}")] public string Commit => Get<string>(() => Commit);
    /// <summary>Directory for dynamic repositories. The default is the temp directory.</summary>
    [Argument(Format = "--dynamic-repo-location {value}")] public string DynamicRepositoryLocation => Get<string>(() => DynamicRepositoryLocation);
    /// <summary>Disables <c>git fetch</c> during version calculation. GitVersion may then calculate a different version than expected.</summary>
    [Argument(Format = "--no-fetch")] public bool? NoFetch => Get<bool?>(() => NoFetch);
    /// <summary>Removed in GitVersion 7.</summary>
    public string Executable => Get<string>(() => Executable);
    /// <summary>Removed in GitVersion 7.</summary>
    public string ExecutableArguments => Get<string>(() => ExecutableArguments);
    /// <summary>Removed in GitVersion 7.</summary>
    public string MSBuildProject => Get<string>(() => MSBuildProject);
    /// <summary>Removed in GitVersion 7.</summary>
    public string MSBuildProjectArguments => Get<string>(() => MSBuildProjectArguments);
    /// <summary>Disables optional sampled usage telemetry.</summary>
    [Argument(Format = "--telemetry-opt-out")] public bool? TelemetryOptOut => Get<bool?>(() => TelemetryOptOut);
    /// <summary>Amount of information to display (<c>Quiet</c>, <c>Minimal</c>, <c>Normal</c>, <c>Verbose</c>, <c>Diagnostic</c>). The default is <c>Normal</c>.</summary>
    [Argument(Format = "--verbosity {value}")] public GitVersionVerbosity Verbosity => Get<GitVersionVerbosity>(() => Verbosity);
}
#endregion
#region GitVersionSettingsExtensions
/// <inheritdoc cref="GitVersionTasks.GitVersion(Fallout.Common.Tools.GitVersion.GitVersionSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class GitVersionSettingsExtensions
{
    #region TargetPath
    /// <inheritdoc cref="GitVersionSettings.TargetPath"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.TargetPath))]
    public static T SetTargetPath<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.TargetPath, v));
    /// <inheritdoc cref="GitVersionSettings.TargetPath"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.TargetPath))]
    public static T ResetTargetPath<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.TargetPath));
    #endregion
    #region Version
    /// <inheritdoc cref="GitVersionSettings.Version"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Version))]
    public static T SetVersion<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Version, v));
    /// <inheritdoc cref="GitVersionSettings.Version"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Version))]
    public static T ResetVersion<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Version));
    /// <inheritdoc cref="GitVersionSettings.Version"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Version))]
    public static T EnableVersion<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Version, true));
    /// <inheritdoc cref="GitVersionSettings.Version"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Version))]
    public static T DisableVersion<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Version, false));
    /// <inheritdoc cref="GitVersionSettings.Version"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Version))]
    public static T ToggleVersion<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Version, !o.Version));
    #endregion
    #region Diagnostics
    /// <inheritdoc cref="GitVersionSettings.Diagnostics"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Diagnostics))]
    public static T SetDiagnostics<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Diagnostics, v));
    /// <inheritdoc cref="GitVersionSettings.Diagnostics"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Diagnostics))]
    public static T ResetDiagnostics<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Diagnostics));
    /// <inheritdoc cref="GitVersionSettings.Diagnostics"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Diagnostics))]
    public static T EnableDiagnostics<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Diagnostics, true));
    /// <inheritdoc cref="GitVersionSettings.Diagnostics"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Diagnostics))]
    public static T DisableDiagnostics<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Diagnostics, false));
    /// <inheritdoc cref="GitVersionSettings.Diagnostics"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Diagnostics))]
    public static T ToggleDiagnostics<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Diagnostics, !o.Diagnostics));
    #endregion
    #region Output
    /// <inheritdoc cref="GitVersionSettings.Output"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Output))]
    public static T SetOutput<T>(this T o, GitVersionOutput v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Output, v));
    /// <inheritdoc cref="GitVersionSettings.Output"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Output))]
    public static T ResetOutput<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Output));
    #endregion
    #region OutputFile
    /// <inheritdoc cref="GitVersionSettings.OutputFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.OutputFile))]
    public static T SetOutputFile<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.OutputFile, v));
    /// <inheritdoc cref="GitVersionSettings.OutputFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.OutputFile))]
    public static T ResetOutputFile<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.OutputFile));
    #endregion
    #region ShowVariable
    /// <inheritdoc cref="GitVersionSettings.ShowVariable"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ShowVariable))]
    public static T SetShowVariable<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.ShowVariable, v));
    /// <inheritdoc cref="GitVersionSettings.ShowVariable"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ShowVariable))]
    public static T ResetShowVariable<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.ShowVariable));
    #endregion
    #region Format
    /// <inheritdoc cref="GitVersionSettings.Format"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Format))]
    public static T SetFormat<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Format, v));
    /// <inheritdoc cref="GitVersionSettings.Format"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Format))]
    public static T ResetFormat<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Format));
    #endregion
    #region LogFile
    /// <inheritdoc cref="GitVersionSettings.LogFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.LogFile))]
    public static T SetLogFile<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.LogFile, v));
    /// <inheritdoc cref="GitVersionSettings.LogFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.LogFile))]
    public static T ResetLogFile<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.LogFile));
    #endregion
    #region Config
    /// <inheritdoc cref="GitVersionSettings.Config"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Config))]
    public static T SetConfig<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Config, v));
    /// <inheritdoc cref="GitVersionSettings.Config"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Config))]
    public static T ResetConfig<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Config));
    #endregion
    #region ShowConfig
    /// <inheritdoc cref="GitVersionSettings.ShowConfig"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ShowConfig))]
    public static T SetShowConfig<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.ShowConfig, v));
    /// <inheritdoc cref="GitVersionSettings.ShowConfig"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ShowConfig))]
    public static T ResetShowConfig<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.ShowConfig));
    /// <inheritdoc cref="GitVersionSettings.ShowConfig"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ShowConfig))]
    public static T EnableShowConfig<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.ShowConfig, true));
    /// <inheritdoc cref="GitVersionSettings.ShowConfig"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ShowConfig))]
    public static T DisableShowConfig<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.ShowConfig, false));
    /// <inheritdoc cref="GitVersionSettings.ShowConfig"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ShowConfig))]
    public static T ToggleShowConfig<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.ShowConfig, !o.ShowConfig));
    #endregion
    #region ConfigurationOverride
    /// <inheritdoc cref="GitVersionSettings.ConfigurationOverride"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ConfigurationOverride))]
    public static T SetConfigurationOverride<T>(this T o, IDictionary<string, object> v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.ConfigurationOverride, v.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase)));
    /// <inheritdoc cref="GitVersionSettings.ConfigurationOverride"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ConfigurationOverride))]
    public static T SetConfigurationOverride<T>(this T o, string k, object v) where T : GitVersionSettings => o.Modify(b => b.SetDictionary(() => o.ConfigurationOverride, k, v));
    /// <inheritdoc cref="GitVersionSettings.ConfigurationOverride"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ConfigurationOverride))]
    public static T AddConfigurationOverride<T>(this T o, string k, object v) where T : GitVersionSettings => o.Modify(b => b.AddDictionary(() => o.ConfigurationOverride, k, v));
    /// <inheritdoc cref="GitVersionSettings.ConfigurationOverride"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ConfigurationOverride))]
    public static T RemoveConfigurationOverride<T>(this T o, string k) where T : GitVersionSettings => o.Modify(b => b.RemoveDictionary(() => o.ConfigurationOverride, k));
    /// <inheritdoc cref="GitVersionSettings.ConfigurationOverride"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ConfigurationOverride))]
    public static T ClearConfigurationOverride<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.ClearDictionary(() => o.ConfigurationOverride));
    #endregion
    #region NoCache
    /// <inheritdoc cref="GitVersionSettings.NoCache"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoCache))]
    public static T SetNoCache<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoCache, v));
    /// <inheritdoc cref="GitVersionSettings.NoCache"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoCache))]
    public static T ResetNoCache<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.NoCache));
    /// <inheritdoc cref="GitVersionSettings.NoCache"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoCache))]
    public static T EnableNoCache<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoCache, true));
    /// <inheritdoc cref="GitVersionSettings.NoCache"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoCache))]
    public static T DisableNoCache<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoCache, false));
    /// <inheritdoc cref="GitVersionSettings.NoCache"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoCache))]
    public static T ToggleNoCache<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoCache, !o.NoCache));
    #endregion
    #region NoNormalize
    /// <inheritdoc cref="GitVersionSettings.NoNormalize"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoNormalize))]
    public static T SetNoNormalize<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoNormalize, v));
    /// <inheritdoc cref="GitVersionSettings.NoNormalize"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoNormalize))]
    public static T ResetNoNormalize<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.NoNormalize));
    /// <inheritdoc cref="GitVersionSettings.NoNormalize"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoNormalize))]
    public static T EnableNoNormalize<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoNormalize, true));
    /// <inheritdoc cref="GitVersionSettings.NoNormalize"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoNormalize))]
    public static T DisableNoNormalize<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoNormalize, false));
    /// <inheritdoc cref="GitVersionSettings.NoNormalize"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoNormalize))]
    public static T ToggleNoNormalize<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoNormalize, !o.NoNormalize));
    #endregion
    #region AllowShallow
    /// <inheritdoc cref="GitVersionSettings.AllowShallow"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.AllowShallow))]
    public static T SetAllowShallow<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.AllowShallow, v));
    /// <inheritdoc cref="GitVersionSettings.AllowShallow"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.AllowShallow))]
    public static T ResetAllowShallow<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.AllowShallow));
    /// <inheritdoc cref="GitVersionSettings.AllowShallow"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.AllowShallow))]
    public static T EnableAllowShallow<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.AllowShallow, true));
    /// <inheritdoc cref="GitVersionSettings.AllowShallow"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.AllowShallow))]
    public static T DisableAllowShallow<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.AllowShallow, false));
    /// <inheritdoc cref="GitVersionSettings.AllowShallow"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.AllowShallow))]
    public static T ToggleAllowShallow<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.AllowShallow, !o.AllowShallow));
    #endregion
    #region UpdateAssemblyInfo
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfo))]
    public static T SetUpdateAssemblyInfo<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateAssemblyInfo, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfo))]
    public static T ResetUpdateAssemblyInfo<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.UpdateAssemblyInfo));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfo))]
    public static T EnableUpdateAssemblyInfo<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateAssemblyInfo, true));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfo))]
    public static T DisableUpdateAssemblyInfo<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateAssemblyInfo, false));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfo))]
    public static T ToggleUpdateAssemblyInfo<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateAssemblyInfo, !o.UpdateAssemblyInfo));
    #endregion
    #region UpdateAssemblyInfoFileNames
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfoFileNames"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfoFileNames))]
    public static T SetUpdateAssemblyInfoFileNames<T>(this T o, params string[] v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateAssemblyInfoFileNames, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfoFileNames"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfoFileNames))]
    public static T SetUpdateAssemblyInfoFileNames<T>(this T o, IEnumerable<string> v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateAssemblyInfoFileNames, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfoFileNames"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfoFileNames))]
    public static T AddUpdateAssemblyInfoFileNames<T>(this T o, params string[] v) where T : GitVersionSettings => o.Modify(b => b.AddCollection(() => o.UpdateAssemblyInfoFileNames, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfoFileNames"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfoFileNames))]
    public static T AddUpdateAssemblyInfoFileNames<T>(this T o, IEnumerable<string> v) where T : GitVersionSettings => o.Modify(b => b.AddCollection(() => o.UpdateAssemblyInfoFileNames, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfoFileNames"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfoFileNames))]
    public static T RemoveUpdateAssemblyInfoFileNames<T>(this T o, params string[] v) where T : GitVersionSettings => o.Modify(b => b.RemoveCollection(() => o.UpdateAssemblyInfoFileNames, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfoFileNames"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfoFileNames))]
    public static T RemoveUpdateAssemblyInfoFileNames<T>(this T o, IEnumerable<string> v) where T : GitVersionSettings => o.Modify(b => b.RemoveCollection(() => o.UpdateAssemblyInfoFileNames, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateAssemblyInfoFileNames"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateAssemblyInfoFileNames))]
    public static T ClearUpdateAssemblyInfoFileNames<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.ClearCollection(() => o.UpdateAssemblyInfoFileNames));
    #endregion
    #region EnsureAssemblyInfo
    /// <inheritdoc cref="GitVersionSettings.EnsureAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.EnsureAssemblyInfo))]
    public static T SetEnsureAssemblyInfo<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.EnsureAssemblyInfo, v));
    /// <inheritdoc cref="GitVersionSettings.EnsureAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.EnsureAssemblyInfo))]
    public static T ResetEnsureAssemblyInfo<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.EnsureAssemblyInfo));
    /// <inheritdoc cref="GitVersionSettings.EnsureAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.EnsureAssemblyInfo))]
    public static T EnableEnsureAssemblyInfo<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.EnsureAssemblyInfo, true));
    /// <inheritdoc cref="GitVersionSettings.EnsureAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.EnsureAssemblyInfo))]
    public static T DisableEnsureAssemblyInfo<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.EnsureAssemblyInfo, false));
    /// <inheritdoc cref="GitVersionSettings.EnsureAssemblyInfo"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.EnsureAssemblyInfo))]
    public static T ToggleEnsureAssemblyInfo<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.EnsureAssemblyInfo, !o.EnsureAssemblyInfo));
    #endregion
    #region UpdateProjectFiles
    /// <inheritdoc cref="GitVersionSettings.UpdateProjectFiles"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateProjectFiles))]
    public static T SetUpdateProjectFiles<T>(this T o, params string[] v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateProjectFiles, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateProjectFiles"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateProjectFiles))]
    public static T SetUpdateProjectFiles<T>(this T o, IEnumerable<string> v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateProjectFiles, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateProjectFiles"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateProjectFiles))]
    public static T AddUpdateProjectFiles<T>(this T o, params string[] v) where T : GitVersionSettings => o.Modify(b => b.AddCollection(() => o.UpdateProjectFiles, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateProjectFiles"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateProjectFiles))]
    public static T AddUpdateProjectFiles<T>(this T o, IEnumerable<string> v) where T : GitVersionSettings => o.Modify(b => b.AddCollection(() => o.UpdateProjectFiles, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateProjectFiles"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateProjectFiles))]
    public static T RemoveUpdateProjectFiles<T>(this T o, params string[] v) where T : GitVersionSettings => o.Modify(b => b.RemoveCollection(() => o.UpdateProjectFiles, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateProjectFiles"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateProjectFiles))]
    public static T RemoveUpdateProjectFiles<T>(this T o, IEnumerable<string> v) where T : GitVersionSettings => o.Modify(b => b.RemoveCollection(() => o.UpdateProjectFiles, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateProjectFiles"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateProjectFiles))]
    public static T ClearUpdateProjectFiles<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.ClearCollection(() => o.UpdateProjectFiles));
    #endregion
    #region UpdateWixVersionFile
    /// <inheritdoc cref="GitVersionSettings.UpdateWixVersionFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateWixVersionFile))]
    public static T SetUpdateWixVersionFile<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateWixVersionFile, v));
    /// <inheritdoc cref="GitVersionSettings.UpdateWixVersionFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateWixVersionFile))]
    public static T ResetUpdateWixVersionFile<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.UpdateWixVersionFile));
    /// <inheritdoc cref="GitVersionSettings.UpdateWixVersionFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateWixVersionFile))]
    public static T EnableUpdateWixVersionFile<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateWixVersionFile, true));
    /// <inheritdoc cref="GitVersionSettings.UpdateWixVersionFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateWixVersionFile))]
    public static T DisableUpdateWixVersionFile<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateWixVersionFile, false));
    /// <inheritdoc cref="GitVersionSettings.UpdateWixVersionFile"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.UpdateWixVersionFile))]
    public static T ToggleUpdateWixVersionFile<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.UpdateWixVersionFile, !o.UpdateWixVersionFile));
    #endregion
    #region Url
    /// <inheritdoc cref="GitVersionSettings.Url"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Url))]
    public static T SetUrl<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Url, v));
    /// <inheritdoc cref="GitVersionSettings.Url"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Url))]
    public static T ResetUrl<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Url));
    #endregion
    #region Branch
    /// <inheritdoc cref="GitVersionSettings.Branch"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Branch))]
    public static T SetBranch<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Branch, v));
    /// <inheritdoc cref="GitVersionSettings.Branch"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Branch))]
    public static T ResetBranch<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Branch));
    #endregion
    #region Username
    /// <inheritdoc cref="GitVersionSettings.Username"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Username))]
    public static T SetUsername<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Username, v));
    /// <inheritdoc cref="GitVersionSettings.Username"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Username))]
    public static T ResetUsername<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Username));
    #endregion
    #region Password
    /// <inheritdoc cref="GitVersionSettings.Password"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Password))]
    public static T SetPassword<T>(this T o, [Secret] string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Password, v));
    /// <inheritdoc cref="GitVersionSettings.Password"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Password))]
    public static T ResetPassword<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Password));
    #endregion
    #region Commit
    /// <inheritdoc cref="GitVersionSettings.Commit"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Commit))]
    public static T SetCommit<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Commit, v));
    /// <inheritdoc cref="GitVersionSettings.Commit"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Commit))]
    public static T ResetCommit<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Commit));
    #endregion
    #region DynamicRepositoryLocation
    /// <inheritdoc cref="GitVersionSettings.DynamicRepositoryLocation"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.DynamicRepositoryLocation))]
    public static T SetDynamicRepositoryLocation<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.DynamicRepositoryLocation, v));
    /// <inheritdoc cref="GitVersionSettings.DynamicRepositoryLocation"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.DynamicRepositoryLocation))]
    public static T ResetDynamicRepositoryLocation<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.DynamicRepositoryLocation));
    #endregion
    #region NoFetch
    /// <inheritdoc cref="GitVersionSettings.NoFetch"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoFetch))]
    public static T SetNoFetch<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoFetch, v));
    /// <inheritdoc cref="GitVersionSettings.NoFetch"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoFetch))]
    public static T ResetNoFetch<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.NoFetch));
    /// <inheritdoc cref="GitVersionSettings.NoFetch"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoFetch))]
    public static T EnableNoFetch<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoFetch, true));
    /// <inheritdoc cref="GitVersionSettings.NoFetch"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoFetch))]
    public static T DisableNoFetch<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoFetch, false));
    /// <inheritdoc cref="GitVersionSettings.NoFetch"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.NoFetch))]
    public static T ToggleNoFetch<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.NoFetch, !o.NoFetch));
    #endregion
    #region Executable
    /// <inheritdoc cref="GitVersionSettings.Executable"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Executable))]
    [Obsolete("Removed in GitVersion 7.")]
    public static T SetExecutable<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Executable, v));
    /// <inheritdoc cref="GitVersionSettings.Executable"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Executable))]
    [Obsolete("Removed in GitVersion 7.")]
    public static T ResetExecutable<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Executable));
    #endregion
    #region ExecutableArguments
    /// <inheritdoc cref="GitVersionSettings.ExecutableArguments"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ExecutableArguments))]
    [Obsolete("Removed in GitVersion 7.")]
    public static T SetExecutableArguments<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.ExecutableArguments, v));
    /// <inheritdoc cref="GitVersionSettings.ExecutableArguments"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.ExecutableArguments))]
    [Obsolete("Removed in GitVersion 7.")]
    public static T ResetExecutableArguments<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.ExecutableArguments));
    #endregion
    #region MSBuildProject
    /// <inheritdoc cref="GitVersionSettings.MSBuildProject"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.MSBuildProject))]
    [Obsolete("Removed in GitVersion 7.")]
    public static T SetMSBuildProject<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.MSBuildProject, v));
    /// <inheritdoc cref="GitVersionSettings.MSBuildProject"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.MSBuildProject))]
    [Obsolete("Removed in GitVersion 7.")]
    public static T ResetMSBuildProject<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.MSBuildProject));
    #endregion
    #region MSBuildProjectArguments
    /// <inheritdoc cref="GitVersionSettings.MSBuildProjectArguments"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.MSBuildProjectArguments))]
    [Obsolete("Removed in GitVersion 7.")]
    public static T SetMSBuildProjectArguments<T>(this T o, string v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.MSBuildProjectArguments, v));
    /// <inheritdoc cref="GitVersionSettings.MSBuildProjectArguments"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.MSBuildProjectArguments))]
    [Obsolete("Removed in GitVersion 7.")]
    public static T ResetMSBuildProjectArguments<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.MSBuildProjectArguments));
    #endregion
    #region TelemetryOptOut
    /// <inheritdoc cref="GitVersionSettings.TelemetryOptOut"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.TelemetryOptOut))]
    public static T SetTelemetryOptOut<T>(this T o, bool? v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.TelemetryOptOut, v));
    /// <inheritdoc cref="GitVersionSettings.TelemetryOptOut"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.TelemetryOptOut))]
    public static T ResetTelemetryOptOut<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.TelemetryOptOut));
    /// <inheritdoc cref="GitVersionSettings.TelemetryOptOut"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.TelemetryOptOut))]
    public static T EnableTelemetryOptOut<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.TelemetryOptOut, true));
    /// <inheritdoc cref="GitVersionSettings.TelemetryOptOut"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.TelemetryOptOut))]
    public static T DisableTelemetryOptOut<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.TelemetryOptOut, false));
    /// <inheritdoc cref="GitVersionSettings.TelemetryOptOut"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.TelemetryOptOut))]
    public static T ToggleTelemetryOptOut<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.TelemetryOptOut, !o.TelemetryOptOut));
    #endregion
    #region Verbosity
    /// <inheritdoc cref="GitVersionSettings.Verbosity"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Verbosity))]
    public static T SetVerbosity<T>(this T o, GitVersionVerbosity v) where T : GitVersionSettings => o.Modify(b => b.Set(() => o.Verbosity, v));
    /// <inheritdoc cref="GitVersionSettings.Verbosity"/>
    [Builder(Type = typeof(GitVersionSettings), Property = nameof(GitVersionSettings.Verbosity))]
    public static T ResetVerbosity<T>(this T o) where T : GitVersionSettings => o.Modify(b => b.Remove(() => o.Verbosity));
    #endregion
}
#endregion
#region GitVersionOutput
/// <summary>Used within <see cref="GitVersionTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<GitVersionOutput>))]
public partial class GitVersionOutput : Enumeration
{
    public static GitVersionOutput json = (GitVersionOutput) "json";
    public static GitVersionOutput file = (GitVersionOutput) "file";
    public static GitVersionOutput buildserver = (GitVersionOutput) "buildserver";
    public static GitVersionOutput dotenv = (GitVersionOutput) "dotenv";
    public static implicit operator GitVersionOutput(string value)
    {
        return new GitVersionOutput { Value = value };
    }
}
#endregion
#region GitVersionVerbosity
/// <summary>Used within <see cref="GitVersionTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<GitVersionVerbosity>))]
public partial class GitVersionVerbosity : Enumeration
{
    public static GitVersionVerbosity Quiet = (GitVersionVerbosity) "Quiet";
    public static GitVersionVerbosity Minimal = (GitVersionVerbosity) "Minimal";
    public static GitVersionVerbosity Normal = (GitVersionVerbosity) "Normal";
    public static GitVersionVerbosity Verbose = (GitVersionVerbosity) "Verbose";
    public static GitVersionVerbosity Diagnostic = (GitVersionVerbosity) "Diagnostic";
    public static implicit operator GitVersionVerbosity(string value)
    {
        return new GitVersionVerbosity { Value = value };
    }
}
#endregion
