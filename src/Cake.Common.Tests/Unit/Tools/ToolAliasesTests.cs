// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Tooling;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools
{
    public sealed class ToolAliasesTests
    {
        public sealed class TheToolInstallerProperty
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // When
                var result = Record.Exception(() => ToolAliases.ToolInstaller(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Return_The_Context_Tool_Installer()
            {
                // Given
                var installer = Substitute.For<IToolInstaller>();
                var context = CreateContext(installer);

                // When
                var result = ToolAliases.ToolInstaller(context);

                // Then
                Assert.Same(installer, result);
            }
        }

        public sealed class TheInstallToolMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null_For_Package_Reference()
            {
                // Given
                var tool = new PackageReference("nuget:?package=tool");

                // When
                var result = Record.Exception(() => ToolAliases.InstallTool(null, tool));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Package_Reference_Is_Null()
            {
                // Given
                var context = CreateContext();

                // When
                var result = Record.Exception(() => ToolAliases.InstallTool(context, (PackageReference)null));

                // Then
                AssertEx.IsArgumentNullException(result, "tool");
            }

            [Fact]
            public void Should_Throw_If_Context_Is_Null_For_String()
            {
                // When
                var result = Record.Exception(() => ToolAliases.InstallTool(null, "nuget:?package=tool"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Tool_String_Is_Null()
            {
                // Given
                var context = CreateContext();

                // When
                var result = Record.Exception(() => ToolAliases.InstallTool(context, (string)null));

                // Then
                AssertEx.IsArgumentNullException(result, "tool");
            }

            [Fact]
            public void Should_Install_Package_Reference_And_Return_Paths()
            {
                // Given
                var installer = Substitute.For<IToolInstaller>();
                var tool = new PackageReference("nuget:?package=tool&version=1.0.0");
                var path = new FilePath("/Working/tools/tool.exe");
                installer.Install(tool).Returns(new[] { path });
                var context = CreateContext(installer);

                // When
                var result = ToolAliases.InstallTool(context, tool);

                // Then
                Assert.Single(result);
                Assert.Equal(path.FullPath, result[0].FullPath);
                installer.Received(1).Install(tool);
            }

            [Fact]
            public void Should_Parse_Tool_String_And_Install()
            {
                // Given
                var installer = Substitute.For<IToolInstaller>();
                var path = new FilePath("/Working/tools/tool.exe");
                installer.Install(Arg.Any<PackageReference>()).Returns(new[] { path });
                var context = CreateContext(installer);

                // When
                var result = ToolAliases.InstallTool(context, "nuget:?package=tool&version=1.0.0");

                // Then
                Assert.Single(result);
                installer.Received(1).Install(
                    Arg.Is<PackageReference>(package =>
                        package.Package == "tool" &&
                        package.Scheme == "nuget" &&
                        package.OriginalString == "nuget:?package=tool&version=1.0.0"));
            }
        }

        public sealed class TheInstallToolsMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null_For_Package_References()
            {
                // When
                var result = Record.Exception(() => ToolAliases.InstallTools(null, new PackageReference("nuget:?package=tool")));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Package_References_Are_Null()
            {
                // Given
                var context = CreateContext();

                // When
                var result = Record.Exception(() => ToolAliases.InstallTools(context, (PackageReference[])null));

                // Then
                AssertEx.IsArgumentNullException(result, "tools");
            }

            [Fact]
            public void Should_Throw_If_Context_Is_Null_For_Strings()
            {
                // When
                var result = Record.Exception(() => ToolAliases.InstallTools(null, "nuget:?package=tool"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Tool_Strings_Are_Null()
            {
                // Given
                var context = CreateContext();

                // When
                var result = Record.Exception(() => ToolAliases.InstallTools(context, (string[])null));

                // Then
                AssertEx.IsArgumentNullException(result, "tools");
            }

            [Fact]
            public void Should_Install_Each_Package_Reference_And_Return_Tuples()
            {
                // Given
                var installer = Substitute.For<IToolInstaller>();
                var first = new PackageReference("nuget:?package=first&version=1.0.0");
                var second = new PackageReference("dotnet:?package=second&version=2.0.0");
                var firstPath = new FilePath("/Working/tools/first.exe");
                var secondPath = new FilePath("/Working/tools/second.exe");
                installer.Install(first).Returns(new[] { firstPath });
                installer.Install(second).Returns(new[] { secondPath });
                var context = CreateContext(installer);

                // When
                var result = ToolAliases.InstallTools(context, first, second);

                // Then
                Assert.Equal(2, result.Length);
                Assert.Same(first, result[0].Tool);
                Assert.Equal(firstPath.FullPath, result[0].Paths[0].FullPath);
                Assert.Same(second, result[1].Tool);
                Assert.Equal(secondPath.FullPath, result[1].Paths[0].FullPath);
                installer.Received(1).Install(first);
                installer.Received(1).Install(second);
            }

            [Fact]
            public void Should_Install_Each_Tool_String_And_Return_Tuples()
            {
                // Given
                var installer = Substitute.For<IToolInstaller>();
                installer.Install(Arg.Any<PackageReference>())
                    .Returns(call => new[] { new FilePath($"/Working/tools/{call.Arg<PackageReference>().Package}.exe") });
                var context = CreateContext(installer);

                // When
                var result = ToolAliases.InstallTools(
                    context,
                    "nuget:?package=first&version=1.0.0",
                    "dotnet:?package=second&version=2.0.0");

                // Then
                Assert.Equal(2, result.Length);
                Assert.Equal("first", result[0].Tool.Package);
                Assert.Equal("/Working/tools/first.exe", result[0].Paths[0].FullPath);
                Assert.Equal("second", result[1].Tool.Package);
                Assert.Equal("/Working/tools/second.exe", result[1].Paths[0].FullPath);
            }
        }

        private static ICakeContext CreateContext(IToolInstaller installer = null)
        {
            return new CakeContext(
                Substitute.For<IFileSystem>(),
                Substitute.For<ICakeEnvironment>(),
                Substitute.For<IGlobber>(),
                Substitute.For<ICakeLog>(),
                Substitute.For<ICakeArguments>(),
                Substitute.For<IProcessRunner>(),
                Substitute.For<IRegistry>(),
                Substitute.For<IToolLocator>(),
                Substitute.For<ICakeDataService>(),
                Substitute.For<ICakeConfiguration>(),
                installer ?? Substitute.For<IToolInstaller>());
        }
    }
}
