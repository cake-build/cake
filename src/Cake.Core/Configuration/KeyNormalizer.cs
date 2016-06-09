// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Configuration
{
    internal static class KeyNormalizer
    {
        public static string Normalize(string key)
        {
            key = key.ToUpperInvariant();
            return key.Trim();
        }
    }
}
