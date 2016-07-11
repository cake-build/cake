// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Diagnostics;

namespace Cake.Scripting.Mono.CodeGen.Parsing
{
    [DebuggerDisplay("{Value,nq} ({Type})")]
    internal sealed class ScriptToken
    {
        private readonly ScriptTokenType _type;
        private readonly string _value;
        private readonly int _index;
        private readonly int _length;

        public ScriptTokenType Type
        {
            get { return _type; }
        }

        public string Value
        {
            get { return _value; }
        }

        public int Index
        {
            get { return _index; }
        }

        public int Length
        {
            get { return _length; }
        }

        public ScriptToken(ScriptTokenType type, string value, int index, int length)
        {
            _type = type;
            _value = value;
            _index = index;
            _length = length;
        }
    }
}
