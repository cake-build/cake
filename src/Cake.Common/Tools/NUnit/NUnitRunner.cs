﻿using System;
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

            // No shadow copy?
            if (!settings.ShadowCopy)
            {
                builder.AppendQuotedText("/noshadow");
            }

            // Output directory?
            if (settings.ResultsDirectory != null)
            {
                var assemblyFilename = assemblyPath.GetFilename().AppendExtension(".xml");
                var resultsPath = settings.ResultsDirectory.GetFilePath(assemblyFilename);

                var fullPath = resultsPath.ToString();

                // nunit-console treats a starting "/" as the root of the current drive
                if (fullPath.StartsWith("/"))
                {
                    // prepend "." to path to ensure nunit-console doesn't
                    // attempt to store the results in a non-existent path
                    // based on the root of the drive
                    fullPath = "." + fullPath;
                }

                builder.AppendQuotedText("/xml:" + fullPath);
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
