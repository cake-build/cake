// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Polyfill;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class NuGetToolContentResolverFixture : NuGetContentResolverFixture
    {
        public NuGetToolContentResolverFixture(string uri)
            : base(".NETFramework,Version=v4.5", Runtime.Clr)
        {
            Package = new PackageReference(uri);
            PackageType = PackageType.Tool;
            Path = new DirectoryPath(string.Concat("/Working/tools/", Package.Package));
        }
    }
}
