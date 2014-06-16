using System;
using System.Collections.Generic;

namespace Cake.Core
{
    public sealed class ActionTask : CakeTask
    {
        private readonly List<Action<ICakeContext>> _actions;

        public List<Action<ICakeContext>> Actions
        {
            get { return _actions; }
        }

        public ActionTask(string name) 
            : base(name)
        {
            _actions = new List<Action<ICakeContext>>();
        }

        public void AddAction(Action<ICakeContext> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            _actions.Add(action);
        }

        public override void Execute(ICakeContext context)
        {
            foreach (var action in _actions)
            {
                action(context);
            }
        }
    }
}