using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.SignTool
{
    /// <summary>
    /// The SignTool SIGN assembly runner.
    /// </summary>
    public sealed class SignToolSignRunner : Tool<SignToolSignSettings>
    {
        private readonly ISignToolResolver _resolver;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignToolSignRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="registry">The registry.</param>
        public SignToolSignRunner(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber,
            IRegistry registry)
            : this(fileSystem, environment, processRunner, globber, registry, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignToolSignRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="registry">The registry.</param>
        /// <param name="resolver">The resolver.</param>
        internal SignToolSignRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber,
            IRegistry registry,
            ISignToolResolver resolver)
            : base(fileSystem, environment, processRunner, globber)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _resolver = resolver ?? new SignToolResolver(_fileSystem, _environment, registry);
        }

        /// <summary>
        /// Signs the specified assembly.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath assemblyPath, SignToolSignSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (assemblyPath.IsRelative)
            {
                assemblyPath = assemblyPath.MakeAbsolute(_environment);
            }

            Run(settings, GetArguments(assemblyPath, settings), settings.ToolPath);
        }

        private ProcessArgumentBuilder GetArguments(FilePath assemblyPath, SignToolSignSettings settings)
        {
            if (!_fileSystem.Exist(assemblyPath))
            {
                const string format = "{0}: The assembly '{1}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName(), assemblyPath.FullPath);
                throw new CakeException(message);
            }

            if (settings.TimeStampUri == null)
            {
                const string format = "{0}: Timestamp server URL is required but not specified.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (settings.CertPath == null)
            {
                const string format = "{0}: Certificate path is required but not specified.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            // Make certificate path absolute.
            settings.CertPath = settings.CertPath.IsRelative
                ? settings.CertPath.MakeAbsolute(_environment)
                : settings.CertPath;

            if (!_fileSystem.Exist(settings.CertPath))
            {
                const string format = "{0}: The certificate '{1}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName(), settings.CertPath.FullPath);
                throw new CakeException(message);
            }

            if (string.IsNullOrEmpty(settings.Password))
            {
                const string format = "{0}: Password is required but not specified.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            var builder = new ProcessArgumentBuilder();

            // SIGN Command.
            builder.Append("SIGN");

            // TimeStamp server.
            builder.Append("/t");
            builder.AppendQuoted(settings.TimeStampUri.AbsoluteUri);

            // Path to PFX Certificate.
            builder.Append("/f");
            builder.AppendQuoted(settings.CertPath.MakeAbsolute(_environment).FullPath);

            // PFX Password.
            builder.Append("/p");
            builder.AppendSecret(settings.Password);

            // Target Assembly to sign.
            builder.AppendQuoted(assemblyPath.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>
        /// The name of the tool (<c>SignTool SIGN</c>).
        /// </returns>
        protected override string GetToolName()
        {
            return "SignTool SIGN";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(SignToolSignSettings settings)
        {
            var path = _resolver.GetPath();

            if (path != null)
            {
                return new[] { path };
            }

            return Enumerable.Empty<FilePath>();
        }
    }
}