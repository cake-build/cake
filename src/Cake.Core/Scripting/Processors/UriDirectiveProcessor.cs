// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using System.Text;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal abstract class UriDirectiveProcessor : LineProcessor
    {
        protected UriDirectiveProcessor(ICakeEnvironment environment)
            : base(environment)
        {
        }

        protected abstract string GetDirectiveName();

        protected abstract void AddToContext(IScriptAnalyzerContext context, Uri uri);

        public sealed override bool Process(IScriptAnalyzerContext context, string line, out string replacement)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            replacement = null;

            var tokens = Split(line);
            var directive = tokens.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(directive))
            {
                if (directive.Equals(GetDirectiveName(), StringComparison.OrdinalIgnoreCase))
                {
                    if (tokens.Length >= 2)
                    {
                        // Try to parse an URI.
                        var uri = ParseUriFromTokens(tokens);
                        if (uri != null)
                        {
                            // Add the URI to the context.
                            AddToContext(context, uri);

                            // Return success.
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static Uri ParseUriFromTokens(string[] tokens)
        {
            Uri uri;
            if (!Uri.TryCreate(tokens[1].UnQuote(), UriKind.Absolute, out uri))
            {
                uri = CreateUriFromLegacyFormat(tokens);
            }
            return uri;
        }

        private static Uri CreateUriFromLegacyFormat(string[] tokens)
        {
            var builder = new StringBuilder();
            builder.Append("nuget:");

            // Fetch optional NuGet source.
            var source = tokens.Skip(2).Select(value => value.UnQuote()).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(source))
            {
                builder.Append(string.Concat(source, "/"));
            }

            // Fetch the addin NuGet ID.
            var id = tokens.Select(value => value.UnQuote()).Skip(1).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(id))
            {
                builder.Append("?package=" + id);
            }

            // Add the package definition for the addin.
            return new Uri(builder.ToString());
        }
    }
}
