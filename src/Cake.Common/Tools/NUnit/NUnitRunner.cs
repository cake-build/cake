using System;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// The NUnit unit test runner.
    /// </summary>
    public sealed class NUnitRunner : Tool<NUnitSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public NUnitRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
        {
            _environment = environment;
            _globber = globber;
        }

        /// <summary>
        /// Runs the tests in the specified assembly.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath assemblyPath, NUnitSettings settings)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException("assemblyPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(assemblyPath, settings), settings.ToolPath);
        }

        private ToolArgumentBuilder GetArguments(FilePath assemblyPath, NUnitSettings settings)
        {
            var builder = new ToolArgumentBuilder();

            // Add the assembly to build.
            builder.AppendQuotedText(assemblyPath.MakeAbsolute(_environment).FullPath);

            if(settings.Framework != null)
            {
                builder.AppendQuotedText("/framework:" + settings.Framework);
            }

            if (settings.Include != null)
            {
                builder.AppendQuotedText("/include:" + settings.Include);
            }

            if (settings.Exclude != null)
            {
                builder.AppendQuotedText("/exclude:" + settings.Exclude);
            }

            if (settings.Timeout.HasValue)
            {
                builder.AppendText("/timeout:" + settings.Timeout.Value);
            }

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                builder.AppendText("/noshadow");
            }

            if (settings.NoLogo)
            {
                builder.AppendText("/nologo");
            }

            if (settings.NoThread)
            {
                builder.AppendText("/nothread");
            }

            if (settings.StopOnError)
            {
                builder.AppendText("/stoponerror");
            }

            if (settings.Trace != null)
            {
                builder.AppendText("/trace:" + settings.Trace);
            }

            if (settings.ResultsFile != null && settings.NoResults)
            {
                throw new ArgumentException(
                    GetToolName() + ": You can't specify both a results file and set NoResults to true.");
            }

            if (settings.ResultsFile != null)
            {
                builder.AppendQuotedText(
                    string.Format(
                    CultureInfo.InvariantCulture,
                    "/result:{0}", settings.ResultsFile.FullPath));
            }
            else if (settings.NoResults)
            {
                builder.AppendQuotedText("/noresult");
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "NUnit";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(NUnitSettings settings)
        {
            const string expression = "./tools/**/nunit-console.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
