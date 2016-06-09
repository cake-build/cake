// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;
using Cake.Tests.Fixtures;

namespace Cake.Tests.Unit.Scripting.Mono
{
    public sealed class MonoScriptProcessorTests
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Ok, this test setup isn't intuitive at all, and I apologize for this.
        //
        // In this project, there's a folder called Data. This folder contains
        // sub folders which in turn contain files as embedded resources.
        // There are two files in each folder; input and output.
        //
        // We will call the `Process` method on the fixture class that will
        // read the content of `input`, process it and return it to us.
        // We then call the `GetExpectedOutput` method that will load the `output`
        // file and return it as a string to us. We can then compare the
        // processed input with the expected output.
        //
        // Like I said, not very intuitive, but it simplifies testing greatly,
        // since all we have to do is to define a new theory that point out the
        // folder that contains the input and output for the test.
        ///////////////////////////////////////////////////////////////////////////////

        [Theory]
        [InlineData("Cake.Tests.Data.MonoScriptProcessor.Simple")]
        [InlineData("Cake.Tests.Data.MonoScriptProcessor.Mixed")]
        [InlineData("Cake.Tests.Data.MonoScriptProcessor.MixedComments")]
        [InlineData("Cake.Tests.Data.MonoScriptProcessor.ArrayInitializer")]
        [InlineData("Cake.Tests.Data.MonoScriptProcessor.Blocks")]
        [InlineData("Cake.Tests.Data.MonoScriptProcessor.Complex")]
        public void Should_Parse_Resources(string resource)
        {
            // Given
            var fixture = new MonoScriptProcessorFixture(resource);

            // When
            var result = fixture.Process();

            // Then
            var expected = fixture.GetExpectedOutput();
            Assert.Equal(expected, result);
        }
    }
}
