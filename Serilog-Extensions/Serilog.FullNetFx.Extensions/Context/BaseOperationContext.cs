using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Serilog.Enrichers;
using Serilog.Events;

namespace Serilog.Context
{
    /// <summary>
    /// Identifies a unit of work that has its own contextual data, along with measurements and information about the work carried out. 
    /// </summary>
    internal abstract class BaseOperationContext<TOptions, TData> : IOperationContext
        where TOptions : OperationContextOptions
        where TData : OperationContextData
    {
        private readonly ILogger _logger;
        private readonly string _identifier;
        private readonly string _description;
        private readonly TOptions _options;

        private readonly IDisposable _operationContextBookmark;
        private IDisposable _contextualPropertiesBookmark;
        private OperationOutcome _outcome = OperationOutcome.Unknown;
        private bool _started;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationContext" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier used for the operation. If not specified, a random guid will be used.</param>
        /// <param name="description">A description for the operation.</param>
        /// <param name="propertyBag">A colletion of additional properties to associate with the current operation. This is typically an anonymous type.</param>
        /// <param name="options">Configuration options for the operation context.</param>
        internal BaseOperationContext(ILogger logger,
                                      string identifier,
                                      string description,
                                      object propertyBag,
                                      TOptions options)
        {
            if(options == null) throw new ArgumentNullException("options");

            _logger = logger;
            _identifier = identifier;
            _description = description;
            _options = options;

            _operationContextBookmark = OperationLogContext.PushOperationId(identifier);

            if (propertyBag != null)
            {
                // Save the first contextual property that we set. We then dispose of this bookmark, reverting the stack to what it was previously
                _contextualPropertiesBookmark = PushProperties(propertyBag);
            }
        }

        protected ILogger Logger { get { return _logger; } }

        protected TOptions Options { get { return _options; } }

        public string Identifier { get { return _identifier; } }

        public string Description { get { return _description; } }

        /// <summary>
        /// Gets a value indicating the outcome of the operation.
        /// </summary>
        public OperationOutcome Outcome
        {
            get { return _outcome; }
        }

        /// <summary>
        /// Mark the operation as having succeeded.
        /// </summary>
        public void Success()
        {
            _outcome = OperationOutcome.Success;
        }

        /// <summary>
        /// Mark the operation as having failed.
        /// </summary>
        public void Fail()
        {
            _outcome = OperationOutcome.Fail;
        }

        /// <summary>
        /// Push a property onto the context, returning an <see cref="IDisposable"/>
        /// that can later be used to remove the property, along with any others that
        /// may have been pushed on top of it and not yet popped. The property must
        /// be popped from the same thread/logical call context.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns>A handle to later remove the property from the context.</returns>
        /// <param name="destructureObjects">If true, and the value is a non-primitive, non-array type,
        /// then the value will be converted to a structure; otherwise, unknown types will
        /// be converted to scalars, which are generally stored as strings.</param>
        /// <returns>A token that must be disposed, in order, to pop properties back off the stack.</returns>
        public IDisposable PushProperty(string name, object value, bool destructureObjects = false)
        {
            var bookmark = LogContext.PushProperty(name, value, destructureObjects);
            if (_contextualPropertiesBookmark == null)
            {
                _contextualPropertiesBookmark = bookmark;
            }
            return bookmark;
        }

        /// <summary>
        /// Push multiple properties onto the context, returning an <see cref="IDisposable"/>
        /// that can later be used to remove the properties. The properties must
        /// be popped from the same thread/logical call context.
        /// </summary>
        /// <param name="propertyBag">An anonymous type containing properties.</param>
        /// <returns>A handle to later remove the property from the context.</returns>
        /// <param name="destructureObjects">If true, and the value is a non-primitive, non-array type,
        /// then the value will be converted to a structure; otherwise, unknown types will
        /// be converted to scalars, which are generally stored as strings.</param>
        /// <returns>A token that must be disposed, in order, to pop properties back off the stack.</returns>
        public IDisposable PushProperties(object propertyBag, bool destructureObjects = false)
        {
            var bookmark = LogContext.PushProperties(new ILogEventEnricher[]
            {
                new PropertyBagEnricher(propertyBag, destructureObjects)
            });
            if (_contextualPropertiesBookmark == null)
            {
                _contextualPropertiesBookmark = bookmark;
            }
            return bookmark;
        }

        /// <summary>
        /// Enriches a given log event with data from the current <see cref="OperationContext"/>.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        internal static void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            OperationLogContext.EnrichLogEvent(logEvent, propertyFactory);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            try
            {
                End();
            }
            finally
            {
                if (_contextualPropertiesBookmark != null)
                    _contextualPropertiesBookmark.Dispose();

                if (_operationContextBookmark != null)
                    _operationContextBookmark.Dispose();
            }
        }

        internal IOperationContext Start()
        {
            OnContextStarted();
            _started = true;
            return this;
        }

        protected virtual void End()
        {
            if (!_started)
            {
                _logger.Warning("OperationContext was not started. Start() should be called after creating the context.");
            }

            var exceptionThrown = HasExceptionBeenThrown();
            if (exceptionThrown && _options.AutoFailOnException)
            {
                _outcome = OperationOutcome.Fail;
            }
            else if (!exceptionThrown && _options.AutoSucceedOnCompletion
                     && _outcome == OperationOutcome.Unknown) // Only auto-succeed if no outcome has been explictly set
            {
                _outcome = OperationOutcome.Success;
            }

            switch (_outcome)
            {
                case OperationOutcome.Fail:
                    OnFailedCompletion();
                    break;
                case OperationOutcome.Success:
                    OnSuccessfulCompletion();
                    break;
                case OperationOutcome.Unknown:
                    OnSuccessfulCompletion();
                    break;
                default:
                    _logger.Warning("Unknown outcome for operation context");
                    break;
            }
        }

        protected abstract TData GetContextData(LogEventLevel? logLevel = null);

        protected virtual void OnContextStarted()
        {
            if (Options.LogMode.ShouldWriteStart())
            {
                Options.OperationStartedLogWriter(Logger, GetContextData());
            }
        }

        protected virtual void OnFailedCompletion()
        {
            if (Options.LogMode.ShouldWriteEnd(LogEventLevel.Error))
            {
                Options.OperationFailedLogWriter(Logger, GetContextData());
            }
        }

        protected virtual void OnSuccessfulCompletion()
        {
            if (Options.LogMode.ShouldWriteEnd(Options.LogLevel))
            {
                Options.OperationCompletedLogWriter(Logger, GetContextData());
            }
        }

        protected virtual void OnUnknownCompletion()
        {
            OnSuccessfulCompletion();
        }

        protected static bool HasExceptionBeenThrown()
        {
            return Marshal.GetExceptionPointers() != IntPtr.Zero ||
                   Marshal.GetExceptionCode() != 0;
        }

        /// <summary>
        /// A private log context, specifically for storing operation data.
        /// </summary>
        private static class OperationLogContext
        {
            private const string OperationIdName = "OperationId";
            private const string ParentOperationIdName = "ParentOperationId";
            private const string OperationStackName = "OperationStack";
            static readonly string DataSlotName = typeof(OperationLogContext).FullName;

            public static IDisposable PushOperationId(object value)
            {
                var stack = GetOrCreateStack();
                var bookmark = new ContextStackBookmark(stack);

                Values = stack.Push(value);

                return bookmark;
            }

            static ImmutableStack<object> GetOrCreateStack()
            {
                var values = Values;
                if (values == null)
                {
                    values = ImmutableStack<object>.Empty;
                    Values = values;
                }
                return values;
            }

            static ImmutableStack<object> Values
            {
                get { return (ImmutableStack<object>)CallContext.LogicalGetData(DataSlotName); }
                set { CallContext.LogicalSetData(DataSlotName, value); }
            }

            internal static void EnrichLogEvent(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                var values = Values;
                if (values == null || values == ImmutableStack<object>.Empty)
                    return;

                // NOTE: removed logging the operation and parent separately for now as this they're redundent.

                //var currentAndParentOps = values.Take(2).ToArray();

                //new PropertyEnricher(OperationIdName, currentAndParentOps[0]).Enrich(logEvent, propertyFactory);

                //if (currentAndParentOps.Length > 1)
                //{
                //    new PropertyEnricher(ParentOperationIdName, currentAndParentOps[1]).Enrich(logEvent, propertyFactory);
                //}

                new PropertyEnricher(OperationStackName, values).Enrich(logEvent, propertyFactory);
            }

            sealed class ContextStackBookmark : IDisposable
            {
                readonly ImmutableStack<object> _bookmark;

                public ContextStackBookmark(ImmutableStack<object> bookmark)
                {
                    _bookmark = bookmark;
                }

                public void Dispose()
                {
                    Values = _bookmark;
                }
            }
        }
    }
}