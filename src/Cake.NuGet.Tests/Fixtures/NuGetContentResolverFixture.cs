// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Runtime.Versioning;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Testing;

namespace Cake.NuGet.Tests.Fixtures
{
    internal abstract class NuGetContentResolverFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public FakeEnvironment Environment { get; set; }
        public Globber Globber { get; set; }
        public ICakeLog Log { get; set; }

        public DirectoryPath Path { get; set; }
        public PackageType PackageType { get; set; }
        public PackageReference Package { get; set; }

        protected NuGetContentResolverFixture(string framework)
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            Environment.Runtime.TargetFramework = new FrameworkName(framework);

            FileSystem = new FakeFileSystem(Environment);
            Globber = new Globber(FileSystem, Environment);
            Log = new FakeLog();

            Path = "/Working";
            PackageType = PackageType.Addin;
            Package = new PackageReference("nuget:?package=Foo");
        }

        public IReadOnlyCollection<IFile> GetFiles()
        {
            var resolver = GetResolver();
            return resolver.GetFiles(Path, Package, PackageType);
        }

        protected abstract INuGetContentResolver GetResolver();
    }
}