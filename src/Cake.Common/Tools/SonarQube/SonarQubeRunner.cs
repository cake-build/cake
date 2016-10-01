using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.SonarQube
{
    /// <summary>
    /// SonarQube analysis runner
    /// </summary>
    public sealed class SonarQubeRunner : Tool<SonarQubeSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SonarQubeRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="runner">The runner.</param>
        /// <param name="tools">The tool locator.</param>
        public SonarQubeRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner runner,
            IToolLocator tools) : base(fileSystem, environment, runner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "MSBuild.SonarQube.Runner.exe" };
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The tool name.</returns>
        protected override string GetToolName()
        {
            return "SonarQube";
        }

        private ProcessArgumentBuilder GetArguments(FilePath solution, SonarQubeSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            return builder;
        }

        internal void Run(FilePath solution, SonarQubeSettings settings)
        {
            Run(settings, GetArguments(solution, settings));
        }
    }
}
