// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Commands
{
    /// <summary>
    /// Represents an executable command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command with the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns><c>true</c> if the command exited successfully; otherwise, <c>false</c>.</returns>
        bool Execute(CakeOptions options);
    }
}
