using System;
#if NET45
using System.Runtime.Serialization;
#endif

namespace Cake.Core
{
    /// <summary>
    /// Represent errors that occur during script execution.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public sealed class CakeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        public CakeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CakeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CakeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NET45
        /// <summary>
        /// Initializes a new instance of the <see cref="CakeException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        private CakeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
