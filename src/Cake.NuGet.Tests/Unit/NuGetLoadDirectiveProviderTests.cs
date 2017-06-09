// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Diagnostics;
using Cake.Core.Packaging;
using Cake.NuGet.Tests.Fixtures;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetLoadDirectiveProviderTests
    {
        public sealed class TheCanLoadMethod
        {
            [Fact]
            public void Should_Return_True_If_Provider_Is_NuGet()
            {
                // Given
                var fixture = new NuGetLoadDirectiveProviderFixture();

                // When
                var result = fixture.CanLoad();

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheLoadMethod
        {
            [RuntimeFact(TestRuntime.CoreClr)]
            public void Should_Throw_On_NET_Core()
            {
                // Given
                var fixture = new NuGetLoadDirectiveProviderFixture();

                // When
                var result = Record.Exception(() => fixture.Load());

                // Then
                AssertEx.IsExceptionWithMessage<NotSupportedException>(result, "The NuGet provider for #load is not supported on .NET Core.");
            }

            [RuntimeFact(TestRuntime.Clr)]
            public void Should_Install_Package()
            {
                // Given
                var fixture = new NuGetLoadDirectiveProviderFixture();

                // When
                var result = fixture.Load();

                // Then
                Assert.Equal("nuget:?package=Cake.Recipe&include=./**/*.cake", result.Package.OriginalString);
            }

            [RuntimeFact(TestRuntime.Clr)]
            public void Should_Install_Correct_Package_Type()
            {
                // Given
                var fixture = new NuGetLoadDirectiveProviderFixture();

                // When
                var result = fixture.Load();

                // Then
                Assert.Equal(PackageType.Tool, result.PackageType);
            }

            [RuntimeFact(TestRuntime.Clr)]
            public void Should_Install_Package_In_The_Tools_Directory()
            {
                // Given
                var fixture = new NuGetLoadDirectiveProviderFixture();

                // When
                var result = fixture.Load();

                // Then
                Assert.Equal("/Working/tools", result.InstallPath.FullPath);
            }

            [RuntimeFact(TestRuntime.Clr)]
            public void Should_Install_Package_In_Custom_Tools_Directory_If_Specified_In_Configuration()
            {
                // Given
                var fixture = new NuGetLoadDirectiveProviderFixture();
                fixture.Configuration.SetValue("Paths_Tools", "./Bar");

                // When
                var result = fixture.Load();

                // Then
                Assert.Equal("/Working/Bar", result.InstallPath.FullPath);
            }

            [RuntimeFact(TestRuntime.Clr)]
            public void Should_Analyze_Installed_Cake_Scripts()
            {
                // Given
                var fixture = new NuGetLoadDirectiveProviderFixture();
                fixture.InstallResult.Add("/Working/tools/Cake.Recipe/file.cake");
                fixture.InstallResult.Add("/Working/tools/Cake.Recipe/file.exe");

                // When
                var result = fixture.Load();

                // Then
                Assert.Equal(1, result.AnalyzedFiles.Count);
                Assert.Equal("/Working/tools/Cake.Recipe/file.cake", result.AnalyzedFiles[0].FullPath);
            }

            [RuntimeFact(TestRuntime.Clr)]
            public void Should_Write_To_Log_If_No_Scripts_Were_Found()
            {
                // Given
                var fixture = new NuGetLoadDirectiveProviderFixture();
                fixture.InstallResult.Clear();

                // When
                fixture.Load();

                // Then
                Assert.Equal(1, fixture.Log.Entries.Count);
                Assert.Equal("No scripts found in NuGet package Cake.Recipe.", fixture.Log.Entries[0].Message);
                Assert.Equal(Verbosity.Minimal, fixture.Log.Entries[0].Verbosity);
                Assert.Equal(LogLevel.Warning, fixture.Log.Entries[0].Level);
            }
        }
    }
}