// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Composition
{
    /// <summary>
    /// Loads module types by applying them to a container and script configuration.
    /// </summary>
    public sealed class ModuleLoader
    {
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleLoader"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public ModuleLoader(ICakeLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Loads module types by applying them to a container and script configuration.
        /// </summary>
        /// <param name="moduleTypes">A list of types implementing <see cref="ICakeModule"/>.</param>
        /// <param name="container">Access to modify the registrations in the container.</param>
        /// <param name="scriptConfiguration">Access to modify the script configuration.</param>
        public void LoadModules(IReadOnlyList<Type> moduleTypes, IContainer container, ScriptConfiguration scriptConfiguration)
        {
            if (moduleTypes.Count > 0)
            {
                foreach (var moduleAssembly in moduleTypes.Select(_ => _.GetTypeInfo().Assembly).Distinct())
                {
                    foreach (var namespaceImport in moduleAssembly.GetCustomAttributes<CakeNamespaceImportAttribute>())
                    {
                        scriptConfiguration.AddScriptNamespace(namespaceImport.Namespace);
                    }

                    foreach (var assemblyReference in moduleAssembly.GetCustomAttributes<CakeAssemblyReferenceAttribute>())
                    {
                        scriptConfiguration.AddScriptAssembly(assemblyReference.AssemblyName);
                    }
                }

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