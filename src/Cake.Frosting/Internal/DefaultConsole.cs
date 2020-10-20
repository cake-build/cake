// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Frosting.Internal
{
    internal sealed class DefaultConsole : IConsole
    {
        /// <inheritdoc/>
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <inheritdoc/>
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <inheritdoc/>
        public bool SupportAnsiEscapeCodes => false;

        /// <inheritdoc/>
        public void Write(string format, params object[] arg)
        {
            Console.Write(format, arg);
        }

        /// <inheritdoc/>
        public void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        /// <inheritdoc/>
        public void WriteError(string format, params object[] arg)
        {
            Console.Error.Write(format, arg);
        }

        /// <inheritdoc/>
        public void WriteErrorLine(string format, params object[] arg)
        {
            Console.Error.WriteLine(format, arg);
        }

        /// <inheritdoc/>
        public void ResetColor()
        {
            Console.ResetColor();
        }
    }
}