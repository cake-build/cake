﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// Provide GitHub Actions workflow information for a current build.
    /// </summary>
    public sealed class GitHubActionsWorkflowInfo : GitHubActionsInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubActionsWorkflowInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitHubActionsWorkflowInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the unique identifier of the action.
        /// </summary>
        /// <value>
        /// The unique identifier of the action.
        /// </value>
        public string Action => GetEnvironmentString("GITHUB_ACTION");

        /// <summary>
        /// Gets the name of the person or app that initiated the workflow.
        /// </summary>
        /// <value>
        /// The name of the person or app that initiated the workflow.
        /// </value>
        public string Actor => GetEnvironmentString("GITHUB_ACTOR");

        /// <summary>
        /// Gets the API URL.
        /// </summary>
        /// <value>
        /// The API URL. For example: https://api.github.com.
        /// </value>
        public string ApiUrl => GetEnvironmentString("GITHUB_API_URL");

        /// <summary>
        /// Gets the branch of the base repository.
        /// </summary>
        /// <value>
        /// The branch of the base repository. Only set for forked repositories.
        /// </value>
        public string BaseRef => GetEnvironmentString("GITHUB_BASE_REF");

        /// <summary>
        /// Gets the name of the webhook event that triggered the workflow.
        /// </summary>
        /// <value>
        /// The name of the webhook event that triggered the workflow.
        /// </value>
        public string EventName => GetEnvironmentString("GITHUB_EVENT_NAME");

        /// <summary>
        /// Gets the path of the file with the complete webhook event payload.
        /// </summary>
        /// <value>
        /// The path of the file with the complete webhook event payload.
        /// </value>
        public string EventPath => GetEnvironmentString("GITHUB_EVENT_PATH");

        /// <summary>
        /// Gets the GraphQL API URL.
        /// </summary>
        /// <value>
        /// The GraphQL API URL. For example: https://api.github.com/graphql.
        /// </value>
        public string GraphQLUrl => GetEnvironmentString("GITHUB_GRAPHQL_URL");

        /// <summary>
        /// Gets the branch of the head repository.
        /// </summary>
        /// <value>
        /// The branch of the head repository. Only set for forked repositories.
        /// </value>
        public string HeadRef => GetEnvironmentString("GITHUB_HEAD_REF");

        /// <summary>
        /// Gets the job name.
        /// </summary>
        /// <value>
        /// The job name.
        /// </value>
        public string Job => GetEnvironmentString("GITHUB_JOB");

        /// <summary>
        /// Gets the branch or tag ref that triggered the workflow.
        /// </summary>
        /// <value>
        /// The branch or tag ref that triggered the workflow.
        /// </value>
        public string Ref => GetEnvironmentString("GITHUB_REF");

        /// <summary>
        /// Gets the owner and repository name.
        /// </summary>
        /// <value>
        /// The owner and repository name.
        /// </value>
        public string Repository => GetEnvironmentString("GITHUB_REPOSITORY");

        /// <summary>
        /// Gets the repository owner.
        /// </summary>
        /// <value>
        /// The repository owner.
        /// </value>
        public string RepositoryOwner => GetEnvironmentString("GITHUB_REPOSITORY_OWNER");

        /// <summary>
        /// Gets the unique number for each run within the repository.
        /// </summary>
        /// <value>
        /// The unique number for each run within the repository.
        /// </value>
        public string RunId => GetEnvironmentString("GITHUB_RUN_ID");

        /// <summary>
        /// Gets the unique number for each run of a particular workflow in the repository.
        /// </summary>
        /// <value>
        /// The unique number for each run of a particular workflow in the repository.
        /// </value>
        public int RunNumber => GetEnvironmentInteger("GITHUB_RUN_NUMBER");

        /// <summary>
        /// Gets the URL of the GitHub server.
        /// </summary>
        /// <value>
        /// The URL of the GitHub server. For example: https://github.com.
        /// </value>
        public string ServerUrl => GetEnvironmentString("GITHUB_SERVER_URL");

        /// <summary>
        /// Gets the commit SHA that triggered the workflow.
        /// </summary>
        /// <value>
        /// The commit SHA that triggered the workflow.
        /// </value>
        public string Sha => GetEnvironmentString("GITHUB_SHA");

        /// <summary>
        /// Gets the name of the workflow.
        /// </summary>
        /// <value>
        /// The name of the workflow.
        /// </value>
        public string Workflow => GetEnvironmentString("GITHUB_WORKFLOW");

        /// <summary>
        /// Gets the GitHub workspace directory path.
        /// </summary>
        /// <value>
        /// The GitHub workspace directory path.
        /// </value>
        public string Workspace => GetEnvironmentString("GITHUB_WORKSPACE");
    }
}
