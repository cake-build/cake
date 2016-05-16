using Cake.Core;

namespace Cake.Common.Build.TravisCI.Data
{
    /// <summary>
    /// Provides Travis CI repository information for the current build.
    /// </summary>
    public sealed class TravisCIRepositoryInfo : TravisCIInfo
    {
        /// <summary>
        /// Gets the commit that the current build is testing.
        /// </summary>
        /// <value>
        /// The commit.
        /// </value>
        public string Commit
        {
            get { return GetEnvironmentString("TRAVIS_COMMIT"); }
        }

        /// <summary>
        /// Gets the commit range for the current pull request.
        /// </summary>
        /// <value>
        /// The commit range.
        /// </value>
        public string CommitRange
        {
            get { return GetEnvironmentString("TRAVIS_COMMIT_RANGE"); }
        }

        /// <summary>
        /// Gets the pull request.
        /// </summary>
        /// <value>
        /// The pull request.
        /// </value>
        public string PullRequest
        {
            get { return GetEnvironmentString("TRAVIS_PULL_REQUEST"); }
        }

        /// <summary>
        /// Gets the slug of the respository currently being built.
        /// </summary>
        /// <value>
        /// The slug.
        /// </value>
        public string Slug
        {
            get { return GetEnvironmentString("TRAVIS_REPO_SLUG"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TravisCIRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TravisCIRepositoryInfo(ICakeEnvironment environment) 
            : base(environment)
        {
        }
    }
}
