// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Frosting.Tests.Data.Tasks
{
    [TaskName("No-Continue-On-Error")]
    public sealed class NoContinueOnErrorTask : FrostingTask
    {
        public override void Run(ICakeContext context)
        {
            throw new InvalidOperationException();
        }
    }
}
