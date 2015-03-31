using System;
using Cake.Core.IO;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// Represents a target platform.
    /// </summary>
    public sealed class TargetPlatform
    {
        private readonly DirectoryPath _path;
        private readonly TargetPlatformVersion _platform;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetPlatform"/> class.
        /// </summary>
        /// <param name="platform">The .NET framework target version.</param>
        public TargetPlatform(TargetPlatformVersion platform)
        {
            _platform = platform;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetPlatform"/> class.
        /// </summary>
        /// <param name="platform">The .NET framework target version.</param>
        /// <param name="path">The directory where <c>mscorlib.dll</c> can be found.</param>
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
        /// Gets the .NET framework target version.
        /// </summary>
        public TargetPlatformVersion Platform
        {
            get { return _platform; }
        }

        /// <summary>
        /// Gets the directory where <c>mscorlib.dll</c> can be found.
        /// </summary>
        public DirectoryPath Path
        {
            get { return _path; }
        }
    }
}