using System;
using System.Collections.Generic;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class ModuleDirectiveProcessor : UriDirectiveProcessor
    {
        protected override IEnumerable<string> GetDirectiveNames()
        {
            return new[] { "#module" };
        }

        protected override void AddToContext(IScriptAnalyzerContext context, Uri uri)
        {
            var package = new PackageReference(uri);
            context.Current.Modules.Add(package);
        }
    }
}
