using System.Collections.Generic;
using System.Reflection;

namespace Cake.Arguments
{
    internal sealed class ArgumentDescription
    {
        private readonly string[] _names;
        private readonly string _parameter;
        private readonly bool _required;
        private readonly string _description;
        private readonly PropertyInfo _action;

        public IEnumerable<string> Names
        {
            get { return _names; }
        }

        public string Parameter
        {
            get { return _parameter; }
        }

        public bool Required
        {
            get { return _required; }
        }

        public string Description
        {
            get { return _description; }
        }

        internal PropertyInfo Property
        {
            get { return _action; }
        }

        public ArgumentDescription(string[] names, string parameter, bool required, string description, PropertyInfo action)
        {
            _names = names;
            _parameter = parameter;
            _required = required;
            _description = description;
            _action = action;
        }
    }
}