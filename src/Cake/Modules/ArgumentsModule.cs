using Cake.Core;
using Cake.Core.Composition;

namespace Cake.Modules
{
    internal sealed class ArgumentsModule : ICakeModule
    {
        private readonly CakeOptions _options;

        public ArgumentsModule(CakeOptions options)
        {
            _options = options;
        }

        public void Register(ICakeContainerRegistry registry)
        {
            registry.RegisterInstance(_options).As<CakeOptions>();
            registry.RegisterType<CakeArguments>().As<ICakeArguments>().Singleton();
        }
    }
}
