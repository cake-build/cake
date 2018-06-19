// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.NuGet.Install.Extensions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using PackageReference = Cake.Core.Packaging.PackageReference;
using PackageType = Cake.Core.Packaging.PackageType;

namespace Cake.NuGet.Install
{
    internal sealed class NuGetPackageInstaller : INuGetPackageInstaller, IDisposable
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly INuGetContentResolver _contentResolver;
        private readonly ICakeLog _log;
        private readonly ICakeConfiguration _config;
        private readonly ISettings _nugetSettings;
        private readonly ILogger _nugetLogger;
        private readonly NuGetFramework _currentFramework;
        private readonly GatherCache _gatherCache;
        private readonly SourceCacheContext _sourceCacheContext;

        static NuGetPackageInstaller()
        {
            // Set User Agent string
            UserAgent.SetUserAgentString(new UserAgentStringBuilder("Cake NuGet Client"));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="contentResolver">The content resolver.</param>
        /// <param name="log">The log.</param>
        /// <param name="config">the configuration</param>
        public NuGetPackageInstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            INuGetContentResolver contentResolver,
            ICakeLog log,
            ICakeConfiguration config)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _contentResolver = contentResolver ?? throw new ArgumentNullException(nameof(contentResolver));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _currentFramework = NuGetFramework.Parse(_environment.Runtime.BuiltFramework.FullName, DefaultFrameworkNameProvider.Instance);
            _nugetLogger = new NuGetLogger(_log);

            var nugetConfig = GetNuGetConfigPath(_environment, _config);
            var nugetConfigDirectoryPath = nugetConfig.Item1;
            var nugetConfigFilePath = nugetConfig.Item2;

            _log.Debug(nugetConfigFilePath != null
                ? $"Found NuGet.config at: {nugetConfigFilePath}"
                : "NuGet.config not found.");

            _nugetSettings = Settings.LoadDefaultSettings(
                nugetConfigDirectoryPath.FullPath,
                nugetConfigFilePath?.GetFilename().ToString(),
                new XPlatMachineWideSetting());
            _gatherCache = new GatherCache();
            _sourceCacheContext = new SourceCacheContext();
        }

        public bool CanInstall(PackageReference package, PackageType type)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }
            return package.Scheme.Equals("nuget", StringComparison.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var packageRoot = path.MakeAbsolute(_environment).FullPath;
            var targetFramework = type == PackageType.Addin ? _currentFramework : NuGetFramework.AnyFramework;
            var sourceRepositoryProvider = new NuGetSourceRepositoryProvider(_nugetSettings, _config, package);
            var pathResolver = new PackagePathResolver(packageRoot);
            var project = new NugetFolderProject(_fileSystem, _contentResolver, _log, pathResolver, packageRoot, targetFramework);
            var packageManager = new NuGetPackageManager(sourceRepositoryProvider, _nugetSettings, project.Root)
            {
                PackagesFolderNuGetProject = project
            };

            var packageIdentity = GetPackageId(package, packageManager, sourceRepositoryProvider, targetFramework, _sourceCacheContext, _nugetLogger);
            if (packageIdentity == null)
            {
                return Array.Empty<IFile>();
            }

            var includePrerelease = package.IsPrerelease();
            var dependencyBehavior = GetDependencyBehavior(type, package);
            var projectContext = new NuGetProjectContext(_log);
            var resolutionContext = new ResolutionContext(dependencyBehavior, includePrerelease, false, VersionConstraints.None, _gatherCache, _sourceCacheContext);
            var downloadContext = new PackageDownloadContext(_sourceCacheContext);

            // First get the install actions.
            // This will give us the list of packages to install, and which feed should be used.
            var actions = packageManager.GetInstallProjectActionsAsync(
                project,
                packageIdentity,
                resolutionContext,
                projectContext,
                sourceRepositoryProvider.GetPrimaryRepositories(),
                sourceRepositoryProvider.GetRepositories(),
                CancellationToken.None).Result;

            // Then install the packages.
            packageManager.ExecuteNuGetProjectActionsAsync(
                project,
                actions,
                projectContext,
                downloadContext,
                CancellationToken.None).Wait();

            return project.GetFiles(path, package, type);
        }

        private DependencyBehavior GetDependencyBehavior(PackageType type, PackageReference package)
        {
            if (type == PackageType.Addin)
            {
                return package.ShouldLoadDependencies(_config) ?
                    DependencyBehavior.Lowest :
                    DependencyBehavior.Ignore;
            }
            return DependencyBehavior.Ignore;
        }

        private static string GetToolPath(ICakeEnvironment environment, ICakeConfiguration config)
        {
            var toolPath = config.GetValue(Constants.Paths.Tools);
            return !string.IsNullOrWhiteSpace(toolPath) ?
                new DirectoryPath(toolPath).MakeAbsolute(environment).FullPath :
                environment.WorkingDirectory.Combine("tools").MakeAbsolute(environment).FullPath;
        }

        private static PackageIdentity GetPackageId(PackageReference package, NuGetPackageManager packageManager, NuGetSourceRepositoryProvider sourceRepositoryProvider, NuGetFramework targetFramework, SourceCacheContext sourceCacheContext, ILogger logger)
        {
            var version = GetNuGetVersion(package, packageManager, sourceRepositoryProvider, targetFramework, sourceCacheContext, logger);
            return version == null ? null : new PackageIdentity(package.Package, version);
        }

        private static NuGetVersion GetNuGetVersion(PackageReference package, NuGetPackageManager packageManager, NuGetSourceRepositoryProvider sourceRepositoryProvider, NuGetFramework targetFramework, SourceCacheContext sourceCacheContext, ILogger logger)
        {
            if (package.Parameters.ContainsKey("version"))
            {
                return new NuGetVersion(package.Parameters["version"].First());
            }

            // Only search for latest version in local and primary sources.
            var repositories = new HashSet<SourceRepository>(new SourceRepositoryComparer());
            repositories.Add(packageManager.PackagesFolderSourceRepository);
            repositories.AddRange(packageManager.GlobalPackageFolderRepositories);
            repositories.AddRange(sourceRepositoryProvider.GetPrimaryRepositories());

            var includePrerelease = package.IsPrerelease();
            NuGetVersion version = null;
            foreach (var sourceRepository in repositories)
            {
                try
                {
                    var dependencyInfoResource = sourceRepository.GetResourceAsync<DependencyInfoResource>().Result;
                    var dependencyInfo = dependencyInfoResource.ResolvePackages(package.Package, targetFramework, sourceCacheContext, logger, CancellationToken.None).Result;
                    var foundVersion = dependencyInfo
                        .Where(p => p.Listed && (includePrerelease || !p.Version.IsPrerelease))
                        .OrderByDescending(p => p.Version, VersionComparer.Default)
                        .Select(p => p.Version)
                        .FirstOrDefault();

                    // Find the highest possible version
                    version = version ?? foundVersion;
                    if (foundVersion != null && foundVersion > version)
                    {
                        version = foundVersion;
                    }
                }
                catch (AggregateException ae)
                {
                    ae.Handle(e =>
                    {
                        logger.LogWarning(e.Message);
                        return true;
                    });
                }
            }
            return version;
        }

        private static Tuple<DirectoryPath, FilePath> GetNuGetConfigPath(ICakeEnvironment environment, ICakeConfiguration config)
        {
            DirectoryPath rootPath;
            FilePath filePath;

            var nugetConfigFile = config.GetValue(Constants.NuGet.ConfigFile);
            if (!string.IsNullOrEmpty(nugetConfigFile))
            {
                var configFilePath = new FilePath(nugetConfigFile);

                rootPath = configFilePath.GetDirectory();
                filePath = configFilePath.GetFilename();
            }
            else
            {
                rootPath = GetToolPath(environment, config);
                filePath = null;
            }

            return Tuple.Create(rootPath, filePath);
        }

        public void Dispose()
        {
            _sourceCacheContext?.Dispose();
        }
    }
}