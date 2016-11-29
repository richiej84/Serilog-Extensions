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

        /// <summary>
        /// The default log writer that is used when the operation starts.
        /// </summary>
        public static readonly Action<ILogger, OperationContextData> DefaultOperationStartedLogWriter = OnOperationStarted;

        /// <summary>
        /// The default log writer that is used when the operation completes.
        /// </summary>
        public static readonly Action<ILogger, OperationContextData> DefaultOperationCompletedLogWriter = OnOperationCompleted;

        /// <summary>
        /// The default log writer that is used when the operation fails.
        /// </summary>
        public static readonly Action<ILogger, OperationContextData> DefaultOperationFailedLogWriter = OnOperationFailed;

        /// <summary>
        /// Initialises a new <see cref="OperationContextOptions"/> instance.
        /// </summary>
        public OperationContextOptions()
            : this(null)
        {
        }

        /// <summary>
        /// Initialises a new <see cref="OperationContextOptions"/> instance, copying values from the supplied options
        /// </summary>
        protected OperationContextOptions(OperationContextOptions options)
        {
            if (options == null)
            {
                LogLevel = LogEventLevel.Information;
                LogMode = OperationContextLogMode.StartAndEndOnlyOnWarningOrWorse;
                AutoSucceedOnCompletion = true;
                AutoFailOnException = true;
                OperationStartedLogWriter = DefaultOperationStartedLogWriter;
                OperationCompletedLogWriter = DefaultOperationCompletedLogWriter;
                OperationFailedLogWriter = DefaultOperationFailedLogWriter;
            }
            else
            {
                LogLevel = options.LogLevel;
                LogMode = options.LogMode;
                AutoSucceedOnCompletion = options.AutoSucceedOnCompletion;
                AutoFailOnException = options.AutoFailOnException;
                OperationStartedLogWriter = options.OperationStartedLogWriter;
                OperationCompletedLogWriter = options.OperationCompletedLogWriter;
                OperationFailedLogWriter = options.OperationFailedLogWriter;
            }
        }

        /// <summary>
        /// The default level to write operation log events at.
        /// The default is <see cref="LogEventLevel.Information"/>.
        /// </summary>
        public LogEventLevel LogLevel { get; set; }

        /// <summary>
        /// Defines what log entries of the operation should be written and when.
        /// The default is <see cref="OperationContextLogMode.StartAndEndOnlyOnWarningOrWorse"/>.
        /// </summary>
        public OperationContextLogMode LogMode { get; set; }

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

        /// <summary>
        /// The log writer to be used when the operation starts.
        /// </summary>
        public Action<ILogger, OperationContextData> OperationStartedLogWriter { get; set; }

        /// <summary>
        /// The log writer to be used when the operation completes.
        /// </summary>
        public Action<ILogger, OperationContextData> OperationCompletedLogWriter { get; set; }

        /// <summary>
        /// The log writer to be used when the operation fails.
        /// </summary>
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
}
