using System;
using System.Linq;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class AddInDirectiveProcessor : LineProcessor
    {
        public AddInDirectiveProcessor(ICakeEnvironment environment)  
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

            if (!directive.Equals("#addin", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Fetch the addin NuGet ID.
            var addInId = tokens
                .Select(value => value.UnQuote())
                .Skip(1).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(addInId))
            {
                return false;
            }

            // Fetch optional NuGet source.
            var source = tokens
                .Skip(2)
                .Select(value => value.UnQuote())
                .FirstOrDefault();

            // Add the package definition for the addin.
            context.Script.Addins.Add(new NuGetPackage(addInId)
            {
                Source = source
            });

            // Return success.
            return true;
        }
    }
}