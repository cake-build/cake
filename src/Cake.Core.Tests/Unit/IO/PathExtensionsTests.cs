// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class PathExtensionsTests
    {
        public sealed class TheExpandEnvironmentVariablesMethod
        {
            public sealed class ThatTakesAFilePath
            {
                [Fact]
                public void Should_Throw_If_Environment_Is_Null()
                {
                    // Given
                    var path = new FilePath("/%FOO%/baz.qux");

                    // When
                    var result = Record.Exception(() => path.ExpandEnvironmentVariables(null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "environment");
                }

                [Fact]
                public void Should_Expand_Existing_Environment_Variables()
                {
                    // Given
                    var environment = FakeEnvironment.CreateWindowsEnvironment();
                    environment.SetEnvironmentVariable("FOO", "bar");
                    var path = new FilePath("/%FOO%/baz.qux");

                    // When
                    var result = path.ExpandEnvironmentVariables(environment);

                    // Then
                    Assert.Equal("/bar/baz.qux", result.FullPath);
                }
            }

            public sealed class ThatTakesADirectoryPath
            {
                [Fact]
                public void Should_Throw_If_Environment_Is_Null()
                {
                    // Given
                    var path = new DirectoryPath("/%FOO%/baz");

                    // When
                    var result = Record.Exception(() => path.ExpandEnvironmentVariables(null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "environment");
                }

                [Fact]
                public void Should_Expand_Existing_Environment_Variables()
                {
                    // Given
                    var environment = FakeEnvironment.CreateWindowsEnvironment();
                    environment.SetEnvironmentVariable("FOO", "bar");
                    var path = new DirectoryPath("/%FOO%/baz");

                    // When
                    var result = path.ExpandEnvironmentVariables(environment);

                    // Then
                    Assert.Equal("/bar/baz", result.FullPath);
                }
            }
        }

        public sealed class TheExpandShortPathMethod
        {
            [Theory]
            [InlineData("C:/Program Files/cake-build/addins", "C:/Program Files/cake-build/addins")]
            [InlineData("C:/PROGRA~1/cake-build/addins", "C:/Program Files/cake-build/addins")]
            public void Will_Normalize_Short_Paths_File(string input, string expected)
            {
                // Given, When
                var path = new FilePath(input);

                path = path.ExpandShortPath();

                // Then
                if (OperatingSystem.IsWindows())
                {
                    Assert.Equal(expected, path.FullPath);
                }
            }

            [Theory]
            [InlineData("C:/Program Files/cake-build/addins", "C:/Program Files/cake-build/addins")]
            [InlineData("C:/PROGRA~1/cake-build/addins", "C:/Program Files/cake-build/addins")]
            public void Will_Normalize_Short_Paths_Directory(string input, string expected)
            {
                // Given, When
                var path = new DirectoryPath(input);

                path = path.ExpandShortPath();

                // Then
                if (OperatingSystem.IsWindows())
                {
                    Assert.Equal(expected, path.FullPath);
                }
            }
        }
    }
}
