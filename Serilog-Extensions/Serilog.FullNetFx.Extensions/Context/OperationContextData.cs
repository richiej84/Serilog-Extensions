using Serilog.Events;

namespace Serilog.Context
{
    /// <summary>
    /// Context data for the operation
    /// </summary>
    public class OperationContextData
    {
        private readonly object _identifier;
        private readonly string _description;
        private readonly LogEventLevel _logLevel;
        private readonly OperationOutcome _outcome;

        /// <summary>
        /// Initialises a new <see cref="OperationContextData"/> instance.
        /// </summary>
        /// <param name="identifier">The identifier used for the operation.</param>
        /// <param name="description">A description for the operation.</param>
        /// <param name="logLevel">The default level to write operation log events at.</param>
        /// <param name="outcome">The outcome of the operation.</param>
        internal OperationContextData(object identifier, string description, LogEventLevel logLevel, OperationOutcome outcome)
        {
            _identifier = identifier;
            _description = description;
            _logLevel = logLevel;
            _outcome = outcome;
        }

        /// <summary>
        /// The identifier used for the operation.
        /// </summary>
        public object Identifier { get { return _identifier; } }

        /// <summary>
        /// The description for the operation.
        /// </summary>
        public string Description { get { return _description; } }

        /// <summary>
        /// The default level to write operation log events at.
        /// </summary>
        public LogEventLevel LogLevel { get { return _logLevel; } }

        /// <summary>
        /// The outcome of the operation.
        /// </summary>
        public OperationOutcome Outcome { get { return _outcome; } }
    }
}
