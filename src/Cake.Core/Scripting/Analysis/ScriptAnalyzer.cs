// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting.Processors;

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
        private readonly LineProcessor[] _lineProcessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAnalyzer"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public ScriptAnalyzer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeLog log)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;

            _lineProcessors = new LineProcessor[]
            {
                new LoadDirectiveProcessor(_environment),
                new ReferenceDirectiveProcessor(_fileSystem, _environment),
                new UsingStatementProcessor(_environment),
                new AddInDirectiveProcessor(_environment),
                new ToolDirectiveProcessor(_environment),
                new ShebangProcessor(_environment),
                new BreakDirectiveProcessor(_environment)
            };
        }

        /// <summary>
        /// Analyzes the specified script path.
        /// </summary>
        /// <param name="path">The path to the script to analyze.</param>
        /// <returns>The script analysis result.</returns>
        public ScriptAnalyzerResult Analyze(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // Make the script path absolute.
            path = path.MakeAbsolute(_environment);

            // Create a new context.
            var context = new ScriptAnalyzerContext(
                _fileSystem, _environment, _log, AnalyzeCallback);

            // Analyze the script.
            context.Analyze(path);

            // Create and return the results.
            return new ScriptAnalyzerResult(
                context.Script,
                context.Lines);
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
        private void AnalyzeCallback(IScriptAnalyzerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var path = context.Script.Path;

            // Read the source.
            _log.Debug("Analyzing {0}...", path.FullPath);
            var lines = ReadLines(path);

            // Iterate all lines in the script.
            foreach (var line in lines)
            {
                string replacement = null;

                if (!_lineProcessors.Any(p => p.Process(context, line, out replacement)))
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
