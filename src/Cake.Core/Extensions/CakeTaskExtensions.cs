using System;

namespace Cake.Core.Extensions
{
    public static class CakeTaskExtensions
    {
        public static CakeTask WithCriteria(this CakeTask task, bool criteria)
        {
            return task.WithCriteria(() => criteria);
        }
    }
}
