// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Xunit;

namespace Cake.Core.Tests.Unit;

public sealed class IConsoleTests
{
    private sealed class RecordingConsole : IConsole
    {
        public ConsoleColor ForegroundColor { get; set; }

        public ConsoleColor BackgroundColor { get; set; }

        public bool SupportAnsiEscapeCodes => false;

        public string LastFormat { get; private set; }

        public object[] LastArgs { get; private set; }

        public void Write(string format, params object[] arg)
        {
            LastFormat = format;
            LastArgs = arg;
        }

        public void WriteLine(string format, params object[] arg)
        {
            LastFormat = format;
            LastArgs = arg;
        }

        public void WriteError(string format, params object[] arg)
        {
            LastFormat = format;
            LastArgs = arg;
        }

        public void WriteErrorLine(string format, params object[] arg)
        {
            LastFormat = format;
            LastArgs = arg;
        }

        public void ResetColor()
        {
        }
    }

    [Fact]
    public void Default_Write_Should_Pass_Literal_As_Format_Argument()
    {
        // Given
        var console = new RecordingConsole();

        // When
        ((IConsole)console).Write("{0}");

        // Then
        Assert.Equal("{0}", console.LastFormat);
        Assert.Equal(["{0}"], console.LastArgs);
        Assert.Equal("{0}", string.Format(CultureInfo.InvariantCulture, console.LastFormat, console.LastArgs));
    }

    [Fact]
    public void Default_WriteLine_Should_Pass_Literal_As_Format_Argument()
    {
        // Given
        var console = new RecordingConsole();

        // When
        ((IConsole)console).WriteLine("{0}");

        // Then
        Assert.Equal("{0}", console.LastFormat);
        Assert.Equal(["{0}"], console.LastArgs);
    }

    [Fact]
    public void Default_WriteError_Should_Pass_Literal_As_Format_Argument()
    {
        // Given
        var console = new RecordingConsole();

        // When
        ((IConsole)console).WriteError("{0}");

        // Then
        Assert.Equal("{0}", console.LastFormat);
        Assert.Equal(["{0}"], console.LastArgs);
    }

    [Fact]
    public void Default_WriteErrorLine_Should_Pass_Literal_As_Format_Argument()
    {
        // Given
        var console = new RecordingConsole();

        // When
        ((IConsole)console).WriteErrorLine("{0}");

        // Then
        Assert.Equal("{0}", console.LastFormat);
        Assert.Equal(["{0}"], console.LastArgs);
    }
}
