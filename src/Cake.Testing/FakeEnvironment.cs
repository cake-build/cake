// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Versioning;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Testing
{
    /// <summary>
    /// Represents a fake environment.
    /// </summary>
    public sealed class FakeEnvironment : ICakeEnvironment
    {
        private readonly Dictionary<string, string> _environmentVariables;
        private readonly Dictionary<SpecialPath, DirectoryPath> _specialPaths;

        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        public DirectoryPath WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets the application root path.
        /// </summary>
        /// <value>The application root path.</value>
        public DirectoryPath ApplicationRoot { get; set; }

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        ICakePlatform ICakeEnvironment.Platform => Platform;

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        public FakePlatform Platform { get; }

        /// <summary>
        /// Gets the runtime Cake is running in.
        /// </summary>
        /// <value>The runtime Cake is running in.</value>
        ICakeRuntime ICakeEnvironment.Runtime => Runtime;

        /// <summary>
        /// Gets the runtime Cake is running in.
        /// </summary>
        /// <value>The runtime Cake is running in.</value>
        public FakeRuntime Runtime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeEnvironment"/> class.
        /// </summary>
        /// <param name="family">The platform family.</param>
        /// <param name="is64Bit">if set to <c>true</c>, the platform is 64 bit.</param>
        public FakeEnvironment(PlatformFamily family, bool is64Bit = true)
        {
            Platform = new FakePlatform(family, is64Bit);
            Runtime = new FakeRuntime();
            _environmentVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _specialPaths = new Dictionary<SpecialPath, DirectoryPath>();
        }

        /// <summary>
        /// Creates a Unix environment.
        /// </summary>
        /// <param name="is64Bit">if set to <c>true</c> the platform is 64 bit.</param>
        /// <returns>A Unix environment.</returns>
        public static FakeEnvironment CreateUnixEnvironment(bool is64Bit = true)
        {
            var environment = new FakeEnvironment(PlatformFamily.Linux, is64Bit);
            environment.WorkingDirectory = new DirectoryPath("/Working");
            environment.ApplicationRoot = "/Working/bin";
            return environment;
        }

        /// <summary>
        /// Creates a Windows environment.
        /// </summary>
        /// <param name="is64Bit">if set to <c>true</c> the platform is 64 bit.</param>
        /// <returns>A Windows environment.</returns>
        public static FakeEnvironment CreateWindowsEnvironment(bool is64Bit = true)
        {
            var environment = new FakeEnvironment(PlatformFamily.Windows, is64Bit);
            environment.WorkingDirectory = new DirectoryPath("C:/Working");
            environment.ApplicationRoot = "C:/Working/bin";
            return environment;
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
            if (_specialPaths.ContainsKey(path))
            {
                return _specialPaths[path];
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
            if (_environmentVariables.ContainsKey(variable))
            {
                return _environmentVariables[variable];
            }
            return null;
        }

        /// <summary>
        /// Gets all environment variables.
        /// </summary>
        /// <returns>The environment variables as IDictionary&lt;string, string&gt; </returns>
        public IDictionary<string, string> GetEnvironmentVariables()
        {
            return new Dictionary<string, string>(_environmentVariables, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Changes the operative system bitness.
        /// </summary>
        /// <param name="is64Bit">if set to <c>true</c>, this is a 64-bit operative system.</param>
        public void ChangeOperativeSystemBitness(bool is64Bit)
        {
            Platform.Is64Bit = is64Bit;
        }

        /// <summary>
        /// Sets a special path.
        /// </summary>
        /// <param name="kind">The special path kind.</param>
        /// <param name="path">The path.</param>
        public void SetSpecialPath(SpecialPath kind, DirectoryPath path)
        {
            _specialPaths[kind] = path;
        }

        /// <summary>
        /// Sets an environment variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The value.</param>
        public void SetEnvironmentVariable(string variable, string value)
        {
            _environmentVariables[variable] = value;
        }

        /// <summary>
        /// Sets the target framework.
        /// </summary>
        /// <param name="targetFramework">The target framework.</param>
        public void SetTargetFramework(FrameworkName targetFramework)
        {
            Runtime.TargetFramework = targetFramework;
        }

        /// <summary>
        /// Gets whether or not the current operative system is 64 bit.
        /// </summary>
        /// <returns>Whether or not the current operative system is 64 bit.</returns>
        [Obsolete("Please use FakeEnvironment.Platform.Is64Bit instead.")]
        public bool Is64BitOperativeSystem()
        {
            return Platform.Is64Bit;
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        [Obsolete("Please use FakeEnvironment.Platform.IsUnix instead.")]
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
        [Obsolete("Please use FakeEnvironment.ApplicationRoot instead.")]
        public DirectoryPath GetApplicationRoot()
        {
            return ApplicationRoot;
        }

        /// <summary>
        /// Gets the target .Net framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        [Obsolete("Please use FakeEnvironment.Runtime.TargetFramework instead.")]
        public FrameworkName GetTargetFramework()
        {
            return Runtime.TargetFramework;
        }
    }
}