// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Solution.Project.Properties;
using Cake.Common.Tests.Fixtures;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution.Project.Properties
{
    public sealed class AssemblyInfoExtensionTests_VB
    {
        [Fact]
        public void Should_Add_CustomAttributes_If_Set_VB()
        {
            // Given
            var fixture = new AssemblyInfoFixture_VB();
            fixture.Settings.AddCustomAttribute("TestAttribute", "Test.NameSpace", "TestValue");

            // When
            var result = fixture.CreateAndReturnContent();

            // Then
            Assert.Contains("Imports Test.NameSpace", result);
            Assert.Contains("<Assembly: TestAttribute(\"TestValue\")>", result);
        }
    }
}