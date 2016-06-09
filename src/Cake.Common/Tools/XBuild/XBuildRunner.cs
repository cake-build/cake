// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.XBuild
{
    /// <summary>
    /// The XBuild runner.
    /// </summary>
    public sealed class XBuildRunner : Tool<XBuildSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="XBuildRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="runner">The runner.</param>
        /// <param name="tools">The tool locator.</param>
        public XBuildRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner runner, IToolLocator tools)
            : base(fileSystem, environment, runner, tools)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Runs XBuild with the specified settings.
        /// </summary>
        /// <param name="solution">The solution to build.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath solution, XBuildSettings settings)
        {
            Run(settings, GetArguments(solution, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath solution, XBuildSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Set the verbosity.
            builder.Append(string.Format(CultureInfo.InvariantCulture, "/v:{0}", GetVerbosityName(settings.Verbosity)));

            // Got a specific configuration in mind?
            if (!string.IsNullOrWhiteSpace(settings.Configuration))
            {
                // Add the configuration as a property.
                var configuration = settings.Configuration;
                builder.Append(string.Concat("/p:\"Configuration\"=", configuration.Quote()));
            }

            // Got any properties?
            if (settings.Properties.Count > 0)
            {
                foreach (var property in GetPropertyArguments(settings.Properties))
                {
                    builder.Append(property);
                }
            }

            // Got any targets?
            if (settings.Targets.Count > 0)
            {
                var targets = string.Join(";", settings.Targets);
                builder.Append(string.Concat("/t:", targets));
            }
            else
            {
                // Use default target.
                builder.Append("/t:Build");
            }

            // Add the solution as the last parameter.
            builder.AppendQuoted(solution.MakeAbsolute(_environment).FullPath);

            return builder;
        }

        private static string GetVerbosityName(Verbosity verbosity)
        {
            switch (verbosity)
            {
            case Verbosity.Quiet:
                return "quiet";
            case Verbosity.Minimal:
                return "minimal";
            case Verbosity.Normal:
                return "normal";
            case Verbosity.Verbose:
                return "detailed";
            case Verbosity.Diagnostic:
                return "diagnostic";
            }
            throw new CakeException("Encountered unknown XBuild build log verbosity.");
        }

        private static IEnumerable<string> GetPropertyArguments(IDictionary<string, IList<string>> properties)
        {
            foreach (var propertyKey in properties.Keys)
            {
                foreach (var propertyValue in properties[propertyKey])
                {
                    yield return string.Concat("/p:", propertyKey.Quote(), "=", propertyValue.Quote());
                }
            }
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "XBuild";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "xunit", "xunit.bat" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(XBuildSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var path = XBuildResolver.GetXBuildPath(_fileSystem, _environment, settings.ToolVersion);

            if (path != null)
            {
                return new[] { path };
            }

            return Enumerable.Empty<FilePath>();
        }
    }
}
