using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// The Octopus Deploy release creator runner
    /// </summary>
    public sealed class OctopusDeployReleaseCreator : Tool<CreateReleaseSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        /// <summary> 
        /// Initializes a new instance of the <see cref="OctopusDeployReleaseCreator"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public OctopusDeployReleaseCreator(IFileSystem fileSystem, ICakeEnvironment environment,
            IGlobber globber, IProcessRunner processRunner) 
            : base(fileSystem, environment, processRunner)
        {
            _environment = environment;
            _globber = globber;
        }

        /// <summary>
        /// Creates a release for the specified project in OctopusDeploy
        /// </summary>
        /// <param name="projectName">The target project name</param>
        /// <param name="settings">The settings</param>
        public void CreateRelease(string projectName, CreateReleaseSettings settings)
        {
            if (projectName == null)
            {
                throw new ArgumentNullException("projectName");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (string.IsNullOrEmpty(settings.Server))
            {
                throw new ArgumentNullException("server");
            }
            if (string.IsNullOrEmpty(settings.ApiKey))
            {
                throw new ArgumentNullException("apiKey");
            }

            var argumentBuilder = new CreateReleaseArgumentBuilder(projectName, settings, _environment);
            Run(settings, argumentBuilder.Get(), settings.ToolPath);
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Octo";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(CreateReleaseSettings settings)
        {
            return _globber.GetFiles("./tools/**/Octo.exe").FirstOrDefault();
        }
    }
}