using System;
using System.Diagnostics;
using Serilog.Events;

namespace Serilog.Context
{
    internal class TimedOperationContext : BaseOperationContext<TimedOperationContextOptions, TimedOperationContextData>
    {
        private readonly Stopwatch _sw;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationContext" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="description">A description for the operation.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        internal TimedOperationContext(ILogger logger,
                                       string identifier,
                                       string description,
                                       object propertyBag,
                                       TimedOperationContextOptions options)
            : base(logger, identifier, description, propertyBag, options ?? new TimedOperationContextOptions())
        {
            _sw = new Stopwatch();
        }

        protected override TimedOperationContextData GetContextData(LogEventLevel? logLevel = null)
        {
            var duration = _sw.IsRunning ? TimeSpan.Zero : _sw.Elapsed;
            return new TimedOperationContextData(Identifier, Description, logLevel ?? Options.LogLevel, Outcome, duration, Options.WarnIfExceeds);
        }

        #region Overrides of BaseOperationContext<TimedOperationContextOptions,TimedOperationContextData>

        protected override void OnContextStarted()
        {
            _sw.Start();
            base.OnContextStarted();
        }

        protected override void End()
        {
            _sw.Stop();
            base.End();
        }

        protected override void OnSuccessfulCompletion()
        {
            // Check if the max time was exceeded
            if (Options.WarnIfExceeds.HasValue && _sw.Elapsed > Options.WarnIfExceeds.Value)
            {
                const LogEventLevel logLevel = LogEventLevel.Warning; // Treat this as a warning
                if (Options.LogMode.ShouldWriteEnd(logLevel))
                {
                    Options.OperationCompletedButExceededTimeoutLogWriter(Logger, GetContextData(logLevel));
                }
            }
            else
            {
                if (Options.LogMode.ShouldWriteEnd(Options.LogLevel))
                {
                    Options.OperationCompletedLogWriter(Logger, GetContextData());
                }
            }
        }

        #endregion
    }
}
