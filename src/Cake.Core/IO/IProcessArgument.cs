// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.IO
{
    /// <summary>
    ///  Represents a process argument.
    /// </summary>
    public interface IProcessArgument
    {
        /// <summary>
        /// Render the arguments as a <see cref="string"/>.
        /// Sensitive information will be included.
        /// </summary>
        /// <returns>A string representation of the argument.</returns>
        string Render();

        /// <summary>
        /// Renders the argument as a <see cref="string"/>.
        /// Sensitive information will be redacted.
        /// </summary>
        /// <returns>A safe string representation of the argument.</returns>
        string RenderSafe();
    }
}
