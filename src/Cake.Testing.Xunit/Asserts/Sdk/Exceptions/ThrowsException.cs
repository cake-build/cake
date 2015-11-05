using System;
using System.Diagnostics.CodeAnalysis;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when code unexpectedly fails to throw an exception.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    public class ThrowsException : AssertActualExpectedException
    {
        readonly string stackTrace = null;

        /// <summary>
        /// Creates a new instance of the <see cref="ThrowsException"/> class. Call this constructor
        /// when no exception was thrown.
        /// </summary>
        /// <param name="expectedType">The type of the exception that was expected</param>
        public ThrowsException(Type expectedType)
            : this(expectedType, "(No exception was thrown)", null, null) { }

        /// <summary>
        /// Creates a new instance of the <see cref="ThrowsException"/> class. Call this constructor
        /// when an exception of the wrong type was thrown.
        /// </summary>
        /// <param name="expectedType">The type of the exception that was expected</param>
        /// <param name="actual">The actual exception that was thrown</param>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "This parameter is verified elsewhere.")]
        public ThrowsException(Type expectedType, Exception actual)
            : this(expectedType, ArgumentFormatter.Format(actual.GetType()), actual.Message, actual.StackTrace) { }

        /// <summary>
        /// THIS CONSTRUCTOR IS FOR UNIT TESTING PURPOSES ONLY.
        /// </summary>
        protected ThrowsException(Type expected, string actual, string actualMessage, string stackTrace)
            : base(expected,
                   actual + (actualMessage == null ? "" : ": " + actualMessage),
                   "Assert.Throws() Failure")
        {
            this.stackTrace = stackTrace;
        }

        /// <summary>
        /// Gets a string representation of the frames on the call stack at the time the current exception was thrown.
        /// </summary>
        /// <returns>A string that describes the contents of the call stack, with the most recent method call appearing first.</returns>
        public override string StackTrace
        {
            get { return stackTrace ?? base.StackTrace; }
        }
    }
}