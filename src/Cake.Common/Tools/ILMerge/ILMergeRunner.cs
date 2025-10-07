// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
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
            ArgumentNullException.ThrowIfNull(outputAssemblyPath);
            ArgumentNullException.ThrowIfNull(primaryAssemblyPath);
            ArgumentNullException.ThrowIfNull(assemblyPaths);

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
            const string separator = ":";

            if (settings.SearchDirectories != null)
            {
                foreach (var searchDirectory in settings.SearchDirectories)
                {
                    var directoryPath = searchDirectory.MakeAbsolute(_environment);
                    builder.AppendSwitchQuoted("/lib", separator, directoryPath.FullPath);
                }
            }

            if (settings.Log && settings.LogFile == null)
            {
                builder.Append("/log");
            }

            if (settings.LogFile != null)
            {
                var logFilePath = settings.LogFile.MakeAbsolute(_environment);
                builder.AppendSwitchQuoted("/log", separator, logFilePath.FullPath);
            }

            if (settings.KeyFile != null)
            {
                var keyFilePath = settings.KeyFile.MakeAbsolute(_environment);
                builder.AppendSwitchQuoted("/keyfile", separator, keyFilePath.FullPath);
            }

            if (!string.IsNullOrEmpty(settings.KeyContainer))
            {
                builder.AppendSwitchQuoted("/keycontainer", separator, settings.KeyContainer);
            }

            if ((settings.KeyFile != null || !string.IsNullOrEmpty(settings.KeyContainer)) && settings.DelaySign)
            {
                builder.Append("/delaysign");
            }

            if (settings.Internalize)
            {
                builder.Append("/internalize");
            }

            if (settings.TargetKind != TargetKind.Default)
            {
                builder.Append(GetTargetKindParameter(settings));
            }

            if (settings.Closed)
            {
                builder.Append("/closed");
            }

            if (settings.NDebug)
            {
                builder.Append("/ndebug");
            }

            if (!string.IsNullOrEmpty(settings.Version))
            {
                builder.AppendSwitch("/ver", separator, settings.Version);
            }

            if (settings.CopyAttributes)
            {
                builder.Append("/copyattrs");

                if (settings.AllowMultiple)
                {
                    builder.Append("/allowMultiple");
                }

                if (settings.KeepFirst)
                {
                    builder.Append("/keepFirst");
                }
            }

            if (settings.XmlDocumentation)
            {
                builder.Append("/xmldocs");
            }

            if (settings.AttributeFile != null)
            {
                var attributeFilePath = settings.AttributeFile.MakeAbsolute(_environment);
                builder.AppendSwitchQuoted("/attr", separator, attributeFilePath.FullPath);
            }

            if (settings.TargetPlatform != null)
            {
                builder.Append(GetTargetPlatformParameter(settings));
            }

            if (settings.UseFullPublicKeyForReferences)
            {
                builder.Append("/useFullPublicKeyForReferences");
            }

            if (settings.Wildcards)
            {
                builder.Append("/wildcards");
            }

            if (settings.ZeroPeKind)
            {
                builder.Append("/zeroPeKind");
            }

            if (settings.AllowDuplicateTypes && settings.DuplicateTypes == null)
            {
                builder.Append("/allowDup");
            }

            if (settings.DuplicateTypes != null)
            {
                foreach (var duplicateType in settings.DuplicateTypes)
                {
                    builder.AppendSwitch("/allowDup", separator, duplicateType);
                }
            }

            if (settings.Union)
            {
                builder.Append("/union");
            }

            if (settings.Align.HasValue)
            {
                builder.AppendSwitch("/align", separator, settings.Align.Value.ToString(CultureInfo.InvariantCulture));
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
            return string.Concat("/targetPlatform:", string.Join(',', result));
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