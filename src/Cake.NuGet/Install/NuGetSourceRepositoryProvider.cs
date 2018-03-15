// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.Packaging;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Cake.NuGet.Install
{
    internal sealed class NuGetSourceRepositoryProvider : ISourceRepositoryProvider
    {
        private readonly List<Lazy<INuGetResourceProvider>> _resourceProviders;
        private readonly ISet<SourceRepository> _repositories;

        public NuGetSourceRepositoryProvider(ISettings settings, ICakeConfiguration config, PackageReference package)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            // Create the package source provider (needed primarily to get default sources)
            PackageSourceProvider = new PackageSourceProvider(settings);

            // Create the default v3 resource provider
            _resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            _resourceProviders.AddRange(Repository.Provider.GetCoreV3());

            // Add repositories
            _repositories = new HashSet<SourceRepository>(new SourceRepositoryComparer());

            if (package.Address != null)
            {
                CreateRepository(package.Address.AbsoluteUri);
            }

            var nugetSources = config.GetValue(Constants.NuGet.Source);
            if (!string.IsNullOrEmpty(nugetSources))
            {
                foreach (var nugetSource in nugetSources.Split(';'))
                {
                    if (!string.IsNullOrWhiteSpace(nugetSource))
                    {
                        CreateRepository(nugetSource);
                    }
                }
            }

            foreach (var source in PackageSourceProvider.LoadPackageSources())
            {
                if (source.IsEnabled)
                {
                    CreateRepository(source);
                }
            }
        }

        public IEnumerable<SourceRepository> GetRepositories() => _repositories;

        public SourceRepository CreateRepository(string source) => CreateRepository(new PackageSource(source));

        public SourceRepository CreateRepository(PackageSource source) => CreateRepository(source, FeedType.Undefined);

        public SourceRepository CreateRepository(PackageSource source, FeedType type)
        {
            var repository = new SourceRepository(source, _resourceProviders, type);
            var comparer = new SourceRepositoryComparer();

            if (_repositories.Contains(repository))
            {
                return _repositories.First(x => comparer.Equals(x, repository));
            }

            _repositories.Add(repository);
            return repository;
        }

        public IPackageSourceProvider PackageSourceProvider { get; }
    }
}
