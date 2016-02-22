using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// The xUnit.net (v1) test runner.
    /// </summary>
    public sealed class XUnitRunner : Tool<XUnitSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="runner">The runner.</param>
        public XUnitRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner runner)
            : base(fileSystem, environment, runner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs the tests in the specified assembly.
        /// </summary>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> assemblyPaths, XUnitSettings settings)
        {
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
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
                if (settings.XmlReport)
                {
                    throw new CakeException("Cannot generate XML report when no output directory has been set.");
                }
            }

            var assemblies = assemblyPaths as FilePath[] ?? assemblyPaths.ToArray();
            Run(settings, GetArguments(assemblies, settings));
        }

        private ProcessArgumentBuilder GetArguments(IReadOnlyList<FilePath> assemblyPaths, XUnitSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assemblies to test.
            foreach (var assembly in assemblyPaths)
            {
                builder.AppendQuoted(assembly.MakeAbsolute(_environment).FullPath);
            }

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                builder.AppendQuoted("/noshadow");
            }

            // Generate HTML report?
            if (settings.HtmlReport)
            {
                var reportFileName = XUnitRunnerUtilities.GetReportFileName(assemblyPaths);
                var assemblyFilename = reportFileName.AppendExtension(".html");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.AppendQuoted("/html");
                builder.AppendQuoted(outputPath.FullPath);
            }

            // Generate XML report?
            if (settings.XmlReport)
            {
                var reportFileName = XUnitRunnerUtilities.GetReportFileName(assemblyPaths);
                var assemblyFilename = reportFileName.AppendExtension(".xml");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.AppendQuoted("/xml");
                builder.AppendQuoted(outputPath.FullPath);
            }

            // Silent mode?
            if (settings.Silent)
            {
                builder.Append("/silent");
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "xUnit.net (v1)";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "xunit.console.clr4.exe" };
        }
    }
}