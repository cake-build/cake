namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains settings for specifying a MSBuild logger.
    /// </summary>
    public sealed class MSBuildLogger
    {
        /// <summary>
        /// Gets or sets the assembly containing the logger. Should match the format {AssemblyName[,StrongName] | AssemblyFile}
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the class implementing the logger. Should match the format [PartialOrFullNamespace.]LoggerClassName
        /// If the assembly contains only one logger, class does not need to be specified.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the parameters to be passed to the logger.
        /// </summary>
        public string Parameters { get; set; }
    }
}