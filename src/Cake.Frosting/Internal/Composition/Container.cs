// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Frosting.Internal.Composition
{
    internal sealed class Container : IDisposable
    {
        public ComponentRegistry Registry { get; }

        public Container()
            : this(null)
        {
        }

        public Container(ComponentRegistry registry)
        {
            Registry = registry ?? new ComponentRegistry();
        }

        public void Dispose()
        {
            Registry.Dispose();
        }

        public Container CreateChildScope()
        {
            return new Container(Registry.CreateCopy());
        }

        public object Resolve(ComponentRegistration registration)
        {
            return registration?.Activator.Activate(this);
        }
    }
}