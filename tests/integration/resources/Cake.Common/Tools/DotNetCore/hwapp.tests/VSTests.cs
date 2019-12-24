using HWApp.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace hwapp.tests
{
    public sealed class GreeterVSTests
    {
        [TestClass]
        public sealed class TheGreaterMethod
        {
            [TestMethod]
            public void Should_Not_Fail_Test()
            {
                Assert.AreNotEqual("true", Environment.GetEnvironmentVariable("hwapp_fail_test"));
            }

            [TestMethod]
            public void Should_Greet_World()
            {
                // Given
                var name = "World";
                var expect = "Hello World!";

                // When
                var result = Greeter.GetGreeting(name);

                // Then
                Assert.AreEqual(expect, result);
            }

            [TestMethod]
            public void Should_Throw_On_Null_Name()
            {
                // Given
                string name = null;

                // When
                try
                {
                    Greeter.GetGreeting(name);
                }
                catch (Exception result)
                {
                    // Then
                    Assert.IsNotNull(result);
                    Assert.IsInstanceOfType(result, typeof(ArgumentNullException));
                    Assert.AreEqual($"Value cannot be null. (Parameter 'name')", result.Message);
                }
            }
        }
    }
}
