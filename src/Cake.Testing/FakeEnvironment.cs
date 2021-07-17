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
        private readonly Dictionary<string, string> _commandLineArguments;
        private readonly Dictionary<SpecialPath, DirectoryPath> _specialPaths;

        /// <inheritdoc/>
        public DirectoryPath WorkingDirectory { get; set; }

        /// <inheritdoc/>
        public DirectoryPath ApplicationRoot { get; set; }

        /// <inheritdoc/>
        ICakePlatform ICakeEnvironment.Platform => Platform;

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        public FakePlatform Platform { get; }

        /// <inheritdoc/>
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
            _commandLineArguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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

        /// <inheritdoc/>
        public DirectoryPath GetSpecialPath(SpecialPath path)
        {
            if (_specialPaths.ContainsKey(path))
            {
                return _specialPaths[path];
            }
            const string format = "The special path '{0}' is not supported.";
            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, format, path));
        }

        /// <inheritdoc/>
        public string GetEnvironmentVariable(string variable)
        {
            if (_environmentVariables.ContainsKey(variable))
            {
                return _environmentVariables[variable];
            }
            return null;
        }

        /// <inheritdoc/>
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
        /// Change the operating system platform family.
        /// </summary>
        /// <param name="family">The platform family.</param>
        public void ChangeOperatingSystemFamily(PlatformFamily family)
        {
            Platform.Family = family;
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
        /// Sets a command line argument.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="value">The value.</param>
        public void SetCommandLineArgument(string argument, string value)
        {
            _commandLineArguments[argument] = value;
        }

        /// <summary>
        /// Sets the built framework.
        /// </summary>
        /// <param name="builtFramework">The target framework.</param>
        public void SetBuiltFramework(FrameworkName builtFramework)
        {
            Runtime.BuiltFramework = builtFramework;
        }

        /// <summary>
        /// Sets if is .NET Core CLR.
        /// </summary>
        /// <param name="isCoreClr">if set to <c>true</c> the runtime is .NET Core.</param>
        public void SetIsCoreClr(bool isCoreClr)
        {
            Runtime.IsCoreClr = isCoreClr;
        }
    }
}