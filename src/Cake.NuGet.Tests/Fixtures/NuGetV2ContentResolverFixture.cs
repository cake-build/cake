// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCORE
using Cake.NuGet.V2;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class NuGetV2ContentResolverFixture : NuGetContentResolverFixture
    {
        public NuGetV2ContentResolverFixture(string framework = ".NETFramework,Version=v4.5")
            : base(framework)
        {
        }

        protected override INuGetContentResolver GetResolver()
        {
            return new NuGetV2ContentResolver(FileSystem, Environment, Log);
        }
    }
}
#endif