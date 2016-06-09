// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    internal static class HashSetExtensions
    {
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    set.Add(item);
                }
            }
        }
    }
}
