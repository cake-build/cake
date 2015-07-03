using System;
using System.Collections.Generic;
using System.Globalization;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.XBuild
{
    /// <summary>
    /// The MSBuild runner.
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
        public XBuildRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner runner)
            : base(fileSystem, environment, runner)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Runs MSBuild with the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Run(XBuildSettings settings)
        {
            Run(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(XBuildSettings settings)
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
            builder.AppendQuoted(settings.Solution.MakeAbsolute(_environment).FullPath);

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
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(XBuildSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            return XBuildResolver.GetXBuildPath(_fileSystem, _environment, settings.ToolVersion);
        }
    }
}
