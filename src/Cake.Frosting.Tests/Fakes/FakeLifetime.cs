// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting.Tests.Fakes
{
    public sealed class FakeLifetime : FrostingLifetime
    {
        public bool CalledSetup { get; private set; }
        public bool CalledTeardown { get; private set; }

        public override void Setup(ICakeContext context)
        {
            CalledSetup = true;
        }

        public override void Teardown(ICakeContext context, ITeardownContext info)
        {
            CalledTeardown = true;
        }

        public sealed class WithoutOverrides : FrostingLifetime
        {
        }
    }
}