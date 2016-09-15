// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cake.Core.Reflection
{
    internal sealed class SimpleAssemblyComparer : IEqualityComparer<Assembly>
    {
        public bool Equals(Assembly x, Assembly y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.FullName.Equals(y.FullName, StringComparison.Ordinal);
        }

        public int GetHashCode(Assembly obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
