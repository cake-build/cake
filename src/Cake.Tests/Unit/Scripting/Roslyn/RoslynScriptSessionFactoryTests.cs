using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Scripting.Roslyn;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting.Roslyn
{
    public sealed class RoslynScriptSessionFactoryTests
    {
        public sealed class TheInitializeMethod
        {
            [Fact]
            public void Should_Not_Install_Roslyn_If_Installed()
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var installer = Substitute.For<IRoslynInstaller>();
                installer.IsInstalled(Arg.Any<DirectoryPath>()).Returns(true);
                var environment = Substitute.For<ICakeEnvironment>();
                var factory = new RoslynScriptSessionFactory(environment, installer, log);

                // When
                factory.Initialize();

                // Then
                installer.Received(0).Install(Arg.Any<DirectoryPath>());
            }

            [Fact]
            public void Should_Install_Roslyn_If_Not_Installed()
            {
                // Given
                var log = Substitute.For<ICakeLog>();
                var installer = Substitute.For<IRoslynInstaller>();
                installer.IsInstalled(Arg.Any<DirectoryPath>()).Returns(false);
                var environment = Substitute.For<ICakeEnvironment>();
                var factory = new RoslynScriptSessionFactory(environment, installer, log);

                // When
                factory.Initialize();

                // Then
                installer.Received(1).Install(Arg.Any<DirectoryPath>());
            }
        }
    }
}
