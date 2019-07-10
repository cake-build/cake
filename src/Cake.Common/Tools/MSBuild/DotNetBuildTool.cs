using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Base class for all dot net build related tools (MSBuild, dotnet core).
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class DotNetBuildTool<TSettings> : Tool<TSettings>
        where TSettings : DotNetSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetBuildTool{TSettings}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        protected DotNetBuildTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Runs the dotnet cli command using the specified settings and arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        protected void RunCommand(TSettings settings, ProcessArgumentBuilder arguments)
        {
            Run(settings, arguments, null, null);
        }

        /// <summary>
        /// Runs the dotnet cli command using the specified settings and arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="processSettings">The processSettings.</param>
        protected void RunCommand(TSettings settings, ProcessArgumentBuilder arguments, ProcessSettings processSettings)
        {
            Run(settings, arguments, processSettings, null);
        }

        /// <summary>
        /// Creates a <see cref="ProcessArgumentBuilder"/> and adds common commandline arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Instance of <see cref="ProcessArgumentBuilder"/>.</returns>
        protected ProcessArgumentBuilder CreateArgumentBuilder(TSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            return builder;
        }
    }
}
