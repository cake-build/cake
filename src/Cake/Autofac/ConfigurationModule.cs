using System.Diagnostics;
using Autofac;
using Cake.Core.Configuration;

namespace Cake.Autofac
{
    internal sealed class ConfigurationModule : Module
    {
        private readonly CakeConfigurationProvider _provider;
        private readonly CakeOptions _options;

        public ConfigurationModule(IContainer container, CakeOptions options)
        {
            _provider = container.Resolve<CakeConfigurationProvider>();
            _options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var configuration = _provider.CreateConfiguration(_options.Arguments);
            Debug.Assert(configuration != null, "Configuration should not be null.");
            builder.RegisterInstance(configuration).As<ICakeConfiguration>();
        }
    }
}
