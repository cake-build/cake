// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;
using Cake.Frosting.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Frosting.Tests.Unit
{
    public sealed class CakeHostBuilderTests
    {
        public sealed class TheConfigureServicesMethod
        {
            [Fact]
            public void Should_Replace_Default_Registrations()
            {
                // Given
                var fixture = new CakeHostBuilderFixture();
                var host = Substitute.For<ICakeHost>();
                fixture.Builder.ConfigureServices(services => services.RegisterInstance(host).As<ICakeHost>());

                // When
                var result = fixture.Build();

                // Then
                Assert.Same(host, result);
            }
        }
    }
}
