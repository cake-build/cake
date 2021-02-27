using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.DotNetTool.Module
{
    /// <summary>
    /// Locates and lists contents of dotnet Tool Packages.
    /// </summary>
    public class DotNetToolContentResolver : IDotNetToolContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;

        private readonly ICakeConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetToolContentResolver"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The Globber.</param>
        /// <param name="log">The Log</param>
        /// <param name="config">the configuration</param>
        public DotNetToolContentResolver(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeLog log,
            ICakeConfiguration config)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _log = log;
            _config = config;
        }

        /// <summary>
        /// Collects all the files for the given dotnet Tool Package.
        /// </summary>
        /// <param name="package">The dotnet Tool Package.</param>
        /// <param name="type">The type of dotnet Tool Package.</param>
        /// <returns>All the files for the Package.</returns>
        public IReadOnlyCollection<IFile> GetFiles(PackageReference package, PackageType type)
        {
            if (type == PackageType.Addin)
            {
                throw new InvalidOperationException("DotNetTool Module does not support Addins'");
            }

            if (type == PackageType.Tool)
            {
                if(package.Parameters.ContainsKey("global"))
                {
                    if(_environment.Platform.IsUnix())
                    {
                        return GetToolFiles(new DirectoryPath(_environment.GetEnvironmentVariable("HOME")).Combine(".dotnet/tools"), package);
                    }
                    else
                    {
                        return GetToolFiles(new DirectoryPath(_environment.GetEnvironmentVariable("USERPROFILE")).Combine(".dotnet/tools"), package);
                    }
                }
                else
                {
                    return GetToolFiles(_config.GetToolPath(_environment.WorkingDirectory, _environment), package);
                }
            }

            throw new InvalidOperationException("Unknown resource type.");
        }

        private IReadOnlyCollection<IFile> GetToolFiles(DirectoryPath installationLocation, PackageReference package)
        {
            var result = new List<IFile>();
            var toolFolder = installationLocation.Combine(".store/" + package.Package.ToLowerInvariant());

            _log.Debug("Tool Folder: {0}", toolFolder);
            var toolDirectory = _fileSystem.GetDirectory(toolFolder);

            if (toolDirectory.Exists)
            {
                result.AddRange(GetFiles(toolFolder, package));
            }
            else
            {
                _log.Debug("Tool folder does not exist: {0}.", toolFolder);
            }

            _log.Debug("Found {0} files in tool folder", result.Count);
            return result;
        }

        private IEnumerable<IFile> GetFiles(DirectoryPath path, PackageReference package, string[] patterns = null)
        {
            var collection = new FilePathCollection(new PathComparer(_environment));

            // Get default files (dll).
            patterns = patterns ?? new[] { path.FullPath + "/**/*.dll" };
            foreach (var pattern in patterns)
            {
                collection.Add(_globber.GetFiles(pattern));
            }

            // Include files.
            if (package.Parameters.ContainsKey("include"))
            {
                foreach (var include in package.Parameters["include"])
                {
                    var includePath = string.Concat(path.FullPath, "/", include.TrimStart('/'));
                    collection.Add(_globber.GetFiles(includePath));
                }
            }

            // Exclude files.
            if (package.Parameters.ContainsKey("exclude"))
            {
                foreach (var exclude in package.Parameters["exclude"])
                {
                    var excludePath = string.Concat(path.FullPath, "/", exclude.TrimStart('/'));
                    collection.Remove(_globber.GetFiles(excludePath));
                }
            }

            // Return the files.
            return collection.Select(p => _fileSystem.GetFile(p)).ToArray();
        }
    }
}
