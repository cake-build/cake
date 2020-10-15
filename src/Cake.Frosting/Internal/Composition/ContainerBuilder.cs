// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Frosting.Internal.Composition
{
    internal sealed class ContainerBuilder
    {
        private readonly Queue<Action<ComponentRegistry>> _registry;

        public ContainerBuilder()
        {
            _registry = new Queue<Action<ComponentRegistry>>();
        }

        public void Register(Action<ComponentRegistry> action)
        {
            _registry.Enqueue(action);
        }

        public Container Build()
        {
            var container = new Container();
            Update(container);
            return container;
        }

        public void Update(Container container)
        {
            while (_registry.Count > 0)
            {
                var action = _registry.Dequeue();
                action(container.Registry);
            }
        }
    }
}
