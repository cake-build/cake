// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.Configuration;

namespace Cake.Testing
{
    /// <summary>
    /// Implementation of a fake <see cref="ICakeConfiguration"/>.
    /// </summary>
    public sealed class FakeConfiguration : ICakeConfiguration
    {
        private readonly Dictionary<string, string> _lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeConfiguration"/> class.
        /// </summary>
        public FakeConfiguration()
        {
            _lookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the value that corresponds to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value for the specified key, or <c>null</c> if key doesn't exists.
        /// </returns>
        public string GetValue(string key)
        {
            string value;
            _lookup.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Sets the value that corresponds to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, string value)
        {
            _lookup[key] = value;
        }
    }
}
