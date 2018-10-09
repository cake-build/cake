// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Cake.Core;

namespace Cake.Common.NuSpec
{
    internal static class NuSpecTransformer
    {
        private const string NuSpecXsd = "http://schemas.microsoft.com/packaging/2016/06/nuspec.xsd";

        private static readonly Dictionary<string, Func<NuSpecSettings, string>> _mappings;
        private static readonly List<string> _cdataElements;

        static NuSpecTransformer()
        {
            _mappings = new Dictionary<string, Func<NuSpecSettings, string>>
            {
                // required
                { "id", settings => ToString(settings.Id) },
                { "version", settings => ToString(settings.Version) },
                { "title", settings => ToString(settings.Title) },
                { "authors", settings => ToCommaSeparatedString(settings.Authors) },

                // optional
                { "owners", settings => ToCommaSeparatedString(settings.Owners) },
                { "description", settings => ToString(settings.Description) },
                { "summary", settings => ToString(settings.Summary) },
                { "licenseUrl", settings => ToString(settings.LicenseUrl) },
                { "projectUrl", settings => ToString(settings.ProjectUrl) },
                { "iconUrl", settings => ToString(settings.IconUrl) },
                { "developmentDependency", settings => ToString(settings.DevelopmentDependency) },
                { "requireLicenseAcceptance", settings => ToString(settings.RequireLicenseAcceptance) },
                { "copyright", settings => ToString(settings.Copyright) },
                { "releaseNotes", settings => ToMultiLineString(settings.ReleaseNotes) },
                { "tags", settings => ToSpaceSeparatedString(settings.Tags) },
                { "language", settings => ToString(settings.Language) },

                // Chocolatey specific
                { "packageSourceUrl", settings => ToString(settings.PackageSourceUrl) },
                { "projectSourceUrl", settings => ToString(settings.ProjectSourceUrl) },
                { "docsUrl", settings => ToString(settings.DocsUrl) },
                { "mailingListUrl", settings => ToString(settings.MailingListUrl) },
                { "bugTrackerUrl", settings => ToString(settings.BugTrackerUrl) },
            };

            _cdataElements = new List<string> { "releaseNotes" };
        }

        public static void Transform(XmlDocument document, NuSpecSettings settings)
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

            if (settings.Repository != null)
            {
                var repositoryNode = FindOrCreateElement(document, namespaceManager, "repository");
                repositoryNode.AddAttributeIfSpecified(settings.Repository.Type, "type");
                repositoryNode.AddAttributeIfSpecified(settings.Repository.Url, "url");
                repositoryNode.AddAttributeIfSpecified(settings.Repository.Commit, "commit");
                repositoryNode.AddAttributeIfSpecified(settings.Repository.Branch, "branch");
            }

            if (settings.Files?.Count > 0)
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
                    AddFileElement(document, filesElement, file);
                }
            }

            if (settings.PackageTypes?.Count > 0)
            {
                var packageTypesElement = FindOrCreateElement(document, namespaceManager, "packageTypes");

                packageTypesElement.RemoveAll();

                foreach (var packageType in settings.PackageTypes)
                {
                    AddPackageTypeElement(document, packageTypesElement, packageType);
                }
            }

            if (settings.Dependencies?.Count > 0)
            {
                var dependenciesElement = FindOrCreateElement(document, namespaceManager, "dependencies");

                // Add the files
                dependenciesElement.RemoveAll();
                if (settings.Dependencies.All(c => string.IsNullOrEmpty(c.TargetFramework)))
                {
                    foreach (var dependency in settings.Dependencies)
                    {
                        AddDependencyElement(document, dependenciesElement, dependency);
                    }
                }
                else
                {
                    foreach (var targetFrameworkDependencies in settings.Dependencies.GroupBy(x => x.TargetFramework))
                    {
                        var groupElement = document.CreateAndAppendElement(dependenciesElement, "group");
                        if (!string.IsNullOrEmpty(targetFrameworkDependencies.Key))
                        {
                            groupElement.AddAttributeIfSpecified(targetFrameworkDependencies.Key, "targetFramework");
                        }
                        foreach (var dependency in targetFrameworkDependencies)
                        {
                            AddDependencyElement(document, groupElement, dependency);
                        }
                    }
                }
            }

            if (settings.FrameworkAssemblies?.Count > 0)
            {
                var frameworkAssembliesElement = FindOrCreateElement(document, namespaceManager, "frameworkAssemblies");

                frameworkAssembliesElement.RemoveAll();

                foreach (var frameworkAssembly in settings.FrameworkAssemblies)
                {
                    AddFrameworkAssemblyElement(document, frameworkAssembliesElement, frameworkAssembly);
                }
            }

            if (settings.References?.Count > 0)
            {
                var referencesElement = FindOrCreateElement(document, namespaceManager, "references");

                // Add the files
                referencesElement.RemoveAll();
                if (settings.References.All(c => string.IsNullOrEmpty(c.TargetFramework)))
                {
                    foreach (var reference in settings.References)
                    {
                        AddReferenceElement(document, referencesElement, reference);
                    }
                }
                else
                {
                    foreach (var targetFrameworkReferences in settings.References.GroupBy(x => x.TargetFramework))
                    {
                        var groupElement = document.CreateAndAppendElement(referencesElement, "group");
                        if (!string.IsNullOrEmpty(targetFrameworkReferences.Key))
                        {
                            groupElement.AddAttributeIfSpecified(targetFrameworkReferences.Key, "targetFramework");
                        }
                        foreach (var reference in targetFrameworkReferences)
                        {
                            AddReferenceElement(document, groupElement, reference);
                        }
                    }
                }
            }

            if (settings.ContentFiles?.Count > 0)
            {
                var contentFilesElement = FindOrCreateElement(document, namespaceManager, "contentFiles");

                contentFilesElement.RemoveAll();

                foreach (var contentFileEntry in settings.ContentFiles)
                {
                    AddContentFileElement(document, contentFilesElement, contentFileEntry);
                }
            }
        }

        private static void AddFileElement(XmlDocument document, XmlNode parent, NuSpecContent file)
        {
            var fileElement = document.CreateAndAppendElement(parent, "file");
            fileElement.AddAttributeIfSpecified(file.Source, "src");
            fileElement.AddAttributeIfSpecified(file.Target, "target");
            fileElement.AddAttributeIfSpecified(file.Exclude, "exclude");
        }

        private static void AddPackageTypeElement(XmlDocument document, XmlNode parent, NuSpecPackageType packageType)
        {
            var packageTypeElement = document.CreateAndAppendElement(parent, "packageType");
            packageTypeElement.AddAttributeIfSpecified(packageType.Name, "name");
            packageTypeElement.AddAttributeIfSpecified(packageType.Version, "version");
        }

        private static void AddFrameworkAssemblyElement(XmlDocument document, XmlNode parent, NuSpecFrameworkAssembly frameworkAssembly)
        {
            var frameworkAssemblyElement = document.CreateAndAppendElement(parent, "frameworkAssembly");
            frameworkAssemblyElement.AddAttributeIfSpecified(frameworkAssembly.AssemblyName, "assemblyName");
            frameworkAssemblyElement.AddAttributeIfSpecified(frameworkAssembly.TargetFramework, "targetFramework");
        }

        private static void AddContentFileElement(XmlDocument document, XmlNode parent, NuSpecContentFile contentFile)
        {
            var contentFileElement = document.CreateAndAppendElement(parent, "files");
            contentFileElement.AddAttributeIfSpecified(contentFile.Include, "include");
            contentFileElement.AddAttributeIfSpecified(contentFile.Exclude, "exclude");
            contentFileElement.AddAttributeIfSpecified(contentFile.BuildAction, "buildAction");
            contentFileElement.AddAttributeIfSpecified(ToString(contentFile.CopyToOutput), "copyToOutput");
            contentFileElement.AddAttributeIfSpecified(ToString(contentFile.Flatten), "flatten");
        }

        private static void AddDependencyElement(XmlDocument document, XmlNode parent, NuSpecDependency dependency)
        {
            var fileElement = document.CreateAndAppendElement(parent, "dependency");
            fileElement.AddAttributeIfSpecified(dependency.Id, "id");
            fileElement.AddAttributeIfSpecified(dependency.Version, "version");
            fileElement.AddAttributeIfSpecified(dependency.Include, "include");
            fileElement.AddAttributeIfSpecified(dependency.Exclude, "exclude");
        }

        private static void AddReferenceElement(XmlDocument document, XmlNode parent, NuSpecReference reference)
        {
            var fileElement = document.CreateAndAppendElement(parent, "dependency");
            fileElement.AddAttributeIfSpecified(reference.File, "file");
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
            return value?.ToString().TrimEnd('/');
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