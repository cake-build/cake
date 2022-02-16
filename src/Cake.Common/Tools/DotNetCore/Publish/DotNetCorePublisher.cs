// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Publish
{
    /// <summary>
    /// .NET Core project runner.
    /// </summary>
    public sealed class DotNetCorePublisher : DotNetCoreTool<DotNetPublishSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCorePublisher" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCorePublisher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Publish the project using the specified path and settings.
        /// </summary>
        /// <param name="path">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Publish(string path, DotNetPublishSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(path, settings));
        }

        private ProcessArgumentBuilder GetArguments(string path, DotNetPublishSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("publish");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--output");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Runtime
            if (!string.IsNullOrEmpty(settings.Runtime))
            {
                builder.Append("--runtime");
                builder.Append(settings.Runtime);
            }

            // Framework
            if (!string.IsNullOrEmpty(settings.Framework))
            {
                builder.Append("--framework");
                builder.Append(settings.Framework);
            }

            // Configuration
            if (!string.IsNullOrEmpty(settings.Configuration))
            {
                builder.Append("--configuration");
                builder.Append(settings.Configuration);
            }

            // Version suffix
            if (!string.IsNullOrEmpty(settings.VersionSuffix))
            {
                builder.Append("--version-suffix");
                builder.Append(settings.VersionSuffix);
            }

            // No Build
            if (settings.NoBuild)
            {
                builder.Append("--no-build");
            }

            // No Dependencies
            if (settings.NoDependencies)
            {
                builder.Append("--no-dependencies");
            }

            // No Restore
            if (settings.NoRestore)
            {
                builder.Append("--no-restore");
            }

            // No Logo
            if (settings.NoLogo)
            {
                builder.Append("--nologo");
            }

            // Force
            if (settings.Force)
            {
                builder.Append("--force");
            }

            // Self contained
            if (settings.SelfContained.HasValue)
            {
                builder.Append("--self-contained");
                if (settings.SelfContained.Value)
                {
                    builder.Append("true");
                }
                else
                {
                    builder.Append("false");
                }
            }

            // publish single file
            if (settings.PublishSingleFile.HasValue)
            {
                if (settings.PublishSingleFile.Value)
                {
                    builder.Append("-p:PublishSingleFile=true");
                }
                else
                {
                    builder.Append("-p:PublishSingleFile=false");
                }
            }

            // publish trimmed
            if (settings.PublishTrimmed.HasValue)
            {
                if (settings.PublishTrimmed.Value)
                {
                    builder.Append("-p:PublishTrimmed=true");
                }
                else
                {
                    builder.Append("-p:PublishTrimmed=false");
                }
            }

            // Tiered Compilation Quick Jit
            if (settings.TieredCompilationQuickJit.HasValue)
            {
                if (settings.TieredCompilationQuickJit.Value)
                {
                    builder.Append("-p:TieredCompilationQuickJit=true");
                }
                else
                {
                    builder.Append("-p:TieredCompilationQuickJit=false");
                }
            }

            // Tiered Compilation
            if (settings.TieredCompilation.HasValue)
            {
                if (settings.TieredCompilation.Value)
                {
                    builder.Append("-p:TieredCompilation=true");
                }
                else
                {
                    builder.Append("-p:TieredCompilation=false");
                }
            }

            // Publish ReadyToRun
            if (settings.PublishReadyToRun.HasValue)
            {
                if (settings.PublishReadyToRun.Value)
                {
                    builder.Append("-p:PublishReadyToRun=true");
                }
                else
                {
                    builder.Append("-p:PublishReadyToRun=false");
                }
            }

            // Publish ReadyToRunShowWarnings
            if (settings.PublishReadyToRunShowWarnings.HasValue)
            {
                if (settings.PublishReadyToRunShowWarnings.Value)
                {
                    builder.Append("-p:PublishReadyToRunShowWarnings=true");
                }
                else
                {
                    builder.Append("-p:PublishReadyToRunShowWarnings=false");
                }
            }

            // Include Native Libraries For Self-Extract
            if (settings.IncludeNativeLibrariesForSelfExtract.HasValue)
            {
                if (settings.IncludeNativeLibrariesForSelfExtract.Value)
                {
                    builder.Append("-p:IncludeNativeLibrariesForSelfExtract=true");
                }
                else
                {
                    builder.Append("-p:IncludeNativeLibrariesForSelfExtract=false");
                }
            }

            // Include All Content For Self-Extract
            if (settings.IncludeAllContentForSelfExtract.HasValue)
            {
                if (settings.IncludeAllContentForSelfExtract.Value)
                {
                    builder.Append("-p:IncludeAllContentForSelfExtract=true");
                }
                else
                {
                    builder.Append("-p:IncludeAllContentForSelfExtract=false");
                }
            }

            // Enable compression on the embedded assemblies
            if (settings.EnableCompressionInSingleFile.HasValue)
            {
                if (settings.EnableCompressionInSingleFile.Value)
                {
                    builder.Append("-p:EnableCompressionInSingleFile=true");
                }
                else
                {
                    builder.Append("-p:EnableCompressionInSingleFile=false");
                }
            }

            // Sources
            if (settings.Sources != null)
            {
                foreach (var source in settings.Sources)
                {
                    builder.Append("--source");
                    builder.AppendQuoted(source);
                }
            }

            if (settings.MSBuildSettings != null)
            {
                builder.AppendMSBuildSettings(settings.MSBuildSettings, _environment);
            }

            return builder;
        }
    }
}
