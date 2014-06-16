using System;

namespace Cake.Core
{
    public sealed class CakeTaskBuilder<T>
        where T : CakeTask
    {
        private readonly T _task;

        public T Task
        {
            get { return _task; }
        }

        public CakeTaskBuilder(T task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            _task = task;
        }
    }
}
