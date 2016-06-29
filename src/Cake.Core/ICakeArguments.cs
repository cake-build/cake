// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Represents arguments passed to script.
    /// </summary>
    public interface ICakeArguments
    {
        /// <summary>
        /// Gets the arguments provided via the command line and their specified values.
        /// </summary>
        /// <value>The arguments dictionary.</value>
        IReadOnlyDictionary<string, string> AsDictionary { get; }

        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>
        ///   <c>true</c> if the argument exist; otherwise <c>false</c>.
        /// </returns>
        bool HasArgument(string name);

        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value if the argument exists, otherwise <c>null</c>.</returns>
        string GetArgument(string name);
    }
}
