// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Diagnostics.Formatting
{
    internal sealed class LiteralToken : FormatToken
    {
        private readonly string _text;

        public string Text
        {
            get { return _text; }
        }

        public LiteralToken(string text)
        {
            _text = text;
        }

        public override string Render(object[] args)
        {
            return _text;
        }
    }
}
