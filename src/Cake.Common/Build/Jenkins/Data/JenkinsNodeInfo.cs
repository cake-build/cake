using Cake.Core;

namespace Cake.Common.Build.Jenkins.Data
{
    /// <summary>
    /// Provides Jenkins node information for a current build.
    /// </summary>
    public class JenkinsNodeInfo : JenkinsInfo
    {
        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public string NodeName
        {
            get { return GetEnvironmentString("NODE_NAME"); }
        }

        /// <summary>
        /// Gets the node labels.
        /// </summary>
        /// <value>
        /// The node labels.
        /// </value>
        public string NodeLabels
        {
            get { return GetEnvironmentString("NODE_LABELS"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsNodeInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsNodeInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
