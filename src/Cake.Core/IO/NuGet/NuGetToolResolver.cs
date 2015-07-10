using System;
using System.Linq;

namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Contains NuGet path resolver functionality
    /// </summary>
    public sealed class NuGetToolResolver : INuGetToolResolver
    {        
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private IFile _cachedPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetToolResolver" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        public NuGetToolResolver(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (globber == null)
            {
                throw new ArgumentNullException("globber");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
        }

        /// <summary>
        /// Gets the tool name.
        /// </summary>
        /// <value>The tool name.</value>
        public string Name
        {
            get { return "NuGet"; }
        }

        /// <summary>
        /// Resolves the tool path.
        /// </summary>
        /// <returns>
        /// The tool path.
        /// </returns>
        /// <exception cref="CakeException">No nuget.exe found by resolver.</exception>
        public FilePath ResolveToolPath()
        {
            // Check if path allready resolved
            if (_cachedPath != null && _cachedPath.Exists)
            {
                return _cachedPath.Path;
            }

            // Check if path set to environment variable
            var environmentExe = _environment.GetEnvironmentVariable("NUGET_EXE");
            if (!string.IsNullOrWhiteSpace(environmentExe))
            {
                var envFile = _fileSystem.GetFile(environmentExe);
                if (envFile.Exists)
                {
                    _cachedPath = envFile;
                    return _cachedPath.Path;
                }
            }

            // On Unix /usr/bin/nuget is a viable option
            if (_environment.IsUnix())
            {
                var nugetUnixPath = new FilePath("/usr/bin/nuget");

                if (_fileSystem.Exist(nugetUnixPath))
                {
                    _cachedPath = _fileSystem.GetFile(nugetUnixPath);
                    return _cachedPath.Path;
                }
            }

            throw new CakeException("Could not locate nuget.");
        }
    }
}
