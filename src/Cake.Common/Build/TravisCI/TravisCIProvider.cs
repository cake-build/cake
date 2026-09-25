// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cake.Common.Build.TravisCI.Data;
using Cake.Core;

namespace Cake.Common.Build.TravisCI;

/// <summary>
/// Responsible for communicating with Travis CI.
/// </summary>
public sealed class TravisCIProvider : ITravisCIProvider
{
    private const string MessagePrefix = "travis_";
    private const string MessagePostfix = "\r";

    private readonly ICakeEnvironment _environment;
    private readonly IBuildSystemServiceMessageWriter _writer;

    private static readonly FrozenDictionary<char, string> _sanitizationTokens;
    private static readonly char[] _specialCharacters;

    static TravisCIProvider()
    {
        _sanitizationTokens = new Dictionary<char, string>
        {
            { '\\', "\\\\" },
            { '\'', "\\'" },
            { '\n', "\\n" },
            { '\r', "\\r" },
            { '[', "\\['" },
            { ']', "\\]" }
        }.ToFrozenDictionary();
        _specialCharacters = [.. _sanitizationTokens.Keys];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TravisCIProvider"/> class.
    /// </summary>
    /// <param name="environment">The environment.</param>
    /// <param name="writer">The log.</param>
    public TravisCIProvider(ICakeEnvironment environment, IBuildSystemServiceMessageWriter writer)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        Environment = new TravisCIEnvironmentInfo(environment);
    }

    /// <inheritdoc/>
    public bool IsRunningOnTravisCI => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TRAVIS"));

    /// <inheritdoc/>
    public TravisCIEnvironmentInfo Environment { get; }

    /// <inheritdoc/>
    public void WriteStartFold(string name)
    {
        WriteServiceMessage("fold", "start", name);
    }

    /// <inheritdoc/>
    public void WriteEndFold(string name)
    {
        WriteServiceMessage("fold", "end", name);
    }

    private void WriteServiceMessage(string messageName, string attributeName, string attributeValue)
    {
        WriteServiceMessage(messageName, new Dictionary<string, string> { { attributeName, attributeValue } });
    }

    private void WriteServiceMessage(string messageName, Dictionary<string, string> values)
    {
        var valueString =
            string.Join(" ",
                values
                    .Select(keypair =>
                    {
                        if (string.IsNullOrWhiteSpace(keypair.Key))
                        {
                            return string.Format(CultureInfo.InvariantCulture, "'{0}'", Sanitize(keypair.Value));
                        }
                        return string.Format(CultureInfo.InvariantCulture, ":{0}:{1}", keypair.Key, Sanitize(keypair.Value));
                    })
                    .ToArray());
        _writer.Write("{0}{1}{2}{3}", MessagePrefix, messageName, valueString, MessagePostfix);
    }

    private static string Sanitize(string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return string.Empty;
        }

        if (source.IndexOfAny(_specialCharacters) < 0)
        {
            return source;
        }

        var stringBuilder = new StringBuilder(source.Length * 2);
        foreach (var sourceChar in source)
        {
            if (_sanitizationTokens.TryGetValue(sourceChar, out var replacement))
            {
                stringBuilder.Append(replacement);
            }
            else
            {
                stringBuilder.Append(sourceChar);
            }
        }

        return stringBuilder.ToString();
    }
}
