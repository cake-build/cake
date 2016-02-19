using System;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class ToolDirectiveProcessor : UriDirectiveProcessor
    {
        public ToolDirectiveProcessor(ICakeEnvironment environment)
            : base(environment)
        {
        }

        protected override string GetDirectiveName()
        {
            return "#tool";
        }

        protected override void AddToContext(IScriptAnalyzerContext context, Uri uri)
        {
            var package = new PackageReference(uri);
            context.Script.Tools.Add(package);
        }
    }
}