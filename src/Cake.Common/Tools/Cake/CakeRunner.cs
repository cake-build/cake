// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Polyfill;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Cake
{
    /// <summary>
    /// Cake out process runner
    /// </summary>
    public sealed class CakeRunner : Tool<CakeSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly IGlobber _globber;
        private static readonly IEnumerable<FilePath> _executingAssemblyToolPaths;

        /// <summary>
        /// Initializes static members of the <see cref="CakeRunner"/> class.
        /// </summary>
        static CakeRunner()
        {
            var entryAssembly = AssemblyHelper.GetExecutingAssembly();
            var executingAssemblyToolPath = entryAssembly.Location;
            _executingAssemblyToolPaths = new FilePath[] { executingAssemblyToolPath };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public CakeRunner(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber, IProcessRunner processRunner, IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
            _fileSystem = fileSystem;
            _globber = globber;
        }

        /// <summary>
        /// Executes supplied cake script in own process and supplied settings
        /// </summary>
        /// <param name="scriptPath">Path to script to execute</param>
        /// <param name="settings">optional cake settings</param>
        public void ExecuteScript(FilePath scriptPath, CakeSettings settings = null)
        {
            if (scriptPath == null)
            {
                throw new ArgumentNullException(nameof(scriptPath));
            }

            scriptPath = scriptPath.MakeAbsolute(_environment);
            if (!_fileSystem.GetFile(scriptPath).Exists)
            {
                throw new FileNotFoundException("Cake script file not found.", scriptPath.FullPath);
            }

            settings = settings ?? new CakeSettings();

            Run(settings, GetArguments(scriptPath, settings));
        }

        /// <summary>
        /// Executes supplied cake code expression in own process and supplied settings
        /// </summary>
        /// <param name="cakeExpression">Code expression to execute</param>
        /// <param name="settings">optional cake settings</param>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public void ExecuteExpression(string cakeExpression, CakeSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(cakeExpression))
            {
                throw new ArgumentNullException(nameof(cakeExpression));
            }
            DirectoryPath tempPath = _environment.GetEnvironmentVariable("TEMP") ?? "./";
            var tempScriptFile = _fileSystem.GetFile(tempPath
                .CombineWithFilePath(string.Format(CultureInfo.InvariantCulture, "{0}.cake", Guid.NewGuid()))
                .MakeAbsolute(_environment));
            try
            {
                using (var stream = tempScriptFile.OpenWrite())
                {
                    using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
                    {
                        streamWriter.WriteLine(cakeExpression);
                    }
                }
                ExecuteScript(tempScriptFile.Path.FullPath, settings);
            }
            finally
            {
                if (tempScriptFile.Exists)
                {
                    tempScriptFile.Delete();
                }
            }
        }

        private ProcessArgumentBuilder GetArguments(FilePath scriptPath, CakeSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.AppendQuoted(scriptPath.MakeAbsolute(_environment).FullPath);

            if (settings.Verbosity.HasValue)
            {
                builder.Append(string.Concat("-verbosity=", settings.Verbosity.Value.ToString()));
            }

            if (settings.Arguments != null)
            {
                foreach (var argument in settings.Arguments)
                {
                    builder.Append(string.Format(
                        CultureInfo.InvariantCulture,
                        "-{0}={1}",
                        argument.Key,
                        (argument.Value ?? string.Empty).Quote()));
                }
            }
            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "Cake";
        }

        /// <summary>
        /// Gets the name of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "Cake.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(CakeSettings settings)
        {
            const string homebrewCakePath = "/usr/local/Cellar/cake/";

            if (!_environment.Platform.IsUnix())
            {
                return _executingAssemblyToolPaths;
            }

            if (!_fileSystem.Exist(new DirectoryPath(homebrewCakePath)))
            {
                return _executingAssemblyToolPaths;
            }

            var files = _globber.GetFiles(homebrewCakePath + "**/Cake.exe");
            var filePaths = files as FilePath[] ?? files.ToArray();
            return filePaths.Length == 0
                ? _executingAssemblyToolPaths
                : filePaths.OrderByDescending(f => f.FullPath);
        }
    }
}