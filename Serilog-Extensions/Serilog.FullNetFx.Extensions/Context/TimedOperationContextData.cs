using System;
using Serilog.Events;

namespace Serilog.Context
{
    /// <summary>
    /// Context data for the timed operation
    /// </summary>
    public class TimedOperationContextData : OperationContextData
    {
        private readonly TimeSpan _duration;
        private readonly TimeSpan? _maxDuration;

        /// <summary>
        /// Initialises a new <see cref="OperationContextData"/> instance.
        /// </summary>
        /// <param name="data">Context data to base this instance on.</param>
        /// <param name="duration">The current duration of the operations.</param>
        /// <param name="maxDuration">An optional max duration for the operation.</param>
        internal TimedOperationContextData(OperationContextData data,
                                           TimeSpan duration,
                                           TimeSpan? maxDuration)
            : this(data.Identifier, data.Description, data.LogLevel, data.Outcome, duration, maxDuration)
        {
        }

        /// <summary>
        /// Initialises a new <see cref="OperationContextData"/> instance.
        /// </summary>
        /// <param name="identifier">The identifier used for the operation.</param>
        /// <param name="description">A description for the operation.</param>
        /// <param name="logLevel">The default level to write operation log events at.</param>
        /// <param name="outcome">The outcome of the operation.</param>
        /// <param name="duration">The current duration of the operations.</param>
        /// <param name="maxDuration">An optional max duration for the operation.</param>
        internal TimedOperationContextData(object identifier,
                                           string description,
                                           LogEventLevel logLevel,
                                           OperationOutcome outcome,
                                           TimeSpan duration,
                                           TimeSpan? maxDuration)
            : base(identifier, description, logLevel, outcome)
        {
            _duration = duration;
            _maxDuration = maxDuration;
        }

        /// <summary>
        /// The current duration of the operations.
        /// </summary>
        public TimeSpan Duration { get { return _duration; } }

        /// <summary>
        /// An optional max duration for the operation.
        /// </summary>
        public TimeSpan? MaxDuration { get { return _maxDuration; } }
    }
}