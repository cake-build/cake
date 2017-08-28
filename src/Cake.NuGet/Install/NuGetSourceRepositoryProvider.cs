using System;
using System.Collections.Generic;
using Cake.Core.Configuration;
using Cake.Core.Packaging;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Cake.NuGet.Install
{
    internal sealed class NuGetSourceRepositoryProvider : ISourceRepositoryProvider
    {
        private readonly List<Lazy<INuGetResourceProvider>> _resourceProviders;
        private readonly List<SourceRepository> _repositories;

        public NuGetSourceRepositoryProvider(ISettings settings, ICakeConfiguration config, PackageReference package)
        {
            // Create the package source provider (needed primarily to get default sources)
            PackageSourceProvider = new PackageSourceProvider(settings);

            // Create the default v3 resource provider
            _resourceProviders = new List<Lazy<INuGetResourceProvider>>();
            _resourceProviders.AddRange(Repository.Provider.GetCoreV3());

            // Add repositories
            _repositories = new List<SourceRepository>();

            if (package.Address != null)
            {
                CreateRepository(package.Address.AbsoluteUri);
            }
            var nugetSource = config.GetValue(Constants.NuGet.Source);
            if (!string.IsNullOrWhiteSpace(nugetSource))
            {
                CreateRepository(nugetSource);
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
            var repository = new SourceRepository(source, _resourceProviders);
            _repositories.Add(repository);
            return repository;
        }

        public IPackageSourceProvider PackageSourceProvider { get; }
    }
}
