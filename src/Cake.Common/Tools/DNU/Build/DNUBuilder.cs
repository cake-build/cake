using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DNU.Build
{
    /// <summary>
    /// DNU project builder.
    /// </summary>
    public sealed class DNUBuilder : DNUTool<DNUBuildSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DNUBuilder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public DNUBuilder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Build the project using the specified path and settings.
        /// </summary>
        /// <param name="path">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Build(string path, DNUBuildSettings settings)
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

        private ProcessArgumentBuilder GetArguments(string path, DNUBuildSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("build");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            // List of frameworks
            if (settings.Frameworks != null && settings.Frameworks.Count > 0)
            {
                builder.Append("--framework");
                builder.AppendQuoted(string.Join(";", settings.Frameworks));
            }

            // List of configurations
            if (settings.Configurations != null && settings.Configurations.Count > 0)
            {
                builder.Append("--configuration");
                builder.AppendQuoted(string.Join(";", settings.Configurations));
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