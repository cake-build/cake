// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.ILRepack
{
    /// <summary>
    /// The ILMerge runner.
    /// </summary>
    public sealed class ILRepackRunner : Tool<ILRepackSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ILRepackRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public ILRepackRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
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
            IEnumerable<FilePath> assemblyPaths, ILRepackSettings settings = null)
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

            settings = settings ?? new ILRepackSettings();

            // Get the ILMerge path.
            Run(settings, GetArguments(outputAssemblyPath, primaryAssemblyPath, assemblyPaths, settings));
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "ILRepack";
        }

        /// <summary>
        /// Gets the name of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "ILRepack.exe" };
        }

        private ProcessArgumentBuilder GetArguments(FilePath outputAssemblyPath,
            FilePath primaryAssemblyFilePath, IEnumerable<FilePath> assemblyPaths, ILRepackSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.Keyfile != null)
            {
                builder.Append("/keyfile:" + settings.Keyfile.MakeAbsolute(_environment).FullPath.Quote());
            }

            if (!string.IsNullOrEmpty(settings.Log))
            {
                builder.Append("/log:" + new FilePath(settings.Log).MakeAbsolute(_environment).FullPath.Quote());
            }

            if (settings.Version != null)
            {
                builder.Append("/ver:" + settings.Version);
            }

            if (settings.Union)
            {
                builder.Append("/union");
            }

            if (settings.NDebug)
            {
                builder.Append("/ndebug");
            }

            if (settings.CopyAttrs)
            {
                builder.Append("/copyattrs");
            }

            if (settings.Attr != null)
            {
                builder.Append("/attr:" + settings.Attr.MakeAbsolute(_environment).FullPath.Quote());
            }

            if (settings.AllowMultiple)
            {
                builder.Append("/allowmultiple");
            }

            if (settings.TargetKind != ILMerge.TargetKind.Default)
            {
                builder.Append(GetTargetKindParameter(settings));
            }

            if (settings.TargetPlatform.HasValue)
            {
                builder.AppendSwitch("/targetplatform", ":", GetTargetPlatformString(settings.TargetPlatform.Value));
            }

            if (settings.XmlDocs)
            {
                builder.Append("/xmldocs");
            }

            if (settings.Libs != null && settings.Libs.Any())
            {
                foreach (var lib in settings.Libs)
                {
                    builder.Append("/lib:" + lib.MakeAbsolute(_environment).FullPath.Quote());
                }
            }

            if (settings.Internalize)
            {
                builder.Append("/internalize");
            }

            if (settings.DelaySign)
            {
                builder.Append("/delaysign");
            }

            if (!string.IsNullOrEmpty(settings.AllowDup))
            {
                builder.Append("/allowdup:" + settings.AllowDup);
            }

            if (settings.AllowDuplicateResources)
            {
                builder.Append("/allowduplicateresources");
            }

            if (settings.ZeroPeKind)
            {
                builder.Append("/zeropekind");
            }

            if (settings.Wildcards)
            {
                builder.Append("/wildcards");
            }

            if (settings.Parallel)
            {
                builder.Append("/parallel");
            }

            if (settings.Pause)
            {
                builder.Append("/pause");
            }

            if (settings.Verbose)
            {
                builder.Append("/verbose");
            }

            builder.Append(GetOutputParameter(outputAssemblyPath.MakeAbsolute(_environment)));

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

        private static string GetTargetKindParameter(ILRepackSettings settings)
        {
            return string.Concat("/target:", GetTargetKindName(settings.TargetKind).Quote());
        }

        private static string GetTargetPlatformString(ILMerge.TargetPlatformVersion version)
        {
            switch (version)
            {
                case ILMerge.TargetPlatformVersion.v1:
                    return "v1";
                case ILMerge.TargetPlatformVersion.v11:
                    return "v1.1";
                case ILMerge.TargetPlatformVersion.v2:
                    return "v2";
                case ILMerge.TargetPlatformVersion.v4:
                    return "v4";
                default:
                    throw new NotSupportedException("The provided ILRepack target platform is not valid.");
            }
        }

        private static string GetTargetKindName(ILMerge.TargetKind kind)
        {
            switch (kind)
            {
                case ILMerge.TargetKind.Dll:
                    return "library";
                case ILMerge.TargetKind.Exe:
                    return "exe";
                case ILMerge.TargetKind.WinExe:
                    return "winexe";
                default:
                    throw new NotSupportedException("The provided ILRepack target kind is not valid.");
            }
        }
    }
}
