// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Processors.Loading;
using Cake.NuGet.Tests.Fixtures;
using Cake.Testing.Xunit;
using NSubstitute;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetModuleTests
    {
        public sealed class TheRegisterMethod
        {
            [Fact]
            public void Should_Register_The_NuGet_Content_Resolver()
            {
                // Given
                var fixture = new NuGetModuleFixture<NuGetContentResolver>();
                var module = fixture.CreateModule();

                // When
                module.Register(fixture.Registrar);

                // Then
                fixture.Registrar.Received(1).RegisterType<NuGetContentResolver>();
                fixture.Builder.Received(1).As<INuGetContentResolver>();
                fixture.Builder.Received(1).Singleton();
            }

            public void Should_Register_The_NuGet_Load_Directive_Provider()
            {
                // Given
                var fixture = new NuGetModuleFixture<NuGetLoadDirectiveProvider>();
                var module = fixture.CreateModule();

                // When
                module.Register(fixture.Registrar);

                // Then
                fixture.Registrar.Received(1).RegisterType<NuGetLoadDirectiveProvider>();
                fixture.Builder.Received(1).As<ILoadDirectiveProvider>();
                fixture.Builder.Received(1).Singleton();
            }

            [Fact]
            public void Should_Register_The_NuGet_Package_Installer()
            {
                // Given
                var fixture = new NuGetModuleFixture<NuGetPackageInstaller>();
                var module = fixture.CreateModule();

                // When
                module.Register(fixture.Registrar);

                // Then
                fixture.Registrar.Received(1).RegisterType<NuGetPackageInstaller>();
                fixture.Builder.Received(1).As<INuGetPackageInstaller>();
                fixture.Builder.Received(1).As<IPackageInstaller>();
                fixture.Builder.Received(1).Singleton();
            }
        }
    }
}