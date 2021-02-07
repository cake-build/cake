// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using PackageReference = Cake.Core.Packaging.PackageReference;

namespace Cake.NuGet
{
    internal sealed class NuGetSourceRepositoryProvider
    {
        private readonly List<Lazy<INuGetResourceProvider>> _resourceProviders;
        private readonly ISet<SourceRepository> _repositories;
        private readonly ISet<SourceRepository> _primaryRepositories;
        private readonly ISet<SourceRepository> _localRepositories;

        public NuGetSourceRepositoryProvider(ISettings settings, ICakeConfiguration config, PackageReference package, string packagesRoot)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (package is null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            // Create the default v3 resource provider
            _resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            _resourceProviders.AddRange(Repository.Provider.GetCoreV3());

            // Add repositories
            var sourceComparer = new NuGetSourceRepositoryComparer();
            _repositories = new HashSet<SourceRepository>(sourceComparer);
            _primaryRepositories = new HashSet<SourceRepository>(sourceComparer);
            _localRepositories = new HashSet<SourceRepository>(sourceComparer);
            _localRepositories.Add(CreateRepository(packagesRoot));
            _localRepositories.Add(CreateRepository(SettingsUtility.GetGlobalPackagesFolder(settings)));
            _localRepositories.AddRange(SettingsUtility.GetFallbackPackageFolders(settings).Select(CreateRepository));

            var packageSources = new PackageSourceProvider(settings).LoadPackageSources().ToList();

            if (package.Address != null)
            {
                var repository = GetOrCreateRepository(package.Address.AbsoluteUri);

                // Sources specified in directive is always primary.
                _repositories.Add(repository);
                _primaryRepositories.Add(repository);
            }

            var nugetSources = config.GetValue(Constants.NuGet.Source);
            if (!string.IsNullOrEmpty(nugetSources))
            {
                foreach (var nugetSource in nugetSources.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(nugetSource))
                    {
                        var repository = GetOrCreateRepository(nugetSource);
                        _repositories.Add(repository);

                        // If source is not specified in directive, add it as primary source.
                        if (package.Address == null)
                        {
                            _primaryRepositories.Add(repository);
                        }
                    }
                }
            }
            else
            {
                // Only add sources added via NuGet.config if nuget_source configuration value is not specified.
                foreach (var source in packageSources)
                {
                    if (source.IsEnabled)
                    {
                        var repository = CreateRepository(source);
                        _repositories.Add(repository);

                        // If source is not specified in directive, add it as primary source.
                        if (package.Address == null)
                        {
                            _primaryRepositories.Add(repository);
                        }
                    }
                }
            }

            SourceRepository GetOrCreateRepository(string source)
            {
                var packageSource = packageSources
                    .Where(p => p.IsEnabled)
                    .FirstOrDefault(p => p.Source.Equals(source, StringComparison.OrdinalIgnoreCase));

                return packageSource == null ?
                    CreateRepository(source) :
                    CreateRepository(packageSource);
            }
        }

        public IEnumerable<SourceRepository> PrimaryRepositories => _primaryRepositories;

        public IEnumerable<SourceRepository> LocalRepositories => _localRepositories;

        public IEnumerable<SourceRepository> Repositories => _repositories;

        private SourceRepository CreateRepository(string source)
        {
            return CreateRepository(new PackageSource(source));
        }

        private SourceRepository CreateRepository(PackageSource source)
        {
            return CreateRepository(source, FeedType.Undefined);
        }

        private SourceRepository CreateRepository(PackageSource source, FeedType type)
        {
            return new SourceRepository(source, _resourceProviders, type);
        }
    }
}
