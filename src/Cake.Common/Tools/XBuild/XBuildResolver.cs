// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Diagnostics;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.XBuild
{
    internal static class XBuildResolver
    {
        private static ICakeEnvironment _environment;
        private static IFileSystem _fileSystem;

        public static FilePath GetXBuildPath(IFileSystem fileSystem, ICakeEnvironment environment, XBuildToolVersion version)
        {
            _environment = environment;
            _fileSystem = fileSystem;

            if (_environment.IsUnix())
            {
                return GetWhichXBuild();
            }
            else
            {
                return GetWindowsXBuild();
            }
        }

        private static FilePath GetWindowsXBuild()
        {
            var whereMono = GetWhereMono();

            if (whereMono != null)
            {
                return whereMono.GetDirectory().CombineWithFilePath("xbuild.bat");
            }
            else
            {
                var monoPath = GetMonoPathWindows();

                if (monoPath != null)
                {
                    var xbuild = monoPath.CombineWithFilePath("xbuild.bat");

                    if (_fileSystem.GetFile(xbuild).Exists)
                    {
                        return xbuild;
                    }
                }
            }

            return null;
        }

        private static FilePath GetWhichXBuild()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/which",
                Arguments = "xbuild",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            string which;
            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();
                which = process.StandardOutput.ReadToEnd();
            }

            return string.IsNullOrEmpty(which) ? null : new FilePath(which.Trim());
        }

        private static FilePath GetWhereMono()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "where",
                Arguments = "mono",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            string path;
            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();
                path = process.StandardOutput.ReadToEnd();
            }

            return string.IsNullOrEmpty(path) ? null : new FilePath(path.Trim());
        }

        private static DirectoryPath GetMonoPathWindows()
        {
            var programFiles = _environment.Is64BitOperativeSystem()
                ? Environment.SpecialFolder.ProgramFilesX86
                : Environment.SpecialFolder.ProgramFiles;

            var programFilesPath = new DirectoryPath(Environment.GetFolderPath(programFiles));
            var monoPath = programFilesPath.Combine("Mono").Combine("bin");

            return _fileSystem.GetDirectory(monoPath).Exists ? monoPath : null;
        }
    }
}
