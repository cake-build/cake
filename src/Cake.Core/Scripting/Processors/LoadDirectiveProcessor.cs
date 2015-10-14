using System;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class LoadDirectiveProcessor : LineProcessor
    {
        public LoadDirectiveProcessor(ICakeEnvironment environment) 
            : base(environment)
        {
        }

        public override bool Process(IScriptAnalyzerContext context, string line)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var tokens = Split(line);
            if (tokens.Length <= 0)
            {
                return false;
            }

            if (!tokens[0].Equals("#l", StringComparison.Ordinal) &&
                !tokens[0].Equals("#load", StringComparison.Ordinal))
            {
                return false;
            }

            var directoryPath = GetAbsoluteDirectory(context.Script.Path);
            var scriptPath = new FilePath(tokens[1].UnQuote()).MakeAbsolute(directoryPath);

            context.Analyze(scriptPath);

            return true;
        }
    }
}