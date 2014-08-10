using System;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Push
{
    /// <summary>
    /// The NuGet package pusher.
    /// </summary>
    public sealed class NuGetPusher : Tool<NuGetPushSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPusher"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public NuGetPusher(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
        {
            _environment = environment;
            _globber = globber;
        }

        /// <summary>
        /// Pushes a NuGet package to a NuGet server and publishes it.
        /// </summary>
        /// <param name="packageFilePath">The package file path.</param>
        /// <param name="settings">The settings.</param>
        public void Push(FilePath packageFilePath, NuGetPushSettings settings)
        {
            if (packageFilePath == null)
            {
                throw new ArgumentNullException("packageFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(packageFilePath, settings), settings.ToolPath);
        }

        private ToolArgumentBuilder GetArguments(FilePath packageFilePath, NuGetPushSettings settings)
        {
            var builder = new ToolArgumentBuilder();
            builder.AppendText("push");

            builder.AppendQuotedText(packageFilePath.MakeAbsolute(_environment).FullPath);

            if (settings.ApiKey != null)
            {
                builder.AppendText(settings.ApiKey);
            }

            builder.AppendText("-NonInteractive");

            if (settings.ConfigFile != null)
            {
                builder.AppendText("-ConfigFile");
                builder.AppendQuotedText(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            if (settings.Source != null)
            {
                builder.AppendText("-Source");
                builder.AppendQuotedText(settings.Source);
            }

            if (settings.Timeout != null)
            {
                builder.AppendText("-Timeout");
                builder.AppendText(Convert.ToInt32(settings.Timeout.Value.TotalSeconds).ToString(CultureInfo.InvariantCulture));
            }

            if (settings.Verbosity != null)
            {
                builder.AppendText("-Verbosity");
                builder.AppendText(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

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
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(NuGetPushSettings settings)
        {
            const string expression = "./tools/**/NuGet.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
