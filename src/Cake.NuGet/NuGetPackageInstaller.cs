using System;
using System.Collections.Generic;
using Cake.Core.Configuration;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    internal sealed class NuGetPackageInstaller : INuGetPackageInstaller
    {
        private readonly bool _useInProcessClient;
        private readonly InProcessInstaller _inProc;
        private readonly OutOfProcessInstaller _outProc;

        public NuGetPackageInstaller(
            ICakeConfiguration configuration,
            InProcessInstaller inProc,
            OutOfProcessInstaller outProc)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _useInProcessClient = UseInProcessClient(configuration);
            _inProc = inProc ?? throw new ArgumentNullException(nameof(inProc));
            _outProc = outProc ?? throw new ArgumentNullException(nameof(outProc));
        }

        private bool UseInProcessClient(ICakeConfiguration configuration)
        {
            var useInProcessClientString = configuration.GetValue(Constants.NuGet.UseInProcessClient) ?? bool.TrueString;
            if (!bool.TryParse(useInProcessClientString, out bool useInProcessClient))
            {
                // If there is no explicit preference, use the in process client.
                return true;
            }

            return useInProcessClient;
        }

        public bool CanInstall(PackageReference package, PackageType type)
        {
            if (_useInProcessClient)
            {
                return _inProc.CanInstall(package, type);
            }

            return _outProc.CanInstall(package, type);
        }

        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path)
        {
            if (_useInProcessClient)
            {
                return _inProc.Install(package, type, path);
            }

            return _outProc.Install(package, type, path);
        }
    }
}
