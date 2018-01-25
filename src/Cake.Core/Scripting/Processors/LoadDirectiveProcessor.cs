// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors.Loading;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class LoadDirectiveProcessor : UriDirectiveProcessor
    {
        private readonly IEnumerable<ILoadDirectiveProvider> _providers;

        public LoadDirectiveProcessor(IEnumerable<ILoadDirectiveProvider> providers)
        {
            _providers = providers ?? Enumerable.Empty<ILoadDirectiveProvider>();
        }

        protected override IEnumerable<string> GetDirectiveNames()
        {
            return new[] { "#l", "#load" };
        }

        protected override Uri CreateUriFromLegacyFormat(string[] tokens)
        {
            var builder = new StringBuilder();
            builder.Append("local:");

            var id = tokens.Select(value => value.UnQuote()).Skip(1).FirstOrDefault();
            builder.Append("?path=" + id);

            return new Uri(builder.ToString());
        }

        protected override void AddToContext(IScriptAnalyzerContext context, Uri uri)
        {
            var reference = new LoadReference(uri);

            foreach (var provider in _providers)
            {
                if (provider.CanLoad(context, reference))
                {
                    provider.Load(context, reference);
                }
            }
        }
    }
}