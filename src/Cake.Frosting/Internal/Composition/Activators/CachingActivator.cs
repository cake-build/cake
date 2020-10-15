// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Frosting.Internal.Composition.Activators
{
    internal class CachingActivator : Activator
    {
        private readonly Activator _activator;
        private object _result;

        public CachingActivator(Activator activator)
        {
            _activator = activator;
            _result = null;
        }

        public override object Activate(Container container)
        {
            return _result ?? (_result = _activator.Activate(container));
        }

        public override Activator CreateCopy()
        {
            return new CachingActivator(_activator.CreateCopy());
        }
    }
}