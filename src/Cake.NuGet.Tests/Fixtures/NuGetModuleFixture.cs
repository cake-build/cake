// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Testing;
using NSubstitute;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class NuGetModuleFixture<T>
    {
        public ICakeContainerRegistrar Registrar { get; }
        public ICakeRegistrationBuilder Builder { get; }
        public FakeConfiguration Configuration { get; }

        public NuGetModuleFixture()
        {
            Registrar = Substitute.For<ICakeContainerRegistrar>();
            Builder = Substitute.For<ICakeRegistrationBuilder>();
            Configuration = new FakeConfiguration();

            Registrar.RegisterType<T>().Returns(Builder);
            Builder.As(Arg.Any<Type>()).Returns(Builder);
            Builder.Singleton().Returns(Builder);
            Builder.Transient().Returns(Builder);
            Builder.AsSelf().Returns(Builder);
        }

        public NuGetModule CreateModule()
        {
            return new NuGetModule(Configuration);
        }
    }
}