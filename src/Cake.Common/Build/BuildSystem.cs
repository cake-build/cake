// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.Bamboo;
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
        private readonly IAppVeyorProvider _appVeyorProvider;
        private readonly ITeamCityProvider _teamCityProvider;
        private readonly IMyGetProvider _myGetProvider;
        private readonly IBambooProvider _bambooProvider;
        private readonly IContinuaCIProvider _continuaCIProvider;
        private readonly IJenkinsProvider _jenkinsProvider;
        private readonly IBitriseProvider _bitriseProvider;
        private readonly ITravisCIProvider _travisCIProvider;

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
        public BuildSystem(IAppVeyorProvider appVeyorProvider, ITeamCityProvider teamCityProvider, IMyGetProvider myGetProvider, IBambooProvider bambooProvider, IContinuaCIProvider continuaCIProvider, IJenkinsProvider jenkinsProvider, IBitriseProvider bitriseProvider, ITravisCIProvider travisCIProvider)
        {
            if (appVeyorProvider == null)
            {
                throw new ArgumentNullException("appVeyorProvider");
            }
            if (teamCityProvider == null)
            {
                throw new ArgumentNullException("teamCityProvider");
            }
            if (myGetProvider == null)
            {
                throw new ArgumentNullException("myGetProvider");
            }
            if (bambooProvider == null)
            {
                throw new ArgumentNullException("bambooProvider");
            }
            if (continuaCIProvider == null)
            {
                throw new ArgumentNullException("continuaCIProvider");
            }
            if (jenkinsProvider == null)
            {
                throw new ArgumentNullException("jenkinsProvider");
            }
            if (bitriseProvider == null)
            {
                throw new ArgumentNullException("bitriseProvider");
            }
            if (travisCIProvider == null)
            {
                throw new ArgumentNullException("travisCIProvider");
            }

            _appVeyorProvider = appVeyorProvider;
            _teamCityProvider = teamCityProvider;
            _myGetProvider = myGetProvider;
            _bambooProvider = bambooProvider;
            _continuaCIProvider = continuaCIProvider;
            _jenkinsProvider = jenkinsProvider;
            _bitriseProvider = bitriseProvider;
            _travisCIProvider = travisCIProvider;
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
        public bool IsRunningOnAppVeyor
        {
            get { return _appVeyorProvider.IsRunningOnAppVeyor; }
        }

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
        public IAppVeyorProvider AppVeyor
        {
            get { return _appVeyorProvider; }
        }

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
        public bool IsRunningOnTeamCity
        {
            get { return _teamCityProvider.IsRunningOnTeamCity; }
        }

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
        public ITeamCityProvider TeamCity
        {
            get { return _teamCityProvider; }
        }

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
        public bool IsRunningOnMyGet
        {
            get { return _myGetProvider.IsRunningOnMyGet; }
        }

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
        public IMyGetProvider MyGet
        {
            get { return _myGetProvider; }
        }

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
        public bool IsRunningOnBamboo
        {
            get { return _bambooProvider.IsRunningOnBamboo; }
        }

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
        public IBambooProvider Bamboo
        {
            get { return _bambooProvider; }
        }

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
        public bool IsRunningOnContinuaCI
        {
            get { return _continuaCIProvider.IsRunningOnContinuaCI; }
        }

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
        public IContinuaCIProvider ContinuaCI
        {
            get { return _continuaCIProvider; }
        }

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
        public bool IsRunningOnJenkins
        {
            get { return _jenkinsProvider.IsRunningOnJenkins; }
        }

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
        public IJenkinsProvider Jenkins
        {
            get { return _jenkinsProvider; }
        }

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
        public bool IsRunningOnBitrise
        {
            get { return _bitriseProvider.IsRunningOnBitrise; }
        }

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
        public IBitriseProvider Bitrise
        {
            get { return _bitriseProvider; }
        }

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
        public bool IsRunningOnTravisCI
        {
            get { return _travisCIProvider.IsRunningOnTravisCI; }
        }

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
        public ITravisCIProvider TravisCI
        {
            get { return _travisCIProvider; }
        }

        /// <summary>
        /// Gets a value indicating whether the current build is local build.
        /// </summary>
        /// <example>
        /// <code>
        /// // Get a flag telling us if this is a local build or not.
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
        public bool IsLocalBuild
        {
            get { return !(IsRunningOnAppVeyor || IsRunningOnTeamCity || IsRunningOnMyGet || IsRunningOnBamboo || IsRunningOnContinuaCI || IsRunningOnJenkins || IsRunningOnBitrise || IsRunningOnTravisCI); }
        }
    }
}
