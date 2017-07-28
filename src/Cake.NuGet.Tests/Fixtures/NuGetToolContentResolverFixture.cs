// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class NuGetToolContentResolverFixture : NuGetContentResolverFixture
    {
        // This is a resolver without the addin assemblies method
        // which is not needed for resolving tools.
        private sealed class NuGetToolContentResolver : NuGetContentResolver
        {
            public NuGetToolContentResolver(IFileSystem fileSystem, ICakeEnvironment environment, IGlobber globber)
                : base(fileSystem, environment, globber)
            {
            }

            protected override IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath path, PackageReference package)
            {
                throw new NotSupportedException("Only tools can be resolved with this resolver.");
            }
        }

        public NuGetToolContentResolverFixture(string uri)
            : base(".NETFramework,Version=v4.5")
        {
            Package = new PackageReference(uri);
            PackageType = PackageType.Tool;
            Path = new DirectoryPath(string.Concat("/Working/tools/", Package.Package));
        }

        protected override INuGetContentResolver GetResolver()
        {
            return new NuGetToolContentResolver(FileSystem, Environment, Globber);
        }
    }
}
