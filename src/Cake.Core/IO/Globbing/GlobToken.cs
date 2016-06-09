// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobToken
    {
        private readonly GlobTokenKind _kind;
        private readonly string _value;

        public GlobTokenKind Kind
        {
            get { return _kind; }
        }

        public string Value
        {
            get { return _value; }
        }

        public GlobToken(GlobTokenKind kind, string value)
        {
            _kind = kind;
            _value = value;
        }
    }
}
