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
        /// Initializes a new instance of the <see cref="CakeEnvironment"/> class.
        /// </summary>
        public CakeEnvironment()
        {
            WorkingDirectory = new DirectoryPath(System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Gets whether or not the current operative system is 64 bit.
        /// </summary>
        /// <returns>
        /// Whether or not the current operative system is 64 bit.
        /// </returns>
        public bool Is64BitOperativeSystem()
        {
            return Machine.Is64BitOperativeSystem();
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>
        /// Whether or not the current machine is running Unix.
        /// </returns>
        public bool IsUnix()
        {
            return Machine.IsUnix();
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
        /// Gets the application root path.
        /// </summary>
        /// <returns>
        /// The application root path.
        /// </returns>
        public DirectoryPath GetApplicationRoot()
        {
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new DirectoryPath(path);
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
        /// Gets the target .Net framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        public FrameworkName GetTargetFramework()
        {
            // Try to get the current framework name from the current application domain,
            // but if that is null, we default to .NET 4.5. The reason for doing this is
            // that this actually is what happens on Mono.
            var frameworkName = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
            return new FrameworkName(frameworkName ?? ".NETFramework,Version=v4.5");
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
