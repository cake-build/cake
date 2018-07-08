// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal abstract class UriDirectiveProcessor : LineProcessor
    {
        private readonly Regex _uriPrefixPattern;

        protected abstract IEnumerable<string> GetDirectiveNames();

        protected abstract void AddToContext(IScriptAnalyzerContext context, Uri uri);

        protected UriDirectiveProcessor()
        {
            _uriPrefixPattern = new Regex("^([a-zA-Z]{2,}:)");
        }

        public sealed override bool Process(IScriptAnalyzerContext context, string line, out string replacement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            replacement = null;

            var tokens = Split(line);
            var directive = tokens.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(directive))
            {
                if (GetDirectiveNames().Any(n => directive.Equals(n, StringComparison.OrdinalIgnoreCase)))
                {
                    if (tokens.Length >= 2)
                    {
                        // Try to parse an URI.
                        var uri = ParseUriFromTokens(tokens);
                        if (uri != null)
                        {
                            try
                            {
                                // Add the URI to the context.
                                AddToContext(context, uri);
                            }
                            catch (Exception e)
                            {
                                // Add any errors to context
                                context.AddScriptError(e.Message);
                            }

                            // Return success.
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private Uri ParseUriFromTokens(string[] tokens)
        {
            Uri uri;
            if (IsUriFromLegacyPattern(tokens))
            {
                uri = CreateUriFromLegacyFormat(tokens);
            }
            else
            {
                uri = new Uri(tokens[1].UnQuote(), UriKind.Absolute);
            }
            return uri;
        }

        private bool IsUriFromLegacyPattern(string[] tokens)
        {
            return !_uriPrefixPattern.IsMatch(tokens[1].UnQuote());
        }

        protected virtual Uri CreateUriFromLegacyFormat(string[] tokens)
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