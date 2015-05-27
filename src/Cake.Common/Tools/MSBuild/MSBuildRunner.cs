using System;
using System.Collections.Generic;
using System.Globalization;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// The MSBuild runner.
    /// </summary>
    public sealed class MSBuildRunner : Tool<MSBuildSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="runner">The runner.</param>
        public MSBuildRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner runner)
            : base(fileSystem, environment, runner)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Runs MSBuild with the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Run(MSBuildSettings settings)
        {
            Run(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(MSBuildSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Set the maximum number of processors.
            builder.Append(settings.MaxCpuCount > 0 ? string.Concat("/m:", settings.MaxCpuCount) : "/m");

            // Set the verbosity.
            builder.Append(string.Format(CultureInfo.InvariantCulture, "/v:{0}", GetVerbosityName(settings.Verbosity)));

            if (settings.NodeReuse != null)
            {
                builder.Append(string.Concat("/nr:", settings.NodeReuse.Value ? "true" : "false"));
            }

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
                builder.Append(string.Concat("/target:", targets));
            }
            else
            {
                // Use default target.
                builder.Append("/target:Build");
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
                    return "verbose";
                case Verbosity.Diagnostic: 
                    return "diagnostic";
            }
            throw new CakeException("Encountered unknown MSBuild build log verbosity.");
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
            return "MSBuild";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(MSBuildSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            return MSBuildResolver.GetMSBuildPath(_fileSystem, _environment, settings.ToolVersion, settings.PlatformTarget);
        }
    }
}
