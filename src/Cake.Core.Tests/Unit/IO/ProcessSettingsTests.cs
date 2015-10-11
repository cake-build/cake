using System;
using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class ProcessSettingsTests
    {
        public sealed class ExtensionMethods
        {
            [Theory]
            [InlineData("Hello World", "Hello World")]
            [InlineData("", "")]
            [InlineData(" \t ", " \t ")]
            [InlineData(null, "")]
            public void Should_Return_Settings_With_Correct_Arguments(string value, string expected)
            {
                var settings = new ProcessSettings().WithArguments(args => { args.Append(value); });

                Assert.Equal(expected, settings.Arguments.Render());
            }

            [Theory]
            [InlineData("C:/Test.zip", "C:/Test.zip")]
            [InlineData("../Test.zip", "../Test.zip")]
            public void Should_Return_Settings_With_Correct_Directory(string value, string expected)
            {
                var settings = new ProcessSettings().UseWorkingDirectory(value);

                Assert.Equal(expected, settings.WorkingDirectory.FullPath);
            }

            [Theory]
            [InlineData(true, true)]
            [InlineData(false, false)]
            public void Should_Return_Settings_With_Correct_Output(bool value, bool expected)
            {
                var settings = new ProcessSettings().SetRedirectStandardOutput(value);

                Assert.Equal(expected, settings.RedirectStandardOutput);
            }

            [Theory]
            [InlineData(true, true)]
            [InlineData(false, false)]
            public void Should_Return_Settings_With_Correct_StandardError(bool value, bool expected)
            {
                var settings = new ProcessSettings().SetRedirectStandardError(value);

                Assert.Equal(expected, settings.RedirectStandardError);
            }

            [Theory]
            [InlineData(0, 0)]
            [InlineData(5000, 5000)]
            [InlineData(15000, 15000)]
            public void Should_Return_Settings_With_Correct_Timeout(int value, int expected)
            {
                var settings = new ProcessSettings().SetTimeout(value);

                Assert.Equal(expected, settings.Timeout);
            }

            [Fact]
            public void Should_Return_Settings_With_Correct_EnvironmentVariables()
            {
                var settings =
                    new ProcessSettings().WithEnvironmentVariable("Key1", "Value1")
                        .WithEnvironmentVariable("Key2", "Value2");

                Assert.Equal("Value1", settings.EnvironmentVariables["Key1"]);
                Assert.Equal("Value2", settings.EnvironmentVariables["Key2"]);
            }

            [Fact]
            public void EnvironmentVariable_Set_Multiple_Times_Should_Use_The_Last_One()
            {
                var settings =
                    new ProcessSettings().WithEnvironmentVariable("Key1", "Value1")
                        .WithEnvironmentVariable("Key1", "Value1-A");

                Assert.Equal("Value1-A", settings.EnvironmentVariables["Key1"]);
            }

            [Fact]
            public void EnvironmentVariable_Key_Should_Not_Be_Null()
            {
                Assert.Throws<ArgumentNullException>(() => new ProcessSettings().WithEnvironmentVariable(null, "Value1"));
            }

            [Fact]
            public void EnvironmentVariables_Should_Be_Empty_By_Default()
            {
                var settings = new ProcessSettings();

                Assert.NotNull(settings.EnvironmentVariables);
                Assert.Empty(settings.EnvironmentVariables.Keys);
            }
        }
    }
}
