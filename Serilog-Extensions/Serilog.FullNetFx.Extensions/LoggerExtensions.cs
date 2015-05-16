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
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="description">A description for this operation.</param>
        /// <param name="level">The level used to write the operation details to the log. By default this is the information level.</param>
        /// <param name="logMode">Indicates what, if any, log entries should be writen and when.</param>
        /// <param name="warnIfExceeds">Specifies a limit, if it takes more than this limit, the level will be set to warning. By default this is not used.</param>
        /// <param name="autoSucceedOnExit">Specifies whether or not the operation should be marked with an outcome of <see cref="OperationOutcome.Success"/> if it completes without exception.</param>
        /// <param name="autoFailOnException">Specifies whether or not the operation should be marked with an outcome of <see cref="OperationOutcome.Fail"/> if an exception is detected.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static OperationContext BeginOperation(
            this ILogger logger,
            string description = null,
            string identifier = null,
            object propertyBag = null,
            LogEventLevel level = LogEventLevel.Debug,
            OperationContext.LogMode logMode = OperationContext.LogMode.EndOnlyOnWarning,
            TimeSpan? warnIfExceeds = null,
            bool autoSucceedOnExit = true,
            bool autoFailOnException = true)
        {
            object operationIdentifier = identifier;
            if (string.IsNullOrEmpty(identifier))
                operationIdentifier = Guid.NewGuid();

            return new OperationContext(logger,
                                        level,
                                        logMode,
                                        warnIfExceeds,
                                        operationIdentifier,
                                        description,
                                        autoSucceedOnExit,
                                        autoFailOnException,
                                        propertyBag);
        }

        /// <summary>
        /// Begins an operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="logMode">Indicates what, if any, log entries should be writen and when.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static OperationContext BeginOperation(
            this ILogger logger,
            string identifier,
            object propertyBag,
            OperationContext.LogMode logMode = OperationContext.LogMode.EndOnlyOnWarning)
        {

            return new OperationContext(logger,
                                        LogEventLevel.Debug,
                                        logMode,
                                        null,
                                        identifier,
                                        null,
                                        true,
                                        true,
                                        propertyBag);
        }

        /// <summary>
        /// Begins an operation that should be declared inside a using block or appropriately disposed of when completed.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="logMode">Indicates what, if any, log entries should be writen and when.</param>
        /// <returns>A disposable object. Wrap this inside a using block so the dispose can be called to stop the timing.</returns>
        public static OperationContext BeginOperation(
            this ILogger logger,
            string identifier,
            OperationContext.LogMode logMode = OperationContext.LogMode.EndOnlyOnWarning)
        {

            return new OperationContext(logger,
                                        LogEventLevel.Debug,
                                        logMode,
                                        null,
                                        identifier,
                                        null,
                                        true,
                                        true,
                                        null);
        }
    }
}