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
        private readonly IGlobber _globber;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitRunner"/> class.
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="runner">The runner.</param>
        public XUnit2Runner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner runner)
            : base(fileSystem, environment, runner)
        {
            _environment = environment;
            _globber = globber;
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
                builder.AppendQuoted("-noshadow");
            }

            // Generate HTML report?
            if (settings.HtmlReport)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".html");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.AppendQuoted("-html");
                builder.AppendQuoted(outputPath.FullPath);
            }

            // Generate XML report?
            if (settings.XmlReport || settings.XmlReportV1)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".xml");
                var outputPath = settings.OutputDirectory.MakeAbsolute(_environment).GetFilePath(assemblyFilename);

                builder.AppendQuoted(settings.XmlReportV1 ? "-xmlv1" : "-xml");
                builder.AppendQuoted(outputPath.FullPath);
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
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(XUnit2Settings settings)
        {
            return _globber.GetFiles("./tools/**/xunit.console.exe").FirstOrDefault();
        }
    }
}
