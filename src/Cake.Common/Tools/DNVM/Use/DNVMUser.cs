using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DNVM.Use
{
    /// <summary>
    /// DNVM use tool
    /// </summary>
    public class DNVMUser : DNVMTool<DNVMSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DNVMUser" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public DNVMUser(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
        }

        /// <summary>
        /// Run the Use command of the dnvm command
        /// </summary>
        /// <param name="version">The version to be used in dnvm</param>
        /// <param name="settings">The settings to be used</param>
        public void Use(string version, DNVMSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (string.IsNullOrEmpty(version))
            {
                throw new ArgumentNullException("version");
            }

            Run(settings, GetArguments(version, settings));
        }

        private ProcessArgumentBuilder GetArguments(string version, DNVMSettings settings)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("use");

            arguments.Append(version);

            if (settings.Architecture != null)
            {
                arguments.Append("-a");
                arguments.Append(settings.Architecture.Value.ToString());
            }

            if (settings.Runtime != null)
            {
                arguments.Append("-r");
                arguments.Append(settings.Runtime.Value.ToString());
            }

            return arguments;
        }
    }
}
