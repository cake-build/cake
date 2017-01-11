// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Solution.Project.Properties;
using Cake.Common.Tests.Fixtures;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution.Project.Properties
{
    public sealed class AssemblyInfoExtensionTests
    {
        [Fact]
        public void Should_Add_CustomAttributes_If_Set()
        {
            // Given
            var fixture = new AssemblyInfoFixture();
            fixture.Settings.AddCustomAttribute("TestAttribute", "Test.NameSpace", "TestValue");

            // When
            var result = fixture.CreateAndReturnContent();

            // Then
            Assert.True(result.Contains("using Test.NameSpace;"));
            Assert.True(result.Contains("[assembly: TestAttribute(\"TestValue\")]"));
        }
    }
}