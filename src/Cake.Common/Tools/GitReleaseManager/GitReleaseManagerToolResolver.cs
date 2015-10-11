using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.GitReleaseManager
{
    /// <summary>
    /// Contains GitReleaseManager path resolver functionality
    /// </summary>
    public sealed class GitReleaseManagerToolResolver : IGitReleaseManagerToolResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private IFile _cachedPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitReleaseManagerToolResolver" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        public GitReleaseManagerToolResolver(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber)
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
        /// Resolves the path to GitReleaseManager.exe.
        /// </summary>
        /// <returns>The path to GitReleaseManager.exe.</returns>
        public FilePath ResolvePath()
        {
            // Check if path already resolved
            if (_cachedPath != null && _cachedPath.Exists)
            {
                return _cachedPath.Path;
            }

            // Check if tool exists in tool folder
            const string expression = "./tools/**/GitReleaseManager.exe";
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

            // Last resort try path
            var envPath = _environment.GetEnvironmentVariable("path");
            if (!string.IsNullOrWhiteSpace(envPath))
            {
                var pathFile = envPath
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(path => _fileSystem.GetDirectory(path))
                    .Where(path => path.Exists)
                    .Select(path => path.Path.CombineWithFilePath("GitReleaseManager.exe"))
                    .Select(_fileSystem.GetFile)
                    .FirstOrDefault(file => file.Exists);

                if (pathFile != null)
                {
                    _cachedPath = pathFile;
                    return _cachedPath.Path;
                }
            }

            throw new CakeException("Could not locate GitReleaseManager.exe.");
        }
    }
}