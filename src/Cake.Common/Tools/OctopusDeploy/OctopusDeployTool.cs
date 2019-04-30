using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Base class for all octopus deploy related tools.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public class OctopusDeployTool<TSettings> : Tool<TSettings>
        where TSettings : OctopusDeployToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OctopusDeployTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            Environment = environment;
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        protected ICakeEnvironment Environment { get; }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Octo";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "Octo.exe", "dotnet-octo", "dotnet-octo.exe" };
        }
    }
}
