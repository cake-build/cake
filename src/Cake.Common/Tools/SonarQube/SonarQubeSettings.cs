using Cake.Core.Tooling;

namespace Cake.Common.Tools.SonarQube
{
    /// <summary>
    /// Contains settings used by <see cref="SonarQubeRunner"/>.
    /// </summary>
    public class SonarQubeSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets SonarQube server address (https://sonarqube.com/ in most cases).
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// Gets or sets the authentication token of a SonarQube user with Execute Analysis permission.
        /// For SonarQube.com you just need to log in with your GitHub account and generate a user token from the “My Account” > “Security” page.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets password to acess SonarQube server (leave empty for SonarQube.com).
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the project key that is unique for each project. Allowed characters are: letters, numbers, -, _, . and :, with at least one non-digit.
        /// </summary>
        public string ProjectKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the project that will be displayed on the web interface.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the project version number.
        /// </summary>
        public string ProjectVersion { get; set; }
    }
}
