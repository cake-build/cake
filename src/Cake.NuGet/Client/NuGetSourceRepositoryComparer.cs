// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

namespace Cake.NuGet
{
    internal sealed class NuGetSourceRepositoryComparer : IEqualityComparer<SourceRepository>
    {
        public bool Equals(SourceRepository x, SourceRepository y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            return x.PackageSource.Equals(y.PackageSource);
        }

        public int GetHashCode(SourceRepository obj)
        {
            return obj.PackageSource.GetHashCode();
        }
    }
}