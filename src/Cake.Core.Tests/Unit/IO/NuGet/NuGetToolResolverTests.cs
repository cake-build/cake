using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Testing.Fakes;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO.NuGet
{
    public sealed class NuGetToolResolverTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var globber = Substitute.For<IGlobber>();
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new NuGetToolResolver(null, environment, globber));

                // Then
                Assert.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = new FakeFileSystem(false);
                var globber = Substitute.For<IGlobber>();

                // When
                var result = Record.Exception(() => new NuGetToolResolver(fileSystem, null, globber));

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Globber_Is_Null()
            {
                // Given
                var fileSystem = new FakeFileSystem(false);
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new NuGetToolResolver(fileSystem, environment, null));

                // Then
                Assert.IsArgumentNullException(result, "globber");
            }
        }

        public sealed class TheResolveToolPathMethod
        {
            [Fact]
            public void Should_Throw_If_NuGet_Exe_Could_Not_Be_Found()
            {
                // Given
                var fileSystem = new FakeFileSystem(false);
                var globber = Substitute.For<IGlobber>();
                globber.GetFiles("./tools/**/NuGet.exe").Returns(new FilePath[] { });
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable("NUGET_EXE").Returns(c => null);
                environment.GetEnvironmentVariable("path").Returns(c => null);
                var resolver = new NuGetToolResolver(fileSystem, environment, globber);

                // When
                var result = Record.Exception(() => resolver.ResolveToolPath());

                // Assert
                Assert.IsCakeException(result, "Could not locate nuget.exe.");
            }

            [Fact]
            public void Should_Resolve_In_Correct_Order()
            {
                // Given
                var fileSystem = new FakeFileSystem(false);
                var globber = Substitute.For<IGlobber>();
                globber.GetFiles("./tools/**/NuGet.exe").Returns(new FilePath[] { });
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable("NUGET_EXE").Returns(c => null);
                environment.GetEnvironmentVariable("path").Returns(c => null);
                var resolver = new NuGetToolResolver(fileSystem, environment, globber);

                // When
                Record.Exception(() => resolver.ResolveToolPath());

                // Then
                Received.InOrder(() =>
                {
                    // 1. Look in the tools directory.
                    globber.GetFiles("./tools/**/NuGet.exe");
                    // 2. Look for the environment variable NUGET_EXE.
                    environment.GetEnvironmentVariable("NUGET_EXE");
                    // 3. Panic and look in the path variable.
                    environment.GetEnvironmentVariable("path");
                });
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_From_The_Tools_Directory()
            {
                // Given
                var globber = Substitute.For<IGlobber>();
                globber.Match("./tools/**/NuGet.exe").Returns(p => new FilePath[] { "/root/tools/nuget.exe" });
                var fileSystem = new FakeFileSystem(false);
                fileSystem.GetCreatedFile("/root/tools/nuget.exe");

                var environment = Substitute.For<ICakeEnvironment>();
                var resolver = new NuGetToolResolver(fileSystem, environment, globber);

                // When
                var result = resolver.ResolveToolPath();

                // Then
                Assert.Equal("/root/tools/nuget.exe", result.FullPath);
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_Via_NuGet_Environment_Variable()
            {
                // Given
                var globber = Substitute.For<IGlobber>();
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable("NUGET_EXE").Returns("/programs/nuget.exe");
                var fileSystem = new FakeFileSystem(false);
                fileSystem.GetCreatedFile("/programs/nuget.exe");
                var resolver = new NuGetToolResolver(fileSystem, environment, globber);

                // When
                var result = resolver.ResolveToolPath();

                // Then
                Assert.Equal("/programs/nuget.exe", result.FullPath);
            }

            [Fact]
            public void Should_Be_Able_To_Resolve_Path_Via_Environment_Path_Variable()
            {
                // Given
                var globber = Substitute.For<IGlobber>();
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable("path").Returns("/temp;stuff/programs;/programs");
                var fileSystem = new FakeFileSystem(false);
                fileSystem.GetCreatedDirectory("stuff/programs");
                fileSystem.GetCreatedFile("stuff/programs/nuget.exe");
                var resolver = new NuGetToolResolver(fileSystem, environment, globber);

                // When
                var result = resolver.ResolveToolPath();

                // Then
                Assert.Equal("stuff/programs/nuget.exe", result.FullPath);
            }
        }
    }
}
