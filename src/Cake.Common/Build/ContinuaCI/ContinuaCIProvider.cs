// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cake.Common.Build.ContinuaCI.Data;
using Cake.Core;

namespace Cake.Common.Build.ContinuaCI;

/// <summary>
/// Responsible for communicating with Continua CI.
/// </summary>
public sealed class ContinuaCIProvider : IContinuaCIProvider
{
    private const string MessagePrefix = "@@continua[";
    private const string MessagePostfix = "]";

    private static readonly FrozenDictionary<char, string> _sanitizationTokens;
    private static readonly char[] _specialCharacters;

    private readonly ICakeEnvironment _environment;
    private readonly IBuildSystemServiceMessageWriter _writer;

    static ContinuaCIProvider()
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
    /// Initializes a new instance of the <see cref="ContinuaCIProvider"/> class.
    /// </summary>
    /// <param name="environment">The cake environment.</param>
    /// <param name="writer">The build system service message writer.</param>
    public ContinuaCIProvider(ICakeEnvironment environment, IBuildSystemServiceMessageWriter writer)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        Environment = new ContinuaCIEnvironmentInfo(environment);
    }

    /// <inheritdoc/>
    public bool IsRunningOnContinuaCI
    {
        get
        {
            var continuaCIVersionVariable = _environment.GetEnvironmentVariable("ContinuaCI.Version");
            return !string.IsNullOrWhiteSpace(continuaCIVersionVariable);
        }
    }

    /// <inheritdoc/>
    public ContinuaCIEnvironmentInfo Environment { get; }

    /// <inheritdoc/>
    public void WriteMessage(string message, ContinuaCIMessageType status)
    {
        var name = Enum.GetName(status);
        if (name != null)
        {
            var statusToWrite = name.ToLower();
            var attrs = new Dictionary<string, string>
            {
                { "text", message },
                { "status", statusToWrite }
            };
            WriteServiceMessage("message", attrs);
        }
    }

    /// <inheritdoc/>
    public void WriteStartGroup(string groupName)
    {
        WriteServiceMessage("startGroup ", "name", groupName);
    }

    /// <inheritdoc/>
    public void WriteEndBlock(string groupName)
    {
        WriteServiceMessage("endGroup", "name", groupName);
    }

    /// <inheritdoc/>
    public void SetVariable(string name, string value, bool skipIfNotDefined = true)
    {
        WriteServiceMessage("setParameter", new Dictionary<string, string>
        {
            { "name", name },
            { "value", value },
            { "skipIfNotDefined", skipIfNotDefined.ToString() }
        });
    }

    /// <inheritdoc/>
    public void SetBuildVersion(string version)
    {
        WriteServiceMessage("setBuildVersion", "value", version);
    }

    /// <inheritdoc/>
    public void SetBuildStatus(string text)
    {
        WriteServiceMessage("setBuildStatus", "value", text);
    }

    private void WriteServiceMessage(string messageName, string attributeName, string attributeValue)
    {
        WriteServiceMessage(messageName, new Dictionary<string, string> { { attributeName, attributeValue } });
    }

    private void WriteServiceMessage(string messageName, Dictionary<string, string> parameters)
    {
        var arrayOfParameters = parameters.Select(keypair =>
                                            {
                                                if (string.IsNullOrWhiteSpace(keypair.Key))
                                                {
                                                    return string.Format(CultureInfo.InvariantCulture, "'{0}'", Sanitize(keypair.Value));
                                                }
                                                return string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", keypair.Key, Sanitize(keypair.Value));
                                            }).ToArray();

        var allParameters = string.Join(" ", arrayOfParameters);

        _writer.Write("{0}{1} {2}{3}", MessagePrefix, messageName, allParameters, MessagePostfix);
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
