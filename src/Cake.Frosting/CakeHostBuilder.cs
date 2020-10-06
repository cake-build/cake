// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Composition;
using Cake.Core.Modules;
using Cake.Frosting.Internal;
using Cake.Frosting.Internal.Composition;

namespace Cake.Frosting
{
    /// <summary>
    /// A builder for <see cref="ICakeHost"/>.
    /// </summary>
    /// <seealso cref="Cake.Frosting.ICakeHostBuilder" />
    public sealed class CakeHostBuilder : ICakeHostBuilder
    {
        private readonly CakeServices _registrations;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeHostBuilder"/> class.
        /// </summary>
        public CakeHostBuilder()
        {
            _registrations = new CakeServices();
        }

        /// <summary>
        /// Adds a delegate for configuring additional services for the host.
        /// </summary>
        /// <param name="configureServices">A delegate for configuring the <see cref="ICakeServices"/>.</param>
        /// <returns>The same <see cref="ICakeHostBuilder"/> instance so that multiple calls can be chained.</returns>
        public ICakeHostBuilder ConfigureServices(Action<ICakeServices> configureServices)
        {
            try
            {
                configureServices(_registrations);
                return this;
            }
            catch (Exception exception)
            {
                return new ErrorCakeHostBuilder(exception);
            }
        }

        /// <summary>
        /// Builds the required services and an <see cref="ICakeHost" /> using the specified options.
        /// </summary>
        /// <returns>The built <see cref="ICakeHost" />.</returns>
        public ICakeHost Build()
        {
            try
            {
                // Create the "base" container with the minimum
                // stuff registered to run Cake at all.
                var registrar = new CakeServices();
                registrar.RegisterModule(new CoreModule());
                registrar.RegisterModule(new Module());
                var container = registrar.Build();

                // Add custom registrations to the container.
                container.Update(_registrations);

                // Find and register tasks with the container.
                RegisterTasks(container);

                // Resolve the application and run it.
                return container.Resolve<ICakeHost>();
            }
            catch (Exception exception)
            {
                return new ErrorCakeHost(exception);
            }
        }

        private static void RegisterTasks(Container container)
        {
            // Create a child scope to not affect the underlying
            // container in case the ICakeTaskFinder references
            // something that is later replaced.
            using (var scope = container.CreateChildScope())
            {
                // Find tasks in registered assemblies.
                var assemblies = scope.Resolve<IEnumerable<Assembly>>();
                var finder = scope.Resolve<ICakeTaskFinder>();
                var tasks = finder.GetTasks(assemblies);

                if (tasks.Length > 0)
                {
                    var registrar = new CakeServices();
                    foreach (var task in tasks)
                    {
                        registrar.RegisterType(task).As<IFrostingTask>().Singleton();
                    }
                    container.Update(registrar);
                }
            }
        }
    }
}
