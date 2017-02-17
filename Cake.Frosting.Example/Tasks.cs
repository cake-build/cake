// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Example
{
    public sealed class Hello : FrostingTask<Settings>
    {
    }

    [Dependency(typeof(Hello))]
    public sealed class World : FrostingTask<Settings>
    {
    }

    [Dependency(typeof(World))]
    public sealed class Magic : FrostingTask<Settings>
    {
        public override bool ShouldRun(Settings context)
        {
            // Don't run this task on OSX.
            return context.Environment.Platform.Family != PlatformFamily.OSX;
        }

        public override void Run(Settings context)
        {
            context.Log.Information("Value is: {0}", context.Magic);
        }
    }

    [TaskName("Default")]
    [Dependency(typeof(Magic))]
    public class DefaultTask : FrostingTask
    {
    }
}
