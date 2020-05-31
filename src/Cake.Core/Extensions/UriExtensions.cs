// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Extensions for <see cref="System.Uri"/>.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Extracts query string of <see cref="System.Uri"/>.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>Collection of parameters and their values.</returns>
        public static IReadOnlyDictionary<string, IReadOnlyList<string>> GetQueryString(this Uri uri)
        {
            var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            if (uri != null)
            {
                var query = uri.Query.TrimStart('?');
                var parts = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var pair = part.Split(new[] { '=' }, StringSplitOptions.None);

                    if (pair.Length == 1)
                    {
                        if (!result.ContainsKey(pair[0]))
                        {
                            result[pair[0]] = new List<string>();
                        }
                    }
                    else if (pair.Length == 2)
                    {
                        if (!result.ContainsKey(pair[0]))
                        {
                            result[pair[0]] = new List<string>();
                        }
                        result[pair[0]].Add(pair[1]);
                    }
                    else
                    {
                        throw new CakeException("Could not parse query string.");
                    }
                }
            }
            return result.ToDictionary(kv => kv.Key, kv => kv.Value as IReadOnlyList<string>, StringComparer.OrdinalIgnoreCase);
        }
    }
}