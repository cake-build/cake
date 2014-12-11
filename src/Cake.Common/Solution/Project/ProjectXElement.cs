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
        internal const string Import  = "{" + XmlNamespace + "}Import";

        /// <summary>
        /// Namespace compile element
        /// </summary>
        internal const string Compile  = "{" + XmlNamespace + "}Compile";

        /// <summary>
        /// Namespace property group element
        /// </summary>
        internal const string PropertyGroup  = "{" + XmlNamespace + "}PropertyGroup";

        /// <summary>
        /// Namespace root namespace element
        /// </summary>
        internal const string RootNamespace  = "{" + XmlNamespace + "}RootNamespace";

        /// <summary>
        /// Namespace output type element
        /// </summary>
        internal const string OutputType  = "{" + XmlNamespace + "}OutputType";

        /// <summary>
        /// Namespace assembly name element
        /// </summary>
        internal const string AssemblyName  = "{" + XmlNamespace + "}AssemblyName";

        /// <summary>
        /// Namespace target framework version element
        /// </summary>
        internal const string TargetFrameworkVersion  = "{" + XmlNamespace + "}TargetFrameworkVersion";

        /// <summary>
        /// Namespace configuration element
        /// </summary>
        internal const string Configuration   = "{" + XmlNamespace + "}Configuration";

        /// <summary>
        /// Namespace platform element
        /// </summary>
        internal const string Platform    = "{" + XmlNamespace + "}Platform";

        /// <summary>
        /// Namespace project guid element
        /// </summary>
        internal const string ProjectGuid    = "{" + XmlNamespace + "}ProjectGuid";

        /// <summary>
        /// Namespace bootstrapper package element
        /// </summary>
        internal const string BootstrapperPackage    = "{" + XmlNamespace + "}BootstrapperPackage";

        /// <summary>
        /// Namespace project reference element
        /// </summary>
        internal const string ProjectReference    = "{" + XmlNamespace + "}ProjectReference";

        /// <summary>
        /// Namespace service element
        /// </summary>
        internal const string Service    = "{" + XmlNamespace + "}Service";
    }
}