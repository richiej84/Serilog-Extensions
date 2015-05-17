using System;
using Serilog.Events;

namespace Serilog.Context
{
    /// <summary>
    /// Configuration options for an <see cref="OperationContext"/>.
    /// </summary>
    public class OperationContextOptions
    {
        private const string OperationStartedMessage = "Starting operation {OperationId}";
        private const string OperationCompletedMessage = "Completed operation {OperationId}";
        private const string OperationFailedMessage = "Failed to complete operation {OperationId}";

        public static readonly Action<ILogger, OperationContextData> DefaultOperationStartedLogWriter = OnOperationStarted;
        public static readonly Action<ILogger, OperationContextData> DefaultOperationCompletedLogWriter = OnOperationCompleted;
        public static readonly Action<ILogger, OperationContextData> DefaultOperationFailedLogWriter = OnOperationFailed;

        /// <summary>
        /// Initialises a new <see cref="OperationContextOptions"/> instance.
        /// </summary>
        public OperationContextOptions()
        {
            LogLevel = LogEventLevel.Information;
            LogMode = OperationContext.LogMode.StartAndEndOnlyOnWarningOrWorse;
            AutoSucceedOnCompletion = true;
            AutoFailOnException = true;
            OperationStartedLogWriter = DefaultOperationStartedLogWriter;
            OperationCompletedLogWriter = DefaultOperationCompletedLogWriter;
            OperationFailedLogWriter = DefaultOperationFailedLogWriter;
        }

        /// <summary>
        /// The default level to write operation log events at.
        /// The default is <see cref="LogEventLevel.Information"/>.
        /// </summary>
        public LogEventLevel LogLevel { get; set; }

        /// <summary>
        /// Defines what log entries of the operation should be written and when.
        /// The default is <see cref="OperationContext.LogMode.StartAndEndOnlyOnWarningOrWorse"/>.
        /// </summary>
        public OperationContext.LogMode LogMode { get; set; }

        /// <summary>
        /// Specifies whether or not the operation should be marked with an outcome of <see cref="OperationOutcome.Success"/> if it completes without exception.
        /// The default is <value>true</value>.
        /// </summary>
        public bool AutoSucceedOnCompletion { get; set; }

        /// <summary>
        /// Specifies whether or not the operation should be marked with an outcome of <see cref="OperationOutcome.Fail"/> if an exception is detected.
        /// The default is <value>true</value>.
        /// </summary>
        public bool AutoFailOnException { get; set; }

        public Action<ILogger, OperationContextData> OperationStartedLogWriter { get; set; }
        public Action<ILogger, OperationContextData> OperationCompletedLogWriter { get; set; }
        public Action<ILogger, OperationContextData> OperationFailedLogWriter { get; set; }

        private static void OnOperationStarted(ILogger logger, OperationContextData data)
        {
            logger.Write(data.LogLevel, OperationStartedMessage, data.Identifier);
        }

        private static void OnOperationCompleted(ILogger logger, OperationContextData data)
        {
            logger.Write(data.LogLevel, OperationCompletedMessage, data.Identifier);
        }

        private static void OnOperationFailed(ILogger logger, OperationContextData data)
        {
            logger.Write(data.LogLevel, OperationFailedMessage, data.Identifier);
        }

    }

    /// <summary>
    /// Configuration options for an <see cref="TimedOperationContextOptions"/>.
    /// </summary>
    public class TimedOperationContextOptions : OperationContextOptions
    {
        /// <summary>
        /// Initialises a new <see cref="TimedOperationContextOptions"/> instance.
        /// </summary>
        public TimedOperationContextOptions()
        {
            WarnIfExceeds = null;
        }

        /// <summary>
        /// Specifies a time limit for the operation, after which the logging level for the operation will be <see cref="LogEventLevel.Warning"/>.
        /// By default this is not used.
        /// </summary>
        public TimeSpan? WarnIfExceeds { get; set; }

        //public TimedOperationLogWriter OnOperationCompletedButExceededTimeout { get; set; }
    }
}
