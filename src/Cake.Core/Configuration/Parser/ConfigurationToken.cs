// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.Configuration.Parser
{
    internal sealed class ConfigurationToken
    {
        public ConfigurationTokenKind Kind { get; }

        public string Value { get; }

        public ConfigurationToken(ConfigurationTokenKind kind, string value)
        {
            Kind = kind;
            Value = value;
        }
    }
}