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
        // ReSharper disable once InconsistentNaming
        x86 = 1,
        
        /// <summary>
        /// x64
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x64 = 2
    }
}
