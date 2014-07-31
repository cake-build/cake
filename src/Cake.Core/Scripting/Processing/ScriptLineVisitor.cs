using System;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Core.Scripting.Processing
{
    internal sealed class ScriptLineVisitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public ScriptLineVisitor(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public void Visit(ScriptProcessor processor, ScriptProcessorContext context, FilePath path, string line)
        {
            if (line.TrimStart().StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                var fragments = line.TrimStart().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var directive = fragments[0].TrimStart(new[] { '#' });

                if (directive.Equals("r", StringComparison.OrdinalIgnoreCase))
                {
                    VisitReference(context, path, fragments.Skip(1).ToArray());
                }
                else if (directive.Equals("l", StringComparison.OrdinalIgnoreCase))
                {
                    VisitLoad(processor, context, path, fragments.Skip(1).ToArray());
                }
                else
                {
                    throw new InvalidOperationException("Unknown pre-processor directive.");
                }
            }
            else
            {
                if (line.TrimStart().StartsWith("using", StringComparison.OrdinalIgnoreCase))
                {
                    var fragments = line.TrimStart().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var parts = fragments.Skip(1).ToArray();
                    VisitUsing(context, parts);
                }
                else
                {
                    // Append the line.
                    context.AppendScriptLine(line);   
                }
            }
        }

        private void VisitReference(ScriptProcessorContext context, FilePath currentScriptPath, string[] line)
        {
            if (line.Length != 1)
            {
                throw new InvalidOperationException("Invalid reference.");
            }

            var directoryPath = GetAbsoluteScriptDirectory(currentScriptPath);
            var referencePath = new FilePath(line[0].UnQuote());
            var absoluteReferencePath = referencePath.MakeAbsolute(directoryPath);
            
            context.AddReference(_fileSystem.Exist(absoluteReferencePath) 
                ? absoluteReferencePath.FullPath 
                : referencePath.FullPath);
        }

        private void VisitLoad(ScriptProcessor processor, ScriptProcessorContext context, FilePath currentScriptPath, string[] line)
        {
            if (line.Length != 1)
            {
                throw new InvalidOperationException("Invalid reference.");
            }

            var directoryPath = GetAbsoluteScriptDirectory(currentScriptPath);
            var scriptPath = new FilePath(line[0].UnQuote()).MakeAbsolute(directoryPath);

            // Process the file.
            processor.Process(scriptPath, context);
        }

        private static void VisitUsing(ScriptProcessorContext context, string[] line)
        {
            if (line.Length != 1)
            {
                throw new InvalidOperationException("Invalid reference.");
            }

            var @namespace = line[0].TrimEnd(new [] { ';' });
            context.AddNamespace(@namespace);
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
    }
}