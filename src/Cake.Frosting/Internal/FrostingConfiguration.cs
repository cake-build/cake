// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Configuration;

namespace Cake.Frosting.Internal
{
    internal sealed class FrostingConfiguration : ICakeConfiguration
    {
        private readonly Dictionary<string, string> _lookup;

        public FrostingConfiguration(IEnumerable<FrostingConfigurationValue> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            _lookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var value in values)
            {
                _lookup[value.Key] = value.Value;
            }
        }

        public string GetValue(string key)
        {
            _lookup.TryGetValue(key, out var value);
            return value;
        }
    }
}
