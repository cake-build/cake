// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.ContinuaCI;
using Cake.Common.Tests.Fakes;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.ContinuaCI
{
    public sealed class ContinuaCIProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var writer = new FakeBuildSystemServiceMessageWriter();
                var result = Record.Exception(() => new ContinuaCIProvider(null, writer));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Writer_Is_Null()
            {
                // Given, When
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var result = Record.Exception(() => new ContinuaCIProvider(environment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "writer");
            }
        }

        public sealed class TheIsRunningOnContinuaCIProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_ContinuaCI()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                fixture.IsRunningOnContinuaCI();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                var result = continuaCI.IsRunningOnContinuaCI;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_ContinuaCI()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                var result = continuaCI.IsRunningOnContinuaCI;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                var result = continuaCI.Environment;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class TheWriteMessageMethod
        {
            [Fact]
            public void Should_Write_Service_Message_With_Status()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                continuaCI.WriteMessage("Hello", ContinuaCIMessageType.Information);

                // Then
                Assert.Single(fixture.Writer.Entries);
                Assert.Equal("@@continua[message text='Hello' status='information']", fixture.Writer.Entries[0]);
            }

            [Fact]
            public void Should_Sanitize_Message_Text()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                continuaCI.WriteMessage("line1\nline2's", ContinuaCIMessageType.Warning);

                // Then
                Assert.Equal("@@continua[message text='line1\\nline2\\'s' status='warning']", fixture.Writer.Entries[0]);
            }
        }

        public sealed class TheWriteStartGroupMethod
        {
            [Fact]
            public void Should_Write_Start_Group_Message()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                continuaCI.WriteStartGroup("Build");

                // Then
                Assert.Equal("@@continua[startGroup  name='Build']", fixture.Writer.Entries[0]);
            }
        }

        public sealed class TheWriteEndBlockMethod
        {
            [Fact]
            public void Should_Write_End_Group_Message()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                continuaCI.WriteEndBlock("Build");

                // Then
                Assert.Equal("@@continua[endGroup name='Build']", fixture.Writer.Entries[0]);
            }
        }

        public sealed class TheSetVariableMethod
        {
            [Fact]
            public void Should_Write_Set_Parameter_Message()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                continuaCI.SetVariable("Version", "1.2.3", skipIfNotDefined: false);

                // Then
                Assert.Equal("@@continua[setParameter name='Version' value='1.2.3' skipIfNotDefined='False']", fixture.Writer.Entries[0]);
            }

            [Fact]
            public void Should_Sanitize_Variable_Value()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                continuaCI.SetVariable("Name", "foo'bar");

                // Then
                Assert.Equal("@@continua[setParameter name='Name' value='foo\\'bar' skipIfNotDefined='True']", fixture.Writer.Entries[0]);
            }
        }

        public sealed class TheSetBuildVersionMethod
        {
            [Fact]
            public void Should_Write_Set_Build_Version_Message()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                continuaCI.SetBuildVersion("2.0.0");

                // Then
                Assert.Equal("@@continua[setBuildVersion value='2.0.0']", fixture.Writer.Entries[0]);
            }
        }

        public sealed class TheSetBuildStatusMethod
        {
            [Fact]
            public void Should_Write_Set_Build_Status_Message()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                continuaCI.SetBuildStatus("Succeeded");

                // Then
                Assert.Equal("@@continua[setBuildStatus value='Succeeded']", fixture.Writer.Entries[0]);
            }
        }
    }
}
