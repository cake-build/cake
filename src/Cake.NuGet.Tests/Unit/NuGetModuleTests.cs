// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;
using Cake.Core.Packaging;
using Cake.NuGet.Tests.Fixtures;
#if NETCORE
using Cake.NuGet.V3;
#else
using Cake.Core.Scripting.Processors.Loading;
using Cake.NuGet.V2;
#endif
using NSubstitute;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetModuleTests
    {
        public sealed class TheRegisterMethod
        {
#if NETCORE
            [Fact]
            public void Should_Register_The_NuGet_Content_Resolver()
            {
                // Given
                var fixture = new NuGetModuleFixture<NuGetV3ContentResolver>();
                var module = new NuGetModule();

                // When
                module.Register(fixture.Registrar);

                // Then
                fixture.Registrar.Received(1).RegisterType<NuGetV3ContentResolver>();
                fixture.Builder.Received(1).As<INuGetContentResolver>();
                fixture.Builder.Received(1).Singleton();
            }
#else
            [Fact]
            public void Should_Register_The_NuGet_Content_Resolver()
            {
                // Given
                var fixture = new NuGetModuleFixture<NuGetV2ContentResolver>();
                var module = new NuGetModule();

                // When
                module.Register(fixture.Registrar);

                // Then
                fixture.Registrar.Received(1).RegisterType<NuGetV2ContentResolver>();
                fixture.Builder.Received(1).As<INuGetContentResolver>();
                fixture.Builder.Received(1).Singleton();
            }

            [Fact]
            public void Should_Register_The_NuGet_Load_Directive_Provider()
            {
                // Given
                var fixture = new NuGetModuleFixture<NuGetLoadDirectiveProvider>();
                var module = new NuGetModule();

                // When
                module.Register(fixture.Registrar);

                // Then
                fixture.Registrar.Received(1).RegisterType<NuGetLoadDirectiveProvider>();
                fixture.Builder.Received(1).As<ILoadDirectiveProvider>();
                fixture.Builder.Received(1).Singleton();
            }
#endif

            [Fact]
            public void Shouls_Register_The_NuGet_Package_Installer()
            {
                // Given
                var fixture = new NuGetModuleFixture<NuGetPackageInstaller>();
                var module = new NuGetModule();

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