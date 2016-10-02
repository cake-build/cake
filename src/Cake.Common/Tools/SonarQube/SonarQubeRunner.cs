using System.Collections.Generic;
using Cake.Common.Tools.MSBuild;
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
        private readonly MSBuildRunner _internalRunner;

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
            _internalRunner = new MSBuildRunner(fileSystem, environment, runner, tools);
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
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected sealed override IEnumerable<FilePath> GetAlternativeToolPaths(SonarQubeSettings settings)
        {
            return new[] { new FilePath(@"C:\ProgramData\chocolatey\lib\msbuild-sonarqube-runner\tools") };
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The tool name.</returns>
        protected override string GetToolName()
        {
            return "SonarQube";
        }

        private ProcessArgumentBuilder GetBeginArguments(SonarQubeSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append($"begin /d:sonar.host.url={settings.HostUrl} /d:sonar.login={settings.Login} /k:\"{settings.ProjectKey}\" /n:\"{settings.ProjectName}\" /v:\"{settings.ProjectVersion}\"");
            if (settings.Password != null)
            {
                builder.Append($" /d:sonar.password={settings.Password}");
            }

            return builder;
        }

        private ProcessArgumentBuilder GetEndArguments()
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("end");

            return builder;
        }

        internal void Run(FilePath solutionPath, SonarQubeSettings settings)
        {
            // Begin
            Run(settings, GetBeginArguments(settings));

            // Compile
            var msBuildSettings = new MSBuildSettings();
            msBuildSettings.Targets.Add("Rebuild");
            _internalRunner.Run(solutionPath, msBuildSettings);

            // End
            Run(settings, GetEndArguments());
        }
    }
}
