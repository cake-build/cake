// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bamboo.Data
{
    /// <summary>
    /// Provides Bamboo repository information for a current build.
    /// </summary>
    public sealed class BambooRepositoryInfo : BambooInfo
    {
        private readonly BambooCommitInfo _commitProvider;

        /// <summary>
        /// Gets the revision control system.
        /// <list type="bullet">
        ///   <item>
        ///     <description>Subversion</description>
        ///   </item>
        ///   <item>
        ///     <description>CVS</description>
        ///   </item>
        ///   <item>
        ///     <description>Perforce</description>
        ///   </item>
        ///   <item>
        ///     <description>Git</description>
        ///   </item>
        ///   <item>
        ///     <description>Mercurial</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <value>
        ///   The revision control system.
        /// </value>
        public string Scm
        {
            get { return GetEnvironmentString("bamboo_planRepository_type"); }
        }

        /// <summary>
        /// Gets the repository name as named in Bamboo
        /// </summary>
        /// <value>
        ///   The bamboo repository name.
        /// </value>
        public string Name
        {
            get { return GetEnvironmentString("bamboo_repository_name"); }
        }

        /// <summary>
        /// Gets the build branch.
        /// </summary>
        /// <value>
        ///   The build branch.
        /// </value>
        public string Branch
        {
            get { return GetEnvironmentString("bamboo_planRepository_branch"); }
        }

        /// <summary>
        /// Gets the commit information for the build.
        /// </summary>
        /// <value>
        ///   The commit information for the build.
        /// </value>
        public BambooCommitInfo Commit
        {
            get { return _commitProvider; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BambooRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BambooRepositoryInfo(ICakeEnvironment environment)
            : base(environment)
        {
            _commitProvider = new BambooCommitInfo(environment);
        }
    }
}
