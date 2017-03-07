// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Autofac;
using Cake.Core;
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
        private readonly ICakeEnvironment _environment;

        public ModuleLoader(ModuleSearcher searcher, ICakeLog log, ICakeConfiguration configuration, ICakeEnvironment environment)
        {
            _searcher = searcher;
            _log = log;
            _configuration = configuration;
            _environment = environment;
        }

        public void LoadModules(IContainer container, CakeOptions options)
        {
            var root = GetModulePath(options.Script.GetDirectory());
            var moduleTypes = _searcher.Search(root);
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

        private DirectoryPath GetToolPath(DirectoryPath root)
        {
            var toolPath = _configuration.GetValue(Constants.Paths.Tools);
            if (!string.IsNullOrWhiteSpace(toolPath))
            {
                return new DirectoryPath(toolPath).MakeAbsolute(_environment);
            }

            return root.Combine("tools");
        }

        private DirectoryPath GetModulePath(DirectoryPath root)
        {
            var modulePath = _configuration.GetValue(Constants.Paths.Modules);
            if (!string.IsNullOrWhiteSpace(modulePath))
            {
                return new DirectoryPath(modulePath).MakeAbsolute(_environment);
            }

            var toolPath = GetToolPath(root);
            return toolPath.Combine("Modules").Collapse();
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