using System;
using System.Linq;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class ToolDirectiveProcessor : LineProcessor
    {
        public ToolDirectiveProcessor(ICakeEnvironment environment) 
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
            var directive = tokens.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(directive))
            {
                return false;
            }

            if (!directive.Equals("#tool", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Fetch the tool NuGet ID.
            var toolId = tokens
                .Select(value => value.UnQuote())
                .Skip(1).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(toolId))
            {
                return false;
            }

            // Fetch optional NuGet source.
            var source = tokens
                .Skip(2)
                .Select(value => value.UnQuote())
                .FirstOrDefault();

            // Add the package definition for the tool.
            context.Script.Tools.Add(new NuGetPackage(toolId)
            {
                Source = source
            });

            return true;
        }
    }
}