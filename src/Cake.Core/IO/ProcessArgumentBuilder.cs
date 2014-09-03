using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO
{
    /// <summary>
    /// Utility for building process arguments.
    /// </summary>
    public sealed class ProcessArgumentBuilder
    {
        private readonly List<ProcessArgument> _tokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessArgumentBuilder"/> class.
        /// </summary>
        public ProcessArgumentBuilder()
        {            
            _tokens = new List<ProcessArgument>();
        }

        /// <summary>
        /// Appends an argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        public void Append(ProcessArgument argument)
        {
            _tokens.Add(argument);
        }

        /// <summary>
        /// Renders the arguments as a <see cref="string"/>.
        /// Sensitive information will be included.
        /// </summary>
        /// <returns>A string representation of the arguments.</returns>
        public string Render()
        {
            return string.Join(" ", _tokens.Select(t => t.Render()));
        }

        /// <summary>
        /// Renders the arguments as a <see cref="string"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the arguments.</returns>
        public string RenderSafe()
        {
            return string.Join(" ", _tokens.Select(t => t.RenderSafe()));
        }
    }
}
