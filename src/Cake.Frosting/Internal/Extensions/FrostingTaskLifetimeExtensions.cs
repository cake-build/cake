// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Internal
{
    internal static class FrostingTaskLifetimeExtensions
    {
        public static bool IsSetupOverridden(this IFrostingTaskLifetime lifetime, IFrostingContext context)
        {
            return lifetime.GetType().GetMethod("Setup", new[] { context.GetType(), typeof(ITaskSetupContext) }).IsOverriden();
        }

        public static bool IsTeardownOverridden(this IFrostingTaskLifetime lifetime, IFrostingContext context)
        {
            return lifetime.GetType().GetMethod("Teardown", new[] { context.GetType(), typeof(ITaskTeardownContext) }).IsOverriden();
        }
    }
}
