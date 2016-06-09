// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// MSBuild Project Xml Element XNames
    /// </summary>
    internal static class ProjectXElement
    {
        private const string XmlNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        /// <summary>
        /// Project root element
        /// </summary>
        internal const string Project = "{" + XmlNamespace + "}Project";

        /// <summary>
        /// Item group element
        /// </summary>
        internal const string ItemGroup = "{" + XmlNamespace + "}ItemGroup";

        /// <summary>
        /// Assembly reference element
        /// </summary>
        internal const string Reference = "{" + XmlNamespace + "}Reference";

        /// <summary>
        /// Namespace import element
        /// </summary>
        internal const string Import = "{" + XmlNamespace + "}Import";

        /// <summary>
        /// Namespace compile element
        /// </summary>
        internal const string Compile = "{" + XmlNamespace + "}Compile";

        /// <summary>
        /// Namespace property group element
        /// </summary>
        internal const string PropertyGroup = "{" + XmlNamespace + "}PropertyGroup";

        /// <summary>
        /// Namespace root namespace element
        /// </summary>
        internal const string RootNamespace = "{" + XmlNamespace + "}RootNamespace";

        /// <summary>
        /// Namespace output type element
        /// </summary>
        internal const string OutputType = "{" + XmlNamespace + "}OutputType";

        /// <summary>
        /// Namespace assembly name element
        /// </summary>
        internal const string AssemblyName = "{" + XmlNamespace + "}AssemblyName";

        /// <summary>
        /// Gets the namespace for the target framework version element.
        /// </summary>
        internal const string TargetFrameworkVersion = "{" + XmlNamespace + "}TargetFrameworkVersion";

        /// <summary>
        /// Gets the namespace for the target framework version element.
        /// </summary>
        internal const string TargetFrameworkProfile = "{" + XmlNamespace + "}TargetFrameworkProfile";

        /// <summary>
        /// Gets the namespace for the configuration element.
        /// </summary>
        internal const string Configuration = "{" + XmlNamespace + "}Configuration";

        /// <summary>
        ///  Gets the namespace for the platform element.
        /// </summary>
        internal const string Platform = "{" + XmlNamespace + "}Platform";

        /// <summary>
        /// Gets the namespace for the project GUID.
        /// </summary>
        internal const string ProjectGuid = "{" + XmlNamespace + "}ProjectGuid";

        /// <summary>
        /// Gets the namespace for the bootstrapper package element.
        /// </summary>
        internal const string BootstrapperPackage = "{" + XmlNamespace + "}BootstrapperPackage";

        /// <summary>
        /// Gets the namespace for the project reference element.
        /// </summary>
        internal const string ProjectReference = "{" + XmlNamespace + "}ProjectReference";

        /// <summary>
        /// Gets the namespace for the service element.
        /// </summary>
        internal const string Service = "{" + XmlNamespace + "}Service";
    }
}
