using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.Tools.DNX.Run;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DNX
{
    /// <summary>
    /// Contains functionality for working with the DNX Utility.
    /// </summary>
    [CakeAliasCategory("DNX")]
    public static class DNXAliases
    {
        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="command">The command to be executed.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var settings = new DNXRunSettings
        ///     {
        ///         Project = "",
        ///         Framework = "dnxcore50",
        ///         Configuration = "Release",
        ///         AppBase = "",
        ///         Lib = ""
        ///     };
        ///
        ///     var testProjects = GetFiles("./test/**/project.json");
        ///     foreach(var testProject in projectestProjectsts)
        ///     {
        ///         settings.Project = testProject.FullPath;
        ///         DNXRun("test", settings);
        ///     }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        [CakeNamespaceImport("Cake.Common.Tools.DNX.Run")]
        public static void DNXRun(this ICakeContext context, string command, DNXRunSettings settings)
        {
            DNXRun(context, new DirectoryPath("."), command, settings);
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">The directory which contains the project.json file.</param>
        /// <param name="command">The command to be executed.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        ///     var settings = new DNXRunSettings
        ///     {
        ///         Project = "",
        ///         Framework = "dnxcore50",
        ///         Configuration = "Release",
        ///         AppBase = "",
        ///         Lib = ""
        ///     };
        ///
        ///     var testProjects = GetFiles("./test/**/project.json");
        ///     foreach(var testProject in projectestProjectsts)
        ///     {
        ///         DNXRun(testProject, "test", settings);
        ///     }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        [CakeNamespaceImport("Cake.Common.Tools.DNX.Run")]
        public static void DNXRun(this ICakeContext context, DirectoryPath directoryPath, string command, DNXRunSettings settings)
        {
            if (settings == null)
            {
                settings = new DNXRunSettings();
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (directoryPath == null)
            {
                throw new ArgumentNullException("directoryPath");
            }
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException("command");
            }

            DNXRunner runner = new DNXRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(directoryPath, command, settings);
        }
    }
}
