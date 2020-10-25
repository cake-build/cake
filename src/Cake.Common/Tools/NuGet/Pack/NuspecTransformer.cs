// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        private const string NuSpecXsd = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd";

        public static void Transform(XmlDocument document, NuGetPackSettings settings)
        {
            // Create the namespace manager.
            var namespaceManager = new XmlNamespaceManager(document.NameTable);
            namespaceManager.AddNamespace("nu", NuSpecXsd);

            AddSimpleTypesToXmlDocument(document, settings, namespaceManager);
            AddComplexTypesToXmlDocument(document, settings, namespaceManager);
        }

        private static void AddSimpleTypesToXmlDocument(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            var mappings = CreateMappings(settings);
            CreateSimpleTypeMetadataFromMappings(document, settings, namespaceManager, mappings);
            CreateReleaseNotes(document, settings, namespaceManager);
            CreateMinClientVersion(document, settings, namespaceManager);
        }

        private static IDictionary<string, Func<NuGetPackSettings, string>> CreateMappings(NuGetPackSettings settings)
        {
            return new Dictionary<string, Func<NuGetPackSettings, string>>
            {
                { "id", x => ToString(settings.Id) },
                { "version", x => ToString(settings.Version) },
                { "title", x => ToString(settings.Title) },
                { "authors", x => ToCommaSeparatedString(settings.Authors) },
                { "owners", x => ToCommaSeparatedString(settings.Owners) },
                { "licenseUrl", x => ToString(settings.LicenseUrl) },
                { "projectUrl", x => ToString(settings.ProjectUrl) },
                { "icon", x => ToString(settings.Icon) },
                { "iconUrl", x => ToString(settings.IconUrl) },
                { "requireLicenseAcceptance", x => ToString(settings.RequireLicenseAcceptance) },
                { "developmentDependency", x => ToString(settings.DevelopmentDependency) },
                { "description", x => ToString(settings.Description) },
                { "summary", x => ToString(settings.Summary) },
                { "copyright", x => ToString(settings.Copyright) },
                { "language", x => ToString(settings.Language) },
                { "tags", x => ToSpaceSeparatedString(settings.Tags) },
                { "serviceable", x => ToString(settings.Serviceable) }
            };
        }

        private static void CreateSimpleTypeMetadataFromMappings(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager, IDictionary<string, Func<NuGetPackSettings, string>> mappings)
        {
            mappings.ToList().ForEach(x =>
            {
                var value = x.Value(settings);
                if (value != null)
                {
                    var node = FindOrCreateElement(document, namespaceManager, x.Key);
                    node.InnerText = value;
                }
            });
        }

        private static void CreateReleaseNotes(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.ReleaseNotes != null && settings.ReleaseNotes.Count > 0)
            {
                var node = FindOrCreateElement(document, namespaceManager, "releaseNotes");
                node.AppendChild(document.CreateCDataSection(ToMultiLineString(settings.ReleaseNotes)));
            }
        }

        private static void CreateMinClientVersion(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.MinClientVersion != null)
            {
                var metadataNode = document.SelectSingleNode("//*[local-name()='metadata']");
                metadataNode.AddAttributeIfSpecified(settings.MinClientVersion, "minClientVersion");
            }
        }

        private static void AddComplexTypesToXmlDocument(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            CreateRepositoryElement(document, settings, namespaceManager);
            CreateLicenseElement(document, settings, namespaceManager);
            CreatePackageTypeElements(document, settings, namespaceManager);
            CreateDependencyElements(document, settings, namespaceManager);
            CreateFrameworkAssemblyElements(document, settings, namespaceManager);
            CreateReferenceElements(document, settings, namespaceManager);
            CreateContentFileElements(document, settings, namespaceManager);
            CreateFileElements(document, settings, namespaceManager);
        }

        private static void CreateFileElements(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.Files != null && settings.Files.Count > 0)
            {
                var filesPath = string.Format(CultureInfo.InvariantCulture,
                    "//*[local-name()='package']/*[local-name()='files']");
                var filesElement = document.SelectSingleNode(filesPath, namespaceManager);
                if (filesElement == null)
                {
                    var package = GetPackageElement(document);
                    filesElement = document.CreateAndAppendElement(package, "files");
                }

                filesElement.RemoveAll();
                foreach (var file in settings.Files)
                {
                    if (file.Source != null)
                    {
                        var fileElement = document.CreateAndAppendElement(filesElement, "file");
                        fileElement.AddAttributeIfSpecified(file.Source, "src");
                        fileElement.AddAttributeIfSpecified(file.Exclude, "exclude");
                        fileElement.AddAttributeIfSpecified(file.Target, "target");
                    }
                }
            }
        }

        private static void CreateContentFileElements(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.ContentFiles != null && settings.ContentFiles.Count > 0)
            {
                var contentFilesElement = FindOrCreateElement(document, namespaceManager, "contentFiles");

                contentFilesElement.RemoveAll();
                foreach (var contentFile in settings.ContentFiles)
                {
                    if (contentFile.Include != null)
                    {
                        var fileElement = document.CreateAndAppendElement(contentFilesElement, "files");
                        fileElement.AddAttributeIfSpecified(contentFile.Include, "include");
                        fileElement.AddAttributeIfSpecified(contentFile.Exclude, "exclude");
                        fileElement.AddAttributeIfSpecified(contentFile.BuildAction, "buildAction");
                        fileElement.AddAttributeIfSpecified(ToString(contentFile.CopyToOutput), "copyToOutput");
                        fileElement.AddAttributeIfSpecified(ToString(contentFile.Flatten), "flatten");
                    }
                }
            }
        }

        private static void CreateReferenceElements(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.References != null && settings.References.Count > 0)
            {
                var referencesElement = FindOrCreateElement(document, namespaceManager, "references");

                referencesElement.RemoveAll();
                if (settings.References.All(c => string.IsNullOrEmpty(c.TargetFramework)))
                {
                    foreach (var reference in settings.References)
                    {
                        AddReference(document, referencesElement, reference);
                    }
                }
                else
                {
                    foreach (var targetFrameworkReferences in settings.References.GroupBy(x => x.TargetFramework))
                    {
                        CreateReferenceGroup(document, referencesElement, targetFrameworkReferences);
                    }
                }
            }
        }

        private static void CreateDependencyElements(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.Dependencies != null && settings.Dependencies.Count > 0)
            {
                var dependenciesElement = FindOrCreateElement(document, namespaceManager, "dependencies");

                dependenciesElement.RemoveAll();
                if (settings.Dependencies.All(c => string.IsNullOrEmpty(c.TargetFramework)))
                {
                    foreach (var dependency in settings.Dependencies)
                    {
                        AddDependency(document, dependenciesElement, dependency);
                    }
                }
                else
                {
                    foreach (var targetFrameworkDependencies in settings.Dependencies.GroupBy(x => x.TargetFramework))
                    {
                        CreateDependencyGroup(document, dependenciesElement, targetFrameworkDependencies);
                    }
                }
            }
        }

        private static void CreateReferenceGroup(XmlDocument document, XmlNode referencesElement,
            IGrouping<string, NuSpecReference> targetFrameworkReferences)
        {
            var groupElement = document.CreateAndAppendElement(referencesElement, "group");
            if (!string.IsNullOrEmpty(targetFrameworkReferences.Key))
            {
                groupElement.AddAttributeIfSpecified(targetFrameworkReferences.Key, "targetFramework");
            }

            foreach (var reference in targetFrameworkReferences)
            {
                AddReference(document, groupElement, reference);
            }
        }

        private static void CreateDependencyGroup(XmlDocument document, XmlNode dependenciesElement,
            IGrouping<string, NuSpecDependency> targetFrameworkDependencies)
        {
            var groupElement = document.CreateAndAppendElement(dependenciesElement, "group");
            if (!string.IsNullOrEmpty(targetFrameworkDependencies.Key))
            {
                groupElement.AddAttributeIfSpecified(targetFrameworkDependencies.Key, "targetFramework");
            }

            foreach (var dependency in targetFrameworkDependencies)
            {
                AddDependency(document, groupElement, dependency);
            }
        }

        private static void CreateFrameworkAssemblyElements(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.FrameworkAssemblies != null && settings.FrameworkAssemblies.Count > 0)
            {
                var frameWorkAssembliesElement = FindOrCreateElement(document, namespaceManager, "frameworkAssemblies");

                frameWorkAssembliesElement.RemoveAll();
                foreach (var frameworkAssembly in settings.FrameworkAssemblies)
                {
                    if (frameworkAssembly.AssemblyName != null)
                    {
                        var fileElement = document.CreateAndAppendElement(frameWorkAssembliesElement, "frameworkAssembly");
                        fileElement.AddAttributeIfSpecified(frameworkAssembly.AssemblyName, "assemblyName");
                        fileElement.AddAttributeIfSpecified(frameworkAssembly.TargetFramework, "targetFramework");
                    }
                }
            }
        }

        private static void CreatePackageTypeElements(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.PackageTypes != null && settings.PackageTypes.Count > 0)
            {
                var packageTypesElement = FindOrCreateElement(document, namespaceManager, "packageTypes");

                packageTypesElement.RemoveAll();
                foreach (var packageType in settings.PackageTypes)
                {
                    if (packageType.Name != null)
                    {
                        var fileElement = document.CreateAndAppendElement(packageTypesElement, "packageType");
                        fileElement.AddAttributeIfSpecified(packageType.Name, "name");
                        fileElement.AddAttributeIfSpecified(packageType.Version, "version");
                    }
                }
            }
        }

        private static void AddDependency(XmlDocument document, XmlNode groupElement, NuSpecDependency dependency)
        {
            if (dependency.Id != null)
            {
                var fileElement = document.CreateAndAppendElement(groupElement, "dependency");
                fileElement.AddAttributeIfSpecified(dependency.Id, "id");
                fileElement.AddAttributeIfSpecified(dependency.Version, "version");
                fileElement.AddAttributeIfSpecified(ToCommaSeparatedString(dependency.Include), "include");
                fileElement.AddAttributeIfSpecified(ToCommaSeparatedString(dependency.Exclude), "exclude");
            }
        }

        private static void AddReference(XmlDocument document, XmlNode groupElement, NuSpecReference reference)
        {
            if (reference.File != null)
            {
                var fileElement = document.CreateAndAppendElement(groupElement, "reference");
                fileElement.AddAttributeIfSpecified(reference.File, "file");
            }
        }

        private static void CreateLicenseElement(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.License?.Type != null)
            {
                var licenseNode = FindOrCreateElement(document, namespaceManager, "license");
                licenseNode.AddAttributeIfSpecified(settings.License.Type, "type");
                licenseNode.AddAttributeIfSpecified(settings.License.Version, "version");
                licenseNode.InnerText = settings.License.Value;
            }
        }

        private static void CreateRepositoryElement(XmlDocument document, NuGetPackSettings settings,
            XmlNamespaceManager namespaceManager)
        {
            if (settings.Repository != null)
            {
                var repositoryNode = FindOrCreateElement(document, namespaceManager, "repository");
                repositoryNode.AddAttributeIfSpecified(settings.Repository.Type, "type");
                repositoryNode.AddAttributeIfSpecified(settings.Repository.Url, "url");
                repositoryNode.AddAttributeIfSpecified(settings.Repository.Commit, "commit");
                repositoryNode.AddAttributeIfSpecified(settings.Repository.Branch, "branch");
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

        private static string ToString(bool? value)
        {
            return value?.ToString().ToLowerInvariant();
        }

        private static string ToCommaSeparatedString(ICollection<string> values)
        {
            return values != null && values.Count != 0
                ? string.Join(",", values)
                : null;
        }

        private static string ToMultiLineString(ICollection<string> values)
        {
            return values != null && values.Count != 0
                ? string.Join("\r\n", values).NormalizeLineEndings()
                : null;
        }

        private static string ToSpaceSeparatedString(ICollection<string> values)
        {
            return values != null && values.Count != 0
                ? string.Join(" ", values.Select(x => x.Replace(" ", "-")))
                : null;
        }
    }
}