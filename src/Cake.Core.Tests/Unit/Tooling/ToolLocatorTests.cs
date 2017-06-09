// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.Tooling
{
    public sealed class ToolLocatorTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var repository = Substitute.For<IToolRepository>();
                var strategy = Substitute.For<IToolResolutionStrategy>();

                // When
                var result = Record.Exception(() => new ToolLocator(null, repository, strategy));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Tool_Repository_Is_Null()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var strategy = Substitute.For<IToolResolutionStrategy>();

                // When
                var result = Record.Exception(() => new ToolLocator(environment, null, strategy));

                // Then
                AssertEx.IsArgumentNullException(result, "repository");
            }

            [Fact]
            public void Should_Throw_If_Tool_Resolution_Strategy_Is_Null()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var repository = Substitute.For<IToolRepository>();

                // When
                var result = Record.Exception(() => new ToolLocator(environment, repository, null));

                // Then
                AssertEx.IsArgumentNullException(result, "strategy");
            }
        }

        public sealed class TheRegisterFileMethod
        {
            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var repository = Substitute.For<IToolRepository>();
                var strategy = Substitute.For<IToolResolutionStrategy>();
                var locator = new ToolLocator(environment, repository, strategy);

                // When
                var result = Record.Exception(() => locator.RegisterFile(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Register_File_Path_With_Tool_Repository()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var repository = Substitute.For<IToolRepository>();
                var strategy = Substitute.For<IToolResolutionStrategy>();
                var locator = new ToolLocator(environment, repository, strategy);

                // When
                locator.RegisterFile("./mytools/test.exe");

                // Then
                repository.Received(1)
                    .Register(Arg.Is<FilePath>(path => path.FullPath == "/Working/mytools/test.exe"));
            }
        }

        public sealed class TheResolveMethod
        {
            [Fact]
            public void Should_Throw_If_Tool_Is_Null()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var repository = Substitute.For<IToolRepository>();
                var strategy = Substitute.For<IToolResolutionStrategy>();
                var locator = new ToolLocator(environment, repository, strategy);

                // When
                var result = Record.Exception(() => locator.Resolve(null));

                // Then
                AssertEx.IsArgumentNullException(result, "tool");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            [InlineData(" ")]
            public void Should_Throw_If_Tool_Is_Empty(string tool)
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var repository = Substitute.For<IToolRepository>();
                var strategy = Substitute.For<IToolResolutionStrategy>();
                var locator = new ToolLocator(environment, repository, strategy);

                // When
                var result = Record.Exception(() => locator.Resolve(tool));

                // Then
                AssertEx.IsArgumentException(result, "tool", "Tool name cannot be empty.");
            }

            [Fact]
            public void Should_Resolve_Tool_Using_The_Tool_Resolution_Strategy()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var repository = Substitute.For<IToolRepository>();
                var strategy = Substitute.For<IToolResolutionStrategy>();
                var locator = new ToolLocator(environment, repository, strategy);

                // When
                locator.Resolve("test.exe");

                // Then
                strategy.Received(1).Resolve(
                    Arg.Is(repository),
                    Arg.Is("test.exe"));
            }
        }
    }
}