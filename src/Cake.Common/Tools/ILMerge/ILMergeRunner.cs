// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// The ILMerge runner.
    /// </summary>
    public sealed class ILMergeRunner : Tool<ILMergeSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ILMergeRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public ILMergeRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Merges the specified assemblies.
        /// </summary>
        /// <param name="outputAssemblyPath">The output assembly path.</param>
        /// <param name="primaryAssemblyPath">The primary assembly path.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        public void Merge(FilePath outputAssemblyPath, FilePath primaryAssemblyPath,
            IEnumerable<FilePath> assemblyPaths, ILMergeSettings settings = null)
        {
            if (outputAssemblyPath == null)
            {
                throw new ArgumentNullException("outputAssemblyPath");
            }
            if (primaryAssemblyPath == null)
            {
                throw new ArgumentNullException("primaryAssemblyPath");
            }
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
            }

            settings = settings ?? new ILMergeSettings();

            // Get the ILMerge path.
            Run(settings, GetArguments(outputAssemblyPath, primaryAssemblyPath, assemblyPaths, settings));
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "ILMerge";
        }

        /// <summary>
        /// Gets the name of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "ILMerge.exe" };
        }

        private ProcessArgumentBuilder GetArguments(FilePath outputAssemblyPath,
            FilePath primaryAssemblyFilePath, IEnumerable<FilePath> assemblyPaths, ILMergeSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append(GetOutputParameter(outputAssemblyPath.MakeAbsolute(_environment)));

            if (settings.TargetKind != TargetKind.Default)
            {
                builder.Append(GetTargetKindParameter(settings));
            }

            if (settings.TargetPlatform != null)
            {
                builder.Append(GetTargetPlatformParameter(settings));
            }

            if (settings.Internalize)
            {
                builder.Append("/internalize");
            }

            // Add primary assembly.
            builder.AppendQuoted(primaryAssemblyFilePath.MakeAbsolute(_environment).FullPath);

            foreach (var file in assemblyPaths)
            {
                builder.AppendQuoted(file.MakeAbsolute(_environment).FullPath);
            }

            // Create the process start info.
            return builder;
        }

        private string GetOutputParameter(FilePath outputAssemblyPath)
        {
            var path = outputAssemblyPath.MakeAbsolute(_environment);
            return string.Concat("/out:", path.FullPath.Quote());
        }

        private static string GetTargetKindParameter(ILMergeSettings settings)
        {
            return string.Concat("/target:", GetTargetKindName(settings.TargetKind).Quote());
        }

        private static string GetTargetPlatformParameter(ILMergeSettings settings)
        {
            var result = new List<string>();
            result.Add(GetTargetPlatformString(settings.TargetPlatform.Platform));
            if (settings.TargetPlatform.Path != null)
            {
                result.Add(settings.TargetPlatform.Path.FullPath.Quote());
            }
            return string.Concat("/targetPlatform:", string.Join(",", result));
        }

        private static string GetTargetPlatformString(TargetPlatformVersion version)
        {
            switch (version)
            {
                case TargetPlatformVersion.v1:
                    return "v1";
                case TargetPlatformVersion.v11:
                    return "v1.1";
                case TargetPlatformVersion.v2:
                    return "v2";
                case TargetPlatformVersion.v4:
                    return "v4";
                default:
                    throw new NotSupportedException("The provided ILMerge target platform is not valid.");
            }
        }

        private static string GetTargetKindName(TargetKind kind)
        {
            switch (kind)
            {
                case TargetKind.Dll:
                    return "dll";
                case TargetKind.Exe:
                    return "exe";
                case TargetKind.WinExe:
                    return "winexe";
                default:
                    throw new NotSupportedException("The provided ILMerge target kind is not valid.");
            }
        }
    }
}
