using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.Fixie
{
    /// <summary>
    /// The Fixie test runner.
    /// </summary>
    public sealed class FixieRunner : Tool<FixieSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixieRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public FixieRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assembly, using the specified settings.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath assemblyPath, FixieSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(new[] { assemblyPath }, settings), settings.ToolPath);
        }

        /// <summary>
        /// Runs the tests in the specified assemblies, using the specified settings.
        /// </summary>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> assemblyPaths, FixieSettings settings)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(assemblyPaths, settings), settings.ToolPath);
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> assemblyPaths, FixieSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assemblies to build.
            foreach (var assemblyPath in assemblyPaths)
            {
                builder.AppendQuoted(assemblyPath.MakeAbsolute(_environment).FullPath);
            }

            // Add NUnit style reporting if necessary
            if (settings.NUnitXml != null)
            {
                builder.Append("--NUnitXml");
                builder.AppendQuoted(settings.NUnitXml.MakeAbsolute(_environment).FullPath);
            }

            // Add xUnit style reporting if necessary
            if (settings.XUnitXml != null)
            {
                builder.Append("--xUnitXml");
                builder.AppendQuoted(settings.XUnitXml.MakeAbsolute(_environment).FullPath);
            }

            if (settings.TeamCity != null)
            {
                builder.Append("--TeamCity");
                builder.Append(settings.TeamCity == TeamCityOutput.On ? "on" : "off");
            }

            if (settings.Options != null && settings.Options.Any())
            {
                foreach (var optionGroup in settings.Options.Select(x => new { x.Key, Options = x.Value }))
                {
                    foreach (var option in optionGroup.Options)
                    {
                        builder.Append(optionGroup.Key);
                        builder.Append(option);
                    }
                }
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Fixie";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "Fixie.Console.exe" };
        }
    }
}