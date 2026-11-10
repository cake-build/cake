// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Core.Tooling;
using Xunit;

namespace Cake.Core.Tests.Unit;

public sealed class CakeContextTests
{
    public sealed class TheConstructor
    {
        [Fact]
        public void Should_Throw_If_File_System_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.FileSystem = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "fileSystem");
        }

        [Fact]
        public void Should_Throw_If_Environment_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.Environment = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "environment");
        }

        [Fact]
        public void Should_Throw_If_Globber_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.Globber = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "globber");
        }

        [Fact]
        public void Should_Throw_If_Log_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.Log = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "log");
        }

        [Fact]
        public void Should_Throw_If_Arguments_Are_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.Arguments = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "arguments");
        }

        [Fact]
        public void Should_Throw_If_Process_Runner_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.ProcessRunner = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "processRunner");
        }

        [Fact]
        public void Should_Throw_If_Registry_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.Registry = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "registry");
        }

        [Fact]
        public void Should_Throw_If_Tools_Are_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.Tools = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "tools");
        }

        [Fact]
        public void Should_Throw_If_Configuration_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.Configuration = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "configuration");
        }

        [Fact]
        public void Should_Throw_If_Tool_Installer_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.ToolInstaller = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "toolInstaller");
        }

        [Fact]
        public void Should_Throw_If_Service_Provider_Is_Null()
        {
            // Given
            var fixture = new CakeContextFixture();
            fixture.ServiceProvider = null;

            // When
            var result = Record.Exception(() => fixture.CreateContext());

            // Then
            AssertEx.IsArgumentNullException(result, "serviceProvider");
        }
    }

    public sealed class TheFileSystemProperty
    {
        [Fact]
        public void Should_Return_Provided_File_System()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var fileSystem = context.FileSystem;

            // Then
            Assert.Same(fixture.FileSystem, fileSystem);
        }

        [Fact]
        public void Should_Return_Provided_Environment()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var environment = context.Environment;

            // Then
            Assert.Same(fixture.Environment, environment);
        }

        [Fact]
        public void Should_Return_Provided_Globber()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var globber = context.Globber;

            // Then
            Assert.Same(fixture.Globber, globber);
        }

        [Fact]
        public void Should_Return_Provided_Log()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var log = context.Log;

            // Then
            Assert.Same(fixture.Log, log);
        }

        [Fact]
        public void Should_Return_Provided_Arguments()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var arguments = context.Arguments;

            // Then
            Assert.Same(fixture.Arguments, arguments);
        }

        [Fact]
        public void Should_Return_Provided_Process_Runner()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var processRunner = context.ProcessRunner;

            // Then
            Assert.Same(fixture.ProcessRunner, processRunner);
        }

        [Fact]
        public void Should_Return_Provided_Configuration()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var configuration = context.Configuration;

            // Then
            Assert.Same(fixture.Configuration, configuration);
        }

        [Fact]
        public void Should_Return_Provided_Tool_Installer()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var toolInstaller = context.ToolInstaller;

            // Then
            Assert.Same(fixture.ToolInstaller, toolInstaller);
        }

        [Fact]
        public void Should_Return_Provided_Service_Provider()
        {
            // Given
            var fixture = new CakeContextFixture();
            var context = fixture.CreateContext();

            // When
            var serviceProvider = context.ServiceProvider;

            // Then
            Assert.Same(fixture.ServiceProvider, serviceProvider);
        }
    }

    public sealed class TheDefaultInterfaceMembers
    {
        [Fact]
        public void Should_Throw_If_Service_Provider_Is_Not_Implemented()
        {
            // Given
            ICakeContext context = new ContextWithoutServiceProvider();

            // When
            var result = Record.Exception(() => context.ServiceProvider);

            // Then
            AssertEx.IsCakeException(result, "The current ICakeContext does not provide a service provider.");
        }

        private sealed class ContextWithoutServiceProvider : ICakeContext
        {
            public IFileSystem FileSystem { get; }
            public ICakeEnvironment Environment { get; }
            public IGlobber Globber { get; }
            public ICakeLog Log { get; }
            public ICakeArguments Arguments { get; }
            public IProcessRunner ProcessRunner { get; }
            public IRegistry Registry { get; }
            public IToolLocator Tools { get; }
            public ICakeDataResolver Data { get; }
            public ICakeConfiguration Configuration { get; }
        }
    }
}
