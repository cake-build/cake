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