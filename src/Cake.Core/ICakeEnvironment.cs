// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Cake.Core.IO;

namespace Cake.Core
{
    /// <summary>
    /// Represents the environment Cake operates in.
    /// </summary>
    public interface ICakeEnvironment
    {
        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        DirectoryPath WorkingDirectory { get; set; }

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <value>The application root path.</value>
        DirectoryPath ApplicationRoot { get; }

        /// <summary>
        /// Gets a special path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="DirectoryPath"/> to the special path.</returns>
        DirectoryPath GetSpecialPath(SpecialPath path);

        /// <summary>
        /// Gets an environment variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns>The value of the environment variable.</returns>
        string GetEnvironmentVariable(string variable);

        /// <summary>
        /// Gets all environment variables.
        /// </summary>
        /// <returns>The environment variables as IDictionary&lt;string, string&gt;. </returns>
        IDictionary<string, string> GetEnvironmentVariables();

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        ICakePlatform Platform { get; }

        /// <summary>
        /// Gets the runtime Cake is running in.
        /// </summary>
        /// <value>The runtime Cake is running in.</value>
        ICakeRuntime Runtime { get; }

        /// <summary>
        /// Gets whether or not the current operative system is 64 bit.
        /// </summary>
        /// <returns>Whether or not the current operative system is 64 bit.</returns>
        [Obsolete("Please use ICakeEnvironment.Platform.Is64Bit instead.")]
        bool Is64BitOperativeSystem();

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        [Obsolete("Please use ICakeEnvironment.Platform.IsUnix instead.")]
        bool IsUnix();

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <returns>The application root path.</returns>
        [Obsolete("Please use ICakeEnvironment.ApplicationRoot instead.")]
        DirectoryPath GetApplicationRoot();
    }
}