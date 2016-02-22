using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.Tools.DNU;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DNX.Run
{
    /// <summary>
    /// Run the dnx tool
    /// </summary>
    public sealed class DNXRunner : DNXTool<DNXRunSettings>
    {
        private readonly ICakeEnvironment environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DNXRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public DNXRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            this.environment = environment;
        }

        /// <summary>
        /// Run the dnx tool
        /// </summary>
        /// <param name="directoryPath">The directory path to the project which contains the command to be executed</param>
        /// <param name="command">The command to be executed</param>
        /// <param name="settings">The settings to be used</param>
        public void Run(DirectoryPath directoryPath, string command, DNXRunSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException("command");
            }
            if (directoryPath == null)
            {
                throw new ArgumentNullException("directoryPath");
            }

            ProcessSettings processSettings = new ProcessSettings();
            processSettings.WorkingDirectory = directoryPath.MakeAbsolute(environment).FullPath;
            Run(settings, GetArguments(command, settings), processSettings, null);
        }

        private ProcessArgumentBuilder GetArguments(string command, DNXRunSettings settings)
        {
            var arguments = new ProcessArgumentBuilder();

            GetDNVMArgumentsForRun(arguments, settings);

            if (settings.Project != null)
            {
                arguments.Append("--project");
                arguments.AppendQuoted(settings.Project.FullPath);
            }

            if (settings.AppBase != null)
            {
                arguments.Append("--appbase");
                arguments.AppendQuoted(settings.AppBase.FullPath);
            }

            if (settings.Lib != null && settings.Lib.Count != 0)
            {
                arguments.Append("--lib");
                arguments.AppendQuoted(string.Join(";", settings.Lib.Select(p => p.FullPath)));
            }

            if (!string.IsNullOrEmpty(settings.Framework))
            {
                arguments.Append("--framework");
                arguments.AppendQuoted(settings.Framework);
            }

            if (!string.IsNullOrEmpty(settings.Configuration))
            {
                arguments.Append("--configuration");
                arguments.AppendQuoted(settings.Configuration);
            }

            if (settings.Packages != null)
            {
                arguments.Append("--packages");
                arguments.AppendQuoted(settings.Packages.FullPath);
            }

            arguments.Append(command);

            return arguments;
        }
    }
}
