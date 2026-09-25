// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using NSubstitute;

namespace Cake.Common.Tests.Unit;

public sealed class ServiceProviderAliasesTests
{
    public sealed class TheServiceProviderProperty
    {
        [Fact]
        public void Should_Throw_If_Context_Is_Null()
        {
            // When
            var result = Record.Exception(() => ServiceProviderAliases.ServiceProvider(null));

            // Then
            AssertEx.IsArgumentNullException(result, "context");
        }

        [Fact]
        public void Should_Return_The_Context_Service_Provider()
        {
            // Given
            var serviceProvider = Substitute.For<IServiceProvider>();
            var context = CreateContext(serviceProvider);

            // When
            var result = ServiceProviderAliases.ServiceProvider(context);

            // Then
            Assert.Same(serviceProvider, result);
        }
    }

    private static ICakeContext CreateContext(IServiceProvider serviceProvider)
    {
        return new CakeContext(
            Substitute.For<IFileSystem>(),
            Substitute.For<ICakeEnvironment>(),
            Substitute.For<IGlobber>(),
            Substitute.For<ICakeLog>(),
            Substitute.For<ICakeArguments>(),
            Substitute.For<IProcessRunner>(),
            Substitute.For<IRegistry>(),
            Substitute.For<IToolLocator>(),
            Substitute.For<ICakeDataService>(),
            Substitute.For<ICakeConfiguration>(),
            Substitute.For<IToolInstaller>(),
            serviceProvider);
    }
}
