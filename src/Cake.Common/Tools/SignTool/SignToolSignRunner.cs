using System;
using System.Globalization;
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
        private readonly IRegistry _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignToolSignRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="registry">The registry.</param>
        public SignToolSignRunner(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IRegistry registry) : this(fileSystem, environment, processRunner, registry, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignToolSignRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="registry">The registry.</param>
        /// <param name="resolver">The resolver.</param>
        internal SignToolSignRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IRegistry registry,
            ISignToolResolver resolver)
            : base(fileSystem, environment, processRunner)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _registry = registry;
            _resolver = resolver ?? new SignToolResolver(_fileSystem, _environment, _registry);
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

            var builder = new ProcessArgumentBuilder("/{0} {1}");

            // SIGN Command.
            builder.Append("SIGN");

            // TimeStamp server.
            builder.AppendNamedQuoted("t", settings.TimeStampUri.AbsoluteUri);

            // Path to PFX Certificate.
            builder.AppendNamedQuoted("f", settings.CertPath.MakeAbsolute(_environment).FullPath);

            // PFX Password.
            builder.AppendNamedSecret("p", settings.Password);

            // Target Assembly to sign.
            return builder.AppendQuoted(assemblyPath.MakeAbsolute(_environment).FullPath);
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
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(SignToolSignSettings settings)
        {
            return (settings == null ? null : settings.ToolPath)
                ?? _resolver.GetPath();
        }
    }
}
