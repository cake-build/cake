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
        /// Signs the specified assemblies.
        /// </summary>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> assemblyPaths, SignToolSignSettings settings)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException(nameof(assemblyPaths));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var absoluteAssemblyPaths = assemblyPaths.Select(p => p.IsRelative ? p.MakeAbsolute(_environment) : p).ToArray();

            Run(settings, GetArguments(absoluteAssemblyPaths, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath[] absoluteAssemblyPaths, SignToolSignSettings settings)
        {
            foreach (var path in absoluteAssemblyPaths)
            {
                if (!_fileSystem.Exist(path))
                {
                    const string format = "{0}: The assembly '{1}' does not exist.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName(), path.FullPath);
                    throw new CakeException(message);
                }
            }

            var builder = new ProcessArgumentBuilder();

            // SIGN Command.
            builder.Append("SIGN");

            // SHA-256.
            if (settings.DigestAlgorithm == SignToolDigestAlgorithm.Sha256)
            {
                builder.Append("/fd sha256");
            }

            // TimeStamp.
            if (settings.TimeStampUri != null)
            {
                if (settings.TimeStampDigestAlgorithm == SignToolDigestAlgorithm.Sha256)
                {
                    // If Sha256 use RFC 3161 timestamp server.
                    builder.Append("/tr");
                    builder.AppendQuoted(settings.TimeStampUri.AbsoluteUri);

                    builder.Append("/td sha256");
                }
                else
                {
                    // Otherwise use SHA-1 Authenticode timestamp server
                    builder.Append("/t");
                    builder.AppendQuoted(settings.TimeStampUri.AbsoluteUri);
                }
            }

            if (settings.CertPath == null && string.IsNullOrEmpty(settings.CertThumbprint) && string.IsNullOrEmpty(settings.CertSubjectName))
            {
                const string format = "{0}: One of Certificate path, Certificate thumbprint or Certificate subject name is required but neither are specified.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (settings.CertPath != null && !string.IsNullOrEmpty(settings.CertThumbprint))
            {
                const string format = "{0}: Certificate path and Certificate thumbprint cannot be specified together.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (settings.CertPath != null && !string.IsNullOrEmpty(settings.CertSubjectName))
            {
                const string format = "{0}: Certificate path and Certificate subject name cannot be specified together.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (!string.IsNullOrEmpty(settings.CertThumbprint) && !string.IsNullOrEmpty(settings.Password))
            {
                const string format = "{0}: Certificate thumbprint and Password cannot be specified together.";
                var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName());
                throw new CakeException(message);
            }

            if (!string.IsNullOrEmpty(settings.CertSubjectName) && !string.IsNullOrEmpty(settings.Password))
            {
                const string format = "{0}: Certificate subject name and Password cannot be specified together.";
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
                    const string format = "{0}: The certificate '{1}' does not exist.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName(), settings.CertPath.FullPath);
                    throw new CakeException(message);
                }

                // Path to PFX Certificate.
                builder.Append("/f");
                builder.AppendQuoted(settings.CertPath.MakeAbsolute(_environment).FullPath);

                // PFX Password.
                if (!string.IsNullOrEmpty(settings.Password))
                {
                    builder.Append("/p");
                    builder.AppendSecret(settings.Password);
                }
            }

            if (settings.AdditionalCertPath != null)
            {
                // Make additional certificate path absolute.
                settings.AdditionalCertPath = settings.AdditionalCertPath.IsRelative
                    ? settings.AdditionalCertPath.MakeAbsolute(_environment)
                    : settings.AdditionalCertPath;

                if (!_fileSystem.Exist(settings.AdditionalCertPath))
                {
                    const string format = "{0}: The additional certificate '{1}' does not exist.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, GetToolName(), settings.AdditionalCertPath.FullPath);
                    throw new CakeException(message);
                }

                // Path to additional certificate.
                builder.Append("/ac");
                builder.AppendQuoted(settings.AdditionalCertPath.MakeAbsolute(_environment).FullPath);
            }

            // Certificate thumbprint.
            if (!string.IsNullOrEmpty(settings.CertThumbprint))
            {
                builder.Append("/sha1");
                builder.AppendQuoted(settings.CertThumbprint);
            }

            if (!string.IsNullOrEmpty(settings.CertSubjectName))
            {
                builder.Append("/n");
                builder.AppendQuoted(settings.CertSubjectName);
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

            // Append signature.
            if (settings.AppendSignature)
            {
                builder.Append("/as");
            }

            // use machine store when requested
            if (settings.UseMachineStore)
            {
                builder.Append("/sm");
            }

            // open a specific certificate store
            if (!string.IsNullOrWhiteSpace(settings.StoreName))
            {
                builder.Append("/s");
                builder.AppendQuoted(settings.StoreName);
            }

            // Target Assemblies to sign.
            foreach (var path in absoluteAssemblyPaths)
            {
                builder.AppendQuoted(path.FullPath);
            }

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
        /// Gets alternative file paths which the tool may exist in.
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