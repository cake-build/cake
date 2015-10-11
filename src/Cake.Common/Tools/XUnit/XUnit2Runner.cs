using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// The xUnit.net v2 test runner.
    /// </summary>
    public sealed class XUnit2Runner : Tool<XUnit2Settings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnit2Runner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="runner">The runner.</param>
        public XUnit2Runner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner runner)
            : base(fileSystem, environment, runner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assembly.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath assemblyPath, XUnit2Settings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Make sure we got output directory set when generating reports.
            if (settings.OutputDirectory == null || string.IsNullOrWhiteSpace(settings.OutputDirectory.FullPath))
            {
                if (settings.HtmlReport)
                {
                    throw new CakeException("Cannot generate HTML report when no output directory has been set.");
                }
                if (settings.XmlReport || settings.XmlReportV1)
                {
                    throw new CakeException("Cannot generate XML report when no output directory has been set.");
                }
            }

            Run(settings, GetArguments(assemblyPath, settings), settings.ToolPath);
        }

        private ProcessArgumentBuilder GetArguments(FilePath assemblyPath, XUnit2Settings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Get the absolute path to the assembly.
            assemblyPath = assemblyPath.MakeAbsolute(_environment);

            // Add the assembly to build.
            builder.AppendQuoted(assemblyPath.FullPath);

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                builder.Append("-noshadow");
            }

            // No app domain?
            if (settings.NoAppDomain)
            {
                builder.Append("-noappdomain");
            }

            // Generate HTML report?
            if (settings.HtmlReport)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".html");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.Append("-html");
                builder.AppendQuoted(outputPath.FullPath);
            }

            // Generate XML report?
            if (settings.XmlReport || settings.XmlReportV1)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".xml");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.Append(settings.XmlReportV1 ? "-xmlv1" : "-xml");
                builder.AppendQuoted(outputPath.FullPath);
            }

            // parallelize test execution?
            if (settings.Parallelism != ParallelismOption.None)
            {
                builder.Append("-parallel " + settings.Parallelism.ToString().ToLowerInvariant());
            }

            // max thread count for collection parallelization
            if (settings.MaxThreads.HasValue)
            {
                if (settings.MaxThreads.Value == 0)
                {
                    builder.Append("-maxthreads unlimited");
                }
                else
                {
                    builder.Append("-maxthreads " + settings.MaxThreads.Value);
                }
            }

            foreach (var trait in settings.TraitsToInclude
                .SelectMany(pair => pair.Value.Select(v => new { Name = pair.Key, Value = v })))
            {
                builder.Append("-trait \"{0}={1}\"", trait.Name, trait.Value);
            }

            foreach (var trait in settings.TraitsToExclude
                .SelectMany(pair => pair.Value.Select(v => new { Name = pair.Key, Value = v })))
            {
                builder.Append("-notrait \"{0}={1}\"", trait.Name, trait.Value);
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "xUnit.net (v2)";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override System.Collections.Generic.IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "xunit.console.exe" };
        }
    }
}