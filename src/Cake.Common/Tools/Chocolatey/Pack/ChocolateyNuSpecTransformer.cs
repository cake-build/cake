// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Cake.Core;

namespace Cake.Common.Tools.Chocolatey.Pack
{
    internal static class ChocolateyNuSpecTransformer
    {
        private static readonly Dictionary<string, Func<ChocolateyPackSettings, string>> _mappings;
        private static readonly List<string> _cdataElements;
        private const string ChocolateyNuSpecXsd = "http://schemas.microsoft.com/packaging/2015/06/nuspec.xsd";

        static ChocolateyNuSpecTransformer()
        {
            _mappings = new Dictionary<string, Func<ChocolateyPackSettings, string>>
            {
                { "id", settings => ToString(settings.Id) },
                { "title", settings => ToString(settings.Title) },
                { "version", settings => ToString(settings.Version) },
                { "authors", settings => ToCommaSeparatedString(settings.Authors) },
                { "owners", settings => ToCommaSeparatedString(settings.Owners) },
                { "summary", settings => ToString(settings.Summary) },
                { "description", settings => ToString(settings.Description) },
                { "projectUrl", settings => ToString(settings.ProjectUrl) },
                { "packageSourceUrl", settings => ToString(settings.PackageSourceUrl) },
                { "projectSourceUrl", settings => ToString(settings.ProjectSourceUrl) },
                { "docsUrl", settings => ToString(settings.DocsUrl) },
                { "mailingListUrl", settings => ToString(settings.MailingListUrl) },
                { "bugTrackerUrl", settings => ToString(settings.BugTrackerUrl) },
                { "tags", settings => ToSpaceSeparatedString(settings.Tags) },
                { "copyright", settings => ToString(settings.Copyright) },
                { "licenseUrl", settings => ToString(settings.LicenseUrl) },
                { "requireLicenseAcceptance", settings => ToString(settings.RequireLicenseAcceptance) },
                { "iconUrl", settings => ToString(settings.IconUrl) },
                { "releaseNotes", settings => ToMultiLineString(settings.ReleaseNotes) }
            };

            _cdataElements = new List<string>
                                {
                                    "releaseNotes"
                                };
        }

        public static void Transform(XmlDocument document, ChocolateyPackSettings settings)
        {
            // Create the namespace manager.
            var namespaceManager = new XmlNamespaceManager(document.NameTable);
            namespaceManager.AddNamespace("nu", ChocolateyNuSpecXsd);

            foreach (var elementName in _mappings.Keys)
            {
                var content = _mappings[elementName](settings);
                if (content != null)
                {
                    // Replace the node content.
                    var node = FindOrCreateElement(document, namespaceManager, elementName);

                    if (_cdataElements.Contains(elementName))
                    {
                        node.AppendChild(document.CreateCDataSection(content));
                    }
                    else
                    {
                        node.InnerText = content;
                    }
                }
            }

            if (settings.Files != null && settings.Files.Count > 0)
            {
                var filesPath = string.Format(CultureInfo.InvariantCulture, "//*[local-name()='package']//*[local-name()='files']");
                var filesElement = document.SelectSingleNode(filesPath, namespaceManager);
                if (filesElement == null)
                {
                    // Get the package element.
                    var package = GetPackageElement(document);
                    filesElement = document.CreateAndAppendElement(package, "files");
                }

                // Add the files
                filesElement.RemoveAll();
                foreach (var file in settings.Files)
                {
                    var fileElement = document.CreateAndAppendElement(filesElement, "file");
                    fileElement.AddAttributeIfSpecified(file.Source, "src");
                    fileElement.AddAttributeIfSpecified(file.Exclude, "exclude");
                    fileElement.AddAttributeIfSpecified(file.Target, "target");
                }
            }
        }

        private static XmlNode GetPackageElement(XmlDocument document)
        {
            var package = document.SelectSingleNode("//*[local-name()='package']");
            if (package == null)
            {
                throw new CakeException("Nuspec file is missing package root.");
            }
            return package;
        }

        private static XmlNode FindOrCreateElement(XmlDocument document, XmlNamespaceManager ns, string name)
        {
            var path = string.Format(CultureInfo.InvariantCulture, "//*[local-name()='package']//*[local-name()='metadata']//*[local-name()='{0}']", name);
            var node = document.SelectSingleNode(path, ns);
            if (node == null)
            {
                var parent = document.SelectSingleNode("//*[local-name()='package']//*[local-name()='metadata']", ns);
                if (parent == null)
                {
                    // Get the package element.
                    var package = GetPackageElement(document);

                    // Create the metadata element.
                    parent = document.CreateElement("metadata", ChocolateyNuSpecXsd);
                    package.PrependChild(parent);
                }

                node = document.CreateAndAppendElement(parent, name);
            }
            return node;
        }

        private static XmlNode CreateAndAppendElement(this XmlDocument document, XmlNode parent, string name)
        {
            // If the parent didn't have a namespace specified, then skip adding one.
            // Otherwise add the parent's namespace. This is a little hackish, but it
            // will avoid empty namespaces. This should probably be done better...
            return parent.AppendChild(
                string.IsNullOrWhiteSpace(parent.NamespaceURI)
                    ? document.CreateElement(name)
                    : document.CreateElement(name, parent.NamespaceURI));
        }

        private static void AddAttributeIfSpecified(this XmlNode element, string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value) || element.OwnerDocument == null || element.Attributes == null)
            {
                return;
            }
            var attr = element.OwnerDocument.CreateAttribute(name);
            attr.Value = value;
            element.Attributes.Append(attr);
        }

        private static string ToString(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value;
        }

        private static string ToString(Uri value)
        {
            return value == null ? null : value.ToString().TrimEnd('/');
        }

        private static string ToString(bool value)
        {
            return value.ToString().ToLowerInvariant();
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
