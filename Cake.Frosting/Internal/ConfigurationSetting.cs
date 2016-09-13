// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Frosting.Internal
{
    internal sealed class ConfigurationSetting
    {
        public string Key { get; }

        public string Value { get; }

        public ConfigurationSetting(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
