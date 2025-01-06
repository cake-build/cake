using System.Runtime.Serialization;

namespace Cake.Common.Build.GitLabCI.Data;

/// <summary>
/// Enumerates possible triggers of a GitLab CI pipeline.
/// </summary>
/// <seealso href="https://docs.gitlab.com/ee/ci/jobs/job_rules.html#ci_pipeline_source-predefined-variable"> CI_PIPELINE_SOURCE predefined variable (GitLab Documentation)</seealso>
public enum GitLabCIPipelineSource
{
    /// <summary>
    /// Pipeline var triggered by the pipelines API.
    /// </summary>
    Api,

    /// <summary>
    /// Pipeline was created using a GitLab ChatOps command.
    /// </summary>
    Chat,

    /// <summary>
    /// Pipeline was created using a CI service other than GitLab
    /// </summary>
    External,

    /// <summary>
    /// Pipeline was created because an external pull request on GitHub was created or updated.
    /// </summary>
    [EnumMember(Value = "external_pull_request_event")]
    ExternalPullRequestEvent,

    /// <summary>
    /// Pipeline was created because a merge request was created or updated.
    /// </summary>
    [EnumMember(Value = "merge_request_event")]
    MergeRequestEvent,

    /// <summary>
    /// Pipeline is an on-demand DAST scan pipeline.
    /// </summary>
    [EnumMember(Value = "ondemand_dast_scan")]
    OnDemandDastScan,

    /// <summary>
    /// Pipeline is an on-demand DAST validation pipeline.
    /// </summary>
    [EnumMember(Value = "ondemand_dast_validation")]
    OnDemandDastValidation,

    /// <summary>
    /// Pipeline was created by a parent pipeline.
    /// </summary>
    [EnumMember(Value = "parent_pipeline")]
    ParentPipeline,

    /// <summary>
    /// Pipeline was created by another pipeline
    /// </summary>
    Pipeline,

    /// <summary>
    /// Pipelune was triggered by a Git push event.
    /// </summary>
    Push,

    /// <summary>
    /// Pipeline was triggered by a schedule.
    /// </summary>
    Schedule,

    /// <summary>
    /// Pipeline is a security orchestration policy pipeline.
    /// </summary>
    [EnumMember(Value = "security_orchestration_policy")]
    SecurityOrchestrationPolicy,

    /// <summary>
    /// Pipeline was created using a trigger token.
    /// </summary>
    Trigger,

    /// <summary>
    /// Pipeline was created by selecting <i>New Pipeline</i> in the GitLab UI.
    /// </summary>
    Web,

    /// <summary>
    /// Pipeline was created using the Web IDE.
    /// </summary>
    WebIde
}