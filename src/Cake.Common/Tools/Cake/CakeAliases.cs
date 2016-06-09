// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.Cake
{
    /// <summary>
    /// Contains functionality related to running Cake scripts out of process.
    /// </summary>
    [CakeAliasCategory("Cake")]
    public static class CakeAliases
    {
        /// <summary>
        /// Executes cake script out of process
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cakeScriptPath">The script file.</param>
        /// <example>
        /// <code>
        /// CakeExecuteScript("./helloworld.cake");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void CakeExecuteScript(this ICakeContext context, FilePath cakeScriptPath)
        {
            context.CakeExecuteScript(cakeScriptPath, null);
        }

        /// <summary>
        /// Executes cake script out of process
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cakeScriptPath">The script file.</param>
        /// <param name="settings">The settings <see cref="CakeSettings"/>.</param>
        /// <example>
        /// <code>
        /// CakeExecuteScript("./helloworld.cake", new CakeSettings{ ToolPath="./Cake.exe" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void CakeExecuteScript(this ICakeContext context, FilePath cakeScriptPath, CakeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var cakeRunner = new CakeRunner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner, context.Tools);
            cakeRunner.ExecuteScript(cakeScriptPath, settings);
        }

        /// <summary>
        /// Executes Cake expression out of process
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cakeExpression">The cake expression</param>
        /// <example>
        /// <code>
        /// CakeExecuteExpression("Information(\"Hello {0}\", \"World\");");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void CakeExecuteExpression(this ICakeContext context, string cakeExpression)
        {
            context.CakeExecuteExpression(cakeExpression, null);
        }

        /// <summary>
        /// Executes Cake expression out of process
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cakeExpression">The cake expression</param>
        /// <param name="settings">The settings <see cref="CakeSettings"/>.</param>
        /// <example>
        /// <code>
        /// CakeExecuteExpression(
        ///     "Information(\"Hello {0}!\", Argument&lt;string&gt;(\"name\"));",
        ///     new CakeSettings {
        ///         ToolPath="./Cake.exe" ,
        ///         Arguments = new Dictionary&lt;string, string&gt;{{"name", "World"}}
        ///         });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void CakeExecuteExpression(this ICakeContext context, string cakeExpression, CakeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var cakeRunner = new CakeRunner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner, context.Tools);
            cakeRunner.ExecuteExpression(cakeExpression, settings);
        }
    }
}
