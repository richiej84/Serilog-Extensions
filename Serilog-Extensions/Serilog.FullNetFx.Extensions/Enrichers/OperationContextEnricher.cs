using Serilog.Core;
using Serilog.Events;
using Serilog.Context;

namespace Serilog.Enrichers
{
    internal sealed class OperationContextEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            OperationContext.Enrich(logEvent, propertyFactory);
        }
    }
}
