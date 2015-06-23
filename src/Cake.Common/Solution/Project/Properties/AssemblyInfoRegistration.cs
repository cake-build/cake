using System;
using System.Collections.Generic;

namespace Cake.Common.Solution.Project.Properties
{
    internal sealed class AssemblyInfoRegistration
    {
        private readonly List<KeyValuePair<string,string>> _attributes;
        private readonly ISet<string> _namespaces;

        public List<KeyValuePair<string, string>> Attributes
        {
            get { return _attributes; }
        }

        public ISet<string> Namespaces
        {
            get { return _namespaces; }
        }

        public AssemblyInfoRegistration()
        {
            _attributes = new List<KeyValuePair<string, string>>(); 
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
            Attributes.Add(new KeyValuePair<string, string>(name, value));
            Namespaces.Add(@namespace);
        }
    }
}