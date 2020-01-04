﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// Provide GitHub Actions workflow information for a current build
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
        /// Gets the branch of the head repository.
        /// </summary>
        /// <value>
        /// The branch of the head repository. Only set for forked repositories.
        /// </value>
        public string HeadRef => GetEnvironmentString("GITHUB_HEAD_REF");

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
