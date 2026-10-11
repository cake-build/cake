// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;

namespace Cake.Core.IO;

/// <summary>
/// Payload written to a process standard input stream.
/// </summary>
public readonly struct ProcessStandardInput : IEquatable<ProcessStandardInput>
{
    private readonly ReadOnlyMemory<byte> _memory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProcessStandardInput"/> struct.
    /// </summary>
    /// <param name="memory">The standard input bytes.</param>
    public ProcessStandardInput(ReadOnlyMemory<byte> memory)
    {
        _memory = memory;
    }

    /// <summary>
    /// Gets the standard input bytes.
    /// </summary>
    public ReadOnlyMemory<byte> Memory => _memory;

    /// <summary>
    /// Gets the standard input bytes as a span.
    /// </summary>
    public ReadOnlySpan<byte> Span => _memory.Span;

    /// <summary>
    /// Converts a string to UTF-8 standard input bytes.
    /// </summary>
    /// <param name="text">The standard input text.</param>
    public static implicit operator ProcessStandardInput(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return new ProcessStandardInput(Encoding.UTF8.GetBytes(text));
    }

    /// <summary>
    /// Converts a UTF-8 literal or other byte span to standard input.
    /// </summary>
    /// <param name="bytes">The standard input bytes.</param>
    public static implicit operator ProcessStandardInput(ReadOnlySpan<byte> bytes)
    {
        return new ProcessStandardInput(bytes.ToArray());
    }

    /// <summary>
    /// Converts a memory region to standard input.
    /// </summary>
    /// <param name="bytes">The standard input bytes.</param>
    public static implicit operator ProcessStandardInput(ReadOnlyMemory<byte> bytes)
    {
        return new ProcessStandardInput(bytes);
    }

    /// <summary>
    /// Converts a byte array to standard input.
    /// </summary>
    /// <param name="bytes">The standard input bytes.</param>
    public static implicit operator ProcessStandardInput(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        return new ProcessStandardInput(bytes);
    }

    /// <inheritdoc />
    public bool Equals(ProcessStandardInput other)
    {
        return _memory.Span.SequenceEqual(other._memory.Span);
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is ProcessStandardInput other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.AddBytes(_memory.Span);
        return hash.ToHashCode();
    }

    /// <summary>
    /// Determines whether two payloads are equal.
    /// </summary>
    /// <param name="left">The first payload.</param>
    /// <param name="right">The second payload.</param>
    /// <returns><c>true</c> if the payloads are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(ProcessStandardInput left, ProcessStandardInput right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two payloads are not equal.
    /// </summary>
    /// <param name="left">The first payload.</param>
    /// <param name="right">The second payload.</param>
    /// <returns><c>true</c> if the payloads are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(ProcessStandardInput left, ProcessStandardInput right)
    {
        return !left.Equals(right);
    }
}
