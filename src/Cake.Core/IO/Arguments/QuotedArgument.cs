// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO.Arguments;

/// <summary>
/// Represents a quoted argument.
/// The inner value is treated as a literal and escaped so a standard
/// Windows argv parser recovers it (including trailing backslashes and quotes).
/// </summary>
public sealed class QuotedArgument : IProcessArgument
{
    private readonly IProcessArgument _argument;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuotedArgument"/> class.
    /// </summary>
    /// <param name="argument">The argument.</param>
    public QuotedArgument(IProcessArgument argument)
    {
        _argument = argument;
    }

    /// <inheritdoc/>
    public string Render()
    {
        return ProcessArgumentEscaper.Escape(_argument.Render(), alwaysQuote: true);
    }

    /// <inheritdoc/>
    public string RenderSafe()
    {
        return ProcessArgumentEscaper.Escape(_argument.RenderSafe(), alwaysQuote: true);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return RenderSafe();
    }
}
