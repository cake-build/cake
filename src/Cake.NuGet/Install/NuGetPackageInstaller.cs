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
    internal sealed class NuGetPackageInstaller : INuGetPackageInstaller
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
            _currentFramework = NuGetFramework.Parse(_environment.Runtime.TargetFramework.FullName, DefaultFrameworkNameProvider.Instance);
            _nugetLogger = new NuGetLogger(_log);
            _nugetSettings = Settings.LoadDefaultSettings(
                GetToolPath(),
                null,
                new XPlatMachineWideSetting());
            _gatherCache = new GatherCache();
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
            sourceRepositoryProvider.CreateRepository(packageRoot);
            var packageIdentity = GetPackageId(package, sourceRepositoryProvider, targetFramework, _nugetLogger);
            if (packageIdentity == null)
            {
                return Array.Empty<IFile>();
            }

            var pathResolver = new PackagePathResolver(packageRoot);
            var project = new NugetFolderProject(_fileSystem, _contentResolver, _config, _log, pathResolver, packageRoot, targetFramework);
            var packageManager = new NuGetPackageManager(sourceRepositoryProvider, _nugetSettings, project.Root)
            {
                PackagesFolderNuGetProject = project
            };
            var sourceRepositories = sourceRepositoryProvider.GetRepositories();
            var includePrerelease = package.IsPrerelease();
            var dependencyBehavior = GetDependencyBehavior(type, package);
            var resolutionContext = new ResolutionContext(dependencyBehavior, includePrerelease, false, VersionConstraints.None, _gatherCache);
            var projectContext = new NuGetProjectContext(_log);

            packageManager.InstallPackageAsync(project, packageIdentity, resolutionContext, projectContext,
                sourceRepositories, Array.Empty<SourceRepository>(),
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

        private string GetToolPath()
        {
            var toolPath = _config.GetValue(Constants.Paths.Tools);
            return !string.IsNullOrWhiteSpace(toolPath) ?
                new DirectoryPath(toolPath).MakeAbsolute(_environment).FullPath :
                _environment.WorkingDirectory.Combine("tools").MakeAbsolute(_environment).FullPath;
        }

        private static PackageIdentity GetPackageId(PackageReference package, NuGetSourceRepositoryProvider sourceRepositoryProvider, NuGetFramework targetFramework, ILogger logger)
        {
            var version = GetNuGetVersion(package, sourceRepositoryProvider, targetFramework, logger);
            return version == null ? null : new PackageIdentity(package.Package, version);
        }

        private static NuGetVersion GetNuGetVersion(PackageReference package, NuGetSourceRepositoryProvider sourceRepositoryProvider, NuGetFramework targetFramework, ILogger logger)
        {
            if (package.Parameters.ContainsKey("version"))
            {
                return new NuGetVersion(package.Parameters["version"].First());
            }

            var includePrerelease = package.IsPrerelease();
            foreach (var sourceRepository in sourceRepositoryProvider.GetRepositories())
            {
                var dependencyInfoResource = sourceRepository.GetResourceAsync<DependencyInfoResource>().Result;
                var dependencyInfo = dependencyInfoResource.ResolvePackages(package.Package, targetFramework, logger, CancellationToken.None).Result;
                var version = dependencyInfo
                    .Where(p => p.Listed && (includePrerelease || !p.Version.IsPrerelease))
                    .OrderByDescending(p => p.Version, VersionComparer.Default)
                    .Select(p => p.Version)
                    .FirstOrDefault();

                if (version != null)
                {
                    return version;
                }
            }
            return null;
        }
    }
}
