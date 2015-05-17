using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;

namespace Serilog.Context
{
    public class OperationContextData
    {
        private readonly object _identifier;
        private readonly string _description;
        private readonly LogEventLevel _logLevel;
        private readonly OperationOutcome _outcome;

        public OperationContextData(object identifier, string description, LogEventLevel logLevel, OperationOutcome outcome)
        {
            _identifier = identifier;
            _description = description;
            _logLevel = logLevel;
            _outcome = outcome;
        }

        public object Identifier { get { return _identifier; } }

        public string Description { get { return _description; } }

        public LogEventLevel LogLevel { get { return _logLevel; } }

        public OperationOutcome Outcome { get { return _outcome; } }
    }
}
