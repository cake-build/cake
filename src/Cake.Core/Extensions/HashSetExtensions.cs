// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="HashSet{T}"/>
    /// </summary>
    public static class HashSetExtensions
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="HashSet{T}"/>.
        /// </summary>
        /// <param name="set">The <see cref="HashSet{T}"/> to add items to.</param>
        /// <param name="items">The collection whose elements should be added to the end of the <see cref="HashSet{T}"/>. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        /// <typeparam name="T">The type of <see cref="HashSet{T}"/>.</typeparam>
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
