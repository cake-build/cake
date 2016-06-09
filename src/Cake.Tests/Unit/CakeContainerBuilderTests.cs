// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeContainerBuilderTests
    {
        public sealed class TheBuildMethod
        {
            [Fact]
            public void Should_Resolve_Last_Registration_Of_Type_When_Requesting_Single_Registration()
            {
                // Given
                var builder = new CakeContainerBuilder();
                builder.Registry.RegisterType<CakeConsole>().As<IConsole>().Singleton();
                builder.Registry.RegisterType<FakeConsole>().As<IConsole>().Singleton();
                var container = builder.Build();

                // When
                var result = container.Resolve<IConsole>();

                // Then
                Assert.IsType<FakeConsole>(result);
            }

            [Fact]
            public void Should_Resolve_Last_Registration_Of_Type_When_Requesting_Enumerable_Of_Registration()
            {
                // Given
                var builder = new CakeContainerBuilder();
                builder.Registry.RegisterType<CakeConsole>().As<IConsole>().Singleton();
                builder.Registry.RegisterType<FakeConsole>().As<IConsole>().Singleton();
                var container = builder.Build();

                // When
                var result = container.Resolve<IEnumerable<IConsole>>().ToList();

                // Then
                Assert.Equal(2, result.Count);
                Assert.True(result.Any(instance => instance is CakeConsole));
                Assert.True(result.Any(instance => instance is FakeConsole));
            }

            [Fact]
            public void Should_Resolve_The_Same_Instance_When_Resolving_Type_Registered_As_Singleton_Twice()
            {
                // Given
                var builder = new CakeContainerBuilder();
                builder.Registry.RegisterType<CakeConsole>().Singleton();
                var container = builder.Build();

                // When
                var first = container.Resolve<CakeConsole>();
                var second = container.Resolve<CakeConsole>();

                // Then
                Assert.Same(first, second);
            }

            [Fact]
            public void Should_Resolve_Different_Instances_When_Resolving_Type_Registered_As_Transient_Twice()
            {
                // Given
                var builder = new CakeContainerBuilder();
                builder.Registry.RegisterType<CakeConsole>().Transient();
                var container = builder.Build();

                // When
                var first = container.Resolve<CakeConsole>();
                var second = container.Resolve<CakeConsole>();

                // Then
                Assert.NotSame(first, second);
            }
        }
    }
}
