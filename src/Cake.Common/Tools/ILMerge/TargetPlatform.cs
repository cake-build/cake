using System;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    ///     Represents target platform option
    ///     Command line option: /targetplatform:version,platformdirectory
    /// </summary>
    /// <example> /targetPlatform:"v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1" </example>
    public class TargetPlatform
    {
        /// <summary>
        ///     Initializes a new instance of the TargetPlatform class.
        /// </summary>
        /// <param name="platform"> .NET Framework target version </param>
        /// <param name="dir"> This is a directory where mscorlib.dll can be found</param>
        public TargetPlatform(TargetPlatformVersion platform, string dir)
        {
            Platform = platform;
            Dir = dir;
        }

        /// <summary>
        ///     Gets or sets .NET Framework target version
        /// </summary>
        public TargetPlatformVersion Platform { get; set; }

        /// <summary>
        ///     Gets or sets a directory where mscorlib.dll can be found
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        ///     Command line option value
        /// </summary>
        /// <returns>Command line option string</returns>
        public string CommandLineValue()
        {
            return string.Format("{0},{1}", GetTargetPlatformVersionString(Platform), Dir);
        }

        private static string GetTargetPlatformVersionString(TargetPlatformVersion version)
        {
            switch (version)
            {
                case TargetPlatformVersion.v1:
                    return "v1";
                case TargetPlatformVersion.v11:
                    return "v1.1";
                case TargetPlatformVersion.v2:
                    return "v2";
                case TargetPlatformVersion.v4:
                    return "v4";
                default:
                    throw new NotSupportedException("The provided ILMerge target platform is not valid.");
            }
        }
    }
}