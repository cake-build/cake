// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting.Internal;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Frosting.Tests.Unit.Internal
{
    public sealed class FrostingConfigurationTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Values_Is_Null()
            {
                // Given
                var fileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                // When
                var result = Record.Exception(() => new FrostingConfiguration(null, fileSystem, environment, arguments));

                // Then
                AssertEx.IsArgumentNullException(result, "values");
            }

            [Fact]
            public void Should_Throw_If_FileSystem_Is_Null()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                // When
                var result = Record.Exception(() => new FrostingConfiguration(values, null, environment, arguments));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var fileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                // When
                var result = Record.Exception(() => new FrostingConfiguration(values, fileSystem, null, arguments));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Arguments_Is_Null()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var fileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());
                var environment = FakeEnvironment.CreateUnixEnvironment();

                // When
                var result = Record.Exception(() => new FrostingConfiguration(values, fileSystem, environment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "arguments");
            }

            [Fact]
            public void Should_Create_Configuration_With_Empty_Values()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                // When
                var result = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Initialize_With_Provided_FrostingConfigurationValue_Values()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("TestKey1", "TestValue1"),
                    new FrostingConfigurationValue("TestKey2", "TestValue2")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                // When
                var result = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // Then
                Assert.Equal("TestValue1", result.GetValue("TestKey1"));
                Assert.Equal("TestValue2", result.GetValue("TestKey2"));
            }
        }

        public sealed class TheGetValueMethod
        {
            [Fact]
            public void Should_Return_Value_From_FrostingConfigurationValue()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("MyKey", "MyValue")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("MyKey");

                // Then
                Assert.Equal("MyValue", result);
            }

            [Fact]
            public void Should_Return_Value_Regardless_Of_Casing()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("MyKey", "MyValue")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("mykey");

                // Then
                Assert.Equal("MyValue", result);
            }

            [Fact]
            public void Should_Return_Null_For_Non_Existent_Key()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("NonExistentKey");

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Return_Value_From_Environment_Variables()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.SetEnvironmentVariable("CAKE_FOO", "BarFromEnv");
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("FOO");

                // Then
                Assert.Equal("BarFromEnv", result);
            }

            [Fact]
            public void Should_Return_Value_From_Configuration_File()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent("ConfigKey=ConfigValue");
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("ConfigKey");

                // Then
                Assert.Equal("ConfigValue", result);
            }

            [Fact]
            public void Should_Return_Value_From_Arguments()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                var argumentDict = new Dictionary<string, ICollection<string>>
                {
                    { "ArgKey", new List<string> { "ArgValue" } }
                };
                arguments.GetArguments().Returns(argumentDict);

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("ArgKey");

                // Then
                Assert.Equal("ArgValue", result);
            }

            [Fact]
            public void Should_Handle_Empty_String_Argument_Value()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                var argumentDict = new Dictionary<string, ICollection<string>>
                {
                    { "EmptyKey", new List<string>() }
                };
                arguments.GetArguments().Returns(argumentDict);

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("EmptyKey");

                // Then
                Assert.Equal(string.Empty, result);
            }

            [Fact]
            public void Should_Handle_Null_Argument_Value()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                var argumentDict = new Dictionary<string, ICollection<string>>
                {
                    { "NullKey", new List<string> { null } }
                };
                arguments.GetArguments().Returns(argumentDict);

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("NullKey");

                // Then
                Assert.Equal(string.Empty, result);
            }

            [Fact]
            public void Should_Use_FrostingConfigurationValue_When_No_Other_Source_Provides_Value()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("OnlyInBase", "BaseValue")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("OnlyInBase");

                // Then
                Assert.Equal("BaseValue", result);
            }

            [Fact]
            public void Should_Override_FrostingConfigurationValue_With_Environment_Variable()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("OverrideKey", "BaseValue")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.SetEnvironmentVariable("CAKE_OverrideKey", "EnvValue");
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("OverrideKey");

                // Then
                Assert.Equal("EnvValue", result);
            }

            [Fact]
            public void Should_Override_FrostingConfigurationValue_And_Environment_Variable_With_Config_File()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("OverrideKey", "BaseValue")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.SetEnvironmentVariable("CAKE_OverrideKey", "EnvValue");
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent("OverrideKey=ConfigValue");
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("OverrideKey");

                // Then
                Assert.Equal("ConfigValue", result);
            }

            [Fact]
            public void Should_Override_All_Other_Sources_With_Argument()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("OverrideKey", "BaseValue")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.SetEnvironmentVariable("CAKE_OverrideKey", "EnvValue");
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent("OverrideKey=ConfigValue");
                var arguments = Substitute.For<ICakeArguments>();
                var argumentDict = new Dictionary<string, ICollection<string>>
                {
                    { "OverrideKey", new List<string> { "ArgValue" } }
                };
                arguments.GetArguments().Returns(argumentDict);

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("OverrideKey");

                // Then
                Assert.Equal("ArgValue", result);
            }

            [Fact]
            public void Should_Handle_Multiple_FrostingConfigurationValue_Values()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("Key1", "Value1"),
                    new FrostingConfigurationValue("Key2", "Value2"),
                    new FrostingConfigurationValue("Key3", "Value3")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When & Then
                Assert.Equal("Value1", configuration.GetValue("Key1"));
                Assert.Equal("Value2", configuration.GetValue("Key2"));
                Assert.Equal("Value3", configuration.GetValue("Key3"));
            }

            [Fact]
            public void Should_Use_Last_Value_When_FrostingConfigurationValue_Has_Duplicate_Keys()
            {
                // Given
                var values = new List<FrostingConfigurationValue>
                {
                    new FrostingConfigurationValue("DuplicateKey", "FirstValue"),
                    new FrostingConfigurationValue("DuplicateKey", "SecondValue")
                };
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("DuplicateKey");

                // Then
                Assert.Equal("SecondValue", result);
            }

            [Fact]
            public void Should_Handle_Configuration_With_Sections_From_File()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/cake.config").SetContent("[Section]\nKey=Value");
                var arguments = Substitute.For<ICakeArguments>();
                arguments.GetArguments().Returns(new Dictionary<string, ICollection<string>>());

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("Section_Key");

                // Then
                Assert.Equal("Value", result);
            }

            [Fact]
            public void Should_Take_First_Argument_Value_When_Multiple_Values_Provided()
            {
                // Given
                var values = new List<FrostingConfigurationValue>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var arguments = Substitute.For<ICakeArguments>();
                var argumentDict = new Dictionary<string, ICollection<string>>
                {
                    { "MultiKey", new List<string> { "FirstValue", "SecondValue", "ThirdValue" } }
                };
                arguments.GetArguments().Returns(argumentDict);

                var configuration = new FrostingConfiguration(values, fileSystem, environment, arguments);

                // When
                var result = configuration.GetValue("MultiKey");

                // Then
                Assert.Equal("FirstValue", result);
            }
        }
    }
}
