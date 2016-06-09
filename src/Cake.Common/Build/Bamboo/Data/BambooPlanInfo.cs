// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bamboo.Data
{
    /// <summary>
    /// Provides Bamboo project information for a current build.
    /// </summary>
    public sealed class BambooPlanInfo : BambooInfo
    {
        /// <summary>
        /// Gets the Bamboo Plan Name
        /// </summary>
        /// <value>
        ///   The Bamboo Plan Name.
        /// </value>
        public string PlanName
        {
            get { return GetEnvironmentString("bamboo_planName"); }
        }

        /// <summary>
        /// Gets the Bamboo short Plan Name
        /// </summary>
        /// <value>
        ///   The Bamboo Plan Name in it's short form.
        /// </value>
        public string ShortPlanName
        {
            get { return GetEnvironmentString("bamboo_shortPlanName"); }
        }

        /// <summary>
        /// Gets the key of the current plan, in the form PROJECT-PLAN, e.g. BAM-MAIN
        /// </summary>
        /// <value>
        /// The project name.
        /// </value>
        public string PlanKey
        {
            get { return GetEnvironmentString("bamboo_planKey"); }
        }

        /// <summary>
        /// Gets the Bamboo short Plan Key.
        /// </summary>
        /// <value>
        ///   The Bamboo Plan Key in it's hort form.
        /// </value>
        public string ShortPlanKey
        {
            get { return GetEnvironmentString("bamboo_shortPlanKey"); }
        }

        /// <summary>
        /// Gets the Bamboo short job key.
        /// </summary>
        /// <value>
        ///   The Bamboo job key in it's short form.
        /// </value>
        public string ShortJobKey
        {
            get { return GetEnvironmentString("bamboo_shortJobKey"); }
        }

        /// <summary>
        /// Gets the Bamboo short Job Name.
        /// </summary>
        /// <value>
        ///   The Bamboo Job Name in it's short form.
        /// </value>
        public string ShortJobName
        {
            get { return GetEnvironmentString("bamboo_shortJobName"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BambooPlanInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BambooPlanInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}
