// Generated from https://github.com/Fallout-build/Fallout/blob/develop/src/Fallout.Common/Tools/CTest/CTest.json

using Fallout.Common;
using Fallout.Common.Tooling;
using Fallout.Common.Tools;
using Fallout.Common.Tools.CMake;
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

namespace Fallout.Common.Tools.CTest;

/// <summary><p>The ctest executable is the CMake test driver program. It runs the tests of a build tree that CMake generated.</p><p>For more details, visit the <a href="https://cmake.org/cmake/help/latest/manual/ctest.1.html">official website</a>.</p></summary>
[ExcludeFromCodeCoverage]
[PathTool(Executable = PathExecutable)]
public partial class CTestTasks : ToolTasks, IRequirePathTool
{
    public static string CTestPath { get => new CTestTasks().GetToolPathInternal(); set => new CTestTasks().SetToolPath(value); }
    public const string PathExecutable = "ctest";
    /// <summary><p>The ctest executable is the CMake test driver program. It runs the tests of a build tree that CMake generated.</p><p>For more details, visit the <a href="https://cmake.org/cmake/help/latest/manual/ctest.1.html">official website</a>.</p></summary>
    public static IReadOnlyCollection<Output> CTest(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Func<IProcess, object> exitHandler = null) => new CTestTasks().Run(arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger, exitHandler);
    /// <summary><p>Runs the tests of a CMake build tree.</p><p>For more details, visit the <a href="https://cmake.org/cmake/help/latest/manual/ctest.1.html">official website</a>.</p></summary>
    /// <remarks><p>This is a <a href="https://github.com/Fallout-build/Fallout">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p><ul><li><c>--build-config</c> via <see cref="CTestSettings.Configuration"/></li><li><c>--debug</c> via <see cref="CTestSettings.Debug"/></li><li><c>--exclude-from-file</c> via <see cref="CTestSettings.ExcludeFromFile"/></li><li><c>--exclude-regex</c> via <see cref="CTestSettings.ExcludeRegex"/></li><li><c>--extra-verbose</c> via <see cref="CTestSettings.ExtraVerbose"/></li><li><c>--fixture-exclude-any</c> via <see cref="CTestSettings.FixtureExcludeAny"/></li><li><c>--fixture-exclude-cleanup</c> via <see cref="CTestSettings.FixtureExcludeCleanup"/></li><li><c>--fixture-exclude-setup</c> via <see cref="CTestSettings.FixtureExcludeSetup"/></li><li><c>--label-exclude</c> via <see cref="CTestSettings.LabelExcludeRegexes"/></li><li><c>--label-regex</c> via <see cref="CTestSettings.LabelRegexes"/></li><li><c>--no-label-summary</c> via <see cref="CTestSettings.NoLabelSummary"/></li><li><c>--no-subproject-summary</c> via <see cref="CTestSettings.NoSubprojectSummary"/></li><li><c>--no-tests</c> via <see cref="CTestSettings.NoTests"/></li><li><c>--output-junit</c> via <see cref="CTestSettings.OutputJunit"/></li><li><c>--output-log</c> via <see cref="CTestSettings.OutputLog"/></li><li><c>--output-on-failure</c> via <see cref="CTestSettings.OutputOnFailure"/></li><li><c>--parallel</c> via <see cref="CTestSettings.Parallel"/></li><li><c>--preset</c> via <see cref="CTestSettings.Preset"/></li><li><c>--presets-file</c> via <see cref="CTestSettings.PresetsFile"/></li><li><c>--progress</c> via <see cref="CTestSettings.Progress"/></li><li><c>--quiet</c> via <see cref="CTestSettings.Quiet"/></li><li><c>--repeat</c> via <see cref="CTestSettings.Repeat"/></li><li><c>--rerun-failed</c> via <see cref="CTestSettings.RerunFailed"/></li><li><c>--resource-spec-file</c> via <see cref="CTestSettings.ResourceSpecFile"/></li><li><c>--schedule-random</c> via <see cref="CTestSettings.ScheduleRandom"/></li><li><c>--stop-on-failure</c> via <see cref="CTestSettings.StopOnFailure"/></li><li><c>--stop-time</c> via <see cref="CTestSettings.StopTime"/></li><li><c>--test-dir</c> via <see cref="CTestSettings.TestDirectory"/></li><li><c>--test-load</c> via <see cref="CTestSettings.TestLoad"/></li><li><c>--test-output-size-failed</c> via <see cref="CTestSettings.TestOutputSizeFailed"/></li><li><c>--test-output-size-passed</c> via <see cref="CTestSettings.TestOutputSizePassed"/></li><li><c>--test-output-truncation</c> via <see cref="CTestSettings.TestOutputTruncation"/></li><li><c>--tests-from-file</c> via <see cref="CTestSettings.TestsFromFile"/></li><li><c>--tests-regex</c> via <see cref="CTestSettings.TestsRegex"/></li><li><c>--timeout</c> via <see cref="CTestSettings.Timeout"/></li><li><c>--verbose</c> via <see cref="CTestSettings.Verbose"/></li></ul></remarks>
    public static IReadOnlyCollection<Output> CTest(CTestSettings options = null) => new CTestTasks().Run<CTestSettings>(options);
    /// <inheritdoc cref="CTestTasks.CTest(Fallout.Common.Tools.CTest.CTestSettings)"/>
    public static IReadOnlyCollection<Output> CTest(Configure<CTestSettings> configurator) => new CTestTasks().Run<CTestSettings>(configurator.Invoke(new CTestSettings()));
    /// <inheritdoc cref="CTestTasks.CTest(Fallout.Common.Tools.CTest.CTestSettings)"/>
    public static IEnumerable<(CTestSettings Settings, IReadOnlyCollection<Output> Output)> CTest(CombinatorialConfigure<CTestSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false) => configurator.Invoke(CTest, degreeOfParallelism, completeOnFailure);
}
#region CTestSettings
/// <inheritdoc cref="CTestTasks.CTest(Fallout.Common.Tools.CTest.CTestSettings)"/>
[ExcludeFromCodeCoverage]
[Command(Type = typeof(CTestTasks), Command = nameof(CTestTasks.CTest))]
public partial class CTestSettings : ToolOptions
{
    /// <summary>Specify a test preset. CTest reads it from <c>CMakePresets.json</c> and <c>CMakeUserPresets.json</c> in the working directory, and takes the test directory from the configure preset of the test preset. Values set on the command line override the values from the preset.</summary>
    [Argument(Format = "--preset {value}")] public string Preset => Get<string>(() => Preset);
    /// <summary>Read presets from the given file instead of <c>CMakePresets.json</c> and <c>CMakeUserPresets.json</c>. Requires CMake 4.4 or later.</summary>
    [Argument(Format = "--presets-file {value}")] public string PresetsFile => Get<string>(() => PresetsFile);
    /// <summary>Specify the project binary directory in which to look for tests. When not set, CTest uses the binary directory of the preset, or the working directory.</summary>
    [Argument(Format = "--test-dir {value}")] public string TestDirectory => Get<string>(() => TestDirectory);
    /// <summary>Specify the configuration to test, for multi-config generators.</summary>
    [Argument(Format = "--build-config {value}")] public CMakeConfiguration Configuration => Get<CMakeConfiguration>(() => Configuration);
    /// <summary>Run tests in parallel, using at most the given number of processes.</summary>
    [Argument(Format = "--parallel {value}")] public int? Parallel => Get<int?>(() => Parallel);
    /// <summary>Do not start new parallel tests while the CPU load is above the given threshold.</summary>
    [Argument(Format = "--test-load {value}")] public int? TestLoad => Get<int?>(() => TestLoad);
    /// <summary>Print the output of the test program if the test fails.</summary>
    [Argument(Format = "--output-on-failure")] public bool? OutputOnFailure => Get<bool?>(() => OutputOnFailure);
    /// <summary>Stop running the tests after one has failed.</summary>
    [Argument(Format = "--stop-on-failure")] public bool? StopOnFailure => Get<bool?>(() => StopOnFailure);
    /// <summary>Enable short progress output from tests.</summary>
    [Argument(Format = "--progress")] public bool? Progress => Get<bool?>(() => Progress);
    /// <summary>Enable verbose output from tests.</summary>
    [Argument(Format = "--verbose")] public bool? Verbose => Get<bool?>(() => Verbose);
    /// <summary>Enable more verbose output from tests.</summary>
    [Argument(Format = "--extra-verbose")] public bool? ExtraVerbose => Get<bool?>(() => ExtraVerbose);
    /// <summary>Print more verbose internals of CTest.</summary>
    [Argument(Format = "--debug")] public bool? Debug => Get<bool?>(() => Debug);
    /// <summary>Make CTest quiet.</summary>
    [Argument(Format = "--quiet")] public bool? Quiet => Get<bool?>(() => Quiet);
    /// <summary>Write all output to the given log file.</summary>
    [Argument(Format = "--output-log {value}")] public string OutputLog => Get<string>(() => OutputLog);
    /// <summary>Write the test results to the given JUnit XML file.</summary>
    [Argument(Format = "--output-junit {value}")] public string OutputJunit => Get<string>(() => OutputJunit);
    /// <summary>Limit the output of passed tests to the given number of bytes.</summary>
    [Argument(Format = "--test-output-size-passed {value}")] public int? TestOutputSizePassed => Get<int?>(() => TestOutputSizePassed);
    /// <summary>Limit the output of failed tests to the given number of bytes.</summary>
    [Argument(Format = "--test-output-size-failed {value}")] public int? TestOutputSizeFailed => Get<int?>(() => TestOutputSizeFailed);
    /// <summary>Specify which part of the test output to drop once the output size limit is reached.</summary>
    [Argument(Format = "--test-output-truncation {value}")] public CTestOutputTruncation TestOutputTruncation => Get<CTestOutputTruncation>(() => TestOutputTruncation);
    /// <summary>Run only the tests whose names match the given regular expression.</summary>
    [Argument(Format = "--tests-regex {value}")] public string TestsRegex => Get<string>(() => TestsRegex);
    /// <summary>Exclude the tests whose names match the given regular expression.</summary>
    [Argument(Format = "--exclude-regex {value}")] public string ExcludeRegex => Get<string>(() => ExcludeRegex);
    /// <summary>Run only the tests with a label that matches the given regular expression. With several expressions, a test runs only if each expression matches at least one of its labels.</summary>
    [Argument(Format = "--label-regex {value}")] public IReadOnlyList<string> LabelRegexes => Get<List<string>>(() => LabelRegexes);
    /// <summary>Exclude the tests with a label that matches the given regular expression. With several expressions, a test is excluded only if each expression matches at least one of its labels.</summary>
    [Argument(Format = "--label-exclude {value}")] public IReadOnlyList<string> LabelExcludeRegexes => Get<List<string>>(() => LabelExcludeRegexes);
    /// <summary>Do not automatically add any tests for fixtures that match the given regular expression.</summary>
    [Argument(Format = "--fixture-exclude-any {value}")] public string FixtureExcludeAny => Get<string>(() => FixtureExcludeAny);
    /// <summary>Do not automatically add setup tests for fixtures that match the given regular expression.</summary>
    [Argument(Format = "--fixture-exclude-setup {value}")] public string FixtureExcludeSetup => Get<string>(() => FixtureExcludeSetup);
    /// <summary>Do not automatically add cleanup tests for fixtures that match the given regular expression.</summary>
    [Argument(Format = "--fixture-exclude-cleanup {value}")] public string FixtureExcludeCleanup => Get<string>(() => FixtureExcludeCleanup);
    /// <summary>Run only the tests listed in the given file, one test name per line.</summary>
    [Argument(Format = "--tests-from-file {value}")] public string TestsFromFile => Get<string>(() => TestsFromFile);
    /// <summary>Exclude the tests listed in the given file, one test name per line.</summary>
    [Argument(Format = "--exclude-from-file {value}")] public string ExcludeFromFile => Get<string>(() => ExcludeFromFile);
    /// <summary>Run only the tests that failed in the previous run.</summary>
    [Argument(Format = "--rerun-failed")] public bool? RerunFailed => Get<bool?>(() => RerunFailed);
    /// <summary>Repeat tests according to the given mode.<para/><ul><li><c>until-fail:&lt;n&gt;</c> runs each test <c>n</c> times and fails on the first failure.</li><li><c>until-pass:&lt;n&gt;</c> runs each test up to <c>n</c> times until it passes.</li><li><c>after-timeout:&lt;n&gt;</c> runs each test up to <c>n</c> times while it times out.</li></ul></summary>
    [Argument(Format = "--repeat {value}")] public string Repeat => Get<string>(() => Repeat);
    /// <summary>Set the default timeout for each test, in seconds.</summary>
    [Argument(Format = "--timeout {value}")] public int? Timeout => Get<int?>(() => Timeout);
    /// <summary>Set a time of day at which all tests stop running.</summary>
    [Argument(Format = "--stop-time {value}")] public string StopTime => Get<string>(() => StopTime);
    /// <summary>Run the tests in a random order.</summary>
    [Argument(Format = "--schedule-random")] public bool? ScheduleRandom => Get<bool?>(() => ScheduleRandom);
    /// <summary>Specify whether finding no tests is an error or is ignored.</summary>
    [Argument(Format = "--no-tests={value}")] public CTestNoTestsAction NoTests => Get<CTestNoTestsAction>(() => NoTests);
    /// <summary>Specify the resource specification file to use for resource allocation.</summary>
    [Argument(Format = "--resource-spec-file {value}")] public string ResourceSpecFile => Get<string>(() => ResourceSpecFile);
    /// <summary>Do not print the timing summary for labels.</summary>
    [Argument(Format = "--no-label-summary")] public bool? NoLabelSummary => Get<bool?>(() => NoLabelSummary);
    /// <summary>Do not print the timing summary for subprojects.</summary>
    [Argument(Format = "--no-subproject-summary")] public bool? NoSubprojectSummary => Get<bool?>(() => NoSubprojectSummary);
}
#endregion
#region CTestSettingsExtensions
/// <inheritdoc cref="CTestTasks.CTest(Fallout.Common.Tools.CTest.CTestSettings)"/>
[ExcludeFromCodeCoverage]
public static partial class CTestSettingsExtensions
{
    #region Preset
    /// <inheritdoc cref="CTestSettings.Preset"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Preset))]
    public static T SetPreset<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Preset, v));
    /// <inheritdoc cref="CTestSettings.Preset"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Preset))]
    public static T ResetPreset<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Preset));
    #endregion
    #region PresetsFile
    /// <inheritdoc cref="CTestSettings.PresetsFile"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.PresetsFile))]
    public static T SetPresetsFile<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.PresetsFile, v));
    /// <inheritdoc cref="CTestSettings.PresetsFile"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.PresetsFile))]
    public static T ResetPresetsFile<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.PresetsFile));
    #endregion
    #region TestDirectory
    /// <inheritdoc cref="CTestSettings.TestDirectory"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestDirectory))]
    public static T SetTestDirectory<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.TestDirectory, v));
    /// <inheritdoc cref="CTestSettings.TestDirectory"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestDirectory))]
    public static T ResetTestDirectory<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.TestDirectory));
    #endregion
    #region Configuration
    /// <inheritdoc cref="CTestSettings.Configuration"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Configuration))]
    public static T SetConfiguration<T>(this T o, CMakeConfiguration v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Configuration, v));
    /// <inheritdoc cref="CTestSettings.Configuration"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Configuration))]
    public static T ResetConfiguration<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Configuration));
    #endregion
    #region Parallel
    /// <inheritdoc cref="CTestSettings.Parallel"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Parallel))]
    public static T SetParallel<T>(this T o, int? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Parallel, v));
    /// <inheritdoc cref="CTestSettings.Parallel"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Parallel))]
    public static T ResetParallel<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Parallel));
    #endregion
    #region TestLoad
    /// <inheritdoc cref="CTestSettings.TestLoad"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestLoad))]
    public static T SetTestLoad<T>(this T o, int? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.TestLoad, v));
    /// <inheritdoc cref="CTestSettings.TestLoad"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestLoad))]
    public static T ResetTestLoad<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.TestLoad));
    #endregion
    #region OutputOnFailure
    /// <inheritdoc cref="CTestSettings.OutputOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputOnFailure))]
    public static T SetOutputOnFailure<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.OutputOnFailure, v));
    /// <inheritdoc cref="CTestSettings.OutputOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputOnFailure))]
    public static T ResetOutputOnFailure<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.OutputOnFailure));
    /// <inheritdoc cref="CTestSettings.OutputOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputOnFailure))]
    public static T EnableOutputOnFailure<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.OutputOnFailure, true));
    /// <inheritdoc cref="CTestSettings.OutputOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputOnFailure))]
    public static T DisableOutputOnFailure<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.OutputOnFailure, false));
    /// <inheritdoc cref="CTestSettings.OutputOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputOnFailure))]
    public static T ToggleOutputOnFailure<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.OutputOnFailure, !o.OutputOnFailure));
    #endregion
    #region StopOnFailure
    /// <inheritdoc cref="CTestSettings.StopOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.StopOnFailure))]
    public static T SetStopOnFailure<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.StopOnFailure, v));
    /// <inheritdoc cref="CTestSettings.StopOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.StopOnFailure))]
    public static T ResetStopOnFailure<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.StopOnFailure));
    /// <inheritdoc cref="CTestSettings.StopOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.StopOnFailure))]
    public static T EnableStopOnFailure<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.StopOnFailure, true));
    /// <inheritdoc cref="CTestSettings.StopOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.StopOnFailure))]
    public static T DisableStopOnFailure<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.StopOnFailure, false));
    /// <inheritdoc cref="CTestSettings.StopOnFailure"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.StopOnFailure))]
    public static T ToggleStopOnFailure<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.StopOnFailure, !o.StopOnFailure));
    #endregion
    #region Progress
    /// <inheritdoc cref="CTestSettings.Progress"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Progress))]
    public static T SetProgress<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Progress, v));
    /// <inheritdoc cref="CTestSettings.Progress"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Progress))]
    public static T ResetProgress<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Progress));
    /// <inheritdoc cref="CTestSettings.Progress"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Progress))]
    public static T EnableProgress<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Progress, true));
    /// <inheritdoc cref="CTestSettings.Progress"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Progress))]
    public static T DisableProgress<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Progress, false));
    /// <inheritdoc cref="CTestSettings.Progress"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Progress))]
    public static T ToggleProgress<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Progress, !o.Progress));
    #endregion
    #region Verbose
    /// <inheritdoc cref="CTestSettings.Verbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Verbose))]
    public static T SetVerbose<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Verbose, v));
    /// <inheritdoc cref="CTestSettings.Verbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Verbose))]
    public static T ResetVerbose<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Verbose));
    /// <inheritdoc cref="CTestSettings.Verbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Verbose))]
    public static T EnableVerbose<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Verbose, true));
    /// <inheritdoc cref="CTestSettings.Verbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Verbose))]
    public static T DisableVerbose<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Verbose, false));
    /// <inheritdoc cref="CTestSettings.Verbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Verbose))]
    public static T ToggleVerbose<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Verbose, !o.Verbose));
    #endregion
    #region ExtraVerbose
    /// <inheritdoc cref="CTestSettings.ExtraVerbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExtraVerbose))]
    public static T SetExtraVerbose<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.ExtraVerbose, v));
    /// <inheritdoc cref="CTestSettings.ExtraVerbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExtraVerbose))]
    public static T ResetExtraVerbose<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.ExtraVerbose));
    /// <inheritdoc cref="CTestSettings.ExtraVerbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExtraVerbose))]
    public static T EnableExtraVerbose<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.ExtraVerbose, true));
    /// <inheritdoc cref="CTestSettings.ExtraVerbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExtraVerbose))]
    public static T DisableExtraVerbose<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.ExtraVerbose, false));
    /// <inheritdoc cref="CTestSettings.ExtraVerbose"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExtraVerbose))]
    public static T ToggleExtraVerbose<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.ExtraVerbose, !o.ExtraVerbose));
    #endregion
    #region Debug
    /// <inheritdoc cref="CTestSettings.Debug"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Debug))]
    public static T SetDebug<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Debug, v));
    /// <inheritdoc cref="CTestSettings.Debug"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Debug))]
    public static T ResetDebug<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Debug));
    /// <inheritdoc cref="CTestSettings.Debug"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Debug))]
    public static T EnableDebug<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Debug, true));
    /// <inheritdoc cref="CTestSettings.Debug"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Debug))]
    public static T DisableDebug<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Debug, false));
    /// <inheritdoc cref="CTestSettings.Debug"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Debug))]
    public static T ToggleDebug<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Debug, !o.Debug));
    #endregion
    #region Quiet
    /// <inheritdoc cref="CTestSettings.Quiet"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Quiet))]
    public static T SetQuiet<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Quiet, v));
    /// <inheritdoc cref="CTestSettings.Quiet"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Quiet))]
    public static T ResetQuiet<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Quiet));
    /// <inheritdoc cref="CTestSettings.Quiet"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Quiet))]
    public static T EnableQuiet<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Quiet, true));
    /// <inheritdoc cref="CTestSettings.Quiet"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Quiet))]
    public static T DisableQuiet<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Quiet, false));
    /// <inheritdoc cref="CTestSettings.Quiet"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Quiet))]
    public static T ToggleQuiet<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.Quiet, !o.Quiet));
    #endregion
    #region OutputLog
    /// <inheritdoc cref="CTestSettings.OutputLog"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputLog))]
    public static T SetOutputLog<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.OutputLog, v));
    /// <inheritdoc cref="CTestSettings.OutputLog"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputLog))]
    public static T ResetOutputLog<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.OutputLog));
    #endregion
    #region OutputJunit
    /// <inheritdoc cref="CTestSettings.OutputJunit"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputJunit))]
    public static T SetOutputJunit<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.OutputJunit, v));
    /// <inheritdoc cref="CTestSettings.OutputJunit"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.OutputJunit))]
    public static T ResetOutputJunit<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.OutputJunit));
    #endregion
    #region TestOutputSizePassed
    /// <inheritdoc cref="CTestSettings.TestOutputSizePassed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestOutputSizePassed))]
    public static T SetTestOutputSizePassed<T>(this T o, int? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.TestOutputSizePassed, v));
    /// <inheritdoc cref="CTestSettings.TestOutputSizePassed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestOutputSizePassed))]
    public static T ResetTestOutputSizePassed<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.TestOutputSizePassed));
    #endregion
    #region TestOutputSizeFailed
    /// <inheritdoc cref="CTestSettings.TestOutputSizeFailed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestOutputSizeFailed))]
    public static T SetTestOutputSizeFailed<T>(this T o, int? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.TestOutputSizeFailed, v));
    /// <inheritdoc cref="CTestSettings.TestOutputSizeFailed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestOutputSizeFailed))]
    public static T ResetTestOutputSizeFailed<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.TestOutputSizeFailed));
    #endregion
    #region TestOutputTruncation
    /// <inheritdoc cref="CTestSettings.TestOutputTruncation"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestOutputTruncation))]
    public static T SetTestOutputTruncation<T>(this T o, CTestOutputTruncation v) where T : CTestSettings => o.Modify(b => b.Set(() => o.TestOutputTruncation, v));
    /// <inheritdoc cref="CTestSettings.TestOutputTruncation"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestOutputTruncation))]
    public static T ResetTestOutputTruncation<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.TestOutputTruncation));
    #endregion
    #region TestsRegex
    /// <inheritdoc cref="CTestSettings.TestsRegex"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestsRegex))]
    public static T SetTestsRegex<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.TestsRegex, v));
    /// <inheritdoc cref="CTestSettings.TestsRegex"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestsRegex))]
    public static T ResetTestsRegex<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.TestsRegex));
    #endregion
    #region ExcludeRegex
    /// <inheritdoc cref="CTestSettings.ExcludeRegex"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExcludeRegex))]
    public static T SetExcludeRegex<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.ExcludeRegex, v));
    /// <inheritdoc cref="CTestSettings.ExcludeRegex"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExcludeRegex))]
    public static T ResetExcludeRegex<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.ExcludeRegex));
    #endregion
    #region LabelRegexes
    /// <inheritdoc cref="CTestSettings.LabelRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelRegexes))]
    public static T SetLabelRegexes<T>(this T o, params string[] v) where T : CTestSettings => o.Modify(b => b.Set(() => o.LabelRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelRegexes))]
    public static T SetLabelRegexes<T>(this T o, IEnumerable<string> v) where T : CTestSettings => o.Modify(b => b.Set(() => o.LabelRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelRegexes))]
    public static T AddLabelRegexes<T>(this T o, params string[] v) where T : CTestSettings => o.Modify(b => b.AddCollection(() => o.LabelRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelRegexes))]
    public static T AddLabelRegexes<T>(this T o, IEnumerable<string> v) where T : CTestSettings => o.Modify(b => b.AddCollection(() => o.LabelRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelRegexes))]
    public static T RemoveLabelRegexes<T>(this T o, params string[] v) where T : CTestSettings => o.Modify(b => b.RemoveCollection(() => o.LabelRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelRegexes))]
    public static T RemoveLabelRegexes<T>(this T o, IEnumerable<string> v) where T : CTestSettings => o.Modify(b => b.RemoveCollection(() => o.LabelRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelRegexes))]
    public static T ClearLabelRegexes<T>(this T o) where T : CTestSettings => o.Modify(b => b.ClearCollection(() => o.LabelRegexes));
    #endregion
    #region LabelExcludeRegexes
    /// <inheritdoc cref="CTestSettings.LabelExcludeRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelExcludeRegexes))]
    public static T SetLabelExcludeRegexes<T>(this T o, params string[] v) where T : CTestSettings => o.Modify(b => b.Set(() => o.LabelExcludeRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelExcludeRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelExcludeRegexes))]
    public static T SetLabelExcludeRegexes<T>(this T o, IEnumerable<string> v) where T : CTestSettings => o.Modify(b => b.Set(() => o.LabelExcludeRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelExcludeRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelExcludeRegexes))]
    public static T AddLabelExcludeRegexes<T>(this T o, params string[] v) where T : CTestSettings => o.Modify(b => b.AddCollection(() => o.LabelExcludeRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelExcludeRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelExcludeRegexes))]
    public static T AddLabelExcludeRegexes<T>(this T o, IEnumerable<string> v) where T : CTestSettings => o.Modify(b => b.AddCollection(() => o.LabelExcludeRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelExcludeRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelExcludeRegexes))]
    public static T RemoveLabelExcludeRegexes<T>(this T o, params string[] v) where T : CTestSettings => o.Modify(b => b.RemoveCollection(() => o.LabelExcludeRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelExcludeRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelExcludeRegexes))]
    public static T RemoveLabelExcludeRegexes<T>(this T o, IEnumerable<string> v) where T : CTestSettings => o.Modify(b => b.RemoveCollection(() => o.LabelExcludeRegexes, v));
    /// <inheritdoc cref="CTestSettings.LabelExcludeRegexes"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.LabelExcludeRegexes))]
    public static T ClearLabelExcludeRegexes<T>(this T o) where T : CTestSettings => o.Modify(b => b.ClearCollection(() => o.LabelExcludeRegexes));
    #endregion
    #region FixtureExcludeAny
    /// <inheritdoc cref="CTestSettings.FixtureExcludeAny"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.FixtureExcludeAny))]
    public static T SetFixtureExcludeAny<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.FixtureExcludeAny, v));
    /// <inheritdoc cref="CTestSettings.FixtureExcludeAny"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.FixtureExcludeAny))]
    public static T ResetFixtureExcludeAny<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.FixtureExcludeAny));
    #endregion
    #region FixtureExcludeSetup
    /// <inheritdoc cref="CTestSettings.FixtureExcludeSetup"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.FixtureExcludeSetup))]
    public static T SetFixtureExcludeSetup<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.FixtureExcludeSetup, v));
    /// <inheritdoc cref="CTestSettings.FixtureExcludeSetup"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.FixtureExcludeSetup))]
    public static T ResetFixtureExcludeSetup<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.FixtureExcludeSetup));
    #endregion
    #region FixtureExcludeCleanup
    /// <inheritdoc cref="CTestSettings.FixtureExcludeCleanup"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.FixtureExcludeCleanup))]
    public static T SetFixtureExcludeCleanup<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.FixtureExcludeCleanup, v));
    /// <inheritdoc cref="CTestSettings.FixtureExcludeCleanup"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.FixtureExcludeCleanup))]
    public static T ResetFixtureExcludeCleanup<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.FixtureExcludeCleanup));
    #endregion
    #region TestsFromFile
    /// <inheritdoc cref="CTestSettings.TestsFromFile"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestsFromFile))]
    public static T SetTestsFromFile<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.TestsFromFile, v));
    /// <inheritdoc cref="CTestSettings.TestsFromFile"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.TestsFromFile))]
    public static T ResetTestsFromFile<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.TestsFromFile));
    #endregion
    #region ExcludeFromFile
    /// <inheritdoc cref="CTestSettings.ExcludeFromFile"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExcludeFromFile))]
    public static T SetExcludeFromFile<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.ExcludeFromFile, v));
    /// <inheritdoc cref="CTestSettings.ExcludeFromFile"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ExcludeFromFile))]
    public static T ResetExcludeFromFile<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.ExcludeFromFile));
    #endregion
    #region RerunFailed
    /// <inheritdoc cref="CTestSettings.RerunFailed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.RerunFailed))]
    public static T SetRerunFailed<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.RerunFailed, v));
    /// <inheritdoc cref="CTestSettings.RerunFailed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.RerunFailed))]
    public static T ResetRerunFailed<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.RerunFailed));
    /// <inheritdoc cref="CTestSettings.RerunFailed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.RerunFailed))]
    public static T EnableRerunFailed<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.RerunFailed, true));
    /// <inheritdoc cref="CTestSettings.RerunFailed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.RerunFailed))]
    public static T DisableRerunFailed<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.RerunFailed, false));
    /// <inheritdoc cref="CTestSettings.RerunFailed"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.RerunFailed))]
    public static T ToggleRerunFailed<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.RerunFailed, !o.RerunFailed));
    #endregion
    #region Repeat
    /// <inheritdoc cref="CTestSettings.Repeat"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Repeat))]
    public static T SetRepeat<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Repeat, v));
    /// <inheritdoc cref="CTestSettings.Repeat"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Repeat))]
    public static T ResetRepeat<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Repeat));
    #endregion
    #region Timeout
    /// <inheritdoc cref="CTestSettings.Timeout"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Timeout))]
    public static T SetTimeout<T>(this T o, int? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.Timeout, v));
    /// <inheritdoc cref="CTestSettings.Timeout"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.Timeout))]
    public static T ResetTimeout<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.Timeout));
    #endregion
    #region StopTime
    /// <inheritdoc cref="CTestSettings.StopTime"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.StopTime))]
    public static T SetStopTime<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.StopTime, v));
    /// <inheritdoc cref="CTestSettings.StopTime"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.StopTime))]
    public static T ResetStopTime<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.StopTime));
    #endregion
    #region ScheduleRandom
    /// <inheritdoc cref="CTestSettings.ScheduleRandom"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ScheduleRandom))]
    public static T SetScheduleRandom<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.ScheduleRandom, v));
    /// <inheritdoc cref="CTestSettings.ScheduleRandom"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ScheduleRandom))]
    public static T ResetScheduleRandom<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.ScheduleRandom));
    /// <inheritdoc cref="CTestSettings.ScheduleRandom"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ScheduleRandom))]
    public static T EnableScheduleRandom<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.ScheduleRandom, true));
    /// <inheritdoc cref="CTestSettings.ScheduleRandom"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ScheduleRandom))]
    public static T DisableScheduleRandom<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.ScheduleRandom, false));
    /// <inheritdoc cref="CTestSettings.ScheduleRandom"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ScheduleRandom))]
    public static T ToggleScheduleRandom<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.ScheduleRandom, !o.ScheduleRandom));
    #endregion
    #region NoTests
    /// <inheritdoc cref="CTestSettings.NoTests"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoTests))]
    public static T SetNoTests<T>(this T o, CTestNoTestsAction v) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoTests, v));
    /// <inheritdoc cref="CTestSettings.NoTests"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoTests))]
    public static T ResetNoTests<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.NoTests));
    #endregion
    #region ResourceSpecFile
    /// <inheritdoc cref="CTestSettings.ResourceSpecFile"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ResourceSpecFile))]
    public static T SetResourceSpecFile<T>(this T o, string v) where T : CTestSettings => o.Modify(b => b.Set(() => o.ResourceSpecFile, v));
    /// <inheritdoc cref="CTestSettings.ResourceSpecFile"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.ResourceSpecFile))]
    public static T ResetResourceSpecFile<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.ResourceSpecFile));
    #endregion
    #region NoLabelSummary
    /// <inheritdoc cref="CTestSettings.NoLabelSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoLabelSummary))]
    public static T SetNoLabelSummary<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoLabelSummary, v));
    /// <inheritdoc cref="CTestSettings.NoLabelSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoLabelSummary))]
    public static T ResetNoLabelSummary<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.NoLabelSummary));
    /// <inheritdoc cref="CTestSettings.NoLabelSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoLabelSummary))]
    public static T EnableNoLabelSummary<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoLabelSummary, true));
    /// <inheritdoc cref="CTestSettings.NoLabelSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoLabelSummary))]
    public static T DisableNoLabelSummary<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoLabelSummary, false));
    /// <inheritdoc cref="CTestSettings.NoLabelSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoLabelSummary))]
    public static T ToggleNoLabelSummary<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoLabelSummary, !o.NoLabelSummary));
    #endregion
    #region NoSubprojectSummary
    /// <inheritdoc cref="CTestSettings.NoSubprojectSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoSubprojectSummary))]
    public static T SetNoSubprojectSummary<T>(this T o, bool? v) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoSubprojectSummary, v));
    /// <inheritdoc cref="CTestSettings.NoSubprojectSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoSubprojectSummary))]
    public static T ResetNoSubprojectSummary<T>(this T o) where T : CTestSettings => o.Modify(b => b.Remove(() => o.NoSubprojectSummary));
    /// <inheritdoc cref="CTestSettings.NoSubprojectSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoSubprojectSummary))]
    public static T EnableNoSubprojectSummary<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoSubprojectSummary, true));
    /// <inheritdoc cref="CTestSettings.NoSubprojectSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoSubprojectSummary))]
    public static T DisableNoSubprojectSummary<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoSubprojectSummary, false));
    /// <inheritdoc cref="CTestSettings.NoSubprojectSummary"/>
    [Builder(Type = typeof(CTestSettings), Property = nameof(CTestSettings.NoSubprojectSummary))]
    public static T ToggleNoSubprojectSummary<T>(this T o) where T : CTestSettings => o.Modify(b => b.Set(() => o.NoSubprojectSummary, !o.NoSubprojectSummary));
    #endregion
}
#endregion
#region CTestOutputTruncation
/// <summary>Used within <see cref="CTestTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<CTestOutputTruncation>))]
public partial class CTestOutputTruncation : Enumeration
{
    public static CTestOutputTruncation tail = (CTestOutputTruncation) "tail";
    public static CTestOutputTruncation middle = (CTestOutputTruncation) "middle";
    public static CTestOutputTruncation head = (CTestOutputTruncation) "head";
    public static implicit operator CTestOutputTruncation(string value)
    {
        return new CTestOutputTruncation { Value = value };
    }
}
#endregion
#region CTestNoTestsAction
/// <summary>Used within <see cref="CTestTasks"/>.</summary>
[Serializable]
[ExcludeFromCodeCoverage]
[TypeConverter(typeof(TypeConverter<CTestNoTestsAction>))]
public partial class CTestNoTestsAction : Enumeration
{
    public static CTestNoTestsAction error = (CTestNoTestsAction) "error";
    public static CTestNoTestsAction ignore = (CTestNoTestsAction) "ignore";
    public static implicit operator CTestNoTestsAction(string value)
    {
        return new CTestNoTestsAction { Value = value };
    }
}
#endregion
