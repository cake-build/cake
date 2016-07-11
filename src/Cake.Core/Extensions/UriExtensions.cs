// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    internal static class UriExtensions
    {
        public static IDictionary<string, string> GetQueryString(this Uri uri)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (uri != null)
            {
                var query = uri.Query.TrimStart('?');
                var parts = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var pair = part.Split(new[] { '=' }, StringSplitOptions.None);
                    switch (pair.Length)
                    {
                        case 1:
                            result[pair[0]] = string.Empty;
                            break;
                        case 2:
                            result[pair[0]] = pair[1];
                            break;
                        default:
                            throw new CakeException("Could not parse query string.");
                    }
                }
            }
            return result;
        }
    }
}
