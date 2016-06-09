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

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// The WiX Light runner.
    /// </summary>
    public sealed class LightRunner : Tool<LightSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The Cake environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public LightRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs Light with the specified input object files and settings.
        /// </summary>
        /// <param name="objectFiles">The object files (<c>.wixobj</c>).</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> objectFiles, LightSettings settings)
        {
            if (objectFiles == null)
            {
                throw new ArgumentNullException("objectFiles");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var objectFilesArray = objectFiles as FilePath[] ?? objectFiles.ToArray();
            if (!objectFilesArray.Any())
            {
                throw new ArgumentException("No object files provided.", "objectFiles");
            }

            Run(settings, GetArguments(objectFilesArray, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> objectFiles, LightSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add defines
            if (settings.Defines != null && settings.Defines.Any())
            {
                var defines = settings.Defines.Select(define => string.Format(CultureInfo.InvariantCulture, "-d{0}={1}", define.Key, define.Value));
                foreach (var define in defines)
                {
                    builder.Append(define);
                }
            }

            // Add extensions
            if (settings.Extensions != null && settings.Extensions.Any())
            {
                var extensions = settings.Extensions.Select(extension => string.Format(CultureInfo.InvariantCulture, "-ext {0}", extension));
                foreach (var extension in extensions)
                {
                    builder.Append(extension);
                }
            }

            // No logo
            if (settings.NoLogo)
            {
                builder.Append("-nologo");
            }

            // Output file
            if (settings.OutputFile != null && !string.IsNullOrEmpty(settings.OutputFile.FullPath))
            {
                builder.Append("-o");
                builder.AppendQuoted(settings.OutputFile.MakeAbsolute(_environment).FullPath);
            }

            // Raw arguments
            if (!string.IsNullOrEmpty(settings.RawArguments))
            {
                builder.Append(settings.RawArguments);
            }

            // Object files (.wixobj)
            foreach (var objectFile in objectFiles.Select(file => file.MakeAbsolute(_environment).FullPath))
            {
                builder.AppendQuoted(objectFile);
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Light";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "light.exe" };
        }
    }
}
