// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

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
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Action: {0}", BuildSystem.GitHubActions.Environment.Workflow.Action);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Action: {0}", GitHubActions.Environment.Workflow.Action);
        /// }
        /// </code>
        /// </example>
        public string Action => GetEnvironmentString("GITHUB_ACTION");

        /// <summary>
        /// Gets the path where your action is located. You can use this path to access files located in the same repository as your action. This variable is only supported in composite run steps actions.
        /// </summary>
        /// <value>
        /// The path where your action is located. You can use this path to access files located in the same repository as your action. This variable is only supported in composite run steps actions.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ActionPath: {0}", BuildSystem.GitHubActions.Environment.Workflow.ActionPath);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ActionPath: {0}", GitHubActions.Environment.Workflow.ActionPath);
        /// }
        /// </code>
        /// </example>
        public DirectoryPath ActionPath => GetEnvironmentDirectoryPath("GITHUB_ACTION_PATH");

        /// <summary>
        /// Gets the owner and repository name of the action being executed.
        /// </summary>
        /// <value>
        /// The owner and repository name of the action being executed. For example, actions/checkout.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ActionRepository: {0}", BuildSystem.GitHubActions.Environment.Workflow.ActionRepository);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ActionRepository: {0}", GitHubActions.Environment.Workflow.ActionRepository);
        /// }
        /// </code>
        /// </example>
        public string ActionRepository => GetEnvironmentString("GITHUB_ACTION_REPOSITORY");

        /// <summary>
        /// Gets the name of the person or app that initiated the workflow.
        /// </summary>
        /// <value>
        /// The name of the person or app that initiated the workflow.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Actor: {0}", BuildSystem.GitHubActions.Environment.Workflow.Actor);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Actor: {0}", GitHubActions.Environment.Workflow.Actor);
        /// }
        /// </code>
        /// </example>
        public string Actor => GetEnvironmentString("GITHUB_ACTOR");

        /// <summary>
        /// Gets the account ID of the person or app that triggered the initial workflow run.
        /// </summary>
        /// <value>
        /// The account ID of the person or app that triggered the initial workflow run.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ActorId: {0}", BuildSystem.GitHubActions.Environment.Workflow.ActorId);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ActorId: {0}", GitHubActions.Environment.Workflow.ActorId);
        /// }
        /// </code>
        /// </example>
        public string ActorId => GetEnvironmentString("GITHUB_ACTOR_ID");

        /// <summary>
        /// Gets the API URL.
        /// </summary>
        /// <value>
        /// The API URL. For example: https://api.github.com.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ApiUrl: {0}", BuildSystem.GitHubActions.Environment.Workflow.ApiUrl);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ApiUrl: {0}", GitHubActions.Environment.Workflow.ApiUrl);
        /// }
        /// </code>
        /// </example>
        public string ApiUrl => GetEnvironmentString("GITHUB_API_URL");

        /// <summary>
        /// Gets the branch of the base repository.
        /// </summary>
        /// <value>
        /// The branch of the base repository. Only set for forked repositories.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow BaseRef: {0}", BuildSystem.GitHubActions.Environment.Workflow.BaseRef);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow BaseRef: {0}", GitHubActions.Environment.Workflow.BaseRef);
        /// }
        /// </code>
        /// </example>
        public string BaseRef => GetEnvironmentString("GITHUB_BASE_REF");

        /// <summary>
        /// Gets the name of the webhook event that triggered the workflow.
        /// </summary>
        /// <value>
        /// The name of the webhook event that triggered the workflow.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow EventName: {0}", BuildSystem.GitHubActions.Environment.Workflow.EventName);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow EventName: {0}", GitHubActions.Environment.Workflow.EventName);
        /// }
        /// </code>
        /// </example>
        public string EventName => GetEnvironmentString("GITHUB_EVENT_NAME");

        /// <summary>
        /// Gets the path of the file with the complete webhook event payload.
        /// </summary>
        /// <value>
        /// The path of the file with the complete webhook event payload.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow EventPath: {0}", BuildSystem.GitHubActions.Environment.Workflow.EventPath);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow EventPath: {0}", GitHubActions.Environment.Workflow.EventPath);
        /// }
        /// </code>
        /// </example>
        public FilePath EventPath => GetEnvironmentFilePath("GITHUB_EVENT_PATH");

        /// <summary>
        /// Gets the GraphQL API URL.
        /// </summary>
        /// <value>
        /// The GraphQL API URL. For example: https://api.github.com/graphql.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow GraphQLUrl: {0}", BuildSystem.GitHubActions.Environment.Workflow.GraphQLUrl);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow GraphQLUrl: {0}", GitHubActions.Environment.Workflow.GraphQLUrl);
        /// }
        /// </code>
        /// </example>
        public string GraphQLUrl => GetEnvironmentString("GITHUB_GRAPHQL_URL");

        /// <summary>
        /// Gets the branch of the head repository.
        /// </summary>
        /// <value>
        /// The branch of the head repository. Only set for forked repositories.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow HeadRef: {0}", BuildSystem.GitHubActions.Environment.Workflow.HeadRef);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow HeadRef: {0}", GitHubActions.Environment.Workflow.HeadRef);
        /// }
        /// </code>
        /// </example>
        public string HeadRef => GetEnvironmentString("GITHUB_HEAD_REF");

        /// <summary>
        /// Gets the job name.
        /// </summary>
        /// <value>
        /// The job name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Job: {0}", BuildSystem.GitHubActions.Environment.Workflow.Job);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Job: {0}", GitHubActions.Environment.Workflow.Job);
        /// }
        /// </code>
        /// </example>
        public string Job => GetEnvironmentString("GITHUB_JOB");

        /// <summary>
        /// Gets the branch or tag ref that triggered the workflow.
        /// </summary>
        /// <value>
        /// The branch or tag ref that triggered the workflow.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Ref: {0}", BuildSystem.GitHubActions.Environment.Workflow.Ref);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Ref: {0}", GitHubActions.Environment.Workflow.Ref);
        /// }
        /// </code>
        /// </example>
        public string Ref => GetEnvironmentString("GITHUB_REF");

        /// <summary>
        /// Gets the owner and repository name.
        /// </summary>
        /// <value>
        /// The owner and repository name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Repository: {0}", BuildSystem.GitHubActions.Environment.Workflow.Repository);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Repository: {0}", GitHubActions.Environment.Workflow.Repository);
        /// }
        /// </code>
        /// </example>
        public string Repository => GetEnvironmentString("GITHUB_REPOSITORY");

        /// <summary>
        /// Gets the ID of the repository.
        /// </summary>
        /// <value>
        /// The ID of the repository.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RepositoryId: {0}", BuildSystem.GitHubActions.Environment.Workflow.RepositoryId);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RepositoryId: {0}", GitHubActions.Environment.Workflow.RepositoryId);
        /// }
        /// </code>
        /// </example>
        public string RepositoryId => GetEnvironmentString("GITHUB_REPOSITORY_ID");

        /// <summary>
        /// Gets the repository owner.
        /// </summary>
        /// <value>
        /// The repository owner.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RepositoryOwner: {0}", BuildSystem.GitHubActions.Environment.Workflow.RepositoryOwner);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RepositoryOwner: {0}", GitHubActions.Environment.Workflow.RepositoryOwner);
        /// }
        /// </code>
        /// </example>
        public string RepositoryOwner => GetEnvironmentString("GITHUB_REPOSITORY_OWNER");

        /// <summary>
        /// Gets the account ID of the repository owner.
        /// </summary>
        /// <value>
        /// The account ID of the repository owner.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RepositoryOwnerId: {0}", BuildSystem.GitHubActions.Environment.Workflow.RepositoryOwnerId);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RepositoryOwnerId: {0}", GitHubActions.Environment.Workflow.RepositoryOwnerId);
        /// }
        /// </code>
        /// </example>
        public string RepositoryOwnerId => GetEnvironmentString("GITHUB_REPOSITORY_OWNER_ID");

        /// <summary>
        /// Gets the number of days that workflow run logs and artifacts are kept.
        /// </summary>
        /// <value>
        /// The number of days that workflow run logs and artifacts are kept.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RetentionDays: {0}", BuildSystem.GitHubActions.Environment.Workflow.RetentionDays);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RetentionDays: {0}", GitHubActions.Environment.Workflow.RetentionDays);
        /// }
        /// </code>
        /// </example>
        public int RetentionDays => GetEnvironmentInteger("GITHUB_RETENTION_DAYS");

        /// <summary>
        /// Gets the unique number for each run within the repository.
        /// </summary>
        /// <value>
        /// The unique number for each run within the repository.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RunId: {0}", BuildSystem.GitHubActions.Environment.Workflow.RunId);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RunId: {0}", GitHubActions.Environment.Workflow.RunId);
        /// }
        /// </code>
        /// </example>
        public string RunId => GetEnvironmentString("GITHUB_RUN_ID");

        /// <summary>
        /// Gets the unique number for each run of a particular workflow in the repository.
        /// </summary>
        /// <value>
        /// The unique number for each run of a particular workflow in the repository.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RunNumber: {0}", BuildSystem.GitHubActions.Environment.Workflow.RunNumber);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RunNumber: {0}", GitHubActions.Environment.Workflow.RunNumber);
        /// }
        /// </code>
        /// </example>
        public int RunNumber => GetEnvironmentInteger("GITHUB_RUN_NUMBER");

        /// <summary>
        /// Gets the URL of the GitHub server.
        /// </summary>
        /// <value>
        /// The URL of the GitHub server. For example: https://github.com.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ServerUrl: {0}", BuildSystem.GitHubActions.Environment.Workflow.ServerUrl);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow ServerUrl: {0}", GitHubActions.Environment.Workflow.ServerUrl);
        /// }
        /// </code>
        /// </example>
        public string ServerUrl => GetEnvironmentString("GITHUB_SERVER_URL");

        /// <summary>
        /// Gets the commit SHA that triggered the workflow.
        /// </summary>
        /// <value>
        /// The commit SHA that triggered the workflow.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Sha: {0}", BuildSystem.GitHubActions.Environment.Workflow.Sha);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Sha: {0}", GitHubActions.Environment.Workflow.Sha);
        /// }
        /// </code>
        /// </example>
        public string Sha => GetEnvironmentString("GITHUB_SHA");

        /// <summary>
        /// Gets the username of the user that initiated the workflow run.
        /// </summary>
        /// <value>
        /// The username of the user that initiated the workflow run. On re-runs, this value may differ from <see cref="Actor"/>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow TriggeringActor: {0}", BuildSystem.GitHubActions.Environment.Workflow.TriggeringActor);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow TriggeringActor: {0}", GitHubActions.Environment.Workflow.TriggeringActor);
        /// }
        /// </code>
        /// </example>
        public string TriggeringActor => GetEnvironmentString("GITHUB_TRIGGERING_ACTOR");

        /// <summary>
        /// Gets the name of the workflow.
        /// </summary>
        /// <value>
        /// The name of the workflow.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow: {0}", BuildSystem.GitHubActions.Environment.Workflow.Workflow);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow: {0}", GitHubActions.Environment.Workflow.Workflow);
        /// }
        /// </code>
        /// </example>
        public string Workflow => GetEnvironmentString("GITHUB_WORKFLOW");

        /// <summary>
        /// Gets the ref path to the workflow file.
        /// </summary>
        /// <value>
        /// The ref path to the workflow file.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow WorkflowRef: {0}", BuildSystem.GitHubActions.Environment.Workflow.WorkflowRef);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow WorkflowRef: {0}", GitHubActions.Environment.Workflow.WorkflowRef);
        /// }
        /// </code>
        /// </example>
        public string WorkflowRef => GetEnvironmentString("GITHUB_WORKFLOW_REF");

        /// <summary>
        /// Gets the commit SHA for the workflow file.
        /// </summary>
        /// <value>
        /// The commit SHA for the workflow file.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow WorkflowSha: {0}", BuildSystem.GitHubActions.Environment.Workflow.WorkflowSha);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow WorkflowSha: {0}", GitHubActions.Environment.Workflow.WorkflowSha);
        /// }
        /// </code>
        /// </example>
        public string WorkflowSha => GetEnvironmentString("GITHUB_WORKFLOW_SHA");

        /// <summary>
        /// Gets the GitHub workspace directory path.
        /// </summary>
        /// <value>
        /// The GitHub workspace directory path.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Workspace: {0}", BuildSystem.GitHubActions.Environment.Workflow.Workspace);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Workspace: {0}", GitHubActions.Environment.Workflow.Workspace);
        /// }
        /// </code>
        /// </example>
        public DirectoryPath Workspace => GetEnvironmentDirectoryPath("GITHUB_WORKSPACE");

        /// <summary>
        /// Gets the number of attempts for current run.
        /// </summary>
        /// <value>
        /// The attempt number  for current run.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Attempt: {0}", BuildSystem.GitHubActions.Environment.Workflow.Attempt);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow Attempt: {0}", GitHubActions.Environment.Workflow.Attempt);
        /// }
        /// </code>
        /// </example>
        public int Attempt => GetEnvironmentInteger("GITHUB_RUN_ATTEMPT");

        /// <summary>
        /// Gets a value indicating whether if branch protections are configured for the ref that triggered the workflow run.
        /// </summary>
        /// <value>
        /// Value whether if branch protections are configured for the ref that triggered the workflow run.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RefProtected: {0}", BuildSystem.GitHubActions.Environment.Workflow.RefProtected);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RefProtected: {0}", GitHubActions.Environment.Workflow.RefProtected);
        /// }
        /// </code>
        /// </example>
        public bool RefProtected => GetEnvironmentBoolean("GITHUB_REF_PROTECTED");

        /// <summary>
        /// Gets the branch or tag name that triggered the workflow run.
        /// </summary>
        /// <value>
        /// The branch or tag name that triggered the workflow run.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RefName: {0}", BuildSystem.GitHubActions.Environment.Workflow.RefName);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RefName: {0}", GitHubActions.Environment.Workflow.RefName);
        /// }
        /// </code>
        /// </example>
        public string RefName => GetEnvironmentString("GITHUB_REF_NAME");

        /// <summary>
        /// Gets the type of ref that triggered the workflow run.
        /// </summary>
        /// <value>
        /// The type of ref that triggered the workflow run. Valid values are branch or tag.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RefType: {0}", BuildSystem.GitHubActions.Environment.Workflow.RefType);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Workflow RefType: {0}", GitHubActions.Environment.Workflow.RefType);
        /// }
        /// </code>
        /// </example>
        public GitHubActionsRefType RefType => GetEnvironmentString("GITHUB_REF_TYPE")
                                                    ?.ToLowerInvariant() switch
                                                    {
                                                        "branch" => GitHubActionsRefType.Branch,
                                                        "tag" => GitHubActionsRefType.Tag,
                                                        _ => GitHubActionsRefType.Unknown
                                                    };
    }
}
