// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Composition;
using Cake.Frosting.Tests.Data;
using Cake.Frosting.Tests.Fakes;
using NSubstitute;

namespace Cake.Frosting.Tests.Fixtures
{
    internal static class CakeHostBuilderFixtureExtensions
    {
        public static CakeHostBuilderFixture RegisterDefaultTask(this CakeHostBuilderFixture fixture)
        {
            fixture.RegisterTask<DummyTask>();
            fixture.Options.Target = typeof(DummyTask).Name;
            return fixture;
        }

        public static CakeHostBuilderFixture RegisterTask<T>(this CakeHostBuilderFixture fixture)
            where T : IFrostingTask
        {
            fixture.Builder.ConfigureServices(services => services.RegisterType<T>().As<IFrostingTask>());
            return fixture;
        }

        public static FakeLifetime RegisterLifetimeSubstitute(this CakeHostBuilderFixture fixture)
        {
            var lifetime = new FakeLifetime();
            return fixture.RegisterLifetimeSubstitute(lifetime);
        }

        public static T RegisterLifetimeSubstitute<T>(this CakeHostBuilderFixture fixture, T lifetime)
            where T : class, IFrostingLifetime
        {
            fixture.Builder.ConfigureServices(s => s.RegisterInstance(lifetime).As<IFrostingLifetime>());
            return lifetime;
        }

        public static FakeTaskLifetime RegisterTaskLifetimeSubstitute(this CakeHostBuilderFixture fixture)
        {
            var lifetime = new FakeTaskLifetime();
            return fixture.RegisterTaskLifetimeSubstitute(lifetime);
        }

        public static T RegisterTaskLifetimeSubstitute<T>(this CakeHostBuilderFixture fixture, T lifetime)
            where T : class, IFrostingTaskLifetime
        {
            fixture.Builder.ConfigureServices(s => s.RegisterInstance(lifetime).As<IFrostingTaskLifetime>());
            return lifetime;
        }

        public static void UseExecutionStrategySubstitute(this CakeHostBuilderFixture fixture)
        {
            fixture.Strategy = Substitute.For<IExecutionStrategy>();
        }
    }
}
