// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.WiX.Heat;
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
    /// install from nuget.org, or specify the ToolPath within the appropriate settings class:
    /// <code>
    /// #tool "nuget:?package=WiX"
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
        public static void WiXCandle(this ICakeContext context, GlobPattern pattern, CandleSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
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
                throw new ArgumentNullException(nameof(context));
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
        public static void WiXLight(this ICakeContext context, GlobPattern pattern, LightSettings settings = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
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
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new LightRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(objectFiles, settings ?? new LightSettings());
        }

        /// <summary>
        /// Harvests files in the provided object files.
        /// </summary>
        /// <example>
        /// <code>
        /// DirectoryPath harvestDirectory = Directory("./src");
        /// var filePath = new FilePath("Wix.Directory.wxs");
        /// WiXHeat(harvestDirectory, filePath, WiXHarvestType.Dir);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">The object files.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Heat")]
        [CakeNamespaceImport("Cake.Common.Tools.WiX.Heat")]
        public static void WiXHeat(this ICakeContext context, DirectoryPath directoryPath, FilePath outputFile, WiXHarvestType harvestType)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new HeatRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(directoryPath, outputFile, harvestType, new HeatSettings());
        }

        /// <summary>
        /// Harvests files in the provided directory path.
        /// </summary>
        /// <example>
        /// <code>
        /// DirectoryPath harvestDirectory = Directory("./src");
        /// var filePath = File("Wix.Directory.wxs");
        /// Information(MakeAbsolute(harvestDirectory).FullPath);
        /// WiXHeat(harvestDirectory, filePath, WiXHarvestType.Dir, new HeatSettings { NoLogo = true });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Heat")]
        [CakeNamespaceImport("Cake.Common.Tools.WiX.Heat")]
        public static void WiXHeat(this ICakeContext context, DirectoryPath directoryPath, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new HeatRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(directoryPath, outputFile, harvestType, settings ?? new HeatSettings());
        }

        /// <summary>
        /// Harvests from the desired files.
        /// </summary>
        /// <example>
        /// <code>
        /// var harvestFile = File("./tools/Cake/Cake.Core.dll");
        /// var filePath = File("Wix.File.wxs");
        /// WiXHeat(harvestFile, filePath, WiXHarvestType.File);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="objectFile">The object file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Heat")]
        [CakeNamespaceImport("Cake.Common.Tools.WiX.Heat")]
        public static void WiXHeat(this ICakeContext context, FilePath objectFile, FilePath outputFile, WiXHarvestType harvestType)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new HeatRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(objectFile, outputFile, harvestType, new HeatSettings());
        }

        /// <summary>
        /// Harvests from the desired files.
        /// </summary>
        /// <example>
        /// <code>
        /// var harvestFiles = File("./tools/Cake/*.dll");
        /// var filePath = File("Wix.File.wxs");
        /// WiXHeat(harvestFiles, filePath, WiXHarvestType.File, new HeatSettings { NoLogo = true });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="objectFile">The object file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Heat")]
        [CakeNamespaceImport("Cake.Common.Tools.WiX.Heat")]
        public static void WiXHeat(this ICakeContext context, FilePath objectFile, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new HeatRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(objectFile, outputFile, harvestType, settings ?? new HeatSettings());
        }

        /// <summary>
        /// Harvests files for a website or performance.
        /// </summary>
        /// <example>
        /// <code>
        /// var filePath = File("Wix.Website.wxs");
        /// WiXHeat("Default Web Site", filePath, WiXHarvestType.Website);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="harvestTarget">The harvest target.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Heat")]
        [CakeNamespaceImport("Cake.Common.Tools.WiX.Heat")]
        public static void WiXHeat(this ICakeContext context, string harvestTarget, FilePath outputFile, WiXHarvestType harvestType)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new HeatRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(harvestTarget, outputFile, harvestType, new HeatSettings());
        }

        /// <summary>
        /// Harvests files for a website or performance.
        /// </summary>
        /// <example>
        /// <code>
        /// var filePath = File("Wix.Website.wxs");
        /// WiXHeat("Default Web Site", filePath, WiXHarvestType.Website, new HeatSettings { NoLogo = true });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="harvestTarget">The harvest target.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="harvestType">The WiX harvest type.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Heat")]
        [CakeNamespaceImport("Cake.Common.Tools.WiX.Heat")]
        public static void WiXHeat(this ICakeContext context, string harvestTarget, FilePath outputFile, WiXHarvestType harvestType, HeatSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new HeatRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(harvestTarget, outputFile, harvestType, settings ?? new HeatSettings());
        }
    }
}
