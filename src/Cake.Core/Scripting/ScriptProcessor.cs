using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting.Processors;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Responsible for processing script files.
    /// </summary>
    public sealed class ScriptProcessor : IScriptProcessor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IEnumerable<LineProcessor> _lineProcessors;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptProcessor"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="nugetToolResolver">Nuget tool resolver</param>
        public ScriptProcessor(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, INuGetToolResolver nugetToolResolver)
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
            if (nugetToolResolver == null)
            {
                throw new ArgumentNullException("nugetToolResolver");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;

            _lineProcessors = new LineProcessor[]
            {
                new LoadDirectiveProcessor(_environment),
                new ReferenceDirectiveProcessor(_fileSystem, _environment),
                new UsingStatementProcessor(_environment),
                new AddInDirectiveProcessor(_fileSystem, _environment, _log, nugetToolResolver)
            };
        }

        /// <summary>
        /// Processes the specified script.
        /// </summary>
        /// <param name="path">The script path.</param>
        /// <param name="context">The context.</param>
        public void Process(FilePath path, ScriptProcessorContext context)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // Already processed this script?
            if (context.HasScriptBeenProcessed(path.FullPath))
            {
                _log.Debug("Skipping {0} since it's already been processed.", path.GetFilename().FullPath);
                return;
            }

            // Add the script.
            context.MarkScriptAsProcessed(path.MakeAbsolute(_environment).FullPath);

            // Read the source.
            _log.Debug("Processing {0}...", path.GetFilename().FullPath);
            var lines = ReadSource(path);

            // Iterate all lines in the script.
            var firstLine = true;
            foreach (var line in lines)
            {
                if (!_lineProcessors.Any(p => p.Process(this, context, path, line)))
                {
                    if (firstLine)
                    {
                        // Append the line directive for the script.
                        context.AppendScriptLine(string.Format(CultureInfo.InvariantCulture, "#line 1 \"{0}\"", path.GetFilename().FullPath));
                        firstLine = false;
                    }

                    context.AppendScriptLine(line);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private IEnumerable<string> ReadSource(FilePath path)
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
