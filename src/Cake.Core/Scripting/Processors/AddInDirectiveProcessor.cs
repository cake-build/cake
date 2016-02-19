using System;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class AddInDirectiveProcessor : UriDirectiveProcessor
    {
        public AddInDirectiveProcessor(ICakeEnvironment environment)
            : base(environment)
        {
        }

        protected override string GetDirectiveName()
        {
            return "#addin";
        }

        protected override void AddToContext(IScriptAnalyzerContext context, Uri uri)
        {
            var package = new PackageReference(uri);
            context.Script.Addins.Add(package);
        }
    }
}