// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    }
}
