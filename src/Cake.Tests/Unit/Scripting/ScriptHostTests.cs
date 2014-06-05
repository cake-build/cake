using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class ScriptHostTests
    {
        public sealed class TheFileSystemProperty
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                var result = host.FileSystem;

                // Then
                Assert.Equal(fixture.Engine.FileSystem, result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                var result = host.Environment;

                // Then
                Assert.Equal(fixture.Engine.Environment, result);
            }
        }

        public sealed class TheLogProperty
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                var result = host.Log;

                // Then
                Assert.Equal(fixture.Engine.Log, result);
            }
        }

        public sealed class TheGlobberProperty
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                var result = host.Globber;

                // Then
                Assert.Equal(fixture.Engine.Globber, result);
            }
        }

        public sealed class TheArgumentsProperty
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                var result = host.Arguments;

                // Then
                Assert.Equal(fixture.Engine.Arguments, result);
            }    
        }

        public sealed class TheTaskMethod
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                host.Task("Task");

                // Then
                fixture.Engine.Received(1).Task("Task");
            }            
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                host.Run("Target");

                // Then
                fixture.Engine.Received(1).Run("Target");
            }
        }
    }
}
