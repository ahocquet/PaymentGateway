using System;
using System.Runtime.Serialization;

namespace PaymentGateway.EventSourcing.Core.Exception
{
    /// <summary>
    /// Marker exception for this library from which all its exceptions derive.
    /// </summary>
    [Serializable]
    public abstract class AggregateExceptionBase : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateExceptionBase"/> class.
        /// </summary>
        protected AggregateExceptionBase() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected AggregateExceptionBase(string message)
            : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        protected AggregateExceptionBase(string message, System.Exception innerException)
            : base(message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateExceptionBase"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected AggregateExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}