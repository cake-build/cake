// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting.Tests
{
    public sealed class FakeTaskLifetime : FrostingTaskLifetime
    {
        public int SetupCount { get; set; }
        public int TeardownCount { get; set; }

        public override void Setup(ICakeContext context, ITaskSetupContext info)
        {
            SetupCount++;
        }

        public override void Teardown(ICakeContext context, ITaskTeardownContext info)
        {
            TeardownCount++;
        }
    }
}
