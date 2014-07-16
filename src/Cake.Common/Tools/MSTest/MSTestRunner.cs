using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSTest
{
    /// <summary>
    /// The MSTest unit test runner.
    /// </summary>
    public sealed class MSTestRunner
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        public MSTestRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _processRunner = processRunner;
        }

        /// <summary>
        /// Runs the tests in the specified assembly.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath assemblyPath, MSTestSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Find MSTest.exe.
            var toolPath = GetToolPath(settings);
            if (toolPath == null)
            {
                throw new CakeException("Could not find MSTest.exe.");   
            }

            // Get the process start info.
            var info = GetProcessStartInfo(toolPath, assemblyPath, settings);

            // Run the process.
            var process = _processRunner.Start(info);
            if (process == null)
            {
                throw new CakeException("MSTest process was not started.");
            }

            // Wait for the process to exit.
            process.WaitForExit();

            // Did an error occur?
            if (process.GetExitCode() != 0)
            {
                throw new CakeException("MSTest process returned failure.");
            }
        }

        private FilePath GetToolPath(MSTestSettings settings)
        {
            if (settings.ToolPath != null)
            {
                return settings.ToolPath.MakeAbsolute(_environment);
            }
            foreach (var version in new[] { "12.0", "11.0", "10.0" })
            {
                var path = GetToolPath(version);
                if (_fileSystem.Exist(path))
                {
                    return path;
                }
            }
            return null;
        }

        private FilePath GetToolPath(string version)
        {
            var programFiles = _environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
            var root = programFiles.Combine(string.Concat("Microsoft Visual Studio ", version, "/Common7/IDE"));
            return root.CombineWithFilePath("mstest.exe");
        }

        private ProcessStartInfo GetProcessStartInfo(FilePath toolPath, FilePath assemblyPath, MSTestSettings settings)
        {
            var parameters = new List<string>();

            // Add the assembly to build.
            parameters.Add(string.Concat("/testcontainer:", assemblyPath.MakeAbsolute(_environment).FullPath).Quote());

            if (settings.NoIsolation)
            {
                parameters.Add("/noisolation".Quote());
            }

            // Create the process start info.
            return new ProcessStartInfo(toolPath.FullPath)
            {
                WorkingDirectory = _environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
        }
    }
}
