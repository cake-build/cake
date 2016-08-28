using Cake.Core.Composition;

namespace Cake.Frosting.Tests.Data
{
    public class DummyModule : ICakeModule
    {
        public sealed class DummyModuleSentinel
        {
        }

        public void Register(ICakeContainerRegistrar registrar)
        {
            registrar.RegisterType<DummyModuleSentinel>();
        }
    }
}
