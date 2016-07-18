using System;
using Cake.Core;
using Cake.Core.Factories;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;

namespace Cake.LoadRemote.Module
{
    //[CakeProcessor]
    public sealed class LoadRemoteProcessor : UriDirectiveProcessor
    {
        private readonly ICakeComponentFactory _componentFactory;

        public LoadRemoteProcessor(ICakeEnvironment environment) : base(environment)
        {
            _componentFactory = new CakeComponentFactory();
        }

        protected override string GetDirectiveName()
        {
            return Constants.DirectiveName;
        }

        protected override void AddToContext(IScriptAnalyzerContext context, Uri uri)
        {
            var package = _componentFactory.CreatePackageReference(uri);
            context.Script.ProcessorValues.Add(this, package);
        }
    }
}