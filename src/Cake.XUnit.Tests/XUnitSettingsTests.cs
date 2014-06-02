using System;
using System.Linq;
using Cake.Core.IO;
using Xunit;

namespace Cake.XUnit.Tests
{
    public sealed class XUnitSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_No_Assmebly_Paths_Were_Provided()
            {
                // Given, When
                var result = Record.Exception(() => new XUnitSettings(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("assemblyPaths", ((ArgumentNullException) result).ParamName);
            }
        }

        public sealed class TheGetAssemblyPathsMethod
        {
            [Fact]
            public void Should_Return_The_Assembly_Paths_Provided_To_The_Constructor()
            {
                // Given
                var assemblyPaths = new FilePath[] {"A", "B"};
                var settings = new XUnitSettings(assemblyPaths);

                // When
                var paths = settings.GetAssemblyPaths().ToArray();

                // Then
                Assert.Contains(assemblyPaths[0], paths);
                Assert.Contains(assemblyPaths[1], paths);
            }
        }
    }
}
