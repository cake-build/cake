using System;
using System.Diagnostics;
using System.Linq;
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
            var which = string.Empty;

            var startInfo = new ProcessStartInfo
            {
                FileName = "/usr/bin/which",
                Arguments = "xbuild",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (var proc = new Process { StartInfo = startInfo })
            {
                proc.Start();
                which = proc.StandardOutput.ReadToEnd();
            }

            return string.IsNullOrEmpty(which) ? null : new FilePath(which.Trim());
        }

        private static FilePath GetWhereMono()
        {
            var where = string.Empty;

            var startInfo = new ProcessStartInfo
            {
                FileName = "where",
                Arguments = "mono",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (var proc = new Process { StartInfo = startInfo })
            {
                proc.Start();
                where = proc.StandardOutput.ReadToEnd();
            }

            return string.IsNullOrEmpty(where) ? null : new FilePath(where.Trim());
        }

        private static DirectoryPath GetMonoPathWindows()
        {            
            DirectoryPath programFiles;

            if (_environment.Is64BitOperativeSystem())
            {
                programFiles = new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            }
            else
            {
                programFiles = new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            }
             
            var monoPath = programFiles.Combine("Mono").Combine("bin");

            if (_fileSystem.GetDirectory(monoPath).Exists)
            {
                return monoPath;
            }

            return null;
        }
    }
}
