using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Fallout.Common.Execution;

/// <summary>
/// Registers <see cref="ILogger"/> over Serilog, so framework code does not need to use Serilog directly.
/// </summary>
/// <remarks>
/// Internal on purpose: this is not public API yet.
/// <c>InternalsVisibleTo</c> in the root <c>AssemblyInfo.cs</c> covers <c>Fallout.Cli</c> and the spec projects.
/// </remarks>
internal static class LoggingServiceCollectionExtensions
{
    /// <summary>
    /// Registers the <see cref="ILogger"/> abstraction over the Serilog pipeline.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method only registers services. It does not change the Serilog pipeline.
    /// So any container can call it, any number of times.
    /// The caller runs <see cref="Logging.Configure"/> before it builds the container.
    /// </para>
    /// <para>
    /// <see cref="Logging.Configure"/> stays out of this method because it replaces
    /// <c>Log.Logger</c> on every call. Without a build, it sets a pipeline with no file sinks,
    /// no host sink and no filter. A second container would then remove the pipeline of the first.
    /// </para>
    /// <para>
    /// This method does not call <c>services.AddLogging(...)</c>. That adds a second level filter,
    /// with a default minimum of <see cref="Microsoft.Extensions.Logging.LogLevel.Information"/>.
    /// Trace and debug records would then never reach Serilog.
    /// Without it, <see cref="Logging.LevelSwitch"/> is the only level filter.
    /// The name is fully qualified because <c>LogLevel</c> alone means Fallout's own type here.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddFalloutLogging(this IServiceCollection services)
    {
        // The factory can be a singleton, but the loggers must be transient.
        // See Logging.CreateSerilogLoggerFactory for why.
        services.TryAddSingleton(_ => Logging.CreateSerilogLoggerFactory());
        services.TryAddTransient(typeof(ILogger<>), typeof(Logger<>));
        services.TryAddTransient(sp => sp.GetRequiredService<ILoggerFactory>().CreateLogger(Logging.DefaultCategoryName));

        return services;
    }
}
