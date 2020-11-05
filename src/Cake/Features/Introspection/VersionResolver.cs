// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Cake.Features.Introspection
{
    public interface IVersionResolver
    {
        string GetVersion();
        string GetProductVersion();
    }

    public sealed class VersionResolver : IVersionResolver
    {
        public string GetVersion()
        {
            var assembly = typeof(Program).Assembly;
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).Comments;

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "Unknown";
            }

            return version;
        }

        public string GetProductVersion()
        {
            var assembly = typeof(Program).Assembly;
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "Unknown";
            }

            return version;
        }
    }
}
