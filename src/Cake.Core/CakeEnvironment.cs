// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core.IO;
using Cake.Core.Polyfill;

namespace Cake.Core
{
    /// <summary>
    /// Represents the environment Cake operates in.
    /// </summary>
    public sealed class CakeEnvironment : ICakeEnvironment
    {
        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        public DirectoryPath WorkingDirectory
        {
            get { return System.IO.Directory.GetCurrentDirectory(); }
            set { SetWorkingDirectory(value); }
        }

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <value>The application root path.</value>
        public DirectoryPath ApplicationRoot { get; }

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        public ICakePlatform Platform { get; }

        /// <summary>
        /// Gets the runtime Cake is running in.
        /// </summary>
        /// <value>The runtime Cake is running in.</value>
        public ICakeRuntime Runtime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeEnvironment" /> class.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <param name="runtime">The runtime.</param>
        public CakeEnvironment(ICakePlatform platform, ICakeRuntime runtime)
        {
            Platform = platform;
            Runtime = runtime;

            // Get the application root.
            var assembly = AssemblyHelper.GetExecutingAssembly();
            var path = System.IO.Path.GetDirectoryName(assembly.Location);
            ApplicationRoot = new DirectoryPath(path);

            // Get the working directory.
            WorkingDirectory = new DirectoryPath(System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Gets a special path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A <see cref="DirectoryPath" /> to the special path.
        /// </returns>
        public DirectoryPath GetSpecialPath(SpecialPath path)
        {
            return SpecialPathHelper.GetFolderPath(Platform, path);
        }

        /// <summary>
        /// Gets an environment variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns>
        /// The value of the environment variable.
        /// </returns>
        public string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        /// <summary>
        /// Gets all environment variables.
        /// </summary>
        /// <returns>The environment variables as IDictionary&lt;string, string&gt; </returns>
        public IDictionary<string, string> GetEnvironmentVariables()
        {
            return Environment.GetEnvironmentVariables()
                .Cast<System.Collections.DictionaryEntry>()
                .ToDictionary(
                    key => (string)key.Key,
                    value => value.Value as string,
                    StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets whether or not the current operative system is 64 bit.
        /// </summary>
        /// <returns>
        /// Whether or not the current operative system is 64 bit.
        /// </returns>
        [Obsolete("Please use CakeEnvironment.Platform.Is64Bit instead.")]
        public bool Is64BitOperativeSystem()
        {
            return Platform.Is64Bit;
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>
        /// Whether or not the current machine is running Unix.
        /// </returns>
        [Obsolete("Please use CakeEnvironment.Platform.IsUnix instead.")]
        public bool IsUnix()
        {
            return Platform.IsUnix();
        }

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <returns>
        /// The application root path.
        /// </returns>
        [Obsolete("Please use CakeEnvironment.ApplicationRoot instead.")]
        public DirectoryPath GetApplicationRoot()
        {
            return ApplicationRoot;
        }

        /// <summary>
        /// Gets the target .Net framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        [Obsolete("Please use CakeEnvironment.Runtime.TargetFramework instead.")]
        public FrameworkName GetTargetFramework()
        {
            return Runtime.TargetFramework;
        }

        private static void SetWorkingDirectory(DirectoryPath path)
        {
            if (path.IsRelative)
            {
                throw new CakeException("Working directory can not be set to a relative path.");
            }
            System.IO.Directory.SetCurrentDirectory(path.FullPath);
        }
    }
}