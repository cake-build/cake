// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Uninstall;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Workload.Uninstall
{
    public sealed class DotNetWorkloadUninstallTests
    {
        public sealed class TheWorkloadSearchMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetWorkloadUninstallerFixture();
                fixture.WorkloadIds = new string[] { "maui" };
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
                var fixture = new DotNetWorkloadUninstallerFixture();
                fixture.WorkloadIds = new string[] { "maui" };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_WorkloadIds_Argument()
            {
                // Given
                var fixture = new DotNetWorkloadUninstallerFixture();
                fixture.WorkloadIds = new string[] { "maui-android", "maui-ios" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("workload uninstall maui-android maui-ios", result.Args);
            }

            [Fact]
            public void Should_Throw_If_WorkloadIds_Is_Empty()
            {
                // Given
                var fixture = new DotNetWorkloadUninstallerFixture();
                fixture.WorkloadIds = new string[] { };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "workloadIds");
            }

            [Fact]
            public void Should_Throw_If_WorkloadIds_Is_Null()
            {
                // Given
                var fixture = new DotNetWorkloadUninstallerFixture();
                fixture.WorkloadIds = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "workloadIds");
            }
        }
    }
}
