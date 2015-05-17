using System;

namespace Serilog.Context
{
    /// <summary>
    /// Identifies a unit of work that has its own contextual data, along with measurements and information about the work carried out. 
    /// </summary>
    public interface IOperationContext : IDisposable
    {
        /// <summary>
        /// Gets a value indicating the outcome of the operation.
        /// </summary>
        OperationOutcome Outcome { get; }

        /// <summary>
        /// Mark the operation as having succeeded.
        /// </summary>
        void Success();

        /// <summary>
        /// Mark the operation as having failed.
        /// </summary>
        void Fail();

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
        IDisposable PushProperty(string name, object value, bool destructureObjects = false);

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
        IDisposable PushProperties(object propertyBag, bool destructureObjects = false);
    }
}