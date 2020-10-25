// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO.Arguments
{
    /// <summary>
    /// Represents a secret argument.
    /// </summary>
    public sealed class SecretArgument : IProcessArgument
    {
        private readonly IProcessArgument _argument;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretArgument"/> class.
        /// </summary>
        /// <param name="argument">The argument.</param>
        public SecretArgument(IProcessArgument argument)
        {
            _argument = argument;
        }

        /// <inheritdoc/>
        public string Render()
        {
            return _argument.Render();
        }

        /// <inheritdoc/>
        public string RenderSafe()
        {
            return "[REDACTED]";
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return RenderSafe();
        }
    }
}