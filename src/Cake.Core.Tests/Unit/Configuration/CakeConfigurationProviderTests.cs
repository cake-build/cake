// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Tests.Fixtures;
using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit.Configuration
{
    public sealed class CakeConfigurationProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheCreateMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Path = null;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_Arguments_Are_Null()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Arguments = null;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                AssertEx.IsArgumentNullException(result, "arguments");
            }

            [Fact]
            public void Should_Add_Prefixed_Environment_Variables()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("CAKE_FOO", "Bar");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Bar", result.GetValue("FOO"));
            }

            [Fact]
            public void Should_Retrieve_Prefixed_Environment_Variables_Regardless_Of_Casing()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("CAKE_FOO", "Bar");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Bar", result.GetValue("foo"));
            }

            [Fact]
            public void Should_Not_Add_Non_Prefixed_Environment_Variables()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("FOO", "Bar");

                // When
                var result = fixture.Create();

                // Then
                Assert.Null(result.GetValue("FOO"));
            }

            [Fact]
            public void Should_Add_Configuration_File_Variables()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.FileSystem.CreateFile("/Working/cake.config").SetContent("Foo=Bar");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Bar", result.GetValue("Foo"));
            }

            [Fact]
            public void Should_Retrieve_Configuration_File_Variables_Regardless_Of_Casing()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.FileSystem.CreateFile("/Working/cake.config").SetContent("Foo=Bar");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Bar", result.GetValue("FOO"));
            }

            [Fact]
            public void Should_Add_Configuration_File_Variables_With_Section()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.FileSystem.CreateFile("/Working/cake.config").SetContent("[Foo]\nBar=Baz");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Baz", result.GetValue("Foo_Bar"));
            }

            [Fact]
            public void Should_Retrieve_Configuration_File_Variables_With_Section_Regardless_Of_Casing()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.FileSystem.CreateFile("/Working/cake.config").SetContent("[Foo]\nBar=Baz");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Baz", result.GetValue("FOO_BAR"));
            }

            [Fact]
            public void Should_Add_Arguments()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Arguments.Add("foo_bar", "Baz");
                fixture.Arguments.Add("baz", "Qux");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Baz", result.GetValue("foo_bar"));
                Assert.Equal("Qux", result.GetValue("baz"));
            }

            [Fact]
            public void Should_Retrieve_Arguments_Regardless_Of_Casing()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Arguments.Add("foo_bar", "Baz");
                fixture.Arguments.Add("baz", "Qux");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Baz", result.GetValue("FOO_BAR"));
                Assert.Equal("Qux", result.GetValue("BAZ"));
            }

            [Fact]
            public void Should_Use_Value_From_Configuration_File_Over_Environment_Variable()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("CAKE_FOO", "Bar");
                fixture.FileSystem.CreateFile("/Working/cake.config").SetContent("FOO=Qux");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Qux", result.GetValue("FOO"));
            }

            [Fact]
            public void Should_Use_Value_From_Argument_Over_Environment_Variable_And_Configuration_File()
            {
                // Given
                var fixture = new CakeConfigurationProviderFixture();
                fixture.Environment.SetEnvironmentVariable("CAKE_FOO", "Bar");
                fixture.FileSystem.CreateFile("/Working/cake.config").SetContent("FOO=Qux");
                fixture.Arguments.Add("FOO", "Baz");

                // When
                var result = fixture.Create();

                // Then
                Assert.Equal("Baz", result.GetValue("FOO"));
            }
        }
    }
}