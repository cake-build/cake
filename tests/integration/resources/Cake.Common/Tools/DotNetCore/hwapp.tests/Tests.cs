using System;
using Xunit;
using HWApp.Common;

namespace HWApp.Tests
{
    public sealed class GreeterTests
    {
        public sealed class TheGreaterMethod
        {
            [Fact]
            public void Should_Not_Fail_Test()
            {
                Assert.NotEqual("true", Environment.GetEnvironmentVariable("hwapp_fail_test"));
            }

            [Fact]
            public void Should_Greet_World()
            {
                // Given
                var name = "World";
                var expect = "Hello World!";

                // When
                var result = Greeter.GetGreeting(name);

                // Then
                Assert.Equal(expect, result);
            }

            [Fact]
            public void Should_Throw_On_Null_Name()
            {
                // Given
                string name = null;

                // When
                var result =  Record.Exception(() => Greeter.GetGreeting(name));

                // Then
                Assert.NotNull(result);
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal($"Value cannot be null. (Parameter 'name')", result.Message);
            }
        }
    }
}
