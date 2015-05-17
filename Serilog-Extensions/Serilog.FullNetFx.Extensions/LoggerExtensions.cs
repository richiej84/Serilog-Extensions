using System;
using Serilog.Events;
using Serilog.Context;

namespace Serilog
{
    /// <summary>
    /// Extends <see cref="ILogger"/>.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Begins an operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="description">A description for this operation.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static OperationContext BeginOperation(
            this ILogger logger,
            string identifier,
            string description,
            object propertyBag = null,
            OperationContextOptions options = null)
        {
            if (string.IsNullOrEmpty(identifier))
                identifier = Guid.NewGuid().ToString();

            return new OperationContext(logger,
                                        identifier,
                                        description,
                                        propertyBag,
                                        options);
        }

        /// <summary>
        /// Begins an operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static OperationContext BeginOperation(
            this ILogger logger,
            string identifier,
            object propertyBag,
            OperationContextOptions options = null)
        {
            return new OperationContext(logger,
                                        identifier,
                                        null,
                                        propertyBag,
                                        options);
        }

        /// <summary>
        /// Begins an operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static OperationContext BeginOperation(
            this ILogger logger,
            string identifier,
            OperationContextOptions options = null)
        {
            return new OperationContext(logger,
                                        identifier,
                                        null,
                                        null,
                                        options);
        }
    }
}