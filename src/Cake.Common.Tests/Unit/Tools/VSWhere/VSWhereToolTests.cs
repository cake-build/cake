// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.VSWhere;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.VSWhere
{
    public sealed class VSWhereToolTests
    {
        public sealed class ToolResolution
        {
            [Fact]
            public void Should_Return_The_Registered_VSWhere_If_VSWhere_Is_Registered()
            {
                // Given
                var fixture = new VSWhereToolFixture(is64BitOperativeSystem: true);

                var registeredVSWhere = fixture.FileSystem.CreateFile("/Registered/vswhere.exe");
                fixture.Tools.RegisterFile(registeredVSWhere.Path);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(registeredVSWhere.Path.FullPath, result.Path.FullPath);
            }

            [Fact]
            public void Should_Return_The_VSWhere_Default_Path_64Bit()
            {
                // Given
                var fixture = new VSWhereToolFixture(is64BitOperativeSystem: true);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Program86/Microsoft Visual Studio/Installer/vswhere.exe", result.Path.FullPath);
            }

#pragma warning disable SA1005 // Single line comments should begin with single space
            //[Fact]
            //public void Should_Return_The_VSWhere_Default_Path_32Bit()
            //{
            //    // Given
            //    var fixture = new VSWhereToolFixture(is64BitOperativeSystem: false);

            //    // When
            //    var result = fixture.Run();

            //    // Then
            //    Assert.Equal("/Program/Microsoft Visual Studio/Installer/vswhere.exe", result.Path.FullPath);
            //}
#pragma warning restore SA1005 // Single line comments should begin with single space
        }
    }
}
