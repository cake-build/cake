using System;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class NugetScriptDirectiveProcessor : UriDirectiveProcessor
    {
        public const string DirectiveName = "#nuscript";

        public NugetScriptDirectiveProcessor(ICakeEnvironment environment)
            : base(environment)
        {
        }

        protected override string GetDirectiveName()
        {
            return DirectiveName;
        }

        protected override void AddToContext(IScriptAnalyzerContext context, Uri uri)
        {
            var package = new PackageReference(uri);
            context.Script.NugetScripts.Add(package);
        }
    }
}