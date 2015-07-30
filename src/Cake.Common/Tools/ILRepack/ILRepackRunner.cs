using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

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
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public ILRepackRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber,
            IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner, globber)
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
            Run(settings, GetArguments(outputAssemblyPath, primaryAssemblyPath, assemblyPaths, settings),
                settings.ToolPath);
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

            if (!string.IsNullOrEmpty(settings.Keyfile))
            {
                builder.Append("/keyfile:" + new FilePath(settings.Keyfile).MakeAbsolute(_environment).FullPath.Quote());
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

            if (!string.IsNullOrEmpty(settings.Attr))
            {
                builder.Append("/attr:" + new FilePath(settings.Attr).MakeAbsolute(_environment).FullPath.Quote());
            }

            if (settings.AllowMultiple)
            {
                builder.Append("/allowmultiple");
            }

            if (settings.TargetKind != TargetKind.Default)
            {
                builder.Append(GetTargetKindParameter(settings));
            }

            if (settings.TargetPlatform.HasValue)
            {
                builder.Append(GetTargetPlatformString(settings.TargetPlatform.Value));
            }

            if (settings.XmlDocs)
            {
                builder.Append("/xmldocs");
            }

            if (settings.Libs != null && settings.Libs.Any())
            {
                foreach (var lib in settings.Libs)
                {
                    builder.Append("/lib:" + new FilePath(lib).MakeAbsolute(_environment).FullPath.Quote());
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
                    throw new NotSupportedException("The provided ILRepack target platform is not valid.");
            }
        }

        private static string GetTargetKindName(TargetKind kind)
        {
            switch (kind)
            {
                case TargetKind.Dll:
                    return "library";
                case TargetKind.Exe:
                    return "exe";
                case TargetKind.WinExe:
                    return "winexe";
                default:
                    throw new NotSupportedException("The provided ILRepack target kind is not valid.");
            }
        }
    }
}