using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.Restore
{
    /// <summary>
    /// The NuGet package restorer used to restore solution packages.
    /// </summary>
    public sealed class NuGetRestorer : Tool<NuGetRestoreSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetRestorer"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public NuGetRestorer(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
        {
            _environment = environment;
            _globber = globber;
        }

        /// <summary>
        /// Restores NuGet packages using the specified settings.
        /// </summary>
        /// <param name="targetFilePath">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Restore(FilePath targetFilePath, NuGetRestoreSettings settings)
        {
            if (targetFilePath == null)
            {
                throw new ArgumentNullException("targetFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(targetFilePath, settings), settings.ToolPath);
        }

        private ToolArgumentBuilder GetArguments(FilePath targetFilePath, NuGetRestoreSettings settings)
        {
            var builder = new ToolArgumentBuilder();

            builder.AppendText("restore");
            builder.AppendQuotedText(targetFilePath.MakeAbsolute(_environment).FullPath);

            // RequireConsent?
            if (settings.RequireConsent)
            {
                builder.AppendText("-RequireConsent");
            }

            // Packages Directory
            if (settings.PackagesDirectory != null)
            {
                builder.AppendText("-PackagesDirectory");
                builder.AppendQuotedText(settings.PackagesDirectory.MakeAbsolute(_environment).FullPath);
            }

            // List of package sources
            if (settings.Source != null && settings.Source.Count > 0)
            {
                builder.AppendText("-Source");
                builder.AppendQuotedText(string.Join(";", settings.Source));
            }

            // No Cache?
            if (settings.NoCache)
            {
                builder.AppendText("-NoCache");
            }

            // Disable Parallel Processing?
            if (settings.DisableParallelProcessing)
            {
                builder.AppendText("-DisableParallelProcessing");
            }

            // Verbosity?
            if (settings.Verbosity.HasValue)
            {
                builder.AppendText("-Verbosity");
                builder.AppendText(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            // Configuration file
            if (settings.ConfigFile != null)
            {
                builder.AppendText("-ConfigFile");
                builder.AppendQuotedText(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            builder.AppendText("-NonInteractive");

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "NuGet";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(NuGetRestoreSettings settings)
        {
            const string expression = "./tools/**/NuGet.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
