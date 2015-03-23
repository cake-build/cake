using System;
using Cake.Core.IO;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// Represents target platform option
    /// Command line option: [/targetplatform:version[,platformdir] | /v1 | /v1.1 | /v2 | /v4]
    /// </summary>
    /// <example> /targetPlatform:v4,"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1" </example>
    public sealed class TargetPlatform
    {
        private readonly DirectoryPath _path;
        private readonly TargetPlatformVersion _platform;

        /// <summary>
        /// Initializes a new instance of the TargetPlatform class.
        /// </summary>
        /// <param name="platform">.NET Framework target version </param>
        /// <param name="path">This is a directory where mscorlib.dll can be found</param>
        public TargetPlatform(TargetPlatformVersion platform, DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _platform = platform;
            _path = path;
        }

        /// <summary>
        /// Initializes a new instance of the TargetPlatform class.
        /// </summary>
        /// <param name="platform">.NET Framework target version </param>
        public TargetPlatform(TargetPlatformVersion platform)
        {
            _platform = platform;
        }

        /// <summary>
        /// Gets .NET Framework target version
        /// </summary>
        public TargetPlatformVersion Platform
        {
            get { return _platform; }
        }

        /// <summary>
        /// Gets a directory where mscorlib.dll can be found
        /// </summary>
        public DirectoryPath Path
        {
            get { return _path; }
        }
    }
}