// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Core.IO.Arguments;

namespace Cake.Core.IO
{
    /// <summary>
    /// Utility for building process arguments.
    /// </summary>
    public sealed class ProcessArgumentBuilder : IReadOnlyCollection<IProcessArgument>
    {
        private readonly List<IProcessArgument> _tokens;

        /// <summary>
        /// Gets the number of arguments contained in the <see cref="ProcessArgumentBuilder"/>.
        /// </summary>
        public int Count
        {
            get { return _tokens.Count; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessArgumentBuilder"/> class.
        /// </summary>
        public ProcessArgumentBuilder()
        {
            _tokens = new List<IProcessArgument>();
        }

        /// <summary>
        /// Clears all arguments from the builder.
        /// </summary>
        public void Clear()
        {
            _tokens.Clear();
        }

        /// <summary>
        /// Appends an argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        public void Append(IProcessArgument argument)
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
            builder.Append(new TextArgument(value));
            return builder;
        }

        IEnumerator<IProcessArgument> IEnumerable<IProcessArgument>.GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_tokens).GetEnumerator();
        }
    }
}
