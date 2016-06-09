// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
            [InlineData(0, 0)]
            [InlineData(5000, 5000)]
            [InlineData(15000, 15000)]
            public void Should_Return_Settings_With_Correct_Timeout(int value, int expected)
            {
                var settings = new ProcessSettings().SetTimeout(value);

                Assert.Equal(expected, settings.Timeout);
            }
        }
    }
}
