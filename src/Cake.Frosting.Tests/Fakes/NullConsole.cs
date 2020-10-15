// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Frosting.Tests.Fakes
{
    internal sealed class NullConsole : IConsole
    {
        public void Write(string format, params object[] arg)
        {
        }

        public void WriteLine(string format, params object[] arg)
        {
        }

        public void WriteError(string format, params object[] arg)
        {
        }

        public void WriteErrorLine(string format, params object[] arg)
        {
        }

        public void ResetColor()
        {
        }

        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public bool SupportAnsiEscapeCodes => false;
    }
}
