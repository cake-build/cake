// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.SDKCheck;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.DotNet
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/dotnet/cli">.NET CLI</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, the .NET CLI tools will need to be installed on the machine where
    /// the Cake script is being executed.  See this <see href="https://www.microsoft.com/net/core">page</see> for information
    /// on how to install.
    /// </para>
    /// </summary>
    public static partial class DotNetAliases
    {
        /// <summary>
        /// Lists the latest available version of the .NET SDK and .NET Runtime.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetSDKCheck();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("SDK")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.SDKCheck")]
        public static void DotNetSDKCheck(this ICakeContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var checker = new DotNetSDKChecker(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            checker.Check();
        }
    }
}
