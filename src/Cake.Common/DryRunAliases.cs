// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Globalization;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to arguments.
    /// </summary>
    [CakeAliasCategory("Dry Run")]
    public static class DryRunAliases
    {
        /// <summary>
        /// Determines whether or not the current script execution is a dry run.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Whether or not the current script execution is a dry run.</returns>
        /// <example>
        /// <code>
        /// Setup(context =>
        /// {
        ///     if (!context.IsDryRun())
        ///     {
        ///         // Do things that you don't want to
        ///         // do during a dry run.
        ///     }
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static bool IsDryRun(this ICakeContext context)
        {
            return (context.Argument<bool?>("dryrun", false) ?? true)
                || (context.Argument<bool?>("noop", false) ?? true)
                || (context.Argument<bool?>("whatif", false) ?? true);
        }
    }
}