// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Cake.Core.Configuration.Parser;
using Cake.Core.Tests.Properties;
using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit.Configuration.Parser
{
    public sealed class ConfigurationParserTests
    {
        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_File_Do_Not_Exist()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var parser = new ConfigurationParser(fileSystem, environment);

                // When
                var result = Record.Exception(() => parser.Read("./cake.config"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("Unable to find the configuration file.", result?.Message);
                Assert.Equal("/Working/cake.config", ((FileNotFoundException)result)?.FileName);
            }

            [Fact]
            public void Should_Throw_If_Section_Contains_Whitespace()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent("[The Section]");
                var parser = new ConfigurationParser(fileSystem, environment);

                // When
                var result = Record.Exception(() => parser.Read("./cake.config"));

                // Then
                AssertEx.IsExceptionWithMessage<InvalidOperationException>(result, "Sections cannot contain whitespace.");
            }

            [Fact]
            public void Should_Throw_If_Equals_Sign_Is_Missing_From_Key_And_Value_Pair()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent("Hello");
                var parser = new ConfigurationParser(fileSystem, environment);

                // When
                var result = Record.Exception(() => parser.Read("./cake.config"));

                // Then
                AssertEx.IsExceptionWithMessage<InvalidOperationException>(result, "Expected to find '=' token.");
            }

            [Fact]
            public void Should_Throw_If_Key_Contains_WhiteSpace()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent("Hello World=True");
                var parser = new ConfigurationParser(fileSystem, environment);

                // When
                var result = Record.Exception(() => parser.Read("./cake.config"));

                // Then
                AssertEx.IsExceptionWithMessage<InvalidOperationException>(result, "The key 'Hello World' contains whitespace.");
            }

            [Fact]
            public void Should_Throw_If_KeyValue_Pair_Is_Not_Followed_By_Section_Or_Another_KeyValue_Pair()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent("Hello=World\n=True");
                var parser = new ConfigurationParser(fileSystem, environment);

                // When
                var result = Record.Exception(() => parser.Read("./cake.config"));

                // Then
                AssertEx.IsExceptionWithMessage<InvalidOperationException>(result, "Encountered unexpected token.");
            }

            [Fact]
            public void Should_Parse_Ini_With_Sections_Correctly()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent(Resources.Ini_Configuration);
                var parser = new ConfigurationParser(fileSystem, environment);

                // When
                var result = parser.Read("/Working/cake.config");

                // Then
                Assert.True(result.ContainsKey("Section1_Foo"));
                Assert.Equal("Bar", result["Section1_Foo"]);
                Assert.True(result.ContainsKey("Section2_Baz"));
                Assert.Equal("Qux", result["Section2_Baz"]);
            }
        }
    }
}