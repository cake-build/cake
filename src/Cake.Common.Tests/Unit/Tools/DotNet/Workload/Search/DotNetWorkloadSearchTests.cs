// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Workload.Search;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Workload.Search
{
    public sealed class DotNetWorkloadSearchTests
    {
        public sealed class TheWorkloadSearchMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetWorkloadSearcherFixture();
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
                var fixture = new DotNetWorkloadSearcherFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_SearchString_Argument()
            {
                // Given
                var fixture = new DotNetWorkloadSearcherFixture();
                fixture.SearchString = "maui";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("workload search maui", result.Args);
            }

            [Fact]
            public void Should_Return_Correct_List_Of_Workloads()
            {
                // Given
                var fixture = new DotNetWorkloadSearcherFixture();
                fixture.SearchString = "maui";
                fixture.GivenAvailableWorkloadsResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Collection(fixture.Workloads,
                    item =>
                    {
                        Assert.Equal(item.Id, "maui");
                        Assert.Equal(item.Description, ".NET MAUI SDK for all platforms");
                    },
                    item =>
                    {
                        Assert.Equal(item.Id, "maui-desktop");
                        Assert.Equal(item.Description, ".NET MAUI SDK for Desktop");
                    },
                    item =>
                    {
                        Assert.Equal(item.Id, "maui-mobile");
                        Assert.Equal(item.Description, ".NET MAUI SDK for Mobile");
                    });
            }
        }
    }
}
