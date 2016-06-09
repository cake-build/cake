// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.NSIS
{
    /// <summary>
    /// <para>Contains functionality related to <see href="http://nsis.sourceforge.net/Main_Page">NSIS</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, NSIS will need to be installed on the machine where
    /// the Cake script is being executed.  See this <see href="http://nsis.sourceforge.net/Download">page</see> for information
    /// on how to download/install.
    /// </para>
    /// </summary>
    [CakeAliasCategory("NSIS")]
    // ReSharper disable once InconsistentNaming
    public static class NSISAliases
    {
        /// <summary>
        /// Compiles the given NSIS script using the default settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="scriptFile">The path to the <c>.nsi</c> script file to compile.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("MakeNSIS")]
        // ReSharper disable once InconsistentNaming
        public static void MakeNSIS(this ICakeContext context, FilePath scriptFile)
        {
            MakeNSIS(context, scriptFile, new MakeNSISSettings());
        }

        /// <summary>
        /// Compiles the given NSIS script using the given <paramref name="settings"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="scriptFile">The path to the <c>.nsi</c> script file to compile.</param>
        /// <param name="settings">The <see cref="MakeNSISSettings"/> to use.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("MakeNSIS")]
        // ReSharper disable once InconsistentNaming
        public static void MakeNSIS(this ICakeContext context, FilePath scriptFile, MakeNSISSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (scriptFile == null)
            {
                throw new ArgumentNullException("scriptFile");
            }

            var runner = new MakeNSISRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(scriptFile, settings ?? new MakeNSISSettings());
        }
    }
}
