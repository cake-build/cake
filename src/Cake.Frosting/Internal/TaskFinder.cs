// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cake.Frosting.Internal
{
    internal sealed class TaskFinder : ICakeTaskFinder
    {
        public Type[] GetTasks(IEnumerable<Assembly> assemblies)
        {
            var result = new List<Type>();
            foreach (var assembly in assemblies)
            {
                if (assembly == null)
                {
                    continue;
                }

                foreach (var type in assembly.GetExportedTypes())
                {
                    var info = type.GetTypeInfo();
                    if (typeof(IFrostingTask).IsAssignableFrom(type) && info.IsClass && !info.IsAbstract)
                    {
                        result.Add(type);
                    }
                }
            }
            return result.ToArray();
        }
    }
}
