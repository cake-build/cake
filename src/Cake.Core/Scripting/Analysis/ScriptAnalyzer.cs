// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting.Processors;
using Cake.Core.Scripting.Processors.Loading;

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// The script analyzer.
    /// </summary>
    public sealed class ScriptAnalyzer : IScriptAnalyzer
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly LineProcessor[] _defaultProcessors;
        private readonly LineProcessor[] _moduleProcessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAnalyzer"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="providers">The load directive providers.</param>
        public ScriptAnalyzer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeLog log,
            IEnumerable<ILoadDirectiveProvider> providers)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;

            _defaultProcessors = new LineProcessor[]
            {
                new LoadDirectiveProcessor(providers),
                new ReferenceDirectiveProcessor(_fileSystem, _environment),
                new UsingStatementProcessor(),
                new AddInDirectiveProcessor(),
                new ToolDirectiveProcessor(),
                new ShebangProcessor(),
                new BreakDirectiveProcessor(),
                new DefineDirectiveProcessor(),
                new ModuleDirectiveProcessor()
            };

            _moduleProcessors = new LineProcessor[]
            {
                new LoadDirectiveProcessor(providers),
                new ModuleDirectiveProcessor()
            };
        }

        /// <inheritdoc/>
        public ScriptAnalyzerResult Analyze(FilePath path, ScriptAnalyzerSettings settings)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            // Make the script path absolute.
            path = path.MakeAbsolute(_environment);

            // Get the correct callback.
            var callback = settings.Mode == ScriptAnalyzerMode.Modules
                ? ModuleAnalyzeCallback
                : (Action<IScriptAnalyzerContext>)AnalyzeCallback;

            // Create a new context.
            var context = new ScriptAnalyzerContext(
                _fileSystem, _environment,
                _log, callback, path);

            // Analyze the script.
            context.Analyze(path);

            // Create and return the results.
            return new ScriptAnalyzerResult(
                context.Current,
                context.Lines,
                context.Errors);
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
        private void ModuleAnalyzeCallback(IScriptAnalyzerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Iterate all lines in the script.
            var lines = ReadLines(context.Current.Path);
            foreach (var line in lines)
            {
                foreach (var processor in _defaultProcessors)
                {
                    if (processor.Process(context, _environment.ExpandEnvironmentVariables(line), out var _))
                    {
                        break;
                    }
                }
            }
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
        private void AnalyzeCallback(IScriptAnalyzerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var path = context.Current.Path;

            // Read the source.
            _log.Debug("Analyzing {0}...", path.FullPath);
            var lines = ReadLines(path);

            // Iterate all lines in the script.
            foreach (var line in lines)
            {
                string replacement = null;

                if (!_defaultProcessors.Any(p => p.Process(context, _environment.ExpandEnvironmentVariables(line), out replacement)))
                {
                    context.AddScriptLine(line);
                }
                else
                {
                    // Add replacement or comment out processed lines to keep line data.
                    context.AddScriptLine(replacement ?? string.Concat("// ", line));
                }
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private IEnumerable<string> ReadLines(FilePath path)
        {
            path = path.MakeAbsolute(_environment);

            // Get the file and make sure it exist.
            var file = _fileSystem.GetFile(path);
            if (!file.Exists)
            {
                var message = string.Format(CultureInfo.InvariantCulture, "Could not find script '{0}'.", path);
                throw new CakeException(message);
            }

            // Read the content from the file.
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(stream))
            {
                var code = reader.ReadToEnd();
                return string.IsNullOrWhiteSpace(code)
                    ? new string[] { }
                    : code.SplitLines();
            }
        }
    }
}