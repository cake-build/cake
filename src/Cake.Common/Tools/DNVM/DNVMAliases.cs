using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.Tools.DNVM.Use;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.DNVM
{
    /// <summary>
    /// Contains functionality for working with the DNVM Utility.
    /// </summary>
    [CakeAliasCategory("DNVM")]
    public static class DNVMAliases
    {
        /// <summary>
        /// Execute the use command with the dnvm tool.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="version">The version to be used by dnvm.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var settings = new DNVMSettings
        ///     {
        ///         Arch = "x64",
        ///         Runtime = "coreclr"
        ///     };
        ///
        ///     DNVMUse("default", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        [CakeNamespaceImport("Cake.Common.Tools.DNVM.Use")]
        public static void DNVMUse(this ICakeContext context, string version, DNVMSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(version))
            {
                throw new ArgumentNullException("version");
            }

            DNVMUser user = new DNVMUser(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            user.Use(version, settings);
        }
    }
}
