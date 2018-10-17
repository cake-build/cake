using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Core.Composition;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting;
using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Features
{
    public sealed class BootstrapFeatureTests
    {
        [Fact]
        public async Task The_Bootstrap_Option_Should_Install_Modules()
        {
            // Given
            var fixture = new ProgramFixture();

            // When
            var result = await fixture.Run("--bootstrap");

            // Then
            fixture.Builder.Processor.Received(1).InstallModules(
                Arg.Any<IReadOnlyCollection<PackageReference>>(),
                Arg.Is<DirectoryPath>(p => p.FullPath == "/Working/tools/Modules"));
        }
    }
}
