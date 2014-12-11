using System;
using System.Collections.Generic;

namespace Cake.Common.Solution.Project.Properties
{
    internal class AssemblyInfoRegistration
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

        public void AddBoolean(string name, string @namespace, bool value)
        {
            Add(name, @namespace, value ? "true" : "false");
        }

        public void AddString(string name, string @namespace, string value)
        {
            Add(name, @namespace, string.Concat("\"", value, "\""));
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