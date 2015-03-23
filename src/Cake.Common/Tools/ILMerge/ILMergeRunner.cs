using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// The ILMerge runner.
    /// </summary>
    public sealed class ILMergeRunner : Tool<ILMergeSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        /// <summary>
        /// Initializes a new instance of the <see cref="ILMergeRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public ILMergeRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber,
            IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
        {
            _environment = environment;
            _globber = globber;
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
            Run(settings, GetArguments(outputAssemblyPath, primaryAssemblyPath, assemblyPaths, settings),
                settings.ToolPath);
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
            return string.Concat("/targetPlatform:", CommandLineValue(settings.TargetPlatform));
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

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "ILMerge";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(ILMergeSettings settings)
        {
            const string expression = "./tools/**/ILMerge.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }

        /// <summary>
        /// Command line option value
        /// </summary>
        /// <param name="targetPlatform">The target platform.</param>
        /// <returns>Command line option string.</returns>
        private static string CommandLineValue(TargetPlatform targetPlatform)
        {
            if (targetPlatform.Path == null)
            {
                return string.Format("{0}", GetTargetPlatformVersionString(targetPlatform.Platform));
            }
            return string.Format("{0},{1}", GetTargetPlatformVersionString(targetPlatform.Platform),
                targetPlatform.Path.FullPath.Quote());
        }

        private static string GetTargetPlatformVersionString(TargetPlatformVersion version)
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
    }
}