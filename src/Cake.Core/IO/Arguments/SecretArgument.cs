namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a secret argument.
    /// </summary>
    public sealed class SecretArgument : ProcessArgument
    {
        private readonly ProcessArgument _argument;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretArgument"/> class.
        /// </summary>
        /// <param name="argument">The argument.</param>
        public SecretArgument(ProcessArgument argument)
        {
            _argument = argument;
        }

        /// <summary>
        /// Render the arguments as a <see cref="string" />.
        /// The secret text will be included.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public override string Render()
        {
            return _argument.Render();
        }

        /// <summary>
        /// Renders the argument as a <see cref="string" />.
        ///  The secret text will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        public override string RenderSafe()
        {
            return "[REDACTED]";
        }
    }
}