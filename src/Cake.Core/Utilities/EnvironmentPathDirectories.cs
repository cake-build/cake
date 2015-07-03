using System;
using Cake.Core.IO;

namespace Cake.Core.Utilities
{
    internal static class EnvironmentPathDirectories
    {
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
