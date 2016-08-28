using Cake.Core;
using Cake.Core.Composition;
using Cake.Frosting.Tests.Data;
using Cake.Frosting.Tests.Fixtures;
using NSubstitute;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Tests
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

        public static IFrostingLifetime RegisterLifetimeSubstitute(this CakeHostBuilderFixture fixture)
        {
            var lifetime = Substitute.For<IFrostingLifetime>();
            fixture.Builder.ConfigureServices(s => s.RegisterInstance(lifetime).As<IFrostingLifetime>());
            return lifetime;
        }

        public static IFrostingTaskLifetime RegisterTaskLifetimeSubstitute(this CakeHostBuilderFixture fixture)
        {
            var lifetime = Substitute.For<IFrostingTaskLifetime>();
            fixture.Builder.ConfigureServices(s => s.RegisterInstance(lifetime).As<IFrostingTaskLifetime>());
            return lifetime;
        }

        public static void UseExecutionStrategySubstitute(this CakeHostBuilderFixture fixture)
        {
            fixture.Strategy = Substitute.For<IExecutionStrategy>();
        }
    }
}
