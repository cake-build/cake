// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Configuration.Parser
{
    internal sealed class ConfigurationToken
    {
        private readonly ConfigurationTokenKind _kind;
        private readonly string _value;

        public ConfigurationTokenKind Kind
        {
            get { return _kind; }
        }

        public string Value
        {
            get { return _value; }
        }

        public ConfigurationToken(ConfigurationTokenKind kind, string value)
        {
            _kind = kind;
            _value = value;
        }
    }
}
