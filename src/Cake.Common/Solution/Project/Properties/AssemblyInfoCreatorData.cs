// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;

namespace Cake.Common.Solution.Project.Properties
{
    internal sealed class AssemblyInfoCreatorData
    {
        private readonly Dictionary<string, string> _dictionary;
        private readonly Dictionary<string, string> _customAttributes;
        private readonly Dictionary<string, string> _metadatattributes;
        private readonly HashSet<string> _namespaces;
        private readonly HashSet<string> _internalVisibleTo;
        private readonly string _trueStringValue;
        private readonly string _falseStringValue;

        public IDictionary<string, string> Attributes => _dictionary;

        public IDictionary<string, string> CustomAttributes => _customAttributes;

        public IDictionary<string, string> MetadataAttributes => _metadatattributes;

        public ISet<string> Namespaces => _namespaces;

        public ISet<string> InternalVisibleTo => _internalVisibleTo;

        public AssemblyInfoCreatorData(AssemblyInfoSettings settings, bool isVisualBasicAssemblyInfoFile)
        {
            _dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _customAttributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _metadatattributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _namespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _internalVisibleTo = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            _falseStringValue = isVisualBasicAssemblyInfoFile ? "False" : "false";
            _trueStringValue = isVisualBasicAssemblyInfoFile ? "True" : "true";

            // Add attributes.
            AddAttribute("AssemblyTitle", "System.Reflection", settings.Title);
            AddAttribute("AssemblyDescription", "System.Reflection", settings.Description);
            AddAttribute("AssemblyCompany", "System.Reflection", settings.Company);
            AddAttribute("AssemblyProduct", "System.Reflection", settings.Product);
            AddAttribute("AssemblyVersion", "System.Reflection", settings.Version);
            AddAttribute("AssemblyFileVersion", "System.Reflection", settings.FileVersion);
            AddAttribute("AssemblyInformationalVersion", "System.Reflection", settings.InformationalVersion);
            AddAttribute("AssemblyCopyright", "System.Reflection", settings.Copyright);
            AddAttribute("AssemblyTrademark", "System.Reflection", settings.Trademark);
            AddAttribute("AssemblyConfiguration", "System.Reflection", settings.Configuration);
            AddAttribute("Guid", "System.Runtime.InteropServices", settings.Guid);
            AddAttribute("ComVisible", "System.Runtime.InteropServices", settings.ComVisible);
            AddAttribute("CLSCompliant", "System", settings.CLSCompliant);

            // Add assemblies that internals are visible to.
            if (settings.InternalsVisibleTo != null)
            {
                foreach (var item in settings.InternalsVisibleTo.Where(item => item != null))
                {
                    _internalVisibleTo.Add(string.Concat("InternalsVisibleTo(\"", item.UnQuote(), "\")"));
                }
                if (_internalVisibleTo.Count > 0)
                {
                    _namespaces.Add("System.Runtime.CompilerServices");
                }
            }
            if (settings.CustomAttributes != null)
            {
                foreach (var item in settings.CustomAttributes.Where(item => item != null))
                {
                    AddCustomAttribute(item.Name, item.NameSpace, item.Value, item.UseRawValue);
                }
            }
            if (settings.MetaDataAttributes != null)
            {
                foreach (var item in settings.MetaDataAttributes.Where(item => item != null))
                {
                    AddMetadataAttribute(item.NameSpace, item.Key, item.Value);
                }
            }
        }

        private void AddAttribute(string name, string @namespace, bool? value)
        {
            if (value != null)
            {
                AddAttributeCore(Attributes, name, @namespace, value.Value ? _trueStringValue : _falseStringValue);
            }
        }

        private void AddAttribute(string name, string @namespace, string value)
        {
            if (value != null)
            {
                AddAttributeCore(Attributes, name, @namespace, string.Concat("\"", value, "\""));
            }
        }

        private void AddCustomAttribute(string name, string @namespace, object value, bool isRawValue)
        {
            var attributeValue = AttributeValueToString(value, isRawValue);

            AddAttributeCore(CustomAttributes, name, @namespace, attributeValue);
        }

        private string AttributeValueToString(object value, bool isRawValue)
        {
            switch (value)
            {
                case null:
                {
                    return string.Empty;
                }
                case bool boolValue:
                {
                    return boolValue ? _trueStringValue : _falseStringValue;
                }
                case string stringValue:
                {
                    return stringValue == string.Empty
                        ? string.Empty
                        : isRawValue
                            ? stringValue
                            : string.Concat("\"", stringValue.Replace("\"", "\\\""), "\"");
                }
                default:
                {
                    return Convert.ToString(value, CultureInfo.InvariantCulture);
                }
            }
        }

        private void AddMetadataAttribute(string @namespace, string key, string value)
        {
            if (key != null && value != null)
            {
                AddAttributeCore(MetadataAttributes, string.Concat("\"", key, "\""), @namespace, string.Concat("\"", value, "\""));
            }
        }

        private void AddAttributeCore(IDictionary<string, string> dict, string name, string @namespace, string value)
        {
            if (dict.ContainsKey(name))
            {
                dict[name] = value;
            }
            else
            {
                dict.Add(name, value);
            }
            Namespaces.Add(@namespace);
        }
    }
}
