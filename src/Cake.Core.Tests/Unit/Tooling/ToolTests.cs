// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit.Tooling
{
    public sealed class ToolTests
    {
        public sealed class TheRunProcessMethod
        {
            [Fact]
            public void Should_Use_Arguments_Provided_In_Tool_Settings()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.Settings.ArgumentCustomization = builder => builder.Append("--bar");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--foo --bar", result.Args);
            }

            [Fact]
            public void Should_Replace_Arguments_Provided_In_Tool_Settings_If_Returning_A_New_Builder()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.Settings.ArgumentCustomization = builder =>
                {
                    var newBuilder = new ProcessArgumentBuilder();
                    newBuilder.Append("--bar");
                    return newBuilder;
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--bar", result.Args);
            }

            [Fact]
            public void Should_Replace_Arguments_Provided_In_Tool_Settings_If_Returning_A_String()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.Settings.ArgumentCustomization = builder => "--bar";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--bar", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory_If_Provided_In_Tool_Settings()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.Settings.WorkingDirectory = "/Other";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Other", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Succeed_On_Zero_ExitCode_Without_Custom_Validation()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.GivenProcessExitsWithCode(0);

                // When
                var result = fixture.Run();

                // Then
                Assert.IsNotType<Exception>(result);
            }

            [Fact]
            public void Should_Throw_On_NonZero_ExitCode_Without_Custom_Validation()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.GivenProcessExitsWithCode(11);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "dummy: Process returned an error (exit code 11).");
            }

            [Fact]
            public void Should_Succeed_On_NonZero_ExitCode_Validated_By_Custom_Validator()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.ExitCodeValidation = ec =>
                {
                    if (ec >= 10)
                    {
                        throw new CakeException("UnitTest");
                    }
                };
                fixture.GivenProcessExitsWithCode(7);

                // When
                var result = fixture.Run();

                // Then
                Assert.IsNotType<Exception>(result);
            }

            [Fact]
            public void Should_Throw_On_Invalid_ExitCode_Validated_By_Custom_Validator()
            {
                // Given
                var fixture = new DummyToolFixture();
                fixture.ExitCodeValidation = ec =>
                {
                    if (ec != 1)
                    {
                        throw new CakeException("UnitTest");
                    }
                };
                fixture.GivenProcessExitsWithCode(10);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "UnitTest");
            }

            [RuntimeFact(TestRuntime.CoreClr)]
            public void Should_Prioritize_Dll_On_CoreClr()
            {
                // Given
                var fixture = new DummyToolFixture(hasDllExe: true);
                fixture.GivenIsCoreClr();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/dummy.dll", result.Path.FullPath);
            }

            [RuntimeFact(TestRuntime.CoreClr)]
            public void Should_Fallback_To_First_Resolved_Tool_On_CoreClr_And_No_Dll_Provided()
            {
                // Given
                var fixture = new DummyToolFixture(hasDllExe: false);
                fixture.GivenIsCoreClr();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/dummy.exe", result.Path.FullPath);
            }
        }
    }
}