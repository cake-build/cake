using System;
using Cake.Core.IO;

namespace Cake.Core.Utilities
{
    /// <summary>
    /// Contains utilities to search for files in environment path directories.
    /// </summary>
    public static class EnvironmentPathDirectories
    {
        /// <summary>
        /// Finds the specified file in one of the directories listed in the PATH environment variable
        /// </summary>
        /// <returns>The file path, or null if no file was found.</returns>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="fileName">The file name to find.</param>
        public static FilePath FindFile(IFileSystem fileSystem, ICakeEnvironment environment, FilePath fileName)
        {            
            if (fileSystem.Exist(fileName))
            {
                return fileName;
            }

            var values = environment.GetEnvironmentVariable("PATH");
            var separator = environment.IsUnix() ? ':' : ';';

            foreach (var path in values.Split(separator))
            {
                var dir = new DirectoryPath(path);

                var fullPath = dir.CombineWithFilePath(fileName);
                if (fileSystem.Exist(fullPath))
                {
                    return fullPath;
                }
            }

            return null;
        }
    }
}
