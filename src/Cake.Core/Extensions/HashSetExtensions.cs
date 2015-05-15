using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
