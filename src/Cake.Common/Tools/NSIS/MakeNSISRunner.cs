using System;
using System.Collections.Generic;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NSIS
{
    /// <summary>
    /// The runner which executes NSIS.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class MakeNSISRunner : Tool<MakeNSISSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MakeNSISRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public MakeNSISRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner, globber)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            if (globber == null)
            {
                throw new ArgumentNullException("globber");
            }

            _environment = environment;
        }

        /// <summary>
        /// Runs <c>makensis.exe</c> with the specified script files and settings.
        /// </summary>
        /// <param name="scriptFile">The script file (<c>.nsi</c>) to compile.</param>
        /// <param name="settings">The settings.</param>
        public void Run(FilePath scriptFile, MakeNSISSettings settings)
        {
            if (scriptFile == null)
            {
                throw new ArgumentNullException("scriptFile");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(scriptFile, settings), settings.ToolPath);
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "MakeNSIS";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "makensis.exe" };
        }

        private ProcessArgumentBuilder GetArguments(FilePath scriptFile, MakeNSISSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // Defines (/Ddefine[=value]
            if (settings.Defines != null)
            {
                foreach (var item in settings.Defines)
                {
                    builder.Append(string.Format(CultureInfo.InvariantCulture, "/D{0}{1}", item.Key, string.IsNullOrEmpty(item.Value) ? string.Empty : "=" + item.Value));
                }
            }

            // NoChangeDirectory (/NOCD)
            if (settings.NoChangeDirectory)
            {
                builder.Append("/NOCD");
            }

            // NoConfig (/NOCONFIG)
            if (settings.NoConfig)
            {
                builder.Append("/NOCONFIG");
            }

            // Script file
            builder.Append(scriptFile.MakeAbsolute(_environment).FullPath);

            return builder;
        }
    }
}