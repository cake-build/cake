// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// Provides file location for wildcard style content inclusions in project files.
    /// <example>
    /// i.e., &lt;Compile Include=&quot;**\*.cs&quot; /&gt;</example>
    /// </summary>
    internal sealed class ProjectFileResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectFileResolver"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public ProjectFileResolver(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Gets the collection of <see cref="FilePath"/> matching the pattern
        /// </summary>
        /// <param name="projectRoot">The project root.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns>Returns the list of <see cref="FilePath"/> matching the specified pattern</returns>
        public IEnumerable<FilePath> GetFiles(DirectoryPath projectRoot, string pattern)
        {
            if (projectRoot == null)
            {
                throw new ArgumentNullException(nameof(projectRoot));
            }
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pattern));
            }

            if (projectRoot.IsRelative)
            {
                projectRoot = projectRoot.MakeAbsolute(_environment);
            }

            return new Globber(_fileSystem, new ScopedCakeEnvironment(_environment, projectRoot)).GetFiles(pattern);
        }

        /// <summary>
        /// Provides an implementation of <see cref="ICakeEnvironment"/> with a fixed <seealso cref="WorkingDirectory"/> value
        /// </summary>
        /// <seealso cref="Cake.Core.ICakeEnvironment" />
        private sealed class ScopedCakeEnvironment : ICakeEnvironment
        {
            private readonly ICakeEnvironment _environment;
            private readonly DirectoryPath _workingDirectory;

            /// <summary>
            /// Initializes a new instance of the <see cref="ScopedCakeEnvironment"/> class.
            /// </summary>
            /// <param name="environment">The environment.</param>
            /// <param name="workingDirectory">The working directory.</param>
            /// <exception cref="System.ArgumentNullException">
            /// environment
            /// or
            /// workingDirectory
            /// </exception>
            /// <exception cref="CakeException">Working directory can not be set to a relative path.</exception>
            public ScopedCakeEnvironment(ICakeEnvironment environment, DirectoryPath workingDirectory)
            {
                if (environment == null)
                {
                    throw new ArgumentNullException(nameof(environment));
                }
                if (workingDirectory == null)
                {
                    throw new ArgumentNullException(nameof(workingDirectory));
                }
                if (workingDirectory.IsRelative)
                {
                    throw new CakeException("Working directory can not be set to a relative path.");
                }

                _environment = environment;
                _workingDirectory = workingDirectory;
            }

            /// <inheritdoc />
            public DirectoryPath WorkingDirectory
            {
                get { return _workingDirectory; }
                set { throw new NotSupportedException($"Cannot set Working Directory in {typeof(ScopedCakeEnvironment).Name}"); }
            }

            /// <inheritdoc />
            public DirectoryPath ApplicationRoot => _environment.ApplicationRoot;

            /// <inheritdoc />
            public DirectoryPath GetSpecialPath(SpecialPath path)
            {
                return _environment.GetSpecialPath(path);
            }

            /// <inheritdoc />
            public string GetEnvironmentVariable(string variable)
            {
                return _environment.GetEnvironmentVariable(variable);
            }

            /// <inheritdoc />
            public IDictionary<string, string> GetEnvironmentVariables()
            {
                return _environment.GetEnvironmentVariables();
            }

            /// <inheritdoc />
            public ICakePlatform Platform => _environment.Platform;

            /// <inheritdoc />
            public ICakeRuntime Runtime => _environment.Runtime;

            /// <inheritdoc />
            public bool Is64BitOperativeSystem()
            {
                return this.Platform.Is64Bit;
            }

            /// <inheritdoc />
            public bool IsUnix()
            {
                return _environment.Platform.IsUnix();
            }

            /// <inheritdoc />
            public DirectoryPath GetApplicationRoot()
            {
                return this.ApplicationRoot;
            }

            /// <inheritdoc />
            public FrameworkName GetTargetFramework()
            {
                return this.Runtime.TargetFramework;
            }
        }
    }
}