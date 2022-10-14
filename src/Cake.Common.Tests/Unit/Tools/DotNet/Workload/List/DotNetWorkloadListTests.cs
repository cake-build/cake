// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.List;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Workload.List
{
    public sealed class DotNetWorkloadListTests
    {
        public sealed class TheWorkloadListMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetWorkloadListerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetWorkloadListerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetWorkloadListerFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Add_Verbosity_Argument()
            {
                // Given
                var fixture = new DotNetWorkloadListerFixture();
                fixture.Settings.Verbosity = Common.Tools.DotNet.DotNetVerbosity.Normal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("workload list --verbosity normal", result.Args);
            }

            [Fact]
            public void Should_Return_Correct_List_Of_Workloads()
            {
                // Given
                var fixture = new DotNetWorkloadListerFixture();
                fixture.GivenInstalledWorkloadsResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Collection(fixture.Workloads,
                    item =>
                    {
                        Assert.Equal(item.Id, "maui-ios");
                        Assert.Equal(item.ManifestVersion, "6.0.312/6.0.300");
                        Assert.Equal(item.InstallationSource, "VS 17.3.32804.467, VS 17.4.32804.182");
                    },
                    item =>
                    {
                        Assert.Equal(item.Id, "maui-windows");
                        Assert.Equal(item.ManifestVersion, "6.0.312/6.0.300");
                        Assert.Equal(item.InstallationSource, "VS 17.3.32804.467, VS 17.4.32804.182");
                    },
                    item =>
                    {
                        Assert.Equal(item.Id, "android");
                        Assert.Equal(item.ManifestVersion, "32.0.301/6.0.300");
                        Assert.Equal(item.InstallationSource, "VS 17.3.32804.467, VS 17.4.32804.182");
                    });
            }

            [Fact]
            public void Should_Return_Empty_List_Of_Workloads()
            {
                // Given
                var fixture = new DotNetWorkloadListerFixture();
                fixture.GivenEmptyInstalledWorkloadsResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Empty(fixture.Workloads);
            }
        }
    }
}
