// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.SDKCheck;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.SDKCheck
{
    public sealed class DotNetSDKCheckTests
    {
        public sealed class TheSDKCheckMethod
        {
            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetSDKCheckerFixture();
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
                var fixture = new DotNetSDKCheckerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }
        }
    }
}
