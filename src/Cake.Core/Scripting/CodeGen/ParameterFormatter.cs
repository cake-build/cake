// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cake.Core.Scripting.CodeGen
{
    /// <summary>
    /// Provides support for cleaning parameter names consisting of reserved keywords.
    /// </summary>
    internal sealed class ParameterFormatter
    {
        private readonly HashSet<string> _keywords;

        internal ParameterFormatter()
        {
            // https://msdn.microsoft.com/en-us/library/x53a06bb.aspx
            // reserved keywords are a no-go, contextual keywords were added in later versions
            // and were designed to allow for legacy code that might utilize them as variables/parameters
            // to work.  i.e., where, yield, etc.
            _keywords = new HashSet<string>(StringComparer.Ordinal)
            {
                "abstract", "as", "base", "bool", "break", "byte", "case",
                "catch", "char", "checked", "class", "const", "continue",
                "decimal", "default", "delegate", "do", "double", "else", "enum",
                "event", "explicit", "extern", "false", "finally", "fixed", "float",
                "for", "foreach", "goto", "if", "implicit", "in", "int", "interface",
                "internal", "is", "lock", "long", "namespace", "new", "null", "object",
                "operator", "out", "override", "params", "private", "protected",
                "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
                "sizeof", "stackalloc", "static", "string", "struct", "switch",
                "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
                "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
            };
        }

        internal string FormatName(ParameterInfo parameterInfo)
        {
            if (parameterInfo == null)
            {
                throw new ArgumentNullException(nameof(parameterInfo));
            }

            return FormatName(parameterInfo.Name);
        }

        internal string FormatName(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                throw new ArgumentException("Parameter name cannot be null or whitespace", nameof(parameterName));
            }

            return _keywords.Contains(parameterName) ? $"@{parameterName}" : parameterName;
        }
    }
}
