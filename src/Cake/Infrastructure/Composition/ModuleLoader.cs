// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Cake.Core.Composition;

namespace Cake.Infrastructure.Composition
{
    /// <summary>
    /// Represents a module loader for Cake modules.
    /// </summary>
    public sealed class ModuleLoader
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleLoader"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ModuleLoader(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Loads modules from the specified types.
        /// </summary>
        /// <param name="types">The module types to load.</param>
        /// <returns>An enumerable of loaded modules.</returns>
        public IEnumerable<ICakeModule> LoadModules(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var module = LoadModule(type);
                if (module != null)
                {
                    yield return module;
                }
            }
        }

        private ICakeModule LoadModule(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            var constructor = GetGreediestConstructor(type);
            var parameters = constructor.GetParameters();

            var arguments = new object[parameters.Length];
            for (int index = 0; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                arguments[index] = _container.Resolve(parameter.ParameterType);
            }

            if (Activator.CreateInstance(type, arguments) is ICakeModule module)
            {
                return module;
            }

            return null;
        }

        private static ConstructorInfo GetGreediestConstructor(Type type)
        {
            return type
                .GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();
        }
    }
}
