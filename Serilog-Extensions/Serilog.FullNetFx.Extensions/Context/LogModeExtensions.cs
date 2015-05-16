using Serilog.Events;

namespace Serilog.Context
{
    internal static class LogModeExtensions
    {
        internal static bool ShouldWriteStart(this OperationContext.LogMode logMode)
        {
            return logMode.HasFlag(OperationContext.LogMode.Start);
        }

        internal static bool ShouldWriteEnd(this OperationContext.LogMode logMode, LogEventLevel logLevel = LogEventLevel.Verbose)
        {
            return logMode.HasFlag(OperationContext.LogMode.End)
                   && (!logMode.HasFlag(OperationContext.LogMode.WarningOrWorse) || logLevel >= LogEventLevel.Warning)
                   && (!logMode.HasFlag(OperationContext.LogMode.Error) || logLevel >= LogEventLevel.Error);
        }
    }
}
