using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DNU
{
    /// <summary>
    /// Base class for all DNU related tools
    /// </summary>
    /// <typeparam name="TSettings">The settings type</typeparam>
    public abstract class DNUTool<TSettings> : DNToolBase<TSettings>
        where TSettings : DNSettingsBase
    {
        private ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DNUTool{TSettings}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        protected DNUTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "DNU";
        }

        /// <summary>
        /// Gets the DNVM arguments to execute run or exec command to work with dnu tool
        /// </summary>
        /// <param name="builder">The argument builder to be used</param>
        /// <param name="settings">The settings to be used</param>
        /// <param name="command">The dnvm command to be called</param>
        protected override void GetDNVMArguments(ProcessArgumentBuilder builder, TSettings settings, string command)
        {
            base.GetDNVMArguments(builder, settings, command);

            if (_environment.IsUnix())
            {
                builder.Append("dnu");
            }
            else
            {
                builder.Append("dnu.cmd");
            }
        }
    }
}