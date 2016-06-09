// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Autofac;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Composition
{
    internal sealed class ModuleLoader
    {
        private readonly ModuleSearcher _searcher;
        private readonly ICakeLog _log;
        private readonly ICakeConfiguration _configuration;

        public ModuleLoader(ModuleSearcher searcher, ICakeLog log, ICakeConfiguration configuration)
        {
            _searcher = searcher;
            _log = log;
            _configuration = configuration;
        }

        public void LoadModules(IContainer container, CakeOptions options)
        {
            var root = GetModulePath(options);
            var moduleTypes = _searcher.Search(root);
            if (moduleTypes.Count > 0)
            {
                using (var temporaryContainer = container.BeginLifetimeScope())
                {
                    // Register modules in the temporary container.
                    RegisterExternalModules(moduleTypes, temporaryContainer);

                    // Now let the modules register their types.
                    var builder = new CakeContainerBuilder();
                    foreach (var module in temporaryContainer.Resolve<IEnumerable<ICakeModule>>())
                    {
                        builder.Registry.RegisterModule(module);
                    }
                    builder.Update(container);
                }
            }
        }

        private DirectoryPath GetModulePath(CakeOptions options)
        {
            var root = _configuration.GetValue(Constants.Paths.Modules);
            if (string.IsNullOrWhiteSpace(root))
            {
                return options.Script.GetDirectory().Combine("tools/modules");
            }
            return new DirectoryPath(root);
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
