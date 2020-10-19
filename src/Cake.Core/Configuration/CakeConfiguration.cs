// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core.Configuration
{
    /// <summary>
    /// The default implementation of the Cake configuration.
    /// </summary>
    public sealed class CakeConfiguration : ICakeConfiguration
    {
        private readonly Dictionary<string, string> _lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeConfiguration"/> class.
        /// </summary>
        /// <param name="lookup">The initial configuration table.</param>
        public CakeConfiguration(IDictionary<string, string> lookup)
        {
            _lookup = new Dictionary<string, string>(lookup, StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public string GetValue(string key)
        {
            key = KeyNormalizer.Normalize(key);
            return _lookup.ContainsKey(key)
                ? _lookup[key] : null;
        }
    }
}