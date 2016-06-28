// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCORE
using Cake.NuGet.V3;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class NuGetV3ContentResolverFixture : NuGetContentResolverFixture
    {
        public NuGetV3ContentResolverFixture(string framework)
            : base(framework)
        {
        }

        protected override INuGetContentResolver GetResolver()
        {
            return new NuGetV3ContentResolver(FileSystem, Environment);
        }
    }
}
#endif