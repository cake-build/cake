using System.Collections.Generic;

namespace Cake.Core.Utilities
{
    /// <summary>
    /// Class used to build tool arguments.
    /// </summary>
    public sealed class ToolArgumentBuilder
    {
        private readonly LinkedList<string> _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolArgumentBuilder"/> class.
        /// </summary>
        public ToolArgumentBuilder()
        {
            _arguments = new LinkedList<string>();
        }

        /// <summary>
        /// Appends text to the argument list.
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void AppendText(string text)
        {
            _arguments.AddLast(text);
        }

        /// <summary>
        /// Appends quoted text to the argument list.
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void AppendQuotedText(string text)
        {
            _arguments.AddLast(text.Quote());
        }

        /// <summary>
        /// Renders the arguments.
        /// </summary>
        /// <returns>A string containing all arguments.</returns>
        public string Render()
        {
            return string.Join(" ", _arguments);
        }
    }
}
