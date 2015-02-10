using System;
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
        private const string NuSpecXsd = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd";

        static NuspecTransformer()
        {
            _mappings = new Dictionary<string, Func<NuGetPackSettings, string>>
            {
                { "id", settings => ToString(settings.Id) },
                { "version", settings => ToString(settings.Version) },
                { "title", settings => ToString(settings.Title) },
                { "authors", settings => ToCommaSeparatedString(settings.Authors) },
                { "owners", settings => ToCommaSeparatedString(settings.Owners) },
                { "description", settings => ToString(settings.Description) },
                { "summary", settings => ToString(settings.Summary) },
                { "licenseUrl", settings => ToString(settings.LicenseUrl) },
                { "projectUrl", settings => ToString(settings.ProjectUrl) },
                { "iconUrl", settings => ToString(settings.IconUrl) },
                { "requireLicenseAcceptance", settings => ToString(settings.RequireLicenseAcceptance) },
                { "copyright", settings => ToString(settings.Copyright) },
                { "releaseNotes", settings => ToMultiLineString(settings.ReleaseNotes) },
                { "tags", settings => ToSpaceSeparatedString(settings.Tags) }
            };
        }

        public static void Transform(XmlDocument document, NuGetPackSettings settings)
        {
            // Create the namespace manager.
            var namespaceManager = new XmlNamespaceManager(document.NameTable);
            namespaceManager.AddNamespace("nu", NuSpecXsd);

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

            if (settings.Files != null && settings.Files.Count > 0)
            {
                var filesPath = string.Format(CultureInfo.InvariantCulture, "/package//*[local-name()='files']");
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
            var package = document.SelectSingleNode("/package");
            if (package == null)
            {
                throw new CakeException("Nuspec file is missing package root.");
            }
            return package;
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
                    var package = GetPackageElement(document);

                    // Create the metadata element.
                    parent = document.CreateElement("metadata", NuSpecXsd);
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
