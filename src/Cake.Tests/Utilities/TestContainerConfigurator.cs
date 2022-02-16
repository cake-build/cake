using System;
using System.Collections.Generic;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Infrastructure;
using Spectre.Console.Cli;

namespace Cake.Tests.Fakes
{
    public sealed class TestContainerConfigurator : IContainerConfigurator
    {
        private readonly List<Action<ICakeContainerRegistrar>> _actions;
        private readonly ContainerConfigurator _decorated;

        public TestContainerConfigurator()
        {
            _actions = new List<Action<ICakeContainerRegistrar>>();
            _decorated = new ContainerConfigurator();
        }

        public void Configure(
            ICakeContainerRegistrar registrar,
            ICakeConfiguration configuration,
            IRemainingArguments arguments)
        {
            _decorated.Configure(registrar, configuration, arguments);

            foreach (var action in _actions)
            {
                action(registrar);
            }
        }

        public void RegisterOverrides(Action<ICakeContainerRegistrar> registration)
        {
            _actions.Add(registration);
        }
    }
}
