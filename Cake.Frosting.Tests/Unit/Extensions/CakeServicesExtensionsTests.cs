// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core.Composition;
using Cake.Core.Packaging;
using Cake.Frosting.Tests.Data;
using NSubstitute;
using Xunit;

namespace Cake.Frosting.Tests.Unit.Extensions
{
    public sealed class CakeServicesExtensionsTests
    {
        public sealed class TheUseContextExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Services_Reference_Is_Null()
            {
                // Given
                ICakeServices services = null;

                // When
                var result = Record.Exception(() => services.UseContext<FrostingContext>());

                // Then
                AssertEx.IsArgumentNullException(result, "services");
            }

            [Fact]
            public void Should_Register_The_Context()
            {
                // Given
                var services = Substitute.For<ICakeServices>();
                var builder = Substitute.For<ICakeRegistrationBuilder>();
                services.RegisterType(Arg.Any<Type>()).Returns(builder); // Return a builder object when registering
                builder.AsSelf().Returns(builder); // Return same builder object when chaining
                builder.As(Arg.Any<Type>()).Returns(builder); // Return same builder object when chaining

                // When
                services.UseContext<FrostingContext>();

                // Then
                Received.InOrder(() =>
                {
                    services.RegisterType<FrostingContext>();
                    builder.AsSelf();
                    builder.As<IFrostingContext>();
                    builder.Singleton();
                });
            }
        }

        public sealed class TheUseLifetimeExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Services_Reference_Is_Null()
            {
                // Given
                ICakeServices services = null;

                // When
                var result = Record.Exception(() => services.UseLifetime<DummyLifetime>());

                // Then
                AssertEx.IsArgumentNullException(result, "services");
            }

            [Fact]
            public void Should_Register_The_Lifetime()
            {
                // Given
                var services = Substitute.For<ICakeServices>();
                var builder = Substitute.For<ICakeRegistrationBuilder>();
                services.RegisterType(Arg.Any<Type>()).Returns(builder); // Return a builder object when registering
                builder.As(Arg.Any<Type>()).Returns(builder); // Return same builder object when chaining

                // When
                services.UseLifetime<DummyLifetime>();

                // Then
                Received.InOrder(() =>
                {
                    services.RegisterType<DummyLifetime>();
                    builder.As<IFrostingLifetime>();
                    builder.Singleton();
                });
            }
        }

        public sealed class TheUseTaskLifetimeExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Services_Reference_Is_Null()
            {
                // Given
                ICakeServices services = null;

                // When
                var result = Record.Exception(() => services.UseTaskLifetime<DummyTaskLifetime>());

                // Then
                AssertEx.IsArgumentNullException(result, "services");
            }

            [Fact]
            public void Should_Register_The_Lifetime()
            {
                // Given
                var services = Substitute.For<ICakeServices>();
                var builder = Substitute.For<ICakeRegistrationBuilder>();
                services.RegisterType(Arg.Any<Type>()).Returns(builder); // Return a builder object when registering
                builder.As(Arg.Any<Type>()).Returns(builder); // Return same builder object when chaining

                // When
                services.UseTaskLifetime<DummyTaskLifetime>();

                // Then
                Received.InOrder(() =>
                {
                    services.RegisterType<DummyTaskLifetime>();
                    builder.As<IFrostingTaskLifetime>();
                    builder.Singleton();
                });
            }
        }

        public sealed class TheUseAssemblyExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Services_Reference_Is_Null()
            {
                // Given
                ICakeServices services = null;
                var assembly = typeof(DateTime).GetTypeInfo().Assembly;

                // When
                var result = Record.Exception(() => services.UseAssembly(assembly));

                // Then
                AssertEx.IsArgumentNullException(result, "services");
            }

            [Fact]
            public void Should_Register_The_Assembly()
            {
                // Given
                var services = Substitute.For<ICakeServices>();
                var builder = Substitute.For<ICakeRegistrationBuilder>();
                services.RegisterInstance(Arg.Any<Assembly>()).Returns(builder); // Return a builder object when registering
                builder.As(Arg.Any<Type>()).Returns(builder); // Return same builder object when chaining

                // When
                services.UseAssembly(typeof(DateTime).GetTypeInfo().Assembly);

                // Then
                Received.InOrder(() =>
                {
                    services.RegisterInstance(Arg.Any<Assembly>());
                    builder.Singleton();
                });
            }
        }

        public sealed class TheUseModuleExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Services_Reference_Is_Null()
            {
                // Given
                ICakeServices services = null;

                // When
                var result = Record.Exception(() => services.UseModule<DummyModule>());

                // Then
                AssertEx.IsArgumentNullException(result, "services");
            }

            [Fact]
            public void Should_Create_Module_And_Call_Registration()
            {
                // Given
                var services = Substitute.For<ICakeServices>();

                // When
                services.UseModule<DummyModule>();

                // Then
                services.Received(1).RegisterType(typeof(DummyModule.DummyModuleSentinel));
            }
        }

        public sealed class TheUsePackageInstallerExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Services_Reference_Is_Null()
            {
                // Given
                ICakeServices services = null;

                // When
                var result = Record.Exception(() => services.UsePackageInstaller<DummyPackageInstaller>());

                // Then
                AssertEx.IsArgumentNullException(result, "services");
            }

            [Fact]
            public void Should_Register_The_Package_Installer()
            {
                // Given
                var services = Substitute.For<ICakeServices>();
                var builder = Substitute.For<ICakeRegistrationBuilder>();
                services.RegisterType(Arg.Any<Type>()).Returns(builder); // Return a builder object when registering
                builder.As(Arg.Any<Type>()).Returns(builder); // Return same builder object when chaining

                // When
                services.UsePackageInstaller<DummyPackageInstaller>();

                // Then
                Received.InOrder(() =>
                {
                    services.RegisterType<DummyPackageInstaller>();
                    builder.As<IPackageInstaller>();
                    builder.Singleton();
                });
            }
        }

        public sealed class TheUseToolExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Services_Reference_Is_Null()
            {
                // Given
                ICakeServices services = null;

                // When
                var result = Record.Exception(() => services.UseTool(new Uri("nuget:?package=Foo")));

                // Then
                AssertEx.IsArgumentNullException(result, "services");
            }

            [Fact]
            public void Should_Register_The_PackageReference()
            {
                // Given
                var services = Substitute.For<ICakeServices>();
                var builder = Substitute.For<ICakeRegistrationBuilder>();
                services.RegisterInstance(Arg.Any<PackageReference>()).Returns(builder); // Return a builder object when registering
                builder.As(Arg.Any<Type>()).Returns(builder); // Return same builder object when chaining

                // When
                services.UseTool(new Uri("nuget:?package=Foo"));

                // Then
                Received.InOrder(() =>
                {
                    services.RegisterInstance(Arg.Is<PackageReference>(
                        r => r.OriginalString == "nuget:?package=Foo"));
                });
            }
        }
    }
}
