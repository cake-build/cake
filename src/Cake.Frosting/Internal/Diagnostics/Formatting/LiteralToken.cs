// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Frosting.Internal.Diagnostics.Formatting
{
    internal sealed class LiteralToken : FormatToken
    {
        public string Text { get; }

        public LiteralToken(string text)
        {
            Text = text;
        }

        public override string Render(object[] args)
        {
            return Text;
        }
    }
}