// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Autofac;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;

namespace Cake.Composition
{
    internal sealed class ModuleLoader
    {
        private readonly ICakeLog _log;

        public ModuleLoader(ICakeLog log)
        {
            _log = log;
        }

        public void LoadModules(IContainer container, IReadOnlyList<Type> moduleTypes)
        {
            if (moduleTypes.Count > 0)
            {
                using (var temporaryContainer = container.BeginLifetimeScope())
                {
                    // Register modules in the temporary container.
                    RegisterExternalModules(moduleTypes, temporaryContainer);

                    // Now let the modules register their types.
                    var builder = new ContainerRegistrar();
                    foreach (var module in temporaryContainer.Resolve<IEnumerable<ICakeModule>>())
                    {
                        builder.RegisterModule(module);
                    }
                    builder.Update(container);
                }
            }
        }

        private void RegisterExternalModules(IEnumerable<Type> moduleTypes, ILifetimeScope scope)
        {
            var builder = new ContainerBuilder();
            foreach (var moduleType in moduleTypes)
            {
                _log.Debug("Registering module {0}...", moduleType.FullName);
                builder.RegisterType(moduleType).As<ICakeModule>().SingleInstance();
            }
            builder.Update(scope.ComponentRegistry);
        }
    }
}