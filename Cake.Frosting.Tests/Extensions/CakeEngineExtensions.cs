using System;
using System.Linq;
using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Tests
{
    public static class CakeEngineExtensions
    {
        public static bool IsTaskRegistered(this ICakeEngine engine, string name)
        {
            return engine.Tasks.Any(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
