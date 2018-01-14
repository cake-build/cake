using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;

namespace Cake.NuGet.Install.Extensions
{
    internal static class NuGetPackageManagerExtensions
    {
        public static async Task<IEnumerable<NuGetProjectAction>> GetInstallProjectActionsAsync(
            this NuGetPackageManager packageManager,
            NugetFolderProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IEnumerable<SourceRepository> sources,
            CancellationToken token)
        {
            if (nuGetProject == null)
            {
                throw new ArgumentNullException(nameof(nuGetProject));
            }
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }
            if (resolutionContext == null)
            {
                throw new ArgumentNullException(nameof(resolutionContext));
            }
            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }
            if (!sources.Any())
            {
                throw new ArgumentException(nameof(sources));
            }
            if (packageIdentity.Version == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            var localSources = new HashSet<SourceRepository>(new SourceRepositoryComparer());
            localSources.Add(packageManager.PackagesFolderSourceRepository);
            localSources.AddRange(packageManager.GlobalPackageFolderRepositories);

            if (resolutionContext.DependencyBehavior == DependencyBehavior.Ignore)
            {
                var logger = new ProjectContextLogger(nuGetProjectContext);

                // First, check only local sources.
                var sourceRepository = await GetSourceRepositoryAsync(packageIdentity, localSources, logger, token);
                if (sourceRepository == null)
                {
                    // Else, check provided sources.
                    sourceRepository = await GetSourceRepositoryAsync(packageIdentity, sources, logger, token);
                }

                // If still not found, we just throw an exception.
                if (sourceRepository == null)
                {
                    throw new InvalidOperationException($"Unable to find version '{packageIdentity.Version}' of package '{packageIdentity.Id}'.");
                }

                return new[] { NuGetProjectAction.CreateInstallProjectAction(packageIdentity, sourceRepository, nuGetProject) };
            }

            var allSources = new HashSet<SourceRepository>(new SourceRepositoryComparer());
            allSources.AddRange(localSources);
            allSources.AddRange(sources);

            // Get the available package dependencies.
            var packageDependencies = await GetAvailablePackageDependencies(
                packageManager,
                nuGetProject,
                packageIdentity,
                resolutionContext,
                nuGetProjectContext,
                allSources,
                token);

            if (!packageDependencies.Any())
            {
                throw new InvalidOperationException($"Unable to gather dependency information for package '{packageIdentity}'");
            }

            // Prune the results down to only what we would allow to be installed.
            packageDependencies = PruneAvailablePackageDependencies(packageIdentity, resolutionContext, packageDependencies);

            // Get the actual packages to install.
            var packagesToInstall = GetPackagesToInstall(
                packageIdentity,
                resolutionContext,
                nuGetProjectContext,
                packageDependencies,
                allSources,
                token);

            if (packagesToInstall == null || !packagesToInstall.Any())
            {
                throw new InvalidOperationException($"Unable to resolve dependencies for package '{packageIdentity}' with DependencyBehavior '{resolutionContext.DependencyBehavior}'");
            }

            // Create the install actions.
            return GetProjectActions(
                packageIdentity,
                packagesToInstall,
                packageDependencies,
                nuGetProject);
        }

        private static async Task<SourceRepository> GetSourceRepositoryAsync(
            PackageIdentity packageIdentity,
            IEnumerable<SourceRepository> sourceRepositories,
            ILogger logger,
            CancellationToken token)
        {
            foreach (var sourceRepository in sourceRepositories)
            {
                var metadataResource = await sourceRepository.GetResourceAsync<MetadataResource>(token);
                if (metadataResource != null)
                {
                    if (await metadataResource.Exists(packageIdentity, logger, token))
                    {
                        return sourceRepository;
                    }
                }
            }

            return null;
        }

        private static async Task<IReadOnlyCollection<SourcePackageDependencyInfo>> GetAvailablePackageDependencies(
            NuGetPackageManager packageManager,
            NugetFolderProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            ISet<SourceRepository> sources,
            CancellationToken token)
        {
            var targetFramework = nuGetProject.GetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework);
            var primaryPackages = new List<PackageIdentity> { packageIdentity };
            var gatherContext = new GatherContext()
            {
                InstalledPackages = Array.Empty<PackageIdentity>(),
                PrimaryTargets = primaryPackages,
                TargetFramework = targetFramework,
                PrimarySources = sources.ToList(),
                AllSources = sources.ToList(),
                PackagesFolderSource = packageManager.PackagesFolderSourceRepository,
                ResolutionContext = resolutionContext,
                AllowDowngrades = false,
                ProjectContext = nuGetProjectContext
            };

            return await ResolverGather.GatherAsync(gatherContext, token);
        }

        private static IReadOnlyCollection<SourcePackageDependencyInfo> PruneAvailablePackageDependencies(
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            IEnumerable<SourcePackageDependencyInfo> availablePackageDependencies)
        {
            var prunedAvailablePackages = PrunePackageTree.RemoveAllVersionsForIdExcept(
                availablePackageDependencies,
                packageIdentity);

            if (resolutionContext.IncludePrerelease)
            {
                return prunedAvailablePackages.ToList();
            }

            return PrunePackageTree.PrunePreleaseForStableTargets(
                    prunedAvailablePackages,
                    new[] { packageIdentity },
                    new[] { packageIdentity }).ToList();
        }

        private static IReadOnlyCollection<PackageIdentity> GetPackagesToInstall(
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            IReadOnlyCollection<SourcePackageDependencyInfo> availablePackages,
            ISet<SourceRepository> sources,
            CancellationToken token)
        {
            var packageResolverContext = new PackageResolverContext(
                resolutionContext.DependencyBehavior,
                new[] { packageIdentity.Id },
                Enumerable.Empty<string>(),
                Enumerable.Empty<PackageReference>(),
                Enumerable.Empty<PackageIdentity>(),
                availablePackages,
                sources.Select(s => s.PackageSource),
                new LoggerAdapter(nuGetProjectContext));

            var packageResolver = new PackageResolver();
            return packageResolver.Resolve(packageResolverContext, token)?.ToList();
        }

        private static IEnumerable<NuGetProjectAction> GetProjectActions(
            PackageIdentity packageIdentity,
            IReadOnlyCollection<PackageIdentity> packagesToInstall,
            IReadOnlyCollection<SourcePackageDependencyInfo> availablePackageDependencies,
            NugetFolderProject nuGetProject)
        {
            var nuGetProjectActions = new List<NuGetProjectAction>();
            foreach (var packageToInstall in packagesToInstall)
            {
                // Find the package match based on identity
                var sourceDepInfo = availablePackageDependencies.SingleOrDefault(p => PackageIdentity.Comparer.Equals(p, packageToInstall));

                if (sourceDepInfo == null)
                {
                    // This really should never happen since dependencies have been resolved.
                    throw new InvalidOperationException($"Package '{packageIdentity}' is not found");
                }

                nuGetProjectActions.Add(NuGetProjectAction.CreateInstallProjectAction(sourceDepInfo, sourceDepInfo.Source, nuGetProject));
            }

            return nuGetProjectActions;
        }
    }
}