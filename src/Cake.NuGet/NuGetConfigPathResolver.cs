// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.IO;

namespace Cake.NuGet
{
    internal static class NuGetConfigPathResolver
    {
        internal static Tuple<DirectoryPath, FilePath> GetPath(ICakeEnvironment environment, ICakeConfiguration config, IFileSystem fileSystem)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(config);
            ArgumentNullException.ThrowIfNull(fileSystem);

            DirectoryPath rootPath;
            FilePath filePath;

            var nugetConfigFile = config.GetValue(Constants.NuGet.ConfigFile);
            if (!string.IsNullOrEmpty(nugetConfigFile))
            {
                var configFilePath = new FilePath(nugetConfigFile).MakeAbsolute(environment);

                if (!fileSystem.Exist(configFilePath))
                {
                    throw new System.IO.FileNotFoundException("NuGet Config file not found.", configFilePath.FullPath);
                }

                rootPath = configFilePath.GetDirectory();
                filePath = configFilePath.GetFilename();
            }
            else
            {
                rootPath = environment.WorkingDirectory.MakeAbsolute(environment);
                filePath = null;
            }

            return Tuple.Create(rootPath, filePath);
        }
    }
}
