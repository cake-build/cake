using System;
using Cake.Core;
using Cake.Core.IO;

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
        public MSTestRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
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

        private ToolArgumentBuilder GetArguments(FilePath assemblyPath, MSTestSettings settings)
        {
            var builder = new ToolArgumentBuilder();

            // Add the assembly to build.
            builder.AppendText(string.Concat("/testcontainer:", assemblyPath.MakeAbsolute(_environment).FullPath).Quote());

            if (settings.NoIsolation)
            {
                builder.AppendQuotedText("/noisolation");
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
        /// Gets the default tool path.
        /// </summary>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(MSTestSettings settings)
        {
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
    }
}
