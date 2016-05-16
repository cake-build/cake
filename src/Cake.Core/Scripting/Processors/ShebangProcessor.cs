using System;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class ShebangProcessor : LineProcessor
    {
        public ShebangProcessor(ICakeEnvironment environment) 
            : base(environment)
        {
        }

        public override bool Process(IScriptAnalyzerContext processor, string line, out string replacement)
        {
            replacement = null;

            // Remove all shebang lines that we encounter.
            return line.StartsWith("#!", StringComparison.OrdinalIgnoreCase);
        }
    }
}
