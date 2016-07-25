// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using Cake.Core.IO;

namespace Cake.Core
{
    /// <summary>
    /// Represents the environment Cake operates in.
    /// </summary>
    public sealed class CakeEnvironment : ICakeEnvironment
    {
        private readonly ICakePlatform _platform;
        private readonly ICakeRuntime _runtime;
        private readonly DirectoryPath _applicationRoot;

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
        public DirectoryPath ApplicationRoot
        {
            get { return _applicationRoot; }
        }

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        public ICakePlatform Platform
        {
            get { return _platform; }
        }

        /// <summary>
        /// Gets the runtime Cake is running in.
        /// </summary>
        /// <value>The runtime Cake is running in.</value>
        public ICakeRuntime Runtime
        {
            get { return _runtime; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeEnvironment" /> class.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <param name="runtime">The runtime.</param>
        public CakeEnvironment(ICakePlatform platform, ICakeRuntime runtime)
        {
            _platform = platform;
            _runtime = runtime;
            _applicationRoot = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
            switch (path)
            {
                case SpecialPath.ApplicationData:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                case SpecialPath.CommonApplicationData:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                case SpecialPath.LocalApplicationData:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                case SpecialPath.ProgramFiles:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                case SpecialPath.ProgramFilesX86:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
                case SpecialPath.Windows:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
                case SpecialPath.LocalTemp:
                    return new DirectoryPath(System.IO.Path.GetTempPath());
            }
            const string format = "The special path '{0}' is not supported.";
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, format, path));
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
            return _platform.Is64Bit;
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
            return _platform.IsUnix();
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
            return _applicationRoot;
        }

        /// <summary>
        /// Gets the target .Net framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        [Obsolete("Please use CakeEnvironment.Runtime.TargetFramework instead.")]
        public FrameworkName GetTargetFramework()
        {
            return _runtime.TargetFramework;
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