// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;

namespace Cake.Frosting.Tests.Data
{
    public sealed class DummyStartup : IFrostingStartup
    {
        public sealed class DummyStartupSentinel
        {
        }

        public void Configure(ICakeServices services)
        {
            services.RegisterType<DummyStartupSentinel>();
        }
    }
}
