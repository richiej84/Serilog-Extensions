using Serilog.Events;

namespace Serilog.Context
{
    internal class OperationContext : BaseOperationContext<OperationContextOptions, OperationContextData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationContext" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="description">A description for the operation.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        internal OperationContext(ILogger logger,
                                      string identifier,
                                      string description,
                                      object propertyBag,
                                      OperationContextOptions options)
            : base(logger, identifier, description, propertyBag, options ?? new OperationContextOptions())
        {
        }

        #region Overrides of BaseOperationContext<OperationContextOptions, OperationContextData>

        protected override OperationContextData GetContextData(LogEventLevel? logLevel = null)
        {
            return new OperationContextData(Identifier, Description, logLevel ?? Options.LogLevel, Outcome);
        }

        #endregion
    }
}
