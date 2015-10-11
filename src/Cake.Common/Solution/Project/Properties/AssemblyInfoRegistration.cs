using System;
using System.Collections.Generic;

namespace Cake.Common.Solution.Project.Properties
{
    internal sealed class AssemblyInfoRegistration
    {
        private readonly IDictionary<string, string> _dictionary;
        private readonly ISet<string> _namespaces;

        public IDictionary<string, string> Attributes
        {
            get { return _dictionary; }
        }

        public ISet<string> Namespaces
        {
            get { return _namespaces; }
        }

        public AssemblyInfoRegistration()
        {
            _dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _namespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddBoolean(string name, string @namespace, bool? value)
        {
            if (value != null)
            {
                Add(name, @namespace, value.Value ? "true" : "false");
            }
        }

        public void AddString(string name, string @namespace, string value)
        {
            if (value != null)
            {
                Add(name, @namespace, string.Concat("\"", value, "\""));
            }
        }

        private void Add(string name, string @namespace, string value)
        {
            if (Attributes.ContainsKey(name))
            {
                Attributes[name] = value;
            }
            else
            {
                Attributes.Add(name, value);
            }
            Namespaces.Add(@namespace);
        }
    }
}