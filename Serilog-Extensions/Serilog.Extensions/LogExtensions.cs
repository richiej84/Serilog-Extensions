using Serilog.Core;
using Serilog.Enrichers;

namespace Serilog
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Create a logger that enriches log events with the additional properties specified in the propertyBag.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="propertyBag">An anonymous type containing properties.</param>
        /// <param name="destructureObjects">If true, and the value is a non-primitive, non-array type,
        /// then the value will be converted to a structure; otherwise, unknown types will
        /// be converted to scalars, which are generally stored as strings.</param>
        /// <returns>A logger that will enrich log events as specified.</returns>
        public static ILogger AdditionalProperties(this ILogger logger, object propertyBag, bool destructureObjects = false)
        {
            return logger.ForContext(new ILogEventEnricher[]
            {
                new PropertyBagEnricher(propertyBag, destructureObjects)
            });
        }
    }
}
