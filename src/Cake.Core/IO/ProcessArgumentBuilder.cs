using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Core.IO.Arguments;

namespace Cake.Core.IO
{
    /// <summary>
    /// Utility for building process arguments.
    /// </summary>
    public sealed class ProcessArgumentBuilder : IProcessArgumentList<ProcessArgumentBuilder>
    {
        private readonly List<IProcessArgument> _tokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessArgumentBuilder"/> class.
        /// </summary>
        public ProcessArgumentBuilder()
        {
            _tokens = new List<IProcessArgument>();
        }

        /// <summary>
        /// Appends an argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>
        /// A <see cref="ProcessArgumentBuilder" /> for fluent chaining.
        /// </returns>
        public ProcessArgumentBuilder Append(IProcessArgument argument)
        {
            _tokens.Add(argument);

            return this;
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

        /// <summary>
        /// Tries to filer any unsafe arguments from string
        /// </summary>
        /// <param name="source">unsafe source string.</param>
        /// <returns>Filtered string.</returns>
        public string FilterUnsafe(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return source;
            }

            return _tokens
                .Select(token => new
                {
                    Safe = token.RenderSafe().Trim('"').Trim(),
                    Unsafe = token.Render().Trim('"').Trim()
                })
                .Where(token => token.Safe != token.Unsafe)
                .Aggregate(
                    new StringBuilder(source),
                    (sb, token) => sb.Replace(token.Unsafe, token.Safe),
                    sb => sb.ToString());
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ProcessArgumentBuilder"/>.
        /// </summary>
        /// <param name="value">The text value to convert.</param>
        /// <returns>A <see cref="FilePath"/>.</returns>
        public static implicit operator ProcessArgumentBuilder(string value)
        {
            return FromString(value);
        }

        /// <summary>
        /// Performs a conversion from <see cref="System.String"/> to <see cref="ProcessArgumentBuilder"/>.
        /// </summary>
        /// <param name="value">The text value to convert.</param>
        /// <returns>A <see cref="FilePath"/>.</returns>
        public static ProcessArgumentBuilder FromString(string value)
        {
            var builder = new ProcessArgumentBuilder();

            return builder.Append(new TextArgument(value));
        }
    }
}