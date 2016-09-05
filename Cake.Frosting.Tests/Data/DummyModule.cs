// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;

namespace Cake.Frosting.Tests.Data
{
    public class DummyModule : ICakeModule
    {
        public sealed class DummyModuleSentinel
        {
        }

        public void Register(ICakeContainerRegistrar registrar)
        {
            registrar.RegisterType<DummyModuleSentinel>();
        }
    }
}
