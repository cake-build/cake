using System;
using Cake.Commands;
using Cake.Core.Diagnostics;
using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeApplicationTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.CreateApplication());

                // Then
                Assert.IsArgumentNullException(result, "log");
            }

            [Fact]
            public void Should_Throw_If_Command_Factory_Is_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.CommandFactory = null;

                // When
                var result = Record.Exception(() => fixture.CreateApplication());

                // Then
                Assert.IsArgumentNullException(result, "commandFactory");
            }

            [Fact]
            public void Should_Throw_If_Argument_Parser_Is_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.ArgumentParser = null;

                // When
                var result = Record.Exception(() => fixture.CreateApplication());

                // Then
                Assert.IsArgumentNullException(result, "argumentParser");
            }

            [Fact]
            public void Should_Throw_If_Console_Is_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Console = null;

                // When
                var result = Record.Exception(() => fixture.CreateApplication());

                // Then
                Assert.IsArgumentNullException(result, "console");
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Set_Verbosity_If_Options_Are_Not_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();

                // When
                fixture.RunApplication();

                // Then
                fixture.Log.Received(1).SetVerbosity(Verbosity.Diagnostic);
            }

            [Fact]
            public void Should_Return_Success_If_No_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                var command = Substitute.For<ICommand>();
                command.Execute(fixture.Options).Returns(true);
                fixture.Options.Script = "./build.cake";
                fixture.CommandFactory.CreateBuildCommand().Returns(command);
                
                // When
                var result = fixture.RunApplication();

                // Then
                Assert.Equal(0, result);
            }

            [Fact]
            public void Should_Return_Failure_If_An_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.ShowHelp = true;
                fixture.CommandFactory.When(x => x.CreateHelpCommand())
                    .Do(info => { throw new Exception("Error!"); });

                // When
                var result = fixture.RunApplication();

                // Then
                Assert.Equal(1, result);
            }

            [Fact]
            public void Should_Create_Help_Command_If_Specified_In_Options()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.ShowHelp = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateHelpCommand();
            }

            [Fact]
            public void Should_Create_Version_Command_If_Specified_In_Options()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.ShowVersion = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateVersionCommand();
            }

            [Fact]
            public void Should_Create_Description_Command_If_Specified_In_Options()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.ShowDescription = true;
                fixture.Options.Script = "./build.cake";

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateDescriptionCommand();
            }

            [Fact]
            public void Should_Not_Create_Description_Command_If_Options_Do_Not_Contain_Script()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.ShowDescription = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateHelpCommand();
            }

            [Fact]
            public void Should_Create_Build_Command_If_Options_Contain_Script()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.Script = "./build.cake";

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateBuildCommand();
            }

            [Fact]
            public void Should_Create_Help_Command_If_Options_Are_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options = null;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateHelpCommand();
            }

            [Fact]
            public void Should_Return_Error_If_Options_Are_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options = null;

                // When
                var result = fixture.RunApplication();

                // Then
                Assert.Equal(1, result);
            }

            [Fact]
            public void Should_Create_Help_Command_Even_If_Script_Is_Specified()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.Script = "./build.cake";
                fixture.Options.ShowHelp = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(0).CreateBuildCommand();
                fixture.CommandFactory.Received(1).CreateHelpCommand();
            }

            [Fact]
            public void Should_Create_Version_Command_Even_If_Script_Is_Specified()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.Script = "./build.cake";
                fixture.Options.ShowVersion = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(0).CreateBuildCommand();
                fixture.CommandFactory.Received(1).CreateVersionCommand();
            }

            [Fact]
            public void Should_Return_Error_If_No_Parameters_Are_Set()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options = new CakeOptions();

                // When
                var result = fixture.RunApplication();

                // Then
                Assert.Equal(1, result);
            }
        }
    }
}
