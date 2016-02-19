using Cake.Common.Tests.Fixtures.Tools.DNVM.Use;
using Cake.Common.Tools.DNVM.Use;
using Cake.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DNVM.Use
{
    public sealed class DNVMUserTests
    {
        public sealed class TheUseMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DNVMUserFixture();
                fixture.Settings = null;
                fixture.Version = "default";
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_DNVM_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new DNVMUserFixture();
                fixture.Settings = new DNVMSettings();
                fixture.Version = "default";
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNVM: Could not locate executable.");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DNVMUserFixture();
                fixture.Settings = new DNVMSettings();
                fixture.Version = "default";
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNVM: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DNVMUserFixture();
                fixture.Settings = new DNVMSettings();
                fixture.Version = "default";
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "DNVM: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Throw_If_Version_Is_Null()
            {
                // Given
                var fixture = new DNVMUserFixture();
                fixture.Version = "";
                fixture.Settings = new DNVMSettings() { };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "version");
            }

            [Fact]
            public void Should_Add_Arch_If_Set()
            {
                // Given
                var fixture = new DNVMUserFixture();
                fixture.Version = "default";
                fixture.Settings = new DNVMSettings() { Architecture = Common.Tools.DNArchitecture.X64 };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("use default -a X64", result.Args);
            }

            [Fact]
            public void Should_Add_Runtime_If_Set()
            {
                // Given
                var fixture = new DNVMUserFixture();
                fixture.Version = "default";
                fixture.Settings = new DNVMSettings() { Runtime = Common.Tools.DNRuntime.CoreClr };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("use default -r CoreClr", result.Args);
            }

            [Fact]
            public void Should_Add_Version()
            {
                // Given
                var fixture = new DNVMUserFixture();
                fixture.Version = "default";
                fixture.Settings = new DNVMSettings() { };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("use default", result.Args);
            }
        }
    }
}
