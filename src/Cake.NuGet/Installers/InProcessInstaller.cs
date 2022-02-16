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
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using PackageReference = Cake.Core.Packaging.PackageReference;
using PackageType = Cake.Core.Packaging.PackageType;

namespace Cake.NuGet
{
    internal sealed class InProcessInstaller : IDisposable
    {
        private static readonly ISet<string> _denyListPackages;

        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly INuGetContentResolver _contentResolver;
        private readonly ICakeLog _log;
        private readonly ICakeConfiguration _config;
        private readonly ISettings _nugetSettings;
        private readonly ILogger _nugetLogger;
        private readonly NuGetFramework _currentFramework;
        private readonly SourceCacheContext _sourceCacheContext;

        static InProcessInstaller()
        {
            // Set User Agent string
            UserAgent.SetUserAgentString(new UserAgentStringBuilder("Cake NuGet Client"));

            // Define packages we don't want to install
            _denyListPackages = new HashSet<string>(new[]
            {
                "Cake",
                "Cake.Common",
                "Cake.Core",
                "Cake.NuGet"
            }, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InProcessInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="contentResolver">The content resolver.</param>
        /// <param name="log">The log.</param>
        /// <param name="config">the configuration.</param>
        public InProcessInstaller(
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

            var nugetConfig = GetNuGetConfigPath(_environment, _config, _fileSystem);
            var nugetConfigDirectoryPath = nugetConfig.Item1;
            var nugetConfigFilePath = nugetConfig.Item2;

            _log.Debug(nugetConfigFilePath != null
                ? $"Found NuGet Config at: {nugetConfigDirectoryPath}/{nugetConfigFilePath}"
                : "NuGet Config not specified. Will use NuGet default mechanism for resolving it.");

            _nugetSettings = Settings.LoadDefaultSettings(
                nugetConfigDirectoryPath.FullPath,
                nugetConfigFilePath?.GetFilename().ToString(),
                new XPlatMachineWideSetting());
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
            var sourceRepositoryProvider = new NuGetSourceRepositoryProvider(_nugetSettings, _config, package, packageRoot);

            var localAndPrimaryRepositories = new HashSet<SourceRepository>(new NuGetSourceRepositoryComparer());
            localAndPrimaryRepositories.AddRange(sourceRepositoryProvider.LocalRepositories);
            localAndPrimaryRepositories.AddRange(sourceRepositoryProvider.PrimaryRepositories);

            var allRepositories = new HashSet<SourceRepository>(new NuGetSourceRepositoryComparer());
            allRepositories.AddRange(localAndPrimaryRepositories);
            allRepositories.AddRange(sourceRepositoryProvider.Repositories);

            var packageIdentity = GetPackageId(package, localAndPrimaryRepositories, targetFramework, _sourceCacheContext, _nugetLogger);
            if (packageIdentity == null)
            {
                _log.Debug("No package identity returned.");
                return Array.Empty<IFile>();
            }

            if (packageIdentity.Version.IsPrerelease && !package.IsPrerelease())
            {
                // If a prerelease version is explicitly specified, we should install that with or without prerelease flag.
                _log.Debug("Prerelease version string explicitly specified. Installing prerelease package version.");
            }

            var pathResolver = new PackagePathResolver(packageRoot);
            var dependencyBehavior = GetDependencyBehavior(type, package);
            var downloadContext = new PackageDownloadContext(_sourceCacheContext);

            var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);
            GetPackageDependencies(packageIdentity,
                targetFramework,
                _sourceCacheContext,
                _nugetLogger,
                allRepositories,
                availablePackages,
                dependencyBehavior,
                localAndPrimaryRepositories);

            var resolverContext = new PackageResolverContext(
                dependencyBehavior,
                new[] { packageIdentity.Id },
                Enumerable.Empty<string>(),
                Enumerable.Empty<global::NuGet.Packaging.PackageReference>(),
                Enumerable.Empty<PackageIdentity>(),
                availablePackages,
                allRepositories.Select(s => s.PackageSource),
                NullLogger.Instance);

            var resolver = new PackageResolver();
            var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
                .Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p))).ToArray();
            if (packagesToInstall.Length == 0)
            {
                _log.Debug("No packages to install after running package resolver.");
            }

            var packageExtractionContext = new PackageExtractionContext(
                PackageSaveMode.Nuspec | PackageSaveMode.Files | PackageSaveMode.Nupkg,
                XmlDocFileSaveMode.None,
                ClientPolicyContext.GetClientPolicy(_nugetSettings, _nugetLogger),
                _nugetLogger);

            var installedFiles = new List<IFile>();
            foreach (var packageToInstall in packagesToInstall)
            {
                var isTargetPackage = packageToInstall.Id.Equals(package.Package, StringComparison.OrdinalIgnoreCase);
                var installPath = new DirectoryPath(pathResolver.GetInstallPath(packageToInstall));
                if (!_fileSystem.Exist(installPath))
                {
                    var downloadResource = packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None).GetAwaiter().GetResult();
                    var downloadResult = downloadResource.GetDownloadResourceResultAsync(
                        packageToInstall,
                        downloadContext,
                        SettingsUtility.GetGlobalPackagesFolder(_nugetSettings),
                        _nugetLogger, CancellationToken.None).GetAwaiter().GetResult();

                    PackageExtractor.ExtractPackageAsync(
                        downloadResult.PackageSource,
                        downloadResult.PackageStream,
                        pathResolver,
                        packageExtractionContext,
                        CancellationToken.None).GetAwaiter().GetResult();

                    // If this is the target package, to avoid problems with casing, get the actual install path from the nuspec
                    if (isTargetPackage)
                    {
                        installPath = new DirectoryPath(pathResolver.GetInstallPath(downloadResult.PackageReader.GetIdentity()));
                    }
                }

                if (_denyListPackages.Contains(packageToInstall.Id))
                {
                    const string format = "Package {0} depends on package {1}. This dependency won't be loaded.";
                    _log.Debug(format, package.Package, packageToInstall.ToString());
                    continue;
                }

                // If the installed package is not the target package, create a new PackageReference
                // which is passed to the content resolver. This makes logging make more sense.
                var installedPackageReference = isTargetPackage ? package : new PackageReference($"nuget:?package={packageToInstall.Id}");
                var assemblies = _contentResolver.GetFiles(installPath, installedPackageReference, type);
                if (assemblies.Count == 0)
                {
                    _log.Debug("No assemblies found after running content resolver.");
                }
                installedFiles.AddRange(assemblies);
            }

            return installedFiles;
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

        private static PackageIdentity GetPackageId(PackageReference package, IEnumerable<SourceRepository> repositories, NuGetFramework targetFramework, SourceCacheContext sourceCacheContext, ILogger logger)
        {
            var version = GetNuGetVersion(package, repositories, targetFramework, sourceCacheContext, logger);
            return version == null ? null : new PackageIdentity(package.Package, version);
        }

        private static NuGetVersion GetNuGetVersion(PackageReference package, IEnumerable<SourceRepository> repositories, NuGetFramework targetFramework, SourceCacheContext sourceCacheContext, ILogger logger)
        {
            NuGetVersion version = null;
            VersionRange versionRange = null;
            if (package.Parameters.ContainsKey("version"))
            {
                var versionString = package.Parameters["version"].First();
                if (NuGetVersion.TryParse(versionString, out version))
                {
                    return version;
                }
                VersionRange.TryParse(versionString, out versionRange);
            }

            var includePrerelease = package.IsPrerelease();
            foreach (var sourceRepository in repositories)
            {
                try
                {
                    var dependencyInfoResource = sourceRepository.GetResourceAsync<DependencyInfoResource>().Result;
                    var dependencyInfo = dependencyInfoResource.ResolvePackages(package.Package, targetFramework, sourceCacheContext, logger, CancellationToken.None).Result;
                    var foundVersions = dependencyInfo
                        .Where(p => p.Listed && (includePrerelease || !p.Version.IsPrerelease))
                        .OrderByDescending(p => p.Version, VersionComparer.Default)
                        .Select(p => p.Version);
                    var foundVersion = versionRange != null ? versionRange.FindBestMatch(foundVersions) : foundVersions.FirstOrDefault();

                    // Find the highest possible version
                    if (version == null || foundVersion > version)
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

        private static void GetPackageDependencies(PackageIdentity package,
            NuGetFramework framework,
            SourceCacheContext cacheContext,
            ILogger logger,
            IEnumerable<SourceRepository> repositories,
            ISet<SourcePackageDependencyInfo> availablePackages,
            DependencyBehavior dependencyBehavior,
            IEnumerable<SourceRepository> primaryRepositories = null)
        {
            if (availablePackages.Contains(package))
            {
                return;
            }

            foreach (var sourceRepository in primaryRepositories ?? repositories)
            {
                var dependencyInfoResource = sourceRepository.GetResourceAsync<DependencyInfoResource>().GetAwaiter().GetResult();
                var dependencyInfo = dependencyInfoResource.ResolvePackage(package, framework, cacheContext, logger, CancellationToken.None).GetAwaiter().GetResult();

                if (dependencyInfo == null)
                {
                    continue;
                }

                availablePackages.Add(dependencyInfo);

                // No need to resolve dependencies if the dependency behavior is ignore.
                if (dependencyBehavior == DependencyBehavior.Ignore)
                {
                    return;
                }

                foreach (var dependency in dependencyInfo.Dependencies)
                {
                    GetPackageDependencies(
                        new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion),
                        framework, cacheContext, logger, repositories, availablePackages, dependencyBehavior);
                }

                // If we have already found package we can return here. No need to enumerate the other sources.
                return;
            }
        }

        private static Tuple<DirectoryPath, FilePath> GetNuGetConfigPath(ICakeEnvironment environment, ICakeConfiguration config, IFileSystem fileSystem)
        {
            DirectoryPath rootPath;
            FilePath filePath;

            var nugetConfigFile = config.GetValue(Constants.NuGet.ConfigFile);
            if (!string.IsNullOrEmpty(nugetConfigFile))
            {
                var configFilePath = new FilePath(nugetConfigFile).MakeAbsolute(environment);

                if (!fileSystem.Exist(configFilePath))
                {
                    throw new System.IO.FileNotFoundException("NuGet Config file not found.", configFilePath.FullPath);
                }

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