using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    public sealed class ScriptProcessor : IScriptProcessor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public ScriptProcessor(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public ScriptProcessorResult Process(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // Get the script root.
            var root = GetAbsoluteScriptDirectory(path);

            // Read the source.
            var lines = ReadSource(path);                        

            // Find references in code.
            var code = new List<string>();
            var references = new HashSet<FilePath>(new PathComparer(_environment.IsUnix()));
            foreach (var line in lines)
            {
                if (line.StartsWith("#r", StringComparison.OrdinalIgnoreCase))
                {
                    var reference = ParseReference(line);
                    if (string.IsNullOrWhiteSpace(reference))
                    {
                        var message = string.Format("Reference directive is malformed: {0}", line);
                        throw new CakeException(message);
                    }

                    references.Add(reference);
                }
                else
                {
                    code.Add(line);   
                }                
            }

            // Return the result.
            return new ScriptProcessorResult(string.Join("\r\n", code),
                root, references);
        }

        private DirectoryPath GetAbsoluteScriptDirectory(FilePath scriptPath)
        {
            // Get the script location.
            var scriptLocation = scriptPath.GetDirectory();
            if (scriptLocation.IsRelative)
            {
                // Concatinate the starting working directory
                // with the script file path.
                scriptLocation = _environment.WorkingDirectory
                    .CombineWithFilePath(scriptPath).GetDirectory();
            }
            return scriptLocation;
        }

        private IEnumerable<string> ReadSource(FilePath path)
        {
            path = path.MakeAbsolute(_environment);

            // Get the file and make sure it exist.
            var file = _fileSystem.GetFile(path);
            if (!file.Exists)
            {
                var message = string.Format("Could not find script '{0}'.", path);
                throw new CakeException(message);
            }

            // Read the content from the file.
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(stream))
            {
                var code = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(code))
                {
                    return new string[] {};
                }
                return code.SplitLines();
            }
        }

        private static string ParseReference(string line)
        {
            // Find the space index.
            var index = line.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
            if (index < 2)
            {
                return null;
            }

            var reference = line.Substring(index + 1).UnQuote().Trim();
            if (reference.StartsWith("\""))
            {
                return null;
            }
            if (reference.EndsWith("\""))
            {
                return null;
            }

            return reference;
        }
    }
}
