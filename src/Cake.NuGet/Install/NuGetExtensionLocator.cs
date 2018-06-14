using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Cake.Core;
using Cake.Core.Configuration;

using NuGet.Configuration;

namespace Cake.NuGet.Install
{
    /// <summary>
    /// Provides a common facility for locating extensions
    /// </summary>
    internal sealed class NuGetExtensionLocator : IExtensionLocator // shamelessly copied from nuget source repo
    {
        private readonly ICakeConfiguration _config;
        private readonly ICakeEnvironment _environment;

        private static readonly string _extensionsDirectoryRoot =
            Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
                "NuGet",
                "Commands");

        private static readonly string _credentialProvidersDirectoryRoot =
            Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
                "NuGet",
                "CredentialProviders");

        private static readonly string CredentialProviderPattern = "CredentialProvider*.exe";

        /// <summary>
        /// PATH Enviroment variable name for path to nuget extensions
        /// </summary>
        public static readonly string ExtensionsEnvar = "NUGET_EXTENSIONS_PATH";

        /// <summary>
        /// PATH Enviroment variable name for path to nuget credentials providers
        /// </summary>
        public static readonly string CredentialProvidersEnvar = "NUGET_CREDENTIALPROVIDERS_PATH";

        public NuGetExtensionLocator(ICakeEnvironment environment, ICakeConfiguration config)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <inheritdoc/>
        public IEnumerable<string> FindCredentialProviders()
        {
            var customPaths = ReadPathsFromEnvar(CredentialProvidersEnvar);
            return FindAll(
                _credentialProvidersDirectoryRoot,
                customPaths,
                CredentialProviderPattern,
                CredentialProviderPattern
                );
        }

        /// <inheritdoc/>
        public IEnumerable<string> FindExtensions()
        {
            var customPaths = ReadPathsFromEnvar(ExtensionsEnvar);
            return FindAll(
                _extensionsDirectoryRoot,
                customPaths,
                "*.dll",
                "*Extensions.dll"
                );
        }

        private IEnumerable<string> FindAll(
            string globalRootDirectory,
            IEnumerable<string> customPaths,
            string assemblyPattern,
            string nugetDirectoryAssemblyPattern)
        {
            var directories = new List<string>();

            // Add all directories from the environment variable if available.
            directories.AddRange(customPaths);

            // add the global root
            directories.Add(globalRootDirectory);

            var paths = new List<string>();
            foreach (var directory in directories.Where(Directory.Exists))
            {
                paths.AddRange(Directory.EnumerateFiles(directory, assemblyPattern, SearchOption.AllDirectories));
            }

            // not sure if the working directory is an appropriate fallback for locating the tools/folder nuget/folder
            var nugetDirectory = Path.GetDirectoryName(_config.GetToolPath(_environment.WorkingDirectory, _environment).FullPath); // directory for locating extensions and credentials providers, such as VSTS CredentialProvider
            if (nugetDirectory == null)
            {
                return paths;
            }

            paths.AddRange(Directory.EnumerateFiles(nugetDirectory, nugetDirectoryAssemblyPattern));

            return paths;
        }

        private static IEnumerable<string> ReadPathsFromEnvar(string key)
        {
            var result = new List<string>();
            var paths = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(paths))
            {
                result.AddRange(
                    paths.Split(new[] { ';' },
                    StringSplitOptions.RemoveEmptyEntries));
            }
            return result;
        }
    }
}
