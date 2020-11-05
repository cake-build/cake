// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a text argument.
    /// </summary>
    public sealed class TextArgument : IProcessArgument
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

        /// <inheritdoc/>
        public string Render()
        {
            return _text ?? string.Empty;
        }

        /// <inheritdoc/>
        public string RenderSafe()
        {
            return Render();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return RenderSafe();
        }
    }
}