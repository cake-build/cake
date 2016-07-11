// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.AppVeyor.Data
{
    /// <summary>
    /// Provides AppVeyor tag information for a current build.
    /// </summary>
    public sealed class AppVeyorTagInfo : AppVeyorInfo
    {
        /// <summary>
        /// Gets a value indicating whether build was started by pushed tag.
        /// </summary>
        /// <value>
        ///   <c>true</c> if build was started by pushed tag; otherwise, <c>false</c>.
        /// </value>
        public bool IsTag
        {
            get { return GetEnvironmentBoolean("APPVEYOR_REPO_TAG"); }
        }

        /// <summary>
        /// Gets the name for builds started by tag; otherwise this variable is undefined.
        /// </summary>
        /// <value>
        ///   The name of the tag.
        /// </value>
        public string Name
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_TAG_NAME"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorTagInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorTagInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}
