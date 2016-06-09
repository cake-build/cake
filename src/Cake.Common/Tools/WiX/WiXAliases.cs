// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// <para>Contains functionality related to <see href="http://wixtoolset.org/">WiX</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the appropriate settings class:
    /// <code>
    /// #tool "nuget:?package=WiX.Toolset"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("WiX")]
    public static class WiXAliases
    {
        /// <summary>
        /// Compiles all <c>.wxs</c> sources matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// CandleSettings settings = new CandleSettings {
        ///     Architecture = Architecture.X64,
        ///     Verbose = true
        ///     };
        /// WiXCandle("./src/*.wxs", settings);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The globbing pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Candle")]
        public static void WiXCandle(this ICakeContext context, string pattern, CandleSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var files = context.Globber.GetFiles(pattern).ToArray();
            if (files.Length == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            WiXCandle(context, files, settings ?? new CandleSettings());
        }

        /// <summary>
        /// Compiles all <c>.wxs</c> sources in the provided source files.
        /// </summary>
        /// <example>
        /// <code>
        /// var files = GetFiles("./src/*.wxs");
        /// CandleSettings settings = new CandleSettings {
        ///     Architecture = Architecture.X64,
        ///     Verbose = true
        ///     };
        /// WiXCandle(files, settings);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="sourceFiles">The source files.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Candle")]
        public static void WiXCandle(this ICakeContext context, IEnumerable<FilePath> sourceFiles, CandleSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new CandleRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(sourceFiles, settings ?? new CandleSettings());
        }

        /// <summary>
        /// Links all <c>.wixobj</c> files matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// LightSettings settings = new LightSettings {
        ///     RawArguments = "-O1 -pedantic -v"
        ///     };
        /// WiXLight("./src/*.wixobj", settings);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The globbing pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Light")]
        public static void WiXLight(this ICakeContext context, string pattern, LightSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var files = context.Globber.GetFiles(pattern).ToArray();
            if (files.Length == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            WiXLight(context, files, settings ?? new LightSettings());
        }

        /// <summary>
        /// Links all <c>.wixobj</c> files in the provided object files.
        /// </summary>
        /// <example>
        /// <code>
        /// var files = GetFiles("./src/*.wxs");
        /// LightSettings settings = new LightSettings {
        ///     RawArguments = "-O1 -pedantic -v"
        ///     };
        /// WiXLight(files, settings);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="objectFiles">The object files.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Light")]
        public static void WiXLight(this ICakeContext context, IEnumerable<FilePath> objectFiles, LightSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new LightRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(objectFiles, settings ?? new LightSettings());
        }
    }
}
