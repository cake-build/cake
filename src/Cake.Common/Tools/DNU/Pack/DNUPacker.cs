using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DNU.Pack
{
    /// <summary>
    /// DNU NuGet package packer.
    /// </summary>
    public sealed class DNUPacker : DNUTool<DNUPackSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DNUPacker" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public DNUPacker(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Create NuGet packages using the specified path and settings.
        /// </summary>
        /// <param name="path">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Pack(string path, DNUPackSettings settings)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(path, settings));
        }

        private ProcessArgumentBuilder GetArguments(string path, DNUPackSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("pack");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            // List of frameworks
            if (settings.Frameworks != null && settings.Frameworks.Count > 0)
            {
                foreach (var source in settings.Frameworks)
                {
                    builder.Append("--framework");
                    builder.AppendQuoted(source);
                }
            }

            // List of configurations
            if (settings.Configurations != null && settings.Configurations.Count > 0)
            {
                foreach (var source in settings.Configurations)
                {
                    builder.Append("--configuration");
                    builder.AppendQuoted(source);
                }
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--out");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Quiet?
            if (settings.Quiet)
            {
                builder.Append("--quiet");
            }

            return builder;
        }
    }
}