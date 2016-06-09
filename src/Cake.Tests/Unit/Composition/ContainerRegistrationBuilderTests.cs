// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Linq;
using Cake.Composition;
using Cake.Core.Diagnostics;
using Cake.Diagnostics;
using Xunit;

namespace Cake.Tests.Unit.Composition
{
    public sealed class ContainerRegistrationBuilderTests
    {
        public sealed class TheAsMethod
        {
            [Fact]
            public void Should_Add_Registration_Type()
            {
                // Given
                var registration = new ContainerRegistration(typeof(CakeBuildLog));
                var builder = new ContainerRegistrationBuilder<CakeBuildLog>(registration);

                // When
                builder.As<ICakeLog>();

                // Then
                Assert.Equal(1, registration.RegistrationTypes.Count);
                Assert.Equal(typeof(ICakeLog), registration.RegistrationTypes.First());
            }
        }

        public sealed class TheAsSelfMethod
        {
            [Fact]
            public void Should_Add_Registration_Type()
            {
                // Given
                var registration = new ContainerRegistration(typeof(CakeBuildLog));
                var builder = new ContainerRegistrationBuilder<CakeBuildLog>(registration);

                // When
                builder.AsSelf();

                // Then
                Assert.Equal(1, registration.RegistrationTypes.Count);
                Assert.Equal(typeof(CakeBuildLog), registration.RegistrationTypes.First());
            }
        }

        public sealed class TheSingletonMethod
        {
            [Fact]
            public void Should_Set_Lifetime()
            {
                // Given
                var registration = new ContainerRegistration(typeof(CakeBuildLog));
                var builder = new ContainerRegistrationBuilder<CakeBuildLog>(registration);

                // When
                builder.Singleton();

                // Then
                Assert.True(registration.IsSingleton);
            }
        }

        public sealed class TheTransientMethod
        {
            [Fact]
            public void Should_Set_Lifetime()
            {
                // Given
                var registration = new ContainerRegistration(typeof(CakeBuildLog));
                var builder = new ContainerRegistrationBuilder<CakeBuildLog>(registration);

                // When
                builder.Transient();

                // Then
                Assert.False(registration.IsSingleton);
            }
        }
    }
}
