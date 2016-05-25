namespace Cake.Common.Tools.DotNetCore.Run
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreRunner" />.
    /// </summary>
    public sealed class DotNetCoreRunSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Gets or sets a specific framework to compile.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets the configuration under which to build.
        /// </summary>
        public string Configuration { get; set; }
    }
}
