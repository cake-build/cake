using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.DotNet.Module
{
    /// <summary>
    /// Locates and lists contents of DotNet Packages.
    /// </summary>
    public class DotNetContentResolver : IDotNetContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetContentResolver"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The Globber.</param>
        /// <param name="log">The Log</param>
        public DotNetContentResolver(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _log = log;
        }

        /// <summary>
        /// Collects all the files for the given DotNet Package.
        /// </summary>
        /// <param name="package">The DotNet Package.</param>
        /// <param name="type">The type of DotNet Package.</param>
        /// <returns>All the files for the Package.</returns>
        public IReadOnlyCollection<IFile> GetFiles(PackageReference package, PackageType type)
        {
            if (type == PackageType.Addin)
            {
                throw new InvalidOperationException("DotNet Module does not support Addins'");
            }

            if (type == PackageType.Tool)
            {
                if(package.Parameters.ContainsKey("toolpath"))
                {
                    return GetToolFiles(new DirectoryPath(package.Parameters["toolpath"].First()), package);
                }
                else if(_environment.Platform.IsUnix())
                {
                    return GetToolFiles(new DirectoryPath(_environment.GetEnvironmentVariable("HOME")).Combine(".dotnet/tools"), package);
                }
                else
                {
                    return GetToolFiles(new DirectoryPath(_environment.GetEnvironmentVariable("USERPROFILE")).Combine(".dotnet/tools"), package);
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
