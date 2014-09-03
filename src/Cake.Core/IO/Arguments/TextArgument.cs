namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a text argument.
    /// </summary>
    public sealed class TextArgument : ProcessArgument
    {
        private readonly string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextArgument"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public TextArgument(string text)
        {
            _text = text;
        }

        /// <summary>
        /// Render the arguments as a <see cref="string" />.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        public override string Render()
        {
            return _text ?? string.Empty;
        }        
    }
}