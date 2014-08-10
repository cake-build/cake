using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// The xUnit unit test runner.
    /// </summary>
    public sealed class XUnitRunner : Tool<XUnitSettings>
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
        public XUnitRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner runner)
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
        public void Run(FilePath assemblyPath, XUnitSettings settings)
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
                if (settings.XmlReport)
                {
                    throw new CakeException("Cannot generate XML report when no output directory has been set.");
                }
            }

            Run(settings, GetArguments(assemblyPath, settings), settings.ToolPath);
        }

        private ToolArgumentBuilder GetArguments(FilePath assemblyPath, XUnitSettings settings)
        {
            var builder = new ToolArgumentBuilder();

            // Add the assembly to build.
            builder.AppendQuotedText(assemblyPath.MakeAbsolute(_environment).FullPath);

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                builder.AppendQuotedText("/noshadow");
            }

            // Generate HTML report?
            if (settings.HtmlReport)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".html");
                var outputPath = settings.OutputDirectory.GetFilePath(assemblyFilename);

                builder.AppendQuotedText("/html");
                builder.AppendQuotedText(outputPath.FullPath);
            }

            // Generate XML report?
            if (settings.XmlReport)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".xml");
                var outputPath = settings.OutputDirectory.GetFilePath(assemblyFilename);

                builder.AppendQuotedText("/xml");
                builder.AppendQuotedText(outputPath.FullPath);
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "xUnit";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(XUnitSettings settings)
        {
            const string expression = "./tools/**/xunit.console.clr4.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
