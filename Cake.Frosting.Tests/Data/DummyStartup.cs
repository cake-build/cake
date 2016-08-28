using Cake.Core.Composition;

namespace Cake.Frosting.Tests.Data
{
    public sealed class DummyStartup : IFrostingStartup
    {
        public sealed class DummyStartupSentinel
        {
        }

        public void Configure(ICakeServices services)
        {
            services.RegisterType<DummyStartupSentinel>();
        }
    }
}
