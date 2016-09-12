// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Frosting.Internal.Composition.Activators
{
    internal sealed class InstanceActivator : Activator
    {
        private readonly object _instance;

        public InstanceActivator(object instance)
        {
            _instance = instance;
        }

        public override object Activate(Container container)
        {
            return _instance;
        }

        public override Activator CreateCopy()
        {
            return new InstanceActivator(_instance);
        }
    }
}