// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace Cake.Frosting.Cli.Reflection
{
    internal sealed class StartupFinder
    {
        public Type FindStartup(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IFrostingStartup).IsAssignableFrom(type))
                {
                    // Get the empty constructor.
                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor != null)
                    {
                        return type;
                    }
                }
            }
            return null;
        }
    }
}
