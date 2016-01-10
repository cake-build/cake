using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools
{
    /// <summary>
    /// Define a base class for all DN* tools
    /// Provide helper method to work with dnvm exec / run commands
    /// </summary>
    /// <typeparam name="TSettings">The tools settings type</typeparam>
    public abstract class DNToolBase<TSettings> : Tool<TSettings>
        where TSettings : DNSettingsBase
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DNToolBase{TSettings}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        protected DNToolBase(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            if (this._environment.IsUnix())
            {
                return new[] { "dnvmwrapper.sh" };
            }
            else
            {
                return new[] { "dnvm.cmd" };
            }
        }

        /// <summary>
        /// Add to the <paramref name="builder"/> the arguments to be used in dnvm exec command
        /// </summary>
        /// <param name="builder">The argument builder to be used to inject the parameters</param>
        /// <param name="settings">The settings to be used</param>
        protected void GetDNVMArgumentsForExec(ProcessArgumentBuilder builder, TSettings settings)
        {
            GetDNVMArguments(builder, settings, "exec");
        }

        /// <summary>
        /// Add to the <paramref name="builder"/> the arguments to be used in dnvm run command
        /// </summary>
        /// <param name="builder">The argument builder to be used to inject the parameters</param>
        /// <param name="settings">The settings to be used</param>
        protected void GetDNVMArgumentsForRun(ProcessArgumentBuilder builder, TSettings settings)
        {
            GetDNVMArguments(builder, settings, "run");
        }

        /// <summary>
        /// Gets the DNVM arguments to execute run or exec command
        /// </summary>
        /// <param name="builder">The argument builder to be used</param>
        /// <param name="settings">The settings to be used</param>
        /// <param name="command">The dnvm command to be called</param>
        protected virtual void GetDNVMArguments(ProcessArgumentBuilder builder, TSettings settings, string command)
        {
            if (string.IsNullOrEmpty(settings.Version))
            {
                throw new CakeException("The settings \"Version\" must be set");
            }

            // Add dnvm arguments to 
            builder.Append(command);
            builder.Append(settings.Version);

            if (settings.Architecture != null)
            {
                builder.Append("-a");
                builder.Append(settings.Architecture.Value.ToString());
            }

            if (settings.Runtime != null)
            {
                builder.Append("-r");
                builder.Append(settings.Runtime.Value.ToString());
            }
        }
    }
}
