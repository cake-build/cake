using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.MSTest
{
    /// <summary>
    /// The MSTest unit test runner.
    /// </summary>
    public sealed class MSTestRunner : Tool<MSTestSettings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSTestRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param> 
        public MSTestRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _fileSystem = fileSystem;
            _environment = environment;
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

            Run(settings, GetArguments(assemblyPath, settings), settings.ToolPath);
        }

        private ProcessArgumentBuilder GetArguments(FilePath assemblyPath, MSTestSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assembly to build.
            builder.Append(string.Concat("/testcontainer:", assemblyPath.MakeAbsolute(_environment).FullPath).Quote());

            if (settings.NoIsolation)
            {
                builder.AppendQuoted("/noisolation");
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The tool name.</returns>
        protected override string GetToolName()
        {
            return "MSTest";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(MSTestSettings settings)
        {
            foreach (var version in new[] { "14.0", "12.0", "11.0", "10.0" })
            {
                var path = GetToolPath(version);
                if (_fileSystem.Exist(path))
                {
                    yield return path;
                }
            }
        }

        private FilePath GetToolPath(string version)
        {
            var programFiles = _environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
            var root = programFiles.Combine(string.Concat("Microsoft Visual Studio ", version, "/Common7/IDE"));
            return root.CombineWithFilePath("mstest.exe");
        }
    }
}
