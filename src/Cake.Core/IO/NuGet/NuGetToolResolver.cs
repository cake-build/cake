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

        private static readonly FilePath[] _unixSystemPaths;

        static NuGetToolResolver()
        {
            _unixSystemPaths = new[]
            {
                new FilePath("/usr/local/bin/nuget"),
                new FilePath("/usr/bin/nuget")
            };
        }

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
        /// Resolves the path to nuget.exe.
        /// </summary>
        /// <returns>The path to nuget.exe.</returns>
        public FilePath ResolvePath()
        {
            // Check if path already resolved
            if (_cachedPath != null && _cachedPath.Exists)
            {
                return _cachedPath.Path;
            }

            // Check if tool exists in tool folder
            const string expression = "./tools/**/NuGet.exe";
            var toolsExe = _globber.GetFiles(expression).FirstOrDefault();
            if (toolsExe != null)
            {
                var toolsFile = _fileSystem.GetFile(toolsExe);
                if (toolsFile.Exists)
                {
                    _cachedPath = toolsFile;
                    return _cachedPath.Path;
                }
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

            // On Unix /usr/bin/nuget or /usr/local/bin/nuget are viable options
            if (_environment.IsUnix())
            {
                foreach (var systemPath in _unixSystemPaths)
                {
                    if (_fileSystem.Exist(systemPath))
                    {
                        _cachedPath = _fileSystem.GetFile(systemPath);
                        return _cachedPath.Path;
                    }
                }
            }

            // Last resort try path
            var envPath = _environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrWhiteSpace(envPath))
            {
                var pathFile = envPath
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(path => _fileSystem.GetDirectory(path))
                    .Where(path => path.Exists)
                    .Select(path => path.Path.CombineWithFilePath("nuget.exe"))
                    .Select(_fileSystem.GetFile)
                    .FirstOrDefault(file => file.Exists);

                if (pathFile != null)
                {
                    _cachedPath = pathFile;
                    return _cachedPath.Path;
                }
            }

            throw new CakeException("Could not locate nuget.exe.");
        }
    }
}