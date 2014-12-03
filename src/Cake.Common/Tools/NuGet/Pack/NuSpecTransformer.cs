﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Cake.Core;

namespace Cake.Common.Tools.NuGet.Pack
{
    internal static class NuspecTransformer
    {
        private static readonly Dictionary<string, Func<NuGetPackSettings, string>> _mappings;

        static NuspecTransformer()
        {
            _mappings = new Dictionary<string, Func<NuGetPackSettings, string>>
            {
                {"id", settings => ToString(settings.Id)},
                {"version", settings => ToString(settings.Version)},
                {"title", settings => ToString(settings.Title)},
                {"authors", settings => ToCommaSeparatedString(settings.Authors)},
                {"owners", settings => ToCommaSeparatedString(settings.Owners)},
                {"description", settings => ToString(settings.Description)},
                {"summary", settings => ToString(settings.Summary)},
                {"licenseUrl", settings => ToString(settings.LicenseUrl)},
                {"projectUrl", settings => ToString(settings.ProjectUrl)},
                {"iconUrl", settings => ToString(settings.IconUrl)},
                {"requireLicenseAcceptance", settings => ToString(settings.RequireLicenseAcceptance)},
                {"copyright", settings => ToString(settings.Copyright)},
                {"releaseNotes", settings => ToMultiLineString(settings.ReleaseNotes)},
                {"tags", settings => ToSpaceSeparatedString(settings.Tags)}
            };
        }

        public static void Transform(XmlDocument document, NuGetPackSettings settings)
        {
            // Create the namespace manager.
            var namespaceManager = new XmlNamespaceManager(document.NameTable);
            namespaceManager.AddNamespace("nu", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

            // Iterate through all mappings.
            foreach (var elementName in _mappings.Keys)
            {
                var content = _mappings[elementName](settings);
                if (content != null)
                {
                    // Replace the node content.
                    var node = FindOrCreateElement(document, namespaceManager, elementName);
                    node.InnerText = content;
                }
            }
        }

        private static XmlNode FindOrCreateElement(XmlDocument document, XmlNamespaceManager ns, string name)
        {
            var path = string.Format(CultureInfo.InvariantCulture, "/package//*[local-name()='metadata']//*[local-name()='{0}']", name);
            var node = document.SelectSingleNode(path, ns);
            if (node == null)
            {
                var parent = document.SelectSingleNode("/package//*[local-name()='metadata']", ns);
                if (parent == null)
                {
                    // Get the package element.
                    var package = document.SelectSingleNode("/package");
                    if (package == null)
                    {
                        throw new CakeException("Nuspec file is missing package root.");
                    }

                    // Create the metadata element.
                    parent = document.CreateElement("metadata", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");
                    package.PrependChild(parent);
                }

                // If the parent didn't have a namespace specified, then skip adding one.
                // Otherwise add the parent's namespace. This is a little hackish, but it 
                // will avoid empty namespaces. This should probably be done better...
                node = parent.NamespaceURI == string.Empty 
                    ? document.CreateElement(name) 
                    : document.CreateElement(name, parent.NamespaceURI);

                parent.AppendChild(node);
            }
            return node;
        }

        private static string ToString(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value;
        }

        private static string ToString(Uri value)
        {
            return value == null ? null : value.ToString().TrimEnd(new[] { '/' });
        }

        private static string ToString(bool value)
        {
            return value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
        }

        private static string ToCommaSeparatedString(IEnumerable<string> values)
        {
            return values != null
                ? string.Join(",", values)
                : null;
        }

        private static string ToMultiLineString(IEnumerable<string> values)
        {
            return values != null
                ? string.Join("\r\n", values).NormalizeLineEndings()
                : null;
        }

        private static string ToSpaceSeparatedString(IEnumerable<string> values)
        {
            return values != null
                ? string.Join(" ", values.Select(x => x.Replace(" ", "-")))
                : null;
        }
    }
}
