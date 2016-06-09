// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Runtime.Versioning;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetPackageAssembliesLocatorTests
    {
        public sealed class TheFindAssembliesMethod
        {
            [Fact]
            public void Should_Throw_If_Package_Directory_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var log = Substitute.For<ICakeLog>();
                var assemblyCompatibilityFilter = Substitute.For<INuGetAssemblyCompatibilityFilter>();
                var environment = Substitute.For<ICakeEnvironment>();
                var locator = new NuGetPackageAssembliesLocator(fileSystem, log, assemblyCompatibilityFilter, environment);

                // When
                var result = Record.Exception(() => locator.FindAssemblies(null));

                // Then
                Assert.IsArgumentNullException(result, "packageDirectory");
            }

            [Fact]
            public void Should_Return_Empty_If_Package_Directory_Does_Not_Exist()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var log = Substitute.For<ICakeLog>();
                var assemblyCompatibilityFilter = Substitute.For<INuGetAssemblyCompatibilityFilter>();

                var locator = new NuGetPackageAssembliesLocator(fileSystem, log, assemblyCompatibilityFilter, environment);

                var addinDirectory = DirectoryPath.FromString("/NonExistentDir");

                // When
                var result = locator.FindAssemblies(addinDirectory);

                // Then
                Assert.Empty(result);
            }

            [Fact]
            public void Should_Throw_If_Package_Directory_Is_Not_Absolute()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var log = Substitute.For<ICakeLog>();
                var assemblyCompatibilityFilter = Substitute.For<INuGetAssemblyCompatibilityFilter>();

                var locator = new NuGetPackageAssembliesLocator(fileSystem, log, assemblyCompatibilityFilter, environment);

                var addinDirectory = DirectoryPath.FromString("RelativeDirPath");
                Assert.True(addinDirectory.IsRelative);

                // When
                var result = Record.Exception(() => locator.FindAssemblies(addinDirectory));

                // Then
                Assert.IsCakeException(result, "Package directory (RelativeDirPath) must be an absolute path.");
            }

            [Fact]
            public void Should_Return_Compatible_Packages_For_NuGet_Compliant_Package()
            {
                // Given
                var targetFramework = new FrameworkName(".NETFramework,Version=v4.5");
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.SetTargetFramework(targetFramework);

                var log = Substitute.For<ICakeLog>();

                var fileSystem = new FakeFileSystem(environment);

                var packageDirectory = fileSystem.CreateDirectory("/Working/addins/MyPackage");

                fileSystem.CreateFile(packageDirectory.Path.CombineWithFilePath("lib/net40/myassembly1.dll"));
                fileSystem.CreateFile(packageDirectory.Path.CombineWithFilePath("lib/net40/myassembly2.dll"));
                var compatibleDll1 = fileSystem.CreateFile(packageDirectory.Path.CombineWithFilePath("lib/net45/myassembly1.dll"));
                var compatibleDll2 = fileSystem.CreateFile(packageDirectory.Path.CombineWithFilePath("lib/net45/myassembly2.dll"));
                fileSystem.CreateFile(packageDirectory.Path.CombineWithFilePath("lib/net452/myassembly1.dll"));
                fileSystem.CreateFile(packageDirectory.Path.CombineWithFilePath("lib/net452/myassembly2.dll"));

                var assemblyCompatibilityFilter = Substitute.For<INuGetAssemblyCompatibilityFilter>();
                assemblyCompatibilityFilter.FilterCompatibleAssemblies(targetFramework, Arg.Any<IEnumerable<FilePath>>())
                    .Returns(ci => new FilePath[] { "lib/net45/myassembly1.dll", "lib/net45/myassembly2.dll" });

                var locator = new NuGetPackageAssembliesLocator(fileSystem, log, assemblyCompatibilityFilter, environment);

                // When
                var result = locator.FindAssemblies(packageDirectory.Path);

                // Then
                Assert.Equal(new List<IFile> { compatibleDll1, compatibleDll2 }.AsReadOnly(), result);
            }

            [Fact]
            public void Should_Return_Empty_If_No_Package_Assemblies_Are_Found()
            {
                // Given
                var targetFramework = new FrameworkName(".NETFramework,Version=v4.5");
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.SetTargetFramework(targetFramework);

                var log = Substitute.For<ICakeLog>();

                var fileSystem = new FakeFileSystem(environment);

                var packageDirectory = fileSystem.CreateDirectory("/Working/addins/MyPackage");

                var assemblyCompatibilityFilter = Substitute.For<INuGetAssemblyCompatibilityFilter>();
                assemblyCompatibilityFilter.FilterCompatibleAssemblies(targetFramework, Arg.Any<IEnumerable<FilePath>>())
                    .Returns(ci => ci[1]);

                var locator = new NuGetPackageAssembliesLocator(fileSystem, log, assemblyCompatibilityFilter, environment);

                // When
                var result = locator.FindAssemblies(packageDirectory.Path);

                // Then
                Assert.Empty(result);
                log.Received()
                    .Write(Verbosity.Minimal, LogLevel.Warning, "Unable to locate any assemblies under {0}",
                        packageDirectory.Path.FullPath);
            }
        }
    }
}
