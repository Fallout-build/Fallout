using Fallout.Common.Tooling;
using Serilog.Events;

namespace Fallout.Common.Tools.Pnpm;

[LogLevelPattern(LogEventLevel.Warning, "^(WARN|pnpm WARN)")]
[LogLevelPattern(LogEventLevel.Debug, "^(pnpm notice)")]
partial class PnpmTasks;
