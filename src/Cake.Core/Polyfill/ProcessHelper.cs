// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;

namespace Cake.Core.Polyfill
{
    internal static class ProcessHelper
    {
        public static void SetEnvironmentVariable(ProcessStartInfo info, string key, string value)
        {
            var envKey = info.Environment.Keys.FirstOrDefault(existingKey => StringComparer.OrdinalIgnoreCase.Equals(existingKey, key)) ?? key;
            info.Environment[envKey] = value;
        }
    }
}
