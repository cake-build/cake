// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
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
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            registry.RegisterInstance(_options).As<CakeOptions>();
            registry.RegisterType<CakeArguments>().As<ICakeArguments>().Singleton();
        }
    }
}
