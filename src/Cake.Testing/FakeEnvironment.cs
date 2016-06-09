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
        private readonly bool _isUnix;
        private readonly Dictionary<string, string> _environmentVariables;
        private readonly Dictionary<SpecialPath, DirectoryPath> _specialPaths;
        private DirectoryPath _applicationRoot;
        private bool _is64Bit;
        private FrameworkName _targetFramework;

        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        public DirectoryPath WorkingDirectory { get; set; }

        private FakeEnvironment(bool isUnix, bool is64Bit)
        {
            _isUnix = isUnix;
            _is64Bit = is64Bit;
            _environmentVariables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _specialPaths = new Dictionary<SpecialPath, DirectoryPath>();
        }

        /// <summary>
        /// Creates a Unix environment.
        /// </summary>
        /// <param name="is64Bit">if set to <c>true</c> the operating system is 64 bit.</param>
        /// <returns>A Unix environment.</returns>
        public static FakeEnvironment CreateUnixEnvironment(bool is64Bit = true)
        {
            var environment = new FakeEnvironment(true, is64Bit);
            environment.WorkingDirectory = new DirectoryPath("/Working");
            environment.SetApplicationRoot("/Working/bin");
            return environment;
        }

        /// <summary>
        /// Creates a Windows environment.
        /// </summary>
        /// <param name="is64Bit">if set to <c>true</c> the operating system is 64 bit.</param>
        /// <returns>A Windows environment.</returns>
        public static FakeEnvironment CreateWindowsEnvironment(bool is64Bit = true)
        {
            var environment = new FakeEnvironment(false, is64Bit);
            environment.WorkingDirectory = new DirectoryPath("C:/Working");
            environment.SetApplicationRoot("C:/Working/bin");
            return environment;
        }

        /// <summary>
        /// Gets whether or not the current operative system is 64 bit.
        /// </summary>
        /// <returns>Whether or not the current operative system is 64 bit.</returns>
        public bool Is64BitOperativeSystem()
        {
            return _is64Bit;
        }

        /// <summary>
        /// Changes the operative system bitness.
        /// </summary>
        /// <param name="is64Bit">if set to <c>true</c>, this is a 64-bit operative system.</param>
        public void ChangeOperativeSystemBitness(bool is64Bit)
        {
            _is64Bit = is64Bit;
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        public bool IsUnix()
        {
            return _isUnix;
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
        /// Sets a special path.
        /// </summary>
        /// <param name="kind">The special path kind.</param>
        /// <param name="path">The path.</param>
        public void SetSpecialPath(SpecialPath kind, DirectoryPath path)
        {
            _specialPaths[kind] = path;
        }

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <returns>
        /// The application root path.
        /// </returns>
        public DirectoryPath GetApplicationRoot()
        {
            return _applicationRoot;
        }

        /// <summary>
        /// Sets the application root path.
        /// </summary>
        /// <param name="applicationRoot">The application root path.</param>
        public void SetApplicationRoot(DirectoryPath applicationRoot)
        {
            _applicationRoot = applicationRoot;
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
        /// Sets an environment variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The value.</param>
        public void SetEnvironmentVariable(string variable, string value)
        {
            _environmentVariables[variable] = value;
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
        /// Gets the target .Net framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        public FrameworkName GetTargetFramework()
        {
            return _targetFramework;
        }

        /// <summary>
        /// Sets the target framework.
        /// </summary>
        /// <param name="targetFramework">The target framework.</param>
        public void SetTargetFramework(FrameworkName targetFramework)
        {
            _targetFramework = targetFramework;
        }
    }
}
