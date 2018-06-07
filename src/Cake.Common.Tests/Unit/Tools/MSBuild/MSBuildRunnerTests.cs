// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Testing;
using Xunit;

#pragma warning disable xUnit1025 // InlineData should be unique within the Theory it belongs to

namespace Cake.Common.Tests.Unit.Tools.MSBuild
{
    public sealed class MSBuildRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Return_The_Highest_MSBuild_Version_If_Tool_Version_Is_Set_To_Default()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(true, PlatformFamily.Windows);
                fixture.Settings.PlatformTarget = PlatformTarget.x64;
                fixture.Settings.ToolVersion = MSBuildToolVersion.Default;

                fixture.GivenDefaultToolDoNotExist();
                fixture.GivenMSBuildIsNotInstalled();
                fixture.FileSystem.CreateFile("/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe");
                fixture.FileSystem.CreateFile("/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe", result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_2(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_35(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2017, PlatformTarget.x64, PlatformFamily.Windows, false, "/Program86/Microsoft Visual Studio/2017/Enterprise/MSBuild/15.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2017, PlatformTarget.x86, PlatformFamily.Windows, false, "/Program86/Microsoft Visual Studio/2017/Enterprise/MSBuild/15.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2017, PlatformTarget.x64, PlatformFamily.Linux, false, "/usr/bin/msbuild")]
            [InlineData(MSBuildToolVersion.VS2017, PlatformTarget.x64, PlatformFamily.OSX, false, "/Library/Frameworks/Mono.framework/Versions/Current/Commands/msbuild")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_15(MSBuildToolVersion version, PlatformTarget target, PlatformFamily family, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, family);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_4(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.MSIL, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.MSIL, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.MSIL, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.MSIL, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.MSIL, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.MSIL, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.ARM, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.ARM, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.ARM, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.ARM, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.ARM, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.ARM, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.Win32, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.Win32, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.Win32, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.Win32, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.Win32, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.Win32, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_12(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.MSIL, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.MSIL, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.MSIL, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.MSIL, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.x86, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.x86, true, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.x86, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.x86, true, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.x64, false, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.x64, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.x64, false, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.x64, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.ARM, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.ARM, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.ARM, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.ARM, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.Win32, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.Win32, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.Win32, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.Win32, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_14(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_2_When_Build_Platform_Set_To_Automatic(MSBuildToolVersion version, MSBuildPlatform buildPlatform, PlatformTarget platformTarget, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.MSBuildPlatform = buildPlatform;
                fixture.Settings.PlatformTarget = platformTarget;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.ARM, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.Win32, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.ARM, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, PlatformTarget.Win32, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_2_When_Build_Platform_Explicitly_Set(MSBuildToolVersion version, MSBuildPlatform buildPlatform, PlatformTarget platformTarget, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.MSBuildPlatform = buildPlatform;
                fixture.Settings.PlatformTarget = platformTarget;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_2_When_Platform_Set(MSBuildToolVersion version, MSBuildPlatform platform, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.MSBuildPlatform = platform;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2008, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_35_When_Platform_Set(MSBuildToolVersion version, MSBuildPlatform platform, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.MSBuildPlatform = platform;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET40, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, MSBuildPlatform.Automatic, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, MSBuildPlatform.Automatic, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, MSBuildPlatform.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, MSBuildPlatform.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, MSBuildPlatform.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, MSBuildPlatform.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_4_When_Platform_Set(MSBuildToolVersion version, MSBuildPlatform platform, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.MSBuildPlatform = platform;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET451, MSBuildPlatform.Automatic, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, MSBuildPlatform.Automatic, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, MSBuildPlatform.Automatic, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, MSBuildPlatform.Automatic, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, MSBuildPlatform.Automatic, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, MSBuildPlatform.Automatic, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, MSBuildPlatform.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, MSBuildPlatform.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, MSBuildPlatform.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, MSBuildPlatform.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, MSBuildPlatform.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, MSBuildPlatform.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, MSBuildPlatform.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, MSBuildPlatform.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, MSBuildPlatform.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, MSBuildPlatform.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, MSBuildPlatform.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, MSBuildPlatform.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_12_When_Platform_Set(MSBuildToolVersion version, MSBuildPlatform platform, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.MSBuildPlatform = platform;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET46, MSBuildPlatform.Automatic, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, MSBuildPlatform.Automatic, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, MSBuildPlatform.Automatic, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, MSBuildPlatform.Automatic, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, MSBuildPlatform.x86, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, MSBuildPlatform.x86, true, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, MSBuildPlatform.x86, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, MSBuildPlatform.x86, true, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, MSBuildPlatform.x64, false, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, MSBuildPlatform.x64, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, MSBuildPlatform.x64, false, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, MSBuildPlatform.x64, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_14_When_Platform_Set(MSBuildToolVersion version, MSBuildPlatform platform, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, PlatformFamily.Windows);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.MSBuildPlatform = platform;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Return_The_Registered_MSBuild_If_MSBuild_Is_Registered()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(true, PlatformFamily.Windows);

                var registeredMSBuild = fixture.FileSystem.CreateFile("/Registered/MSBuild.exe");
                fixture.Tools.RegisterFile(registeredMSBuild.Path);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(registeredMSBuild.Path.FullPath, result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_MSBuild_Executable_Did_Not_Exist()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(true, PlatformFamily.Windows);
                fixture.Settings.PlatformTarget = PlatformTarget.x86;
                fixture.Settings.ToolVersion = MSBuildToolVersion.NET20;

                fixture.GivenMSBuildIsNotInstalled();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "MSBuild: Could not locate executable.");
            }

            [Fact]
            public void Should_Use_As_Many_Processors_As_Possible_If_MaxCpuCount_Is_Zero()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.MaxCpuCount = 0;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Specified_Number_Of_Max_Processors()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.MaxCpuCount = 4;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m:4 /v:normal /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Correct_Default_Target_In_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Node_Reuse_If_Specified()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.NodeReuse = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /nr:true /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Detailed_Summary_If_Specified()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.DetailedSummary = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/ds /v:normal /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_No_Console_Logger_If_Specified()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.NoConsoleLogger = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/noconlog /v:normal /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Append_Targets_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithTarget("A");
                fixture.Settings.WithTarget("B");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /target:A;B " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Append_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithProperty("A", "B");
                fixture.Settings.WithProperty("C", "D");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /p:A=B /p:C=D /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Append_Property_With_Multiple_Values_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithProperty("A", "B", "E");
                fixture.Settings.WithProperty("C", "D");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /p:A=B /p:A=E /p:C=D /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Theory]
            [InlineData("Release", "/v:normal /p:Configuration=\"Release\" /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData("Custom Spaced", "/v:normal /p:Configuration=\"Custom Spaced\" /target:Build \"C:/Working/src/Solution.sln\"")]
            public void Should_Append_Configuration_As_Property_To_Process_Arguments(string configuration, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.SetConfiguration(configuration);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(PlatformTarget.MSIL, "/v:normal /p:Platform=\"Any CPU\" /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData(PlatformTarget.x86, "/v:normal /p:Platform=x86 /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData(PlatformTarget.x64, "/v:normal /p:Platform=x64 /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData(PlatformTarget.ARM, "/v:normal /p:Platform=arm /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData(PlatformTarget.Win32, "/v:normal /p:Platform=Win32 /target:Build \"C:/Working/src/Solution.sln\"")]
            public void Should_Append_Platform_As_Property_To_Process_Arguments(PlatformTarget platform, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.SetPlatformTarget(platform);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Append_MSIL_Platform_As_AnyCPU_For_Project()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.SetPlatformTarget(PlatformTarget.MSIL);
                fixture.Solution = "/Working/src/Project.csproj";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /p:Platform=AnyCPU /target:Build \"/Working/src/Project.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Build_Arm_With_x86_When_Specified()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(true, PlatformFamily.Windows);
                fixture.Settings.PlatformTarget = PlatformTarget.ARM;
                fixture.Settings.MSBuildPlatform = MSBuildPlatform.x86;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /p:Platform=arm /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
                Assert.Equal("/Program86/MSBuild/12.0/Bin/MSBuild.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Omit_Platform_Property_In_Process_Arguments_If_It_Is_Null()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Linux);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "MSBuild: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "MSBuild: Process returned an error (exit code 1).");
            }

            [Theory]
            [InlineData(Verbosity.Quiet, "/v:quiet /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData(Verbosity.Minimal, "/v:minimal /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData(Verbosity.Normal, "/v:normal /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData(Verbosity.Verbose, "/v:detailed /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData(Verbosity.Diagnostic, "/v:diagnostic /target:Build \"C:/Working/src/Solution.sln\"")]
            public void Should_Append_Correct_Verbosity(Verbosity verbosity, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.Verbosity = verbosity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Throw_If_Verbosity_Is_Unknown()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.Verbosity = (Verbosity)int.MaxValue;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Encountered unknown MSBuild build log verbosity.");
            }

            [Fact]
            public void Should_Append_Logger_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithLogger("A", "B", "C");
                fixture.Settings.WithLogger("D", "E");
                fixture.Settings.WithLogger("F");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /target:Build /logger:B,\"A\";C /logger:E,\"D\" /logger:\"F\" " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Append_FileLogger_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.AddFileLogger(new MSBuildFileLogger { AppendToLogFile = false, Encoding = "E", HideVerboseItemAndPropertyList = false, LogFile = "A", MSBuildFileLoggerOutput = MSBuildFileLoggerOutput.All, PerformanceSummaryEnabled = false, ShowCommandLine = false, ShowEventId = false, ShowTimestamp = false, SummaryDisabled = false, Verbosity = Verbosity.Diagnostic });
                fixture.Settings.AddFileLogger(new MSBuildFileLogger { AppendToLogFile = true, HideVerboseItemAndPropertyList = true, MSBuildFileLoggerOutput = MSBuildFileLoggerOutput.ErrorsOnly, PerformanceSummaryEnabled = true, ShowCommandLine = true, ShowEventId = true, ShowTimestamp = true, SummaryDisabled = true, Verbosity = Verbosity.Minimal });
                fixture.Settings.AddFileLogger(new MSBuildFileLogger { MSBuildFileLoggerOutput = MSBuildFileLoggerOutput.WarningsOnly, Verbosity = Verbosity.Normal });
                fixture.Settings.AddFileLogger(new MSBuildFileLogger { Verbosity = Verbosity.Quiet });
                fixture.Settings.AddFileLogger(new MSBuildFileLogger { Verbosity = Verbosity.Verbose });
                fixture.Settings.AddFileLogger(new MSBuildFileLogger { });
                fixture.Settings.AddFileLogger();

                // When
                var result = fixture.Run();
                // Then
                Assert.Equal(@"/v:normal /target:Build /fl /flp:logfile=""C:/Working/A"";Encoding=E;Verbosity=diagnostic /fl1 /flp1:Append;PerformanceSummary;NoSummary;ErrorsOnly;NoItemAndPropertyList;ShowCommandLine;ShowTimestamp;ShowEventId;Verbosity=minimal /fl2 /flp2:WarningsOnly;Verbosity=normal /fl3 /flp3:Verbosity=quiet /fl4 /flp4:Verbosity=detailed /fl5 /fl6 ""C:/Working/src/Solution.sln""", result.Args);
            }

            [Fact]
            public void Should_Append_Default_FileLogger_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.AddFileLogger();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(@"/v:normal /target:Build /fl ""C:/Working/src/Solution.sln""", result.Args);
            }

            [Fact]
            public void Should_Throw_Exception_For_Too_Many_FileLoggers()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();
                fixture.Settings.AddFileLogger();

                // When
                var ex = Assert.Throws<System.InvalidOperationException>(() => fixture.Run());

                // Then
                Assert.Equal(@"Too Many FileLoggers", ex.Message);
            }

            [Fact]
            public void Should_Use_Binary_Logging_If_Specified()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.BinaryLogger = new MSBuildBinaryLogSettings()
                {
                    Enabled = true,
                    FileName = "mylog.binlog",
                    Imports = MSBuildBinaryLogImports.ZipFile
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/v:normal /target:Build /bl:mylog.binlog;ProjectImports=ZipFile \"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Theory]
            [InlineData("Value1,Value2", "/v:normal /p:Property=Value1%2CValue2 /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData("Value1;Value2", "/v:normal /p:Property=Value1%3BValue2 /target:Build \"C:/Working/src/Solution.sln\"")]
            [InlineData("Value1 Value2", "/v:normal /p:Property=Value1%20Value2 /target:Build \"C:/Working/src/Solution.sln\"")]
            public void Should_Escape_Special_Characters_In_Property_Value(string propertyValue, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithProperty("Property", propertyValue);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Use_WarnAsError_If_Specified()
            {
                // Given
                var expected = "/v:normal /target:Build /warnaserror \"C:/Working/src/Solution.sln\"";
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithWarningsAsError();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Use_WarnAsError_Codes_If_Specified()
            {
                // Given
                const string expected = "/v:normal /target:Build /warnaserror:\"12345\" \"C:/Working/src/Solution.sln\"";
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithWarningsAsError("12345");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Use_WarnAsMessage_Codes_If_Specified()
            {
                // Given
                const string expected = "/v:normal /target:Build /warnasmessage:\"12345\" \"C:/Working/src/Solution.sln\"";
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithWarningsAsMessage("12345");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Use_WarnAsError_And_WarnAsMessage_Codes_If_Specified()
            {
                // Given
                var expected = "/v:normal /target:Build /warnaserror /warnasmessage:\"12345\" \"C:/Working/src/Solution.sln\"";
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithWarningsAsError().WithWarningsAsMessage("12345");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("\r", "%0D")]
            [InlineData("\n", "%0A")]
            [InlineData("\r\n", "%0D%0A")]
            public void Should_URL_Convert_NewLine_Characters_In_Properties(string input, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithProperty("Foo", input);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"/v:normal /p:Foo={expected} /target:Build " +
                             "\"C:/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Restore_If_Specified()
            {
                // Given
                var expected = "/v:normal /target:Build /restore \"C:/Working/src/Solution.sln\"";
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                fixture.Settings.WithRestore();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(new string[] { }, @"/v:normal /target:Build ""C:/Working/src/Solution.sln""")]
            [InlineData(new string[] { "ForceConsoleColor" }, @"/v:normal /target:Build /clp:ForceConsoleColor ""C:/Working/src/Solution.sln""")]
            [InlineData(new string[] { "ForceConsoleColor", "ShowCommandLine" }, @"/v:normal /target:Build /clp:ForceConsoleColor;ShowCommandLine ""C:/Working/src/Solution.sln""")]
            public void Should_Append_ConsoleLoggerParameters(string[] parameters, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, PlatformFamily.Windows);
                foreach (var parameter in parameters)
                {
                    fixture.Settings.ConsoleLoggerParameters.Add(parameter);
                }

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}

#pragma warning restore xUnit1025 // InlineData should be unique within the Theory it belongs to