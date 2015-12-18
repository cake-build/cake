using Cake.Core;

namespace Cake.Common.Build.Bamboo.Data
{
    /// <summary>
    /// Provides Bamboo tag information for a current build.
    /// </summary>
    public sealed class BambooCustomBuildInfo : BambooInfo
    {
        /// <summary>
        /// Gets a value indicating whether build was started by pushed tag.
        /// </summary>
        /// <value>
        ///   <c>true</c> if build was started by pushed tag; otherwise, <c>false</c>.
        /// </value>
        public bool IsCustomBuild
        {
            get { return !string.IsNullOrWhiteSpace(GetEnvironmentString("bamboo_customRevision")); }
        }

        /// <summary>
        /// Gets the name for builds started by tag; otherwise this variable is undefined.
        /// </summary>
        /// <value>
        ///   The name of the tag.
        /// </value>
        public string RevisonName
        {
            get { return GetEnvironmentString("bamboo_customRevision"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BambooCustomBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BambooCustomBuildInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}