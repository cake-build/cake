// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.Bamboo;
using Cake.Common.Build.BitbucketPipelines;
using Cake.Common.Build.Bitrise;
using Cake.Common.Build.ContinuaCI;
using Cake.Common.Build.Jenkins;
using Cake.Common.Build.MyGet;
using Cake.Common.Build.TeamCity;
using Cake.Common.Build.TravisCI;

namespace Cake.Common.Build
{
    /// <summary>
    /// Provides functionality for interacting with
    /// different build systems.
    /// </summary>
    public sealed class BuildSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildSystem" /> class.
        /// </summary>
        /// <param name="appVeyorProvider">The AppVeyor Provider.</param>
        /// <param name="teamCityProvider">The TeamCity Provider.</param>
        /// <param name="myGetProvider">The MyGet Provider.</param>
        /// <param name="bambooProvider">The Bamboo Provider.</param>
        /// <param name="continuaCIProvider">The Continua CI Provider.</param>
        /// <param name="jenkinsProvider">The Jenkins Provider.</param>
        /// <param name="bitriseProvider">The Bitrise Provider.</param>
        /// <param name="travisCIProvider">The Travis CI provider.</param>
        /// <param name="bitbucketPipelinesProvider">The Bitbucket Pipelines provider.</param>
        public BuildSystem(IAppVeyorProvider appVeyorProvider, ITeamCityProvider teamCityProvider, IMyGetProvider myGetProvider, IBambooProvider bambooProvider, IContinuaCIProvider continuaCIProvider, IJenkinsProvider jenkinsProvider, IBitriseProvider bitriseProvider, ITravisCIProvider travisCIProvider, IBitbucketPipelinesProvider bitbucketPipelinesProvider)
        {
            if (appVeyorProvider == null)
            {
                throw new ArgumentNullException(nameof(appVeyorProvider));
            }
            if (teamCityProvider == null)
            {
                throw new ArgumentNullException(nameof(teamCityProvider));
            }
            if (myGetProvider == null)
            {
                throw new ArgumentNullException(nameof(myGetProvider));
            }
            if (bambooProvider == null)
            {
                throw new ArgumentNullException(nameof(bambooProvider));
            }
            if (continuaCIProvider == null)
            {
                throw new ArgumentNullException(nameof(continuaCIProvider));
            }
            if (jenkinsProvider == null)
            {
                throw new ArgumentNullException(nameof(jenkinsProvider));
            }
            if (bitriseProvider == null)
            {
                throw new ArgumentNullException(nameof(bitriseProvider));
            }
            if (travisCIProvider == null)
            {
                throw new ArgumentNullException(nameof(travisCIProvider));
            }
            if (bitbucketPipelinesProvider == null)
            {
                throw new ArgumentNullException(nameof(bitbucketPipelinesProvider));
            }

            AppVeyor = appVeyorProvider;
            TeamCity = teamCityProvider;
            MyGet = myGetProvider;
            Bamboo = bambooProvider;
            ContinuaCI = continuaCIProvider;
            Jenkins = jenkinsProvider;
            Bitrise = bitriseProvider;
            TravisCI = travisCIProvider;
            BitbucketPipelines = bitbucketPipelinesProvider;
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on AppVeyor.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnAppVeyor)
        /// {
        ///     // Upload artifact to AppVeyor.
        ///     AppVeyor.UploadArtifact("./build/release_x86.zip");
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if the build currently is running on AppVeyor; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAppVeyor => AppVeyor.IsRunningOnAppVeyor;

        /// <summary>
        /// Gets the AppVeyor Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnAppVeyor)
        /// {
        ///     // Upload artifact to AppVeyor.
        ///     BuildSystem.AppVeyor.UploadArtifact("./build/release_x86.zip");
        /// }
        /// </code>
        /// </example>
        public IAppVeyorProvider AppVeyor { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on TeamCity.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnTeamCity)
        /// {
        ///     TeamCity.ProgressMessage("Doing an action...");
        ///     // Do action...
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if the build currently is running on TeamCity; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnTeamCity => TeamCity.IsRunningOnTeamCity;

        /// <summary>
        /// Gets the TeamCity Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnTeamCiy)
        /// {
        ///     // Set the build number.
        ///     BuildSystem.TeamCity.SetBuildNumber("1.2.3.4");
        /// }
        /// </code>
        /// </example>
        public ITeamCityProvider TeamCity { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on MyGet.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnMyGet)
        /// {
        ///     MyGet.BuildProblem("Something went wrong...");
        ///     // Do action...
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if the build currently is running on MyGet; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnMyGet => MyGet.IsRunningOnMyGet;

        /// <summary>
        /// Gets the MyGet Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnMyGet)
        /// {
        ///     // Set the build number.
        ///     BuildSystem.MyGet.SetBuildNumber("1.2.3.4");
        /// }
        /// </code>
        /// </example>
        public IMyGetProvider MyGet { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on Bamboo.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnBamboo)
        /// {
        ///     // Get the build number.
        ///     var buildNumber = BuildSystem.Bamboo.Number;
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if the build currently is running on Bamboo; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnBamboo => Bamboo.IsRunningOnBamboo;

        /// <summary>
        /// Gets the Bamboo Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnBamboo)
        /// {
        ///     //Get the Bamboo Plan Name
        ///     var planName = BuildSystem.Bamboo.Project.PlanName
        /// }
        /// </code>
        /// </example>
        public IBambooProvider Bamboo { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on Continua CI.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnContinuaCI)
        /// {
        ///     // Get the build version.
        ///     var buildVersion = BuildSystem.ContinuaCI.Environment.Build.Version;
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if the build currently is running on Continua CI; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnContinuaCI => ContinuaCI.IsRunningOnContinuaCI;

        /// <summary>
        /// Gets the Continua CI Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnContinuaCI)
        /// {
        ///     //Get the Continua CI Project Name
        ///     var projectName = BuildSystem.ContinuaCI.Environment.Project.Name;
        /// }
        /// </code>
        /// </example>
        public IContinuaCIProvider ContinuaCI { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is running on Jenkins.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnJenkins)
        /// {
        ///     // Get the build number.
        ///     var buildNumber = BuildSystem.Jenkins.Environment.Build.BuildNumber;
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if this instance is running on jenkins; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnJenkins => Jenkins.IsRunningOnJenkins;

        /// <summary>
        /// Gets the Jenkins Provider.
        /// </summary>
        /// <value>
        /// The jenkins.
        /// </value>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnJenkins)
        /// {
        ///     // Get the job name.
        ///     var jobName = BuildSystem.Jenkins.Environment.Build.JobName;
        /// }
        /// </code>
        /// </example>
        public IJenkinsProvider Jenkins { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is running on Bitrise.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnBitrise)
        /// {
        ///     // Get the build number.
        ///     var buildNumber = BuildSystem.Bitrise.Environment.Build.BuildNumber;
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if this instance is running on bitrise; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnBitrise => Bitrise.IsRunningOnBitrise;

        /// <summary>
        /// Gets the Bitrise Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnBitrise)
        /// {
        ///     // Get the provision profile url.
        ///     var buildNumber = BuildSystem.Bitrise.Environment.Provisioning.ProvisionUrl;
        /// }
        /// </code>
        /// </example>
        public IBitriseProvider Bitrise { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is running on Travis CI.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnTravisCI)
        /// {
        ///     // Get the build directory.
        ///     var buildDirectory = BuildSystem.TravisCI.Environment.Build.BuildDirectory;
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if this instance is running on Travis CI; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnTravisCI => TravisCI.IsRunningOnTravisCI;

        /// <summary>
        /// Gets the Travis CI provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnTravisCI)
        /// {
        ///     // Get the operating system name.
        ///     var osName = BuildSystem.TravisCI.Environment.Job.OSName;
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// The Travis CI.
        /// </value>
        public ITravisCIProvider TravisCI { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is running on Bitbucket Pipelines.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnBitbucketPipelines)
        /// {
        ///     // Get the build commit hash.
        ///     var commitHash = BuildSystem.BitbucketPipelines.Environment.Repository.Commit;
        /// }
        /// </code>
        /// </example>
        /// <value>
        /// <c>true</c> if this instance is running on Bitbucket Pipelines; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnBitbucketPipelines => BitbucketPipelines.IsRunningOnBitbucketPipelines;

        /// <summary>
        /// Gets the Bitbucket Pipelines Provider.
        /// </summary>
        /// <example>
        /// <code>
        /// if(BuildSystem.IsRunningOnBitbucketPipelines)
        /// {
        ///     // Get the URL friendly repo name.
        ///     var repoSlug = BuildSystem.BitbucketPipelines.Environment.Repository.RepoSlug;
        /// }
        /// </code>
        /// </example>
        public IBitbucketPipelinesProvider BitbucketPipelines { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is local build.
        /// </summary>
        /// <example>
        /// <code>
        /// // Gets a flag telling us if this is a local build or not.
        /// var isLocal = BuildSystem.IsLocalBuild;
        ///
        /// // Define a task that only runs locally.
        /// Task("LocalOnly")
        ///   .WithCriteria(isLocal)
        ///   .Does(() =>
        /// {
        /// });
        /// </code>
        /// </example>
        /// <value>
        ///   <c>true</c> if the current build is local build; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocalBuild => !(IsRunningOnAppVeyor || IsRunningOnTeamCity || IsRunningOnMyGet || IsRunningOnBamboo || IsRunningOnContinuaCI || IsRunningOnJenkins || IsRunningOnBitrise || IsRunningOnTravisCI || IsRunningOnBitbucketPipelines);
    }
}