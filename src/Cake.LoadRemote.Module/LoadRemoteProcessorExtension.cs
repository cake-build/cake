using System;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;

namespace Cake.LoadRemote.Module
{
    public class LoadRemoteProcessorExtension : IProcessorExtension
    {
        public bool Process(IScriptAnalyzerContext analyzer, string line, out string replacement)
        {
            // How to Support both "#l" and "#load", replace to #nuscript and process it with the NugetScriptDirectiveProcessor.
            line = line.Replace("#l", Constants.DirectiveName);
            line = line.Replace("#load", Constants.DirectiveName);

            // Set the replacement line to the modified line.
            replacement = string.Concat("// ", line);

            return true;
        }

        public bool CanProcessDirective(string alias, string value)
        {
            if ((alias.Equals("#l", StringComparison.Ordinal) || alias.Equals("#load", StringComparison.Ordinal)) && value.Contains("nuget:"))
            {
                return true;
            }

            return false;
        }
    }
}
