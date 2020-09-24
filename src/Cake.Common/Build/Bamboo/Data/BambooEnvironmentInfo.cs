// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Bamboo.Data
{
    /// <summary>
    /// Provides Bamboo environment information for a current build.
    /// </summary>
    public sealed class BambooEnvironmentInfo : BambooInfo
    {
        /// <summary>
        /// Gets Bamboo plan information.
        /// </summary>
        /// <value>
        ///   The Bamboo plan information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bamboo.IsRunningOnBamboo)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Plan Name: {0}
        ///         Short Plan Name: {1}
        ///         Plan Key: {2}
        ///         Short Plan Key: {3}
        ///         Short Job Key: {4}
        ///         Short Job Name: {5}",
        ///         BuildSystem.Bamboo.Environment.Plan.PlanName,
        ///         BuildSystem.Bamboo.Environment.Plan.ShortPlanName,
        ///         BuildSystem.Bamboo.Environment.Plan.PlanKey,
        ///         BuildSystem.Bamboo.Environment.Plan.ShortPlanKey,
        ///         BuildSystem.Bamboo.Environment.Plan.ShortJobKey,
        ///         BuildSystem.Bamboo.Environment.Plan.ShortJobName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bamboo");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bamboo.</para>
        /// <example>
        /// <code>
        /// if (Bamboo.IsRunningOnBamboo)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Plan Name: {0}
        ///         Short Plan Name: {1}
        ///         Plan Key: {2}
        ///         Short Plan Key: {3}
        ///         Short Job Key: {4}
        ///         Short Job Name: {5}",
        ///         Bamboo.Environment.Plan.PlanName,
        ///         Bamboo.Environment.Plan.ShortPlanName,
        ///         Bamboo.Environment.Plan.PlanKey,
        ///         Bamboo.Environment.Plan.ShortPlanKey,
        ///         Bamboo.Environment.Plan.ShortJobKey,
        ///         Bamboo.Environment.Plan.ShortJobName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bamboo");
        /// }
        /// </code>
        /// </example>
        public BambooPlanInfo Plan { get; }

        /// <summary>
        /// Gets Bamboo build information.
        /// </summary>
        /// <value>
        ///   The Bamboo build information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bamboo.IsRunningOnBamboo)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Folder: {0}
        ///         Number: {1}
        ///         Build Key: {2}
        ///         Result Key: {3}
        ///         Results Url: {4}
        ///         Build Timestamp: {5}
        ///         Is Custom: {6}
        ///         Revision Name: {7}",
        ///         BuildSystem.Bamboo.Environment.Build.Folder,
        ///         BuildSystem.Bamboo.Environment.Build.Number,
        ///         BuildSystem.Bamboo.Environment.Build.BuildKey,
        ///         BuildSystem.Bamboo.Environment.Build.ResultKey,
        ///         BuildSystem.Bamboo.Environment.Build.ResultsUrl,
        ///         BuildSystem.Bamboo.Environment.Build.BuildTimestamp,
        ///         BuildSystem.Bamboo.Environment.Build.CustomBuild.IsCustomBuild,
        ///         BuildSystem.Bamboo.Environment.Build.CustomBuild.RevisionName);
        /// }
        /// else
        /// {
        ///     Information("Not running on Bamboo");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bamboo.</para>
        /// <example>
        /// <code>
        /// if (Bamboo.IsRunningOnBamboo)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Folder: {0}
        ///         Number: {1}
        ///         Build Key: {2}
        ///         Result Key: {3}
        ///         Results Url: {4}
        ///         Build Timestamp: {5}
        ///         Is Custom: {6}
        ///         Revision Name: {7}",
        ///         Bamboo.Environment.Build.Folder,
        ///         Bamboo.Environment.Build.Number,
        ///         Bamboo.Environment.Build.BuildKey,
        ///         Bamboo.Environment.Build.ResultKey,
        ///         Bamboo.Environment.Build.ResultsUrl,
        ///         Bamboo.Environment.Build.BuildTimestamp,
        ///         Bamboo.Environment.Build.CustomBuild.IsCustomBuild,
        ///         Bamboo.Environment.Build.CustomBuild.RevisionName);
        /// }
        /// else
        /// {
        ///     Information("Not running on Bamboo");
        /// }
        /// </code>
        /// </example>
        public BambooBuildInfo Build { get; }

        /// <summary>
        /// Gets Bamboo repository information.
        /// </summary>
        /// <value>
        ///   The Bamboo repository information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bamboo.IsRunningOnBamboo)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         Name: {1}
        ///         Repository Revision: {2}
        ///         Scm: {3}",
        ///         BuildSystem.Bamboo.Environment.Repository.Branch,
        ///         BuildSystem.Bamboo.Environment.Repository.Name,
        ///         BuildSystem.Bamboo.Environment.Repository.Commit.RepositoryRevision,
        ///         BuildSystem.Bamboo.Environment.Repository.Scm
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bamboo");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bamboo.</para>
        /// <example>
        /// <code>
        /// if (Bamboo.IsRunningOnBamboo)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         Name: {1}
        ///         Repository Revision: {2}
        ///         Scm: {3}",
        ///         Bamboo.Environment.Repository.Branch,
        ///         Bamboo.Environment.Repository.Name,
        ///         Bamboo.Environment.Repository.Commit.RepositoryRevision,
        ///         Bamboo.Environment.Repository.Scm
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bamboo");
        /// }
        /// </code>
        /// </example>
        public BambooRepositoryInfo Repository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BambooEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BambooEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Plan = new BambooPlanInfo(environment);
            Build = new BambooBuildInfo(environment);
            Repository = new BambooRepositoryInfo(environment);
        }
    }
}