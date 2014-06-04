using Cake.Bootstrapping;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Tests.Unit.Bootstrapper
{
    public sealed class CakeBootstrapperTests
    {
        [Fact]
        public void Should_Skip_Downloading_Roslyn_If_Already_Downloaded()
        {
            // Given
            var csharpFile = Substitute.For<IFile>();
            csharpFile.Exists.Returns(true);
            csharpFile.Path.Returns(new FilePath("Roslyn.Compilers.CSharp.dll"));

            var compilerFile = Substitute.For<IFile>();
            compilerFile.Exists.Returns(true);
            compilerFile.Path.Returns(new FilePath("Roslyn.Compilers.dll"));

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath.EndsWith("Roslyn.Compilers.CSharp.dll"))).Returns(csharpFile);
            fileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath.EndsWith("Roslyn.Compilers.dll"))).Returns(compilerFile);

            var log = Substitute.For<ICakeLog>();
            var installer = Substitute.For<INuGetInstaller>();
            var bootstrapper = new CakeBootstrapper(fileSystem, log, installer);
            var root = new DirectoryPath("/Build");

            // When
            bootstrapper.Bootstrap(root);

            // Then
            installer.Received(0).Install(Arg.Any<DirectoryPath>());
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void Should_Downloading_Roslyn_If_Missing(bool csharpMissing, bool compilerMissing)
        {
            // Given
            var csharpFile = Substitute.For<IFile>();
            csharpFile.Exists.Returns(csharpMissing);
            csharpFile.Path.Returns(new FilePath("Roslyn.Compilers.CSharp.dll"));

            var compilerFile = Substitute.For<IFile>();
            compilerFile.Exists.Returns(compilerMissing);
            compilerFile.Path.Returns(new FilePath("Roslyn.Compilers.dll"));

            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath.EndsWith("Roslyn.Compilers.CSharp.dll"))).Returns(csharpFile);
            fileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath.EndsWith("Roslyn.Compilers.dll"))).Returns(compilerFile);

            var log = Substitute.For<ICakeLog>();
            var installer = Substitute.For<INuGetInstaller>();
            var bootstrapper = new CakeBootstrapper(fileSystem, log, installer);
            var root = new DirectoryPath("/Build");

            // When
            bootstrapper.Bootstrap(root);

            // Then
            installer.Received(1).Install(Arg.Any<DirectoryPath>());
        }
    }
}
