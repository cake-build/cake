// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.InnoSetup
{
    /// <summary>
    /// <para>Contains functionality related to <see href="http://www.jrsoftware.org/isinfo.php">Inno Setup</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, Inno Setup will need to be installed on the machine where
    /// the Cake script is being executed.  See this <see href="http://www.jrsoftware.org/isdl.php">page</see> for information
    /// on how to download/install.
    /// </para>
    /// </summary>
    [CakeAliasCategory("Inno Setup")]
    public static class InnoSetupAliases
    {
        /// <summary>
        /// Compiles the given Inno Setup script using the default settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="scriptFile">The path to the <c>.iss</c> script file to compile.</param>
        /// <example>
        /// <code>
        /// InnoSetup("./src/Cake.iss");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void InnoSetup(this ICakeContext context, FilePath scriptFile)
        {
            InnoSetup(context, scriptFile, new InnoSetupSettings());
        }

        /// <summary>
        /// Compiles the given Inno Setup script using the given <paramref name="settings"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="scriptFile">The path to the <c>.iss</c> script file to compile.</param>
        /// <param name="settings">The <see cref="InnoSetupSettings"/> to use.</param>
        /// <example>
        /// <code>
        /// InnoSetup("./src/Cake.iss", new InnoSetupSettings {
        ///     OutputDirectory = outputDirectory
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void InnoSetup(this ICakeContext context, FilePath scriptFile, InnoSetupSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (scriptFile == null)
            {
                throw new ArgumentNullException(nameof(scriptFile));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var runner = new InnoSetupRunner(context.FileSystem, context.Registry, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(scriptFile, settings);
        }
    }
}