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
        public static IOperationContext BeginOperation(
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
                                        options).Start();
        }

        /// <summary>
        /// Begins an operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static IOperationContext BeginOperation(
            this ILogger logger,
            string identifier,
            object propertyBag,
            OperationContextOptions options = null)
        {
            return new OperationContext(logger,
                                        identifier,
                                        null,
                                        propertyBag,
                                        options).Start();
        }

        /// <summary>
        /// Begins an operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static IOperationContext BeginOperation(
            this ILogger logger,
            string identifier,
            OperationContextOptions options = null)
        {
            return new OperationContext(logger,
                                        identifier,
                                        null,
                                        null,
                                        options).Start();
        }

        /// <summary>
        /// Begins an timed operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="description">A description for this operation.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static IOperationContext BeginTimedOperation(
            this ILogger logger,
            string identifier,
            string description,
            object propertyBag = null,
            TimedOperationContextOptions options = null)
        {
            if (string.IsNullOrEmpty(identifier))
                identifier = Guid.NewGuid().ToString();

            return new TimedOperationContext(logger,
                                             identifier,
                                             description,
                                             propertyBag,
                                             options).Start();
        }

        /// <summary>
        /// Begins an timed operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="description">A description for this operation.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="warnIfExceeds">Specifies a time limit for the operation.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static IOperationContext BeginTimedOperation(
            this ILogger logger,
            string identifier,
            string description,
            TimeSpan warnIfExceeds,
            object propertyBag = null,
            OperationContextOptions options = null)
        {
            if (string.IsNullOrEmpty(identifier))
                identifier = Guid.NewGuid().ToString();

            var timedOperationOptions = new TimedOperationContextOptions(options) {WarnIfExceeds = warnIfExceeds};

            return new TimedOperationContext(logger,
                                             identifier,
                                             description,
                                             propertyBag,
                                             timedOperationOptions).Start();
        }

        /// <summary>
        /// Begins an timed operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static IOperationContext BeginTimedOperation(
            this ILogger logger,
            string identifier,
            object propertyBag,
            TimedOperationContextOptions options = null)
        {
            return new TimedOperationContext(logger,
                                             identifier,
                                             null,
                                             propertyBag,
                                             options).Start();
        }

        /// <summary>
        /// Begins an timed operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="warnIfExceeds">Specifies a time limit for the operation.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static IOperationContext BeginTimedOperation(
            this ILogger logger,
            string identifier,
            TimeSpan warnIfExceeds,
            object propertyBag,
            OperationContextOptions options = null)
        {
            var timedOperationOptions = new TimedOperationContextOptions(options) { WarnIfExceeds = warnIfExceeds };
            return new TimedOperationContext(logger,
                                             identifier,
                                             null,
                                             propertyBag,
                                             timedOperationOptions).Start();
        }

        /// <summary>
        /// Begins an timedoperation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static IOperationContext BeginTimedOperation(
            this ILogger logger,
            string identifier,
            TimedOperationContextOptions options = null)
        {
            return new TimedOperationContext(logger,
                                             identifier,
                                             null,
                                             null,
                                             options).Start();
        }

        /// <summary>
        /// Begins an timedoperation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="warnIfExceeds">Specifies a time limit for the operation.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static IOperationContext BeginTimedOperation(
            this ILogger logger,
            string identifier,
            TimeSpan warnIfExceeds,
            OperationContextOptions options = null)
        {
            var timedOperationOptions = new TimedOperationContextOptions(options) { WarnIfExceeds = warnIfExceeds };
            return new TimedOperationContext(logger,
                                             identifier,
                                             null,
                                             null,
                                             timedOperationOptions).Start();
        }
    }
}