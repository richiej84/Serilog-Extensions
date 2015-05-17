using Serilog.Events;

namespace Serilog.Context
{
    internal static class LogModeExtensions
    {
        internal static bool ShouldWriteStart(this OperationContextLogMode logMode)
        {
            return logMode.HasFlag(OperationContextLogMode.Start);
        }

        internal static bool ShouldWriteEnd(this OperationContextLogMode logMode, LogEventLevel logLevel = LogEventLevel.Verbose)
        {
            return logMode.HasFlag(OperationContextLogMode.End)
                   && (!logMode.HasFlag(OperationContextLogMode.WarningOrWorse) || logLevel >= LogEventLevel.Warning)
                   && (!logMode.HasFlag(OperationContextLogMode.Error) || logLevel >= LogEventLevel.Error);
        }
    }
}
