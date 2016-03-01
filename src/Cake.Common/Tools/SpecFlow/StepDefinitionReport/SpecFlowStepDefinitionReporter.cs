using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.SpecFlow.StepDefinitionReport
{
    /// <summary>
    /// SpecFlow StepDefinition execution report runner.
    /// </summary>
    public sealed class SpecFlowStepDefinitionReporter : SpecFlowTool<SpecFlowStepDefinitionReportSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecFlowStepDefinitionReporter" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public SpecFlowStepDefinitionReporter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs SpecFlow StepDefinitionReport with the specified settings.
        /// </summary>
        /// <param name="projectFile">The project file path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath projectFile,
            SpecFlowStepDefinitionReportSettings settings)
        {
            if (projectFile == null)
            {
                throw new ArgumentNullException("projectFile");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Run the tool.
            Run(settings, GetArguments(settings, projectFile));
        }

        private ProcessArgumentBuilder GetArguments(
            SpecFlowStepDefinitionReportSettings settings,
            FilePath projectFile)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("stepdefinitionreport");

            // Set the project file
            builder.AppendQuoted(projectFile.MakeAbsolute(_environment).FullPath);

            // Set the bin folder.
            if (settings.BinFolder != null)
            {
                var binFolder = settings.BinFolder.MakeAbsolute(_environment);
                builder.AppendSwitch("/binFolder", ":", binFolder.FullPath.Quote());
            }

            // Get the SpecFlowSettings arguments
            AppendArguments(settings, builder);

            return builder;
        }
    }
}