// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.Configuration;
using Cake.Core.IO;
using Cake.Core.Polyfill;

namespace Cake.Core.Reflection
{
    /// <summary>
    /// Responsible for loading assemblies.
    /// </summary>
    public sealed class AssemblyLoader : IAssemblyLoader
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly IAssemblyVerifier _verifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="verifier">The assembly verifier.</param>
        public AssemblyLoader(ICakeEnvironment environment, IFileSystem fileSystem, IAssemblyVerifier verifier)
        {
            _environment = environment;
            _fileSystem = fileSystem;
            _verifier = verifier;
        }

        /// <summary>
        /// Loads an assembly from its assembly name.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>The loaded assembly.</returns>
        public Assembly Load(AssemblyName assemblyName)
        {
            return AssemblyHelper.LoadAssembly(assemblyName);
        }

        /// <summary>
        /// Loads an assembly from the specified path.
        /// </summary>
        /// <param name="path">The assembly path to load.</param>
        /// <param name="verify">If the assembly should be verified whether or not it will work properly with Cake or not.</param>
        /// <returns>The loaded assembly.</returns>
        public Assembly Load(FilePath path, bool verify)
        {
            var assembly = AssemblyHelper.LoadAssembly(_environment, _fileSystem, path);
            if (verify && assembly != null)
            {
                _verifier.Verify(assembly);
            }
            return assembly;
        }
    }
}