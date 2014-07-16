using System;
using System.Runtime.Serialization;

namespace Cake.Core
{
    /// <summary>
    /// Represent errors that occur during script execution.
    /// </summary>
    [Serializable]
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

        private CakeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
