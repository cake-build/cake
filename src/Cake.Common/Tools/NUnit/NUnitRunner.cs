using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

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
        /// Runs the tests in the specified assembly, using the specified settings.
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

            Run(settings, GetArguments(new[] { assemblyPath }, settings), settings.ToolPath);
        }

        /// <summary>
        /// Runs the tests in the specified assemblies, using the specified settings.
        /// </summary>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> assemblyPaths, NUnitSettings settings)
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

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> assemblyPaths, NUnitSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Add the assemblies to build.
            foreach (var assemblyPath in assemblyPaths)
            {
                builder.AppendQuoted(assemblyPath.MakeAbsolute(_environment).FullPath);
            }

            if (settings.Framework != null)
            {
                builder.AppendQuoted("/framework:" + settings.Framework);
            }

            if (settings.Include != null)
            {
                builder.AppendQuoted("/include:" + settings.Include);
            }

            if (settings.Exclude != null)
            {
                builder.AppendQuoted("/exclude:" + settings.Exclude);
            }

            if (settings.Timeout.HasValue)
            {
                builder.Append("/timeout:" + settings.Timeout.Value);
            }

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                builder.Append("/noshadow");
            }

            if (settings.NoLogo)
            {
                builder.Append("/nologo");
            }

            if (settings.NoThread)
            {
                builder.Append("/nothread");
            }

            if (settings.StopOnError)
            {
                builder.Append("/stoponerror");
            }

            if (settings.Trace != null)
            {
                builder.Append("/trace:" + settings.Trace);
            }

            if (settings.OutputFile != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "/output:{0}", settings.OutputFile.MakeAbsolute(_environment).FullPath));
            }

            if (settings.ErrorOutputFile != null)
            {
                builder.AppendQuoted(string.Format(CultureInfo.InvariantCulture, "/err:{0}", settings.ErrorOutputFile.MakeAbsolute(_environment).FullPath));
            }

            if (settings.ResultsFile != null && settings.NoResults)
            {
                throw new ArgumentException(
                    GetToolName() + ": You can't specify both a results file and set NoResults to true.");
            }

            if (settings.ResultsFile != null)
            {
                builder.AppendQuoted(
                    string.Format(
                    CultureInfo.InvariantCulture,
                    "/result:{0}", settings.ResultsFile.MakeAbsolute(_environment).FullPath));
            }
            else if (settings.NoResults)
            {
                builder.AppendQuoted("/noresult");
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
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(NUnitSettings settings)
        {
            const string expression = "./tools/**/nunit-console.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
