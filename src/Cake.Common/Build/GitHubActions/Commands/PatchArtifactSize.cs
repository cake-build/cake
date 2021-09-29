// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.GitHubActions.Commands
{
    internal sealed class PatchArtifactSize
    {
        public long Size { get; set; }

        public PatchArtifactSize(long size)
        {
            Size = size;
        }
    }
}