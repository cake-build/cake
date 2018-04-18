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
            IEnumerable<SourceRepository> primarySourceRepositories,
            IEnumerable<SourceRepository> secondarySourceRepositories,
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
            if (primarySourceRepositories == null)
            {
                throw new ArgumentNullException(nameof(primarySourceRepositories));
            }
            if (!primarySourceRepositories.Any())
            {
                throw new ArgumentException(nameof(primarySourceRepositories));
            }
            if (secondarySourceRepositories == null)
            {
                throw new ArgumentNullException(nameof(secondarySourceRepositories));
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
                var sourceRepository = await GetSourceRepositoryAsync(packageIdentity, localSources, resolutionContext.SourceCacheContext, logger, token);
                if (sourceRepository == null)
                {
                    // Else, check provided sources. We use only primary sources when we don't care about dependencies.
                    sourceRepository = await GetSourceRepositoryAsync(packageIdentity, primarySourceRepositories, resolutionContext.SourceCacheContext, logger, token);
                }

                // If still not found, we just throw an exception.
                if (sourceRepository == null)
                {
                    throw new InvalidOperationException($"Unable to find version '{packageIdentity.Version}' of package '{packageIdentity.Id}'.");
                }

                return new[] { NuGetProjectAction.CreateInstallProjectAction(packageIdentity, sourceRepository, nuGetProject) };
            }

            var primarySources = new HashSet<SourceRepository>(new SourceRepositoryComparer());
            var allSources = new HashSet<SourceRepository>(new SourceRepositoryComparer());

            Tuple<IReadOnlyCollection<PackageIdentity>, IReadOnlyCollection<SourcePackageDependencyInfo>> packageAndDependencies = null;
            var allDependenciesResolved = false;

            // If target package is already installed, there's a slight chance that all packages are already installed.
            if (await PackageExistsInSourceRepository(
                packageIdentity,
                packageManager.PackagesFolderSourceRepository,
                resolutionContext.SourceCacheContext,
                new ProjectContextLogger(nuGetProjectContext),
                token))
            {
                primarySources.Add(packageManager.PackagesFolderSourceRepository);
                allSources.Add(packageManager.PackagesFolderSourceRepository);

                try
                {
                    // Try get all dependencies from local sources only.
                    packageAndDependencies = await GetPackageAndDependenciesToInstall(
                        packageManager,
                        nuGetProject,
                        packageIdentity,
                        resolutionContext,
                        nuGetProjectContext,
                        primarySources,
                        allSources,
                        token);

                    allDependenciesResolved = true;
                }
                catch (NuGetResolverConstraintException)
                {
                    // We didn't manage to find all dependencies in the local packages folder.
                    allDependenciesResolved = false;
                }
            }

            // If we didn't manage to resolve all dependencies from the local sources, check in all sources.
            if (!allDependenciesResolved)
            {
                primarySources.AddRange(localSources);
                primarySources.AddRange(primarySourceRepositories);
                allSources.AddRange(primarySources);
                allSources.AddRange(secondarySourceRepositories);

                packageAndDependencies = await GetPackageAndDependenciesToInstall(
                        packageManager,
                        nuGetProject,
                        packageIdentity,
                        resolutionContext,
                        nuGetProjectContext,
                        primarySources,
                        allSources,
                        token);
            }

            // Create the install actions.
            return GetProjectActions(
                packageIdentity,
                packageAndDependencies.Item1,
                packageAndDependencies.Item2,
                nuGetProject);
        }

        private static async Task<SourceRepository> GetSourceRepositoryAsync(
            PackageIdentity packageIdentity,
            IEnumerable<SourceRepository> sourceRepositories,
            SourceCacheContext sourceCacheContext,
            ILogger logger,
            CancellationToken token)
        {
            foreach (var sourceRepository in sourceRepositories)
            {
                if (await PackageExistsInSourceRepository(packageIdentity, sourceRepository, sourceCacheContext, logger, token))
                {
                    return sourceRepository;
                }
            }

            return null;
        }

        private static async Task<bool> PackageExistsInSourceRepository(
            PackageIdentity packageIdentity,
            SourceRepository sourceRepository,
            SourceCacheContext sourceCacheContext,
            ILogger logger,
            CancellationToken token)
        {
            var metadataResource = await sourceRepository.GetResourceAsync<MetadataResource>(token);

            if (metadataResource == null)
            {
                return false;
            }

            return await metadataResource.Exists(packageIdentity, sourceCacheContext, logger, token);
        }

        private static async Task<Tuple<IReadOnlyCollection<PackageIdentity>, IReadOnlyCollection<SourcePackageDependencyInfo>>> GetPackageAndDependenciesToInstall(
            NuGetPackageManager packageManager,
            NugetFolderProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            ISet<SourceRepository> primarySources,
            ISet<SourceRepository> allSources,
            CancellationToken token)
        {
            // Get the available package dependencies.
            var packageDependencies = await GetAvailablePackageDependencies(
                packageManager,
                nuGetProject,
                packageIdentity,
                resolutionContext,
                nuGetProjectContext,
                primarySources,
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

            return Tuple.Create(packagesToInstall, packageDependencies);
        }

        private static async Task<IReadOnlyCollection<SourcePackageDependencyInfo>> GetAvailablePackageDependencies(
            NuGetPackageManager packageManager,
            NugetFolderProject nuGetProject,
            PackageIdentity packageIdentity,
            ResolutionContext resolutionContext,
            INuGetProjectContext nuGetProjectContext,
            ISet<SourceRepository> primarySources,
            ISet<SourceRepository> allSources,
            CancellationToken token)
        {
            var targetFramework = nuGetProject.GetMetadata<NuGetFramework>(NuGetProjectMetadataKeys.TargetFramework);
            var primaryPackages = new List<PackageIdentity> { packageIdentity };
            var gatherContext = new GatherContext()
            {
                InstalledPackages = Array.Empty<PackageIdentity>(),
                PrimaryTargets = primaryPackages,
                TargetFramework = targetFramework,
                PrimarySources = primarySources.ToList(),
                AllSources = allSources.ToList(),
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