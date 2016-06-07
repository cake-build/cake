using System.Diagnostics;
using Autofac;
using Cake.Core.Composition;
using Cake.Core.Configuration;

namespace Cake.Modules
{
    internal sealed class ConfigurationModule : ICakeModule
    {
        private readonly CakeConfigurationProvider _provider;
        private readonly CakeOptions _options;

        public ConfigurationModule(IContainer container, CakeOptions options)
        {
            _provider = container.Resolve<CakeConfigurationProvider>();
            _options = options;
        }

        public void Register(ICakeContainerRegistry registry)
        {
            var configuration = _provider.CreateConfiguration(_options.Arguments);
            Debug.Assert(configuration != null, "Configuration should not be null.");
            registry.RegisterInstance(configuration).As<ICakeConfiguration>();
        }
    }
}
