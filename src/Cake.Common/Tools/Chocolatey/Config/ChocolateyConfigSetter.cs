using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey.Config
{
    /// <summary>
    /// The Chocolatey configuration setter.
    /// </summary>
    public sealed class ChocolateyConfigSetter : ChocolateyTool<ChocolateyConfigSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyConfigSetter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyConfigSetter(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, IGlobber globber, IChocolateyToolResolver resolver)
            : base(fileSystem, environment, processRunner, globber, resolver)
        {
        }

        /// <summary>
        /// Sets Chocolatey configuration paramaters using the settings.
        /// </summary>
        /// <param name="name">The name of the config parameter.</param>
        /// <param name="value">The value to assign to the parameter.</param>
        /// <param name="settings">The settings.</param>
        public void Set(string name, string value, ChocolateyConfigSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("value");
            }

            Run(settings, GetArguments(name, value, settings), settings.ToolPath);
        }

        private ProcessArgumentBuilder GetArguments(string name, string value, ChocolateyConfigSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("config");
            builder.Append("set");

            builder.Append("--name");
            builder.AppendQuoted(name);

            builder.Append("--value");
            builder.AppendQuoted(value);

            // Debug
            if (settings.Debug)
            {
                builder.Append("-d");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("-v");
            }

            // Always say yes, so as to not show interactive prompt
            builder.Append("-y");

            // Force
            if (settings.Force)
            {
                builder.Append("-f");
            }

            // Noop
            if (settings.Noop)
            {
                builder.Append("--noop");
            }

            // Limit Output
            if (settings.LimitOutput)
            {
                builder.Append("-r");
            }

            // Execution Timeout
            if (settings.ExecutionTimeout != 0)
            {
                builder.Append("--execution-timeout");
                builder.AppendQuoted(settings.ExecutionTimeout.ToString(CultureInfo.InvariantCulture));
            }

            // Cache Location
            if (!string.IsNullOrWhiteSpace(settings.CacheLocation))
            {
                builder.Append("-c");
                builder.AppendQuoted(settings.CacheLocation);
            }

            // Allow Unoffical
            if (settings.AllowUnoffical)
            {
                builder.Append("--allowunofficial");
            }

            return builder;
        }
    }
}