namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Represents a MSBuild tool version.
    /// </summary>
    public enum MSBuildToolVersion : byte
    {
        /// <summary>
        /// The highest available MSBuild tool version.
        /// </summary>
        Default = 0,

        /// <summary>
        /// .NET 2.0
        /// </summary>
        NET20 = 1,
        
        /// <summary>
        /// .NET 3.0
        /// </summary>
        NET30 = 1,
        
        /// <summary>
        /// Visual Studio 2005
        /// </summary>
        VS2005 = 1,
        
        /// <summary>
        /// .NET 3.5
        /// </summary>
        NET35 = 2,
        
        /// <summary>
        /// Visual Studio 2008
        /// </summary>
        VS2008 = 2,
        
        /// <summary>
        /// .NET 4.0
        /// </summary>
        NET40 = 3,
        
        /// <summary>
        /// .NET 4.5
        /// </summary>
        NET45 = 3,
        
        /// <summary>
        /// Visual Studio 2010
        /// </summary>
        VS2010 = 3,
        
        /// <summary>
        /// Visual Studio 2011
        /// </summary>
        VS2011 = 3,
        
        /// <summary>
        /// Visual Studio 2012
        /// </summary>
        VS2012 = 3,
        
        /// <summary>
        /// .NET 4.5.1
        /// </summary>
        NET451 = 4,
        
        /// <summary>
        /// .NET 4.5.2
        /// </summary>
        NET452 = 4,

        /// <summary>
        /// Visual Studio 2013
        /// </summary>
        VS2013 = 4,
    }
}
