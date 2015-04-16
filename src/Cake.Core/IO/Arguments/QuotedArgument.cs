namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a quoted argument.
    /// </summary>
    public sealed class QuotedArgument : IProcessArgument
    {
        private readonly IProcessArgument _argument;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuotedArgument"/> class.
        /// </summary>
        /// <param name="argument">The argument.</param>
        public QuotedArgument(IProcessArgument argument)
        {
            _argument = argument;
        }

        /// <summary>
        /// Render the arguments as a <see cref="string" />.
        /// Sensitive information will be included.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public string Render()
        {
            return string.Concat("\"", _argument.Render(), "\"");
        }

        /// <summary>
        /// Renders the argument as a <see cref="string" />.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public string RenderSafe()
        {
            return string.Concat("\"", _argument.RenderSafe(), "\"");
        }
    }
}