using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core
{
    public abstract class CakeTask
    {
        private readonly string _name;
        private readonly List<string> _dependencies;
        private readonly List<Func<bool>> _criterias;

        public string Name
        {
            get { return _name; }
        }

        public IReadOnlyList<string> Dependencies
        {
            get { return _dependencies; }
        }

        public IReadOnlyList<Func<bool>> Criterias
        {
            get { return _criterias; }
        }

        public bool ContinueOnError { get; set; }

        protected CakeTask(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Task name cannot be empty.");
            }
            _name = name;
            _dependencies = new List<string>();
            _criterias = new List<Func<bool>>();
        }


        public void AddDependency(string name)
        {
            if (_dependencies.Any(x => x == name))
            {
                const string format = "The task '{0}' already have a dependency on '{1}'.";
                var message = string.Format(format, _name, name);
                throw new CakeException(message);
            }
            _dependencies.Add(name);
        }

        public void AddCriteria(Func<bool> criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }
            _criterias.Add(criteria);
        }

        public abstract void Execute(ICakeContext context);
    }
}
