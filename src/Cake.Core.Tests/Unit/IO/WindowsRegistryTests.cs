// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit.IO;

public sealed class WindowsRegistryTests
{
    public sealed class TheCurrentUserProperty
    {
        [Fact]
        public void Should_Not_Throw_When_Accessed()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var result = Record.Exception(() => registry.CurrentUser);

            // Then
            Assert.Null(result);
        }

        [Fact]
        public void Should_Return_A_Registry_Key()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var key = registry.CurrentUser;

            // Then
            Assert.NotNull(key);
        }
    }

    public sealed class TheLocalMachineProperty
    {
        [Fact]
        public void Should_Not_Throw_When_Accessed()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var result = Record.Exception(() => registry.LocalMachine);

            // Then
            Assert.Null(result);
        }

        [Fact]
        public void Should_Return_A_Registry_Key()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var key = registry.LocalMachine;

            // Then
            Assert.NotNull(key);
        }
    }

    public sealed class TheClassesRootProperty
    {
        [Fact]
        public void Should_Not_Throw_When_Accessed()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var result = Record.Exception(() => registry.ClassesRoot);

            // Then
            Assert.Null(result);
        }

        [Fact]
        public void Should_Return_A_Registry_Key()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var key = registry.ClassesRoot;

            // Then
            Assert.NotNull(key);
        }
    }

    public sealed class TheUsersProperty
    {
        [Fact]
        public void Should_Not_Throw_When_Accessed()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var result = Record.Exception(() => registry.Users);

            // Then
            Assert.Null(result);
        }

        [Fact]
        public void Should_Return_A_Registry_Key()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var key = registry.Users;

            // Then
            Assert.NotNull(key);
        }
    }

    public sealed class ThePerformanceDataProperty
    {
        [Fact]
        public void Should_Not_Throw_When_Accessed()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var result = Record.Exception(() => registry.PerformanceData);

            // Then
            Assert.Null(result);
        }

        [Fact]
        public void Should_Return_A_Registry_Key()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var key = registry.PerformanceData;

            // Then
            Assert.NotNull(key);
        }
    }

    public sealed class TheCurrentConfigProperty
    {
        [Fact]
        public void Should_Not_Throw_When_Accessed()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var result = Record.Exception(() => registry.CurrentConfig);

            // Then
            Assert.Null(result);
        }

        [Fact]
        public void Should_Return_A_Registry_Key()
        {
            // Given
            var registry = new WindowsRegistry();

            // When
            var key = registry.CurrentConfig;

            // Then
            Assert.NotNull(key);
        }
    }

    public sealed class TheReflectiveWalkOfTheRegistry
    {
        [Fact]
        public void Should_Not_Throw_When_All_Registry_Properties_Are_Accessed_Via_Reflection()
        {
            // Given
            var registry = new WindowsRegistry();
            var properties = typeof(IRegistry).GetProperties();

            // When
            foreach (var property in properties)
            {
                object key = default;
                var exception = Record.Exception(() => key = property.GetValue(registry));

                // Then
                Assert.Null(exception);
                Assert.NotNull(key);
            }
        }
    }

    public sealed class DeferredKeyCreation
    {
        [Fact]
        public void Should_Not_Create_The_Underlying_Key_When_Disposed()
        {
            // Given
            var count = 0;
            var key = new WindowsRegistryKey(() =>
            {
                count++;
                return null;
            });

            // When
            var exception = Record.Exception(() => key.Dispose());

            // Then
            Assert.Null(exception);
            Assert.Equal(0, count);
        }

        [Fact]
        public void Should_Create_The_Underlying_Key_Once_When_Used()
        {
            // Given
            var count = 0;
            var key = new WindowsRegistryKey(() =>
            {
                count++;
                return null;
            });

            // When
            var exception = Record.Exception(() => key.GetSubKeyNames());
            var exception2 = Record.Exception(() => key.GetSubKeyNames());

            // Then
            Assert.NotNull(exception);
            Assert.NotNull(exception2);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Should_Propagate_Exception_From_The_Underlying_Key_Creation()
        {
            // Given
            var key = new WindowsRegistryKey(() => throw new PlatformNotSupportedException());

            // When
            var result = Record.Exception(() => key.GetSubKeyNames());

            // Then
            Assert.IsType<PlatformNotSupportedException>(result);
        }

        [Fact]
        public void Should_Create_The_Underlying_Key_Once_When_OpenKey_Is_Used()
        {
            // Given
            var count = 0;
            var key = new WindowsRegistryKey(() =>
            {
                count++;
                return null;
            });

            // When
            var exception = Record.Exception(() => key.OpenKey("Cake"));
            var exception2 = Record.Exception(() => key.OpenKey("Cake"));

            // Then
            Assert.NotNull(exception);
            Assert.NotNull(exception2);
            Assert.Equal(1, count);
        }

        [Fact]
        public void Should_Create_The_Underlying_Key_Once_When_GetValue_Is_Used()
        {
            // Given
            var count = 0;
            var key = new WindowsRegistryKey(() =>
            {
                count++;
                return null;
            });

            // When
            var exception = Record.Exception(() => key.GetValue("Cake"));
            var exception2 = Record.Exception(() => key.GetValue("Cake"));

            // Then
            Assert.NotNull(exception);
            Assert.NotNull(exception2);
            Assert.Equal(1, count);
        }

        [NonWindowsFact]
        public void Should_Throw_Platform_Not_Supported_Exception_When_Used_On_Non_Windows_Platform()
        {
            // Given
            var registry = new WindowsRegistry();
            var key = registry.CurrentUser;

            // When
            var result = Record.Exception(() => key.GetSubKeyNames());

            // Then
            Assert.IsType<PlatformNotSupportedException>(result);
        }

        [NonWindowsFact]
        public void Should_Throw_Platform_Not_Supported_Exception_When_OpenKey_Is_Used_On_Non_Windows_Platform()
        {
            // Given
            var registry = new WindowsRegistry();
            var key = registry.CurrentUser;

            // When
            var result = Record.Exception(() => key.OpenKey("Cake"));

            // Then
            Assert.IsType<PlatformNotSupportedException>(result);
        }

        [NonWindowsFact]
        public void Should_Throw_Platform_Not_Supported_Exception_When_GetValue_Is_Used_On_Non_Windows_Platform()
        {
            // Given
            var registry = new WindowsRegistry();
            var key = registry.CurrentUser;

            // When
            var result = Record.Exception(() => key.GetValue("Cake"));

            // Then
            Assert.IsType<PlatformNotSupportedException>(result);
        }

        [WindowsFact]
        public void Should_Return_Sub_Key_Names_When_Used_On_Windows_Platform()
        {
            // Given
            var registry = new WindowsRegistry();
            var key = registry.CurrentUser;

            // When
            var result = key.GetSubKeyNames();

            // Then
            Assert.NotNull(result);
        }
    }
}
