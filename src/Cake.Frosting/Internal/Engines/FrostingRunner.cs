// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Cli;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Internal
{
    internal sealed class FrostingRunner : FrostingEngine<BuildScriptHost<IFrostingContext>>
    {
        public FrostingRunner(BuildScriptHost<IFrostingContext> host,
            ICakeEngine engine, IFrostingContext context, ICakeLog log,
            IEnumerable<IFrostingTask> tasks,
            IFrostingSetup setup = null, IFrostingTeardown teardown = null,
            IFrostingTaskSetup taskSetup = null, IFrostingTaskTeardown taskTeardown = null)
            : base(host, engine, context, log, tasks, setup, teardown, taskSetup, taskTeardown)
        {
        }
    }
}
