// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Testing
{
    public static class CakeEngineExtensions
    {
        public static bool IsTaskRegistered(this ICakeEngine engine, string name)
        {
            return engine.Tasks.Any(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
