using System;
using System.Diagnostics;
using System.Linq;
using Cake.Common.NuGet;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.NuGet
{
    public sealed class NuGetPackerTests
    {
        public sealed class ThePackMethod
        {
            [Fact]
            public void Should_Throw_If_Nuspec_File_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() => packer.Pack(null, new NuGetPackSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("nuspecFilePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() => packer.Pack("./existing.nuspec", null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.Globber.Match("./tools/**/NuGet.exe").Returns(Enumerable.Empty<Path>());
                var packer = fixture.CreatePacker();

                // When
                var result = Record.Exception(() => packer.Pack("./existing.nuspec", new NuGetPackSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not find NuGet.exe.", result.Message);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    Version = "1.0.0"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack -Version \"1.0.0\" \"existing.nuspec\""));
            }

            [Fact]
            public void Should_Add_Base_Path_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    BasePath = "./build"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack -BasePath \"build\" \"existing.nuspec\""));
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    OutputDirectory = "./build/output"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack -OutputDirectory \"build/output\" \"existing.nuspec\""));
            }

            [Fact]
            public void Should_Add_No_Package_Analysis_Flag_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    NoPackageAnalysis = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack \"existing.nuspec\" -NoPackageAnalysis"));
            }

            [Fact]
            public void Should_Add_Symbols_Flag_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var packer = fixture.CreatePacker();

                // When
                packer.Pack("./existing.nuspec", new NuGetPackSettings
                {
                    Symbols = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "pack \"existing.nuspec\" -Symbols"));
            }
        }
    }
}
