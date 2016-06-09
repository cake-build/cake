// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

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
        /// <param name="tools">The tool locator.</param>
        /// <param name="registry">The registry.</param>
        public SignToolSignRunner(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IRegistry registry) : this(fileSystem, environment, processRunner, tools, registry, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignToolSignRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="registry">The registry.</param>
        /// <param name="resolver">The resolver.</param>
        internal SignToolSignRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IRegistry registry,
            ISignToolResolver resolver) : base(fileSystem, environment, processRunner, tools)
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

            Run(settings, GetArguments(assemblyPath, settings));
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

            var builder = new ProcessArgumentBuilder();

            // SIGN Command.
            builder.Append("SIGN");

            // TimeStamp server.
            builder.Append("/t");
            builder.AppendQuoted(settings.TimeStampUri.AbsoluteUri);

            if (settings.CertPath == null && string.IsNullOrEmpty(settings.CertThumbprint))
            {
                const string format = "{0}: One of Certificate path or Certificate thumbprint is required but neither are specified.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (settings.CertPath != null && !string.IsNullOrEmpty(settings.CertThumbprint))
            {
                const string format = "{0}: Certificate path and Certificate thumbprint cannot be specified together.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (settings.CertPath != null && string.IsNullOrEmpty(settings.Password))
            {
                const string format = "{0}: Password is required with Certificate path but not specified.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (!string.IsNullOrEmpty(settings.CertThumbprint) && !string.IsNullOrEmpty(settings.Password))
            {
                const string format = "{0}: Certificate thumbprint and Password cannot be specified together.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (settings.CertPath != null)
            {
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

                // Path to PFX Certificate.
                builder.Append("/f");
                builder.AppendQuoted(settings.CertPath.MakeAbsolute(_environment).FullPath);

                // PFX Password.
                builder.Append("/p");
                builder.AppendSecret(settings.Password);
            }

            // Certificate thumbprint.
            if (!string.IsNullOrEmpty(settings.CertThumbprint))
            {
                builder.Append("/sha1");
                builder.AppendQuoted(settings.CertThumbprint);
            }

            // Signed content description.
            if (!string.IsNullOrEmpty(settings.Description))
            {
                builder.Append("/d");
                builder.AppendQuoted(settings.Description);
            }

            // Signed content expanded description URL.
            if (settings.DescriptionUri != null)
            {
                builder.Append("/du");
                builder.AppendQuoted(settings.DescriptionUri.AbsoluteUri);
            }

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
            return new[] { "signtool.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(SignToolSignSettings settings)
        {
            var path = _resolver.GetPath();
            return path != null
                ? new[] { path }
                : Enumerable.Empty<FilePath>();
        }
    }
}
