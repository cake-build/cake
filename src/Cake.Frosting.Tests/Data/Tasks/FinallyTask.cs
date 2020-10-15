// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Tests.Data.Tasks
{
    [TaskName("Finally")]
    public sealed class FinallyTask : FrostingTask<ICakeContext>
    {
        public override void Run(ICakeContext context)
        {
            context.Log.Information("Run method called");
            throw new InvalidOperationException("On test exception");
        }

        public override void OnError(Exception exception, ICakeContext context)
        {
            context.Log.Information("OnError method called");
        }

        public override void Finally(ICakeContext context)
        {
            context.Log.Information("Finally method called");
        }
    }
}