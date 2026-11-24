using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fallout.Common.CI;
using Fallout.Common.Execution.Theming;
using Fallout.Common.IO;
using Fallout.Common.Utilities;
using Fallout.Common.Utilities.Collections;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

// Serilog and Microsoft.Extensions.Logging both declare ILogger.
// In this file, ILogger means the Microsoft.Extensions.Logging one.
// Serilog's pipeline is reached through the static Log class.
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Fallout.Common.Execution;

public static class Logging
{
    public static readonly LoggingLevelSwitch LevelSwitch = new();

    /// <summary>
    /// Category for framework log records that have no category of their own.
    /// </summary>
    internal const string DefaultCategoryName = "Fallout";

    private static ILoggerFactory loggerFactory;

    internal static bool SupportsAnsiOutput =>
        Environment.GetEnvironmentVariable("TERM") is { } term && term.StartsWithOrdinalIgnoreCase("xterm");

    internal static IHostTheme DefaultTheme { get; } = SupportsAnsiOutput
        ? AnsiConsoleHostTheme.Default256AnsiColorTheme
        : SystemConsoleHostTheme.DefaultSystemColorTheme;

    internal static string ErrorsAndWarningsOutputTemplate => "[{Level:u3}] {ExecutingTarget}: {Message:l}{NewLine}";

    internal static string StandardOutputTemplate => "[{Level:u3}] {Message:l}{NewLine}{Exception}";

    internal static string TimestampOutputTemplate => $"{{Timestamp:HH:mm:ss}} {StandardOutputTemplate}";

    private const int TargetNameLength = 20;

    public static LogLevel Level
    {
        get => LevelSwitch.MinimumLevel.ToLogLevel();
        set => LevelSwitch.MinimumLevel = value.ToLogEventLevel();
    }

    /// <summary>
    /// Logger factory for the current build run. It writes to the Serilog pipeline that
    /// <see cref="Configure"/> installs.
    /// </summary>
    /// <remarks>
    /// During a build run, <c>BuildManager</c> sets this from its container.
    /// The CLI commands have no container, so this then falls back to a new factory over
    /// Serilog's current pipeline.
    /// </remarks>
    internal static ILoggerFactory Factory => loggerFactory ??= CreateSerilogLoggerFactory();

    /// <summary>
    /// Logger for framework code that has no category of its own.
    /// </summary>
    /// <remarks>
    /// Not cached: each access creates a new logger.
    /// See <see cref="CreateSerilogLoggerFactory"/> for why.
    /// </remarks>
    internal static ILogger Logger => Factory.CreateLogger(DefaultCategoryName);

    /// <summary>
    /// Sets <see cref="Factory"/> to <paramref name="factory"/> until the returned object is disposed.
    /// </summary>
    /// <remarks>
    /// Disposing the returned object restores the previous factory.
    /// It does not dispose <paramref name="factory"/>. The caller still owns it.
    /// </remarks>
    internal static IDisposable UseLoggerFactory(ILoggerFactory factory)
    {
        return DelegateDisposable.SetAndRestore(() => loggerFactory, factory.NotNull());
    }

    /// <summary>
    /// Bridges <see cref="ILogger"/> onto Serilog.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the one place that explains how loggers follow pipeline changes.
    /// </para>
    /// <para>
    /// Serilog's <see cref="Log.Logger"/> changes during a run.
    /// <see cref="Configure"/> sets it late, and sets it again when it is called again.
    /// <c>Host.WriteErrorsAndWarnings</c> replaces it to write the end-of-build summary.
    /// </para>
    /// <para>
    /// The factory gets no logger of its own.
    /// So each logger it creates uses the <see cref="Log.Logger"/> of that moment.
    /// A logger keeps that pipeline for its whole life, because Serilog adds the category
    /// (<c>SourceContext</c>) when the logger is created.
    /// </para>
    /// <para>
    /// The callers depend on this in three ways:
    /// </para>
    /// <list type="bullet">
    /// <item><description><c>BuildManager</c> calls <see cref="Configure"/> before it builds the container.</description></item>
    /// <item><description>The container registers <c>ILogger&lt;T&gt;</c> and <see cref="ILogger"/> as transient, so each resolution gets a new logger.</description></item>
    /// <item><description><see cref="Logger"/> is not cached.</description></item>
    /// </list>
    /// <para>
    /// A logger that is kept across a pipeline change still writes to the old pipeline.
    /// Code that lives that long must read <see cref="Logger"/> each time it writes.
    /// </para>
    /// <para>
    /// Serilog closes the pipeline (<c>Log.CloseAndFlush</c>), so the factory uses <c>dispose: false</c>.
    /// </para>
    /// </remarks>
    internal static ILoggerFactory CreateSerilogLoggerFactory()
    {
        return new SerilogLoggerFactory(logger: null, dispose: false);
    }

    public static void Configure(IFalloutBuild build = null)
    {
        if (build != null)
        {
            if (build.IsInterceptorExecution)
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console(new CompactJsonFormatter())
                    .CreateLogger();

                return;
            }

            DeleteOldLogFiles(build);
        }

        Log.Logger = new LoggerConfiguration()
            .ConfigureEnricher()
            .ConfigureHost(build)
            .ConfigureConsole(build)
            .ConfigureInMemory(build)
            .ConfigureFiles(build)
            .ConfigureLevel()
            .ConfigureFilter(build)
            .CreateLogger();
    }

    public static LoggerConfiguration ConfigureEnricher(this LoggerConfiguration configuration)
    {
        return configuration.Enrich.With<ExecutingTargetLogEventEnricher>();
    }

    public static LoggerConfiguration ConfigureLevel(this LoggerConfiguration configuration)
    {
        return configuration.MinimumLevel.Verbose();
    }

    public static LoggerConfiguration ConfigureFilter(this LoggerConfiguration configuration, IFalloutBuild build)
    {
        if (build == null)
        {
            return configuration;
        }

        return configuration.Filter.ByExcluding(x => build.Host.FilterMessage(x.MessageTemplate.Text));
    }

    public static LoggerConfiguration ConfigureConsole(this LoggerConfiguration configuration, IFalloutBuild build)
    {
        return configuration
            .WriteTo.Console(outputTemplate: build != null && build.IsOutputEnabled(DefaultOutput.Timestamps)
                    ? build.Host.OutputTemplate
                    : StandardOutputTemplate,
                theme: (ConsoleTheme)(build != null ? build.Host.Theme : DefaultTheme),
                applyThemeToRedirectedOutput: true,
                levelSwitch: LevelSwitch);
    }

    public static LoggerConfiguration ConfigureHost(this LoggerConfiguration configuration, IFalloutBuild build)
    {
        if (build == null)
        {
            return configuration;
        }

        return configuration
            .WriteTo.Sink(new Host.LogEventSink(build.Host), restrictedToMinimumLevel: LogEventLevel.Warning);
    }

    public static LoggerConfiguration ConfigureInMemory(this LoggerConfiguration configuration, IFalloutBuild build)
    {
        if (build == null)
        {
            return configuration;
        }

        return configuration
            .WriteTo.Sink(InMemorySink.Instance, LogEventLevel.Warning);
    }

    public static LoggerConfiguration ConfigureFiles(this LoggerConfiguration configuration, IFalloutBuild build)
    {
        if (build == null || build.Host is IBuildServer)
        {
            return configuration;
        }

        var buildLogFile = build.TemporaryDirectory / "build.log";
        return configuration
            .WriteTo.File(
                path: buildLogFile,
                outputTemplate:
                $"{{Timestamp:HH:mm:ss.fff}} | {{Level:u1}} | {{ExecutingTarget,-{TargetNameLength}}} | {{Message:l}}{{NewLine}}{{Exception}}")
            .WriteTo.File(
                path: Path.ChangeExtension(buildLogFile, $".{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log"),
                outputTemplate:
                $"{{Level:u1}} | {{ExecutingTarget,-{TargetNameLength}}} | {{Message:l}}{{NewLine}}{{Exception}}");
    }

    private static void DeleteOldLogFiles(IFalloutBuild build)
    {
        if (BuildServerConfigurationGeneration.IsActive)
        {
            return;
        }

        build.TemporaryDirectory.GlobFiles("build.*.log").OrderByDescending(x => x.ToString()).Skip(5)
            .ForEach(x => x.DeleteFile());

        var buildLogFile = build.TemporaryDirectory / "build.log";
        if (buildLogFile.Exists())
        {
            using var filestream = File.OpenWrite(buildLogFile);
            filestream.SetLength(0);
        }
    }

    internal static void Test()
    {
        const string Esc = "\u001b[";
        const string Reset = "\u001b[0m";

        for (var i = 30; i < 47; i++)
        {
            Console.Write($"{Esc}{i}m{i}  {Reset} ");
        }

        Console.WriteLine();
        for (var i = 30; i < 47; i++)
        {
            Console.Write($"{Esc}{i};1m{i};1{Reset} ");
        }

        Console.WriteLine();
        for (var i = 30; i < 47; i++)
        {
            Console.Write($"{Esc}{i};2m{i};2{Reset} ");
        }

        Console.WriteLine();
        for (var i = 30; i < 47; i++)
        {
            Console.Write($"{Esc}{i};3m{i};2{Reset} ");
        }

        Console.WriteLine();

        for (var i = 90; i < 107; i++)
        {
            Console.Write($"{Esc}{i}m{i}  {Reset} ");
        }

        Console.WriteLine();
        for (var i = 90; i < 107; i++)
        {
            Console.Write($"{Esc}{i};1m{i};1{Reset} ");
        }

        Console.WriteLine();
        for (var i = 90; i < 107; i++)
        {
            Console.Write($"{Esc}{i};2m{i};2{Reset} ");
        }

        Console.WriteLine();
        for (var i = 90; i < 107; i++)
        {
            Console.Write($"{Esc}{i};3m{i};2{Reset} ");
        }

        Console.WriteLine();

        for (var i = 0; i < 255; i++)
        {
            var code = i.ToString().PadLeft(3, '0');
            Console.Write($"{Esc}38;5;{code}m{code}{Reset} ");
            if ((i + 1) % 16 == 0)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine();

        for (var i = 0; i <= 255; i++)
        {
            var code = i.ToString().PadLeft(3, '0');
            Console.Write($"{Esc}38;5;{code};1m{code}{Reset} ");
            if ((i + 1) % 16 == 0)
            {
                Console.WriteLine();
            }
        }
    }

    public static IDisposable SetTarget(string name)
    {
        return ExecutingTargetLogEventEnricher.SetTargetEventProperty(name);
    }

    public class InMemorySink : ILogEventSink, IDisposable
    {
        public static InMemorySink Instance { get; } = new();

        private readonly List<LogEvent> logEvents;

        private InMemorySink()
        {
            logEvents = new List<LogEvent>();
        }

        public IReadOnlyCollection<LogEvent> LogEvents => logEvents.AsReadOnly();

        public void Emit(LogEvent logEvent)
        {
            logEvent.AddOrUpdateProperty(ExecutingTargetLogEventEnricher.Current);
            logEvents.Add(logEvent);
        }

        /// <summary>Drops accumulated events so a subsequent build in the same process starts clean. FT-1 / #306.</summary>
        public void Clear()
        {
            logEvents.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }

    internal class ExecutingTargetLogEventEnricher : ILogEventEnricher
    {
        public static LogEventProperty Current => property ?? defaultProperty;

        private static readonly LogEventProperty defaultProperty = GetTargetEventProperty(string.Empty);
#pragma warning disable CS0649
        private static LogEventProperty property;
#pragma warning restore CS0649

        public static IDisposable SetTargetEventProperty(string name)
        {
            return DelegateDisposable.SetAndRestore(() => property, GetTargetEventProperty(name));
        }

        private static LogEventProperty GetTargetEventProperty(string name)
        {
            var length = Math.Min(name.Length, TargetNameLength);
            var paddedName = name[..length];
            return new LogEventProperty("ExecutingTarget", new ScalarValue(paddedName));
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(Current);
        }
    }
}
