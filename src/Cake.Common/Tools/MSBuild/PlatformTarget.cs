namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Represents a MSBuild platform target.
    /// </summary>
    public enum PlatformTarget
    {
        /// <summary>
        /// MSIL (AnyCPU).
        /// </summary>
        MSIL = 0,
        
        /// <summary>
        /// x86
        /// </summary>
        x86 = 1,
        
        /// <summary>
        /// x64
        /// </summary>
        x64 = 2
    }
}
