// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Represents arguments passed to the executing script.
    /// </summary>
    public interface ICakeArguments
    {
        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>
        ///   <c>true</c> if the argument exist; otherwise <c>false</c>.
        /// </returns>
        bool HasArgument(string name);

        /// <summary>
        /// Gets all values for an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument values.</returns>
        ICollection<string> GetArguments(string name);

        /// <summary>
        /// Gets all command line arguments.
        /// </summary>
        /// <returns>The command line arguments as IDictionary&lt;string, ICollection&lt;string&gt;&gt;.</returns>
        IDictionary<string, ICollection<string>> GetArguments();
    }
}