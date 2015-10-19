namespace Cake.Common.Tools.GitReleaseNotes
{
    /// <summary>
    /// The IssueTracker to be used.
    /// </summary>
    public enum GitReleaseNotesIssueTracker
    {
        /// <summary>
        /// Uses BitBucket as Issue Tracker.
        /// </summary>
        BitBucket,

        /// <summary>
        /// Uses GitHub as Issue Tracker.
        /// </summary>
        GitHub,

        /// <summary>
        /// Uses Jira as Issue Tracker.
        /// </summary>
        Jira,

        /// <summary>
        /// Uses YouTrack as Issue Tracker.
        /// </summary>
        YouTrack
    }
}