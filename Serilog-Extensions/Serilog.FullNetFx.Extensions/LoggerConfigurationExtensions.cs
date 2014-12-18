using System;
using Serilog.Configuration;
using Serilog.Context;
using Serilog.Enrichers;

namespace Serilog
{
    /// <summary>
    /// Extends <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Enrich log events with properties from <see cref="OperationContext"/>.
        /// </summary>
        /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LoggerConfiguration FromOperationContext(
            this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException("enrichmentConfiguration");
            return enrichmentConfiguration.With<OperationContextEnricher>();
        }
    }
}
