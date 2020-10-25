// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        /// <inheritdoc/>
        public string Render()
        {
            return string.Concat("\"", _argument.Render(), "\"");
        }

        /// <inheritdoc/>
        public string RenderSafe()
        {
            return string.Concat("\"", _argument.RenderSafe(), "\"");
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return RenderSafe();
        }
    }
}