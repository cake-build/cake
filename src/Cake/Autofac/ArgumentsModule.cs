using Autofac;
using Cake.Core;

namespace Cake.Autofac
{
    internal sealed class ArgumentsModule : Module
    {
        private readonly CakeOptions _options;

        public ArgumentsModule(CakeOptions options)
        {
            _options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_options).As<CakeOptions>();
            builder.RegisterType<CakeArguments>().As<ICakeArguments>().SingleInstance();
        }
    }
}
