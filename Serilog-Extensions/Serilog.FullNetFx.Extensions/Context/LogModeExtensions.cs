using Serilog.Events;

namespace Serilog.Context
{
    internal static class LogModeExtensions
    {
        internal static bool ShouldWriteStart(this OperationContext.LogMode logMode)
        {
            return logMode == OperationContext.LogMode.StartAndEnd 
                || logMode == OperationContext.LogMode.StartOnly;
        }

        internal static bool ShouldWriteEnd(this OperationContext.LogMode logMode, LogEventLevel logLevel = LogEventLevel.Verbose)
        {
            return logMode == OperationContext.LogMode.EndOnly 
                || logMode == OperationContext.LogMode.StartAndEnd 
                || (logMode == OperationContext.LogMode.EndOnlyOnWarning && logLevel >= LogEventLevel.Warning);
        }
    }
}
