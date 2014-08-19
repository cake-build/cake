using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// The WiX Light runner.
    /// </summary>
    public sealed class LightRunner : Tool<LightSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The Cake environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public LightRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner)
            : base(fileSystem, environment, processRunner)
        {
            if (environment == null) throw new ArgumentNullException("environment");
            if (globber == null) throw new ArgumentNullException("globber");

            _environment = environment;
            _globber = globber;
        }

        /// <summary>
        /// Runs Light with the specified input object files and settings.
        /// </summary>
        /// <param name="objectFiles">The object files (.wixobj).</param>
        /// <param name="settings">The settings.</param>
        public void Run(IEnumerable<FilePath> objectFiles, LightSettings settings)
        {
            if (objectFiles == null)
            {
                throw new ArgumentNullException("objectFiles");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var objectFilesArray = objectFiles as FilePath[] ?? objectFiles.ToArray();
            if (!objectFilesArray.Any())
            {
                throw new ArgumentException("No object files provided.", "objectFiles");
            }

            Run(settings, GetArguments(objectFilesArray, settings), settings.ToolPath);
        }

        private ToolArgumentBuilder GetArguments(IEnumerable<FilePath> objectFiles, LightSettings settings)
        {
            var builder = new ToolArgumentBuilder();

            // Add defines
            if (settings.Defines != null && settings.Defines.Any())
            {
                var defines = settings.Defines.Select(define => string.Format(CultureInfo.InvariantCulture, "-d{0}={1}", define.Key, define.Value));
                foreach (var define in defines)
                {
                    builder.AppendText(define);
                }       
            }

            // Add extensions
            if (settings.Extensions != null && settings.Extensions.Any())
            {
                var extensions = settings.Extensions.Select(extension => string.Format(CultureInfo.InvariantCulture, "-ext {0}", extension));
                foreach (var extension in extensions)
                {
                    builder.AppendText(extension);
                }
            }

            // No logo
            if (settings.NoLogo)
            {
                builder.AppendText("-nologo");
            }

            // Output file
            if (settings.OutputFile != null && !string.IsNullOrEmpty(settings.OutputFile.FullPath))
            {
                builder.AppendText("-o");
                builder.AppendQuotedText(settings.OutputFile.MakeAbsolute(_environment).FullPath);
            }

            // Raw arguments
            if (!string.IsNullOrEmpty(settings.RawArguments))
            {
                builder.AppendText(settings.RawArguments);
            }

            // Object files (.wixobj)
            foreach (var objectFile in objectFiles.Select(file => file.MakeAbsolute(_environment).FullPath))
            {
                builder.AppendQuotedText(objectFile);
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Light";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(LightSettings settings)
        {
            const string expression = "./tools/**/light.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
