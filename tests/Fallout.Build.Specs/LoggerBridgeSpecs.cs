using System;
using System.Collections.Generic;
using System.Linq;
using Fallout.Common.Execution;
using Fallout.Common.Utilities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Fallout.Common.Specs;

/// <summary>
/// Covers the <see cref="ILogger" /> seam over Serilog (#428).
/// The seam must keep levels, message templates and exceptions, and must not filter levels itself.
/// </summary>
/// <remarks>
/// These specs write through <see cref="Log.Logger" />, which the whole process shares.
/// Spec classes outside this collection run at the same time and also write log records.
/// So every message here contains <see cref="Marker" />, and only those records are checked.
/// </remarks>
[Collection(ProcessGlobalStateCollection.Name)]
public class LoggerBridgeSpecs
{
    /// <summary>
    /// Separates this class's log records from those of specs that run at the same time.
    /// </summary>
    private const string Marker = "loggerbridgespec";

    [Theory]
    [InlineData(MsLogLevel.Trace, LogEventLevel.Verbose)]
    [InlineData(MsLogLevel.Debug, LogEventLevel.Debug)]
    [InlineData(MsLogLevel.Information, LogEventLevel.Information)]
    [InlineData(MsLogLevel.Warning, LogEventLevel.Warning)]
    [InlineData(MsLogLevel.Error, LogEventLevel.Error)]
    [InlineData(MsLogLevel.Critical, LogEventLevel.Fatal)]
    public void Each_logger_level_maps_to_its_serilog_level(MsLogLevel level, LogEventLevel expected)
    {
        // Act
        var events = Capture(logger => logger.Log(level, Marker + " a line"));

        // Assert
        events.Should().ContainSingle().Which.Level.Should().Be(expected);
    }

    [Fact]
    public void The_bridge_does_not_filter_below_information()
    {
        // Guards against registering the seam through services.AddLogging(...).
        // See LoggingServiceCollectionExtensions.AddFalloutLogging.

        // Arrange
        using var pipeline = PreserveAmbientPipeline();
        using var services = new ServiceCollection().AddFalloutLogging().BuildServiceProvider();

        // Act
        var events = CaptureAmbient(() =>
        {
            var logger = services.GetRequiredService<ILogger<LoggerBridgeSpecs>>();
            logger.LogTrace(Marker + " trace line");
            logger.LogDebug(Marker + " debug line");
        });

        // Assert
        events.Select(x => x.Level).Should().Equal(LogEventLevel.Verbose, LogEventLevel.Debug);
    }

    [Fact]
    public void The_level_switch_still_gates_the_bridge()
    {
        // Arrange
        var original = FalloutBuild.Verbosity;
        try
        {
            FalloutBuild.Verbosity = Verbosity.Minimal;

            // Act
            var events = Capture(
                logger =>
                {
                    logger.LogInformation(Marker + " below the gate");
                    logger.LogWarning(Marker + " above the gate");
                },
                configuration => configuration.MinimumLevel.ControlledBy(Logging.LevelSwitch));

            // Assert
            events.Should().ContainSingle().Which.Level.Should().Be(LogEventLevel.Warning);
        }
        finally
        {
            FalloutBuild.Verbosity = original;
        }
    }

    [Fact]
    public void Message_templates_survive_the_bridge()
    {
        // Arrange
        const string Template = Marker + " restored {PackageCount} packages";

        // Act
        var events = Capture(logger => logger.LogInformation(Template, 12));

        // Assert
        // The structured sinks (compact JSON, file sinks) need the template, not a rendered string.
        var logEvent = events.Should().ContainSingle().Subject;
        logEvent.MessageTemplate.Text.Should().Be(Template);
        logEvent.Properties.Should().ContainKey("PackageCount")
            .WhoseValue.Should().BeOfType<ScalarValue>()
            .Which.Value.Should().Be(12);
    }

    [Fact]
    public void Exceptions_reach_the_log_event()
    {
        // Arrange
        var exception = new InvalidOperationException("boom");

        // Act
        var events = Capture(logger => logger.LogError(exception, Marker + " the target failed"));

        // Assert
        events.Should().ContainSingle().Which.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public void The_factory_is_not_pinned_to_one_pipeline()
    {
        // See Logging.CreateSerilogLoggerFactory for why the pipeline changes during a run.

        // Arrange
        var factory = Logging.CreateSerilogLoggerFactory();
        var first = new CollectingSink();
        var second = new CollectingSink();

        // Act
        using (PreserveAmbientPipeline())
        {
            Log.Logger = CreateLogger(first);
            factory.CreateLogger(Logging.DefaultCategoryName).LogWarning(Marker + " before the swap");

            Log.Logger = CreateLogger(second);
            factory.CreateLogger(Logging.DefaultCategoryName).LogWarning(Marker + " after the swap");
        }

        // Assert
        first.Marked.Should().ContainSingle();
        second.Marked.Should().ContainSingle();
    }

    [Fact]
    public void The_default_logger_tracks_the_current_pipeline()
    {
        // Arrange
        var sink = new CollectingSink();

        using (PreserveAmbientPipeline())
        {
            var before = Logging.Logger;

            // Act
            Log.Logger = CreateLogger(sink);
            Logging.Logger.LogWarning(Marker + " after the swap");

            // Assert
            Logging.Logger.Should().NotBeSameAs(before);
        }

        sink.Marked.Should().ContainSingle();
    }

    [Fact]
    public void The_default_logger_works_without_a_container()
    {
        // The CLI commands call Logging.Configure() directly and have no container.

        // Act
        var events = CaptureAmbient(() => Logging.Logger.LogInformation(Marker + " no container here"));

        // Assert
        events.Should().ContainSingle().Which.Level.Should().Be(LogEventLevel.Information);
    }

    [Fact]
    public void The_container_resolves_the_logging_abstractions()
    {
        // Arrange
        using var pipeline = PreserveAmbientPipeline();
        using var services = new ServiceCollection().AddFalloutLogging().BuildServiceProvider();

        // Act
        var act = () =>
        {
            services.GetRequiredService<ILoggerFactory>();
            services.GetRequiredService<ILogger<LoggerBridgeSpecs>>();
            services.GetRequiredService<ILogger>();
        };

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void A_container_logger_binds_the_pipeline_current_at_resolution()
    {
        // Fails if ILogger<T> is registered as a singleton.

        // Arrange
        var sink = new CollectingSink();

        using var pipeline = PreserveAmbientPipeline();
        using var services = new ServiceCollection().AddFalloutLogging().BuildServiceProvider();

        services.GetRequiredService<ILogger<LoggerBridgeSpecs>>();

        // Act
        Log.Logger = CreateLogger(sink);
        services.GetRequiredService<ILogger<LoggerBridgeSpecs>>().LogWarning(Marker + " after the swap");

        // Assert
        sink.Marked.Should().ContainSingle();
    }

    [Fact]
    public void The_container_hands_out_a_new_logger_per_resolution()
    {
        // Arrange
        using var pipeline = PreserveAmbientPipeline();
        using var services = new ServiceCollection().AddFalloutLogging().BuildServiceProvider();

        // Act & Assert
        services.GetRequiredService<ILogger<LoggerBridgeSpecs>>()
            .Should().NotBeSameAs(services.GetRequiredService<ILogger<LoggerBridgeSpecs>>());
        services.GetRequiredService<ILogger>()
            .Should().NotBeSameAs(services.GetRequiredService<ILogger>());
    }

    [Fact]
    public void Registering_the_seam_leaves_the_pipeline_alone()
    {
        // A second container must not remove the pipeline that the first one uses.

        // Arrange
        using var pipeline = PreserveAmbientPipeline();
        Log.Logger = CreateLogger(new CollectingSink());
        var installed = Log.Logger;

        // Act
        using var services = new ServiceCollection().AddFalloutLogging().BuildServiceProvider();

        // Assert
        Log.Logger.Should().BeSameAs(installed);
    }

    [Fact]
    public void Using_a_logger_factory_restores_the_previous_one()
    {
        // Arrange
        var previous = Logging.Factory;
        var replacement = NullLoggerFactory.Instance;

        // Act
        using (Logging.UseLoggerFactory(replacement))
        {
            // Assert
            Logging.Factory.Should().BeSameAs(replacement);
        }

        Logging.Factory.Should().BeSameAs(previous);
    }

    [Fact]
    public void Using_a_null_logger_factory_is_rejected()
    {
        // Act
        var act = () => Logging.UseLoggerFactory(factory: null);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Writes through a bridged logger to a pipeline that collects this class's records, and returns them.
    /// <paramref name="configure" /> replaces the minimum-level rule.
    /// </summary>
    private static LogEvent[] Capture(
        Action<ILogger> write,
        Func<LoggerConfiguration, LoggerConfiguration> configure = null)
    {
        return CaptureAmbient(
            () => write.Invoke(Logging.CreateSerilogLoggerFactory().CreateLogger(Logging.DefaultCategoryName)),
            configure);
    }

    /// <summary>
    /// Runs <paramref name="write" /> with a <see cref="Log.Logger" /> that collects records.
    /// </summary>
    private static LogEvent[] CaptureAmbient(
        Action write,
        Func<LoggerConfiguration, LoggerConfiguration> configure = null)
    {
        var sink = new CollectingSink();
        using (PreserveAmbientPipeline())
        {
            Log.Logger = CreateLogger(sink, configure);
            write.Invoke();
        }

        return sink.Marked.ToArray();
    }

    /// <summary>
    /// Restores <see cref="Log.Logger" /> when the returned object is disposed.
    /// </summary>
    private static IDisposable PreserveAmbientPipeline()
    {
        var original = Log.Logger;
        return DelegateDisposable.CreateBracket(cleanup: () => Log.Logger = original);
    }

    private static Serilog.Core.Logger CreateLogger(
        ILogEventSink sink,
        Func<LoggerConfiguration, LoggerConfiguration> configure = null)
    {
        var configuration = new LoggerConfiguration();
        configuration = configure?.Invoke(configuration) ?? configuration.MinimumLevel.Verbose();
        return configuration.WriteTo.Sink(sink).CreateLogger();
    }

    private class CollectingSink : ILogEventSink
    {
        private readonly List<LogEvent> events = new();

        /// <summary>
        /// The records this class wrote, without the records of other specs.
        /// </summary>
        public IReadOnlyList<LogEvent> Marked
        {
            get
            {
                lock (events)
                    return events.Where(x => x.MessageTemplate.Text.Contains(Marker)).ToList();
            }
        }

        public void Emit(LogEvent logEvent)
        {
            lock (events)
                events.Add(logEvent);
        }
    }
}
