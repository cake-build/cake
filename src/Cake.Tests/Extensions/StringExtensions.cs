// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Text;
using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Cake.Tests
{
    public static class StringExtensions
    {
        public static string Reassemble(this IEnumerable<string> lines)
        {
            var builder = new StringBuilder();
            foreach (var line in lines)
            {
                builder.AppendLine(line);
            }
            return builder.ToString().NormalizeLineEndings();
        }
    }
}
