// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Autofac;
using Cake.Composition;
using Cake.Core;
using Cake.Core.Composition;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Composition
{
    public sealed class ContainerBuilderAdapterTests
    {
        public interface IFoo
        {
        }

        public sealed class Foo : IFoo
        {
        }

        public sealed class Bar : IFoo
        {
        }

        public sealed class TheRegisterInstanceMethod
        {
            [Fact]
            public void Should_Resolve_Last_Registration_Of_Instance_When_Requesting_Single_Registration()
            {
                // Given
                var builder = new ContainerRegistrar();
                var instance1 = Substitute.For<IFoo>();
                var instance2 = Substitute.For<IFoo>();
                builder.RegisterInstance(instance1).Singleton();
                builder.RegisterInstance(instance2).Singleton();
                var container = builder.Build();

                // When
                var result = container.Resolve<IFoo>();

                // Then
                Assert.Same(instance2, result);
            }

            [Fact]
            public void Should_Resolve_All_Instances_When_Requesting_Enumerable_Of_Registration()
            {
                // Given
                var builder = new ContainerRegistrar();
                var instance1 = Substitute.For<IFoo>();
                var instance2 = Substitute.For<IFoo>();
                builder.RegisterInstance(instance1).Singleton();
                builder.RegisterInstance(instance2).Singleton();
                var container = builder.Build();

                // When
                var result = container.Resolve<IEnumerable<IFoo>>().ToList();

                // Then
                Assert.Equal(2, result.Count);
                Assert.Contains(result, instance => instance == instance1);
                Assert.Contains(result, instance => instance == instance2);
            }

            [Fact]
            public void Should_Resolve_The_Same_Instances_When_Resolving_Registered_Instance()
            {
                // Given
                var builder = new ContainerRegistrar();
                var instance = Substitute.For<IFoo>();
                builder.RegisterInstance(instance).Singleton();
                var container = builder.Build();

                // When
                var first = container.Resolve<IFoo>();
                var second = container.Resolve<IFoo>();

                // Then
                Assert.Same(instance, first);
                Assert.Same(instance, second);
            }
        }

        public sealed class TheRegisterTypeMethod
        {
            [Fact]
            public void Should_Resolve_Last_Registration_When_Requesting_Single_Registration()
            {
                // Given
                var builder = new ContainerRegistrar();
                builder.RegisterType<Foo>().As<IFoo>();
                builder.RegisterType<Bar>().As<IFoo>();
                var container = builder.Build();

                // When
                var result = container.Resolve<IFoo>();

                // Then
                Assert.IsType<Bar>(result);
            }

            [Fact]
            public void Should_Resolve_All_Registrations_When_Requesting_Enumerable_Of_Registration()
            {
                // Given
                var builder = new ContainerRegistrar();
                builder.RegisterType<Foo>().As<IFoo>();
                builder.RegisterType<Bar>().As<IFoo>();
                var container = builder.Build();

                // When
                var result = container.Resolve<IEnumerable<IFoo>>().ToList();

                // Then
                Assert.Equal(2, result.Count);
                Assert.Contains(result, instance => instance is Foo);
                Assert.Contains(result, instance => instance is Bar);
            }

            [Fact]
            public void Should_Resolve_The_Same_Instance_When_Resolving_Singleton_Twice()
            {
                // Given
                var builder = new ContainerRegistrar();
                builder.RegisterType<Foo>().Singleton();
                var container = builder.Build();

                // When
                var first = container.Resolve<Foo>();
                var second = container.Resolve<Foo>();

                // Then
                Assert.Same(first, second);
            }

            [Fact]
            public void Should_Resolve_Different_Instances_When_Resolving_Type_Registered_As_Transient_Twice()
            {
                // Given
                var builder = new ContainerRegistrar();
                builder.RegisterType<Foo>().Transient();
                var container = builder.Build();

                // When
                var first = container.Resolve<Foo>();
                var second = container.Resolve<Foo>();

                // Then
                Assert.NotSame(first, second);
            }
        }
    }
}