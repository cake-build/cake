// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.AppVeyor.Data
{
    /// <summary>
    /// Provides AppVeyor environment information for a current build.
    /// </summary>
    public sealed class AppVeyorEnvironmentInfo : AppVeyorInfo
    {
        /// <summary>
        /// Gets the AppVeyor build agent API URL.
        /// </summary>
        /// <value>
        ///   The AppVeyor build agent API URL.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"API URL:{0},
        ///         BuildSystem.AppVeyor.Environment.ApiUrl
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"API URL:{0},
        ///         AppVeyor.Environment.ApiUrl
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public string ApiUrl => GetEnvironmentString("APPVEYOR_API_URL");

        /// <summary>
        /// Gets the AppVeyor unique job ID.
        /// </summary>
        /// <value>
        ///  The AppVeyor unique job ID.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Job Id:{0},
        ///         BuildSystem.AppVeyor.Environment.JobId
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Job Id:{0},
        ///         AppVeyor.Environment.JobId
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public string JobId => GetEnvironmentString("APPVEYOR_JOB_ID");

        /// <summary>
        /// Gets the AppVeyor Job Name.
        /// </summary>
        /// <value>
        ///   The AppVeyor Job Name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Job Name:{0},
        ///         BuildSystem.AppVeyor.Environment.JobName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Job Name:{0},
        ///         AppVeyor.Environment.JobName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public string JobName => GetEnvironmentString("APPVEYOR_JOB_NAME");

        /// <summary>
        /// Gets a value indicating whether the build runs by scheduler.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the build runs by scheduler; otherwise, <c>false</c>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Scheduled Build:{0},
        ///         BuildSystem.AppVeyor.Environment.ScheduledBuild
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Scheduled Build:{0},
        ///         AppVeyor.Environment.ScheduledBuild
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public bool ScheduledBuild => GetEnvironmentBoolean("APPVEYOR_SCHEDULED_BUILD");

        /// <summary>
        /// Gets the platform name set on build tab of project settings (or through platform parameter in appveyor.yml).
        /// </summary>
        /// <value>
        ///   The platform name set on build tab of project settings (or through platform parameter in appveyor.yml).
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Platform:{0},
        ///         BuildSystem.AppVeyor.Environment.Platform
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Platform:{0},
        ///         AppVeyor.Environment.Platform
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public string Platform => GetEnvironmentString("PLATFORM");

        /// <summary>
        /// Gets the configuration name set on build tab of project settings (or through configuration parameter in appveyor.yml).
        /// </summary>
        /// <value>
        ///   The configuration name set on build tab of project settings (or through configuration parameter in appveyor.yml).
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Configuration:{0},
        ///         BuildSystem.AppVeyor.Environment.Configuration
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Configuration:{0},
        ///         AppVeyor.Environment.Configuration
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public string Configuration => GetEnvironmentString("CONFIGURATION");

        /// <summary>
        /// Gets AppVeyor project information.
        /// </summary>
        /// <value>
        ///   The AppVeyor project information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Project:
        ///         Id: {0}
        ///         Name: {1}
        ///         Slug: {2}",
        ///         BuildSystem.AppVeyor.Environment.Project.Id,
        ///         BuildSystem.AppVeyor.Environment.Project.Name,
        ///         BuildSystem.AppVeyor.Environment.Project.Slug
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// // via AppVeyor
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Project:
        ///         Id: {0}
        ///         Name: {1}
        ///         Slug: {2}",
        ///         AppVeyor.Environment.Project.Id,
        ///         AppVeyor.Environment.Project.Name,
        ///         AppVeyor.Environment.Project.Slug
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public AppVeyorProjectInfo Project { get; }

        /// <summary>
        /// Gets AppVeyor build information.
        /// </summary>
        /// <value>
        ///   The AppVeyor build information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Folder: {0}
        ///         Id: {1}
        ///         Number: {2}
        ///         Version: {3}",
        ///         BuildSystem.AppVeyor.Environment.Build.Folder,
        ///         BuildSystem.AppVeyor.Environment.Build.Id,
        ///         BuildSystem.AppVeyor.Environment.Build.Number,
        ///         BuildSystem.AppVeyor.Environment.Build.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Folder: {0}
        ///         Id: {1}
        ///         Number: {2}
        ///         Version: {3}",
        ///         AppVeyor.Environment.Build.Folder,
        ///         AppVeyor.Environment.Build.Id,
        ///         AppVeyor.Environment.Build.Number,
        ///         AppVeyor.Environment.Build.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public AppVeyorBuildInfo Build { get; }

        /// <summary>
        /// Gets AppVeyor pull request information.
        /// </summary>
        /// <value>
        ///   The AppVeyor pull request information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Number: {1}
        ///         Title: {2}",
        ///         BuildSystem.AppVeyor.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.AppVeyor.Environment.PullRequest.Number,
        ///         BuildSystem.AppVeyor.Environment.PullRequest.Title
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Number: {1}
        ///         Title: {2}",
        ///         AppVeyor.Environment.PullRequest.IsPullRequest,
        ///         AppVeyor.Environment.PullRequest.Number,
        ///         AppVeyor.Environment.PullRequest.Title
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public AppVeyorPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Gets AppVeyor repository information.
        /// </summary>
        /// <value>
        ///   The AppVeyor repository information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         Name: {1}
        ///         Provider: {2}
        ///         Scm: {3}",
        ///         BuildSystem.AppVeyor.Environment.Repository.Branch,
        ///         BuildSystem.AppVeyor.Environment.Repository.Name,
        ///         BuildSystem.AppVeyor.Environment.Repository.Provider,
        ///         BuildSystem.AppVeyor.Environment.Repository.Scm
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         Name: {1}
        ///         Provider: {2}
        ///         Scm: {3}",
        ///         AppVeyor.Environment.Repository.Branch,
        ///         AppVeyor.Environment.Repository.Name,
        ///         AppVeyor.Environment.Repository.Provider,
        ///         AppVeyor.Environment.Repository.Scm
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public AppVeyorRepositoryInfo Repository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Project = new AppVeyorProjectInfo(environment);
            Build = new AppVeyorBuildInfo(environment);
            PullRequest = new AppVeyorPullRequestInfo(environment);
            Repository = new AppVeyorRepositoryInfo(environment);
        }
    }
}