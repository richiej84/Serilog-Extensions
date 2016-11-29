using System;
using Serilog.Events;

namespace Serilog.Context
{
    /// <summary>
    /// Configuration options for an <see cref="TimedOperationContextOptions"/>.
    /// </summary>
    public class TimedOperationContextOptions : OperationContextOptions
    {
        private const string OperationCompletedMessage = "Completed operation {OperationId} in {OperationElapsed} ({OperationElapsedInMs}ms)";
        private const string OperationFailedMessage = "Failed to complete operation {OperationId}";
        private const string OperationCompletedButExceededTimeoutMessage = "Completed operation {OperationId} in {OperationElapsed} ({OperationElapsedInMs}ms) but exceeded time limit of {WarningLimit}";

        /// <summary>
        /// The default log writer that is used when the operation completes.
        /// </summary>
        public static readonly Action<ILogger, TimedOperationContextData> DefaultTimedOperationCompletedLogWriter = OnTimedOperationCompleted;

        /// <summary>
        /// The default log writer that is used when the operation fails.
        /// </summary>
        public static readonly Action<ILogger, TimedOperationContextData> DefaultTimedOperationFailedLogWriter = OnTimedOperationFailed;

        /// <summary>
        /// The default log writer that is used when the operation completes but exceeds the timeout.
        /// </summary>
        public static readonly Action<ILogger, TimedOperationContextData> DefaultTimedOperationCompletedButExceededTimeoutLogWriter = OnTimedOperationCompletedButExceededTimeout;

        /// <summary>
        /// Initialises a new <see cref="TimedOperationContextOptions"/> instance.
        /// </summary>
        public TimedOperationContextOptions()
            : this(null)
        {
        }

        /// <summary>
        /// Initialises a new <see cref="TimedOperationContextOptions"/> instance, copying values from the supplied options.
        /// </summary>
        public TimedOperationContextOptions(OperationContextOptions options)
            : base(options)
        {
            WarnIfExceeds = null;
            OperationCompletedLogWriter = DefaultTimedOperationCompletedLogWriter;
            OperationFailedLogWriter = DefaultTimedOperationFailedLogWriter;
            OperationCompletedButExceededTimeoutLogWriter = DefaultTimedOperationCompletedButExceededTimeoutLogWriter;
        }

        /// <summary>
        /// Specifies a time limit for the operation, after which the logging level for the operation will be <see cref="LogEventLevel.Warning"/>.
        /// By default this is not used.
        /// </summary>
        public TimeSpan? WarnIfExceeds { get; set; }

        /// <summary>
        /// The log writer to be used when the operation completes.
        /// </summary>
        public new Action<ILogger, TimedOperationContextData> OperationCompletedLogWriter { get; set; }

        /// <summary>
        /// The log writer to be used when the operation fails.
        /// </summary>
        public new Action<ILogger, TimedOperationContextData> OperationFailedLogWriter { get; set; }

        /// <summary>
        /// The log writer to be used when the operation completes but exceeds the timeout.
        /// </summary>
        public Action<ILogger, TimedOperationContextData> OperationCompletedButExceededTimeoutLogWriter { get; set; }

        private static void OnTimedOperationCompleted(ILogger logger, TimedOperationContextData data)
        {
            logger.Write(data.LogLevel,
                         OperationCompletedMessage,
                         data.Identifier,
                         data.Duration.ToString("g"),
                         data.Duration.TotalMilliseconds.ToString("f3"));
        }

        private static void OnTimedOperationFailed(ILogger logger, TimedOperationContextData data)
        {
            logger.Write(data.LogLevel, OperationFailedMessage, data.Identifier);
        }

        private static void OnTimedOperationCompletedButExceededTimeout(ILogger logger, TimedOperationContextData data)
        {
            if(data.MaxDuration.HasValue)
            {
                logger.Write(data.LogLevel,
                             OperationCompletedButExceededTimeoutMessage,
                             data.Identifier,
                             data.Duration.ToString("g"),
                             data.Duration.TotalMilliseconds.ToString("f3"),
                             data.MaxDuration);
            }
            else
            {
                OnTimedOperationCompleted(logger, data);
            }
        }
    }
}