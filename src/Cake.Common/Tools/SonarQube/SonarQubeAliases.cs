using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.SonarQube
{
    /// <summary>
    /// <para>Contains functionality related to running <see href="http://docs.sonarqube.org/display/SCAN/Analyzing+with+SonarQube+Scanner+for+MSBuild">SonarQube</see> analysis.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from Chocolatey.org, or specify the ToolPath within the <see cref="SonarQubeSettings" /> class:
    /// <code>
    /// ChocolateyInstall(sonarqube-msbuild-runner)
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("SonarQube")]
    public static class SonarQubeAliases
    {
        /// <summary>
        /// Analyse the specified solution using SonarQube and push it to SonarQube.com
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution to analyse.</param>
        /// <param name="token">User token for SonarQube.com (log in with your GitHub account and generate a user token from the “My Account” > “Security” page).</param>
        /// <param name="projectKey">Project key that is unique for each project.</param>
        /// <param name="projectName">Name of the project that will be displayed on the web interface.</param>
        /// <param name="projectVersion">Project version number</param>
        [CakeMethodAlias]
        public static void SonarQube(this ICakeContext context, FilePath solution, string token, string projectKey, string projectName, string projectVersion)
        {
            SonarQube(context, solution, new SonarQubeSettings()
            {
                HostUrl = "https://sonarqube.com/",
                Login = token,
                ProjectKey = projectKey,
                ProjectName = projectName,
                ProjectVersion = projectVersion
            });
        }

        /// <summary>
        /// Analyse the specified solution using SonarQube
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution to analyse.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// SonarQube("./src/Cake.sln", new SonarQubeSettings {
        ///     HostUrl = "https://sonarqube.com/",
        ///     Login = e5fef67e7cba8c67b914e9edb4ecee3e9a697d53,
        ///     ProjectKey = "test:my_project",
        ///     ProjectName = "My Test Project",
        ///     ProjectVersion = "1.0.1"
        ///     });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void SonarQube(this ICakeContext context, FilePath solution, SonarQubeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (solution == null)
            {
                throw new ArgumentNullException(nameof(solution));
            }

            var runner = new SonarQubeRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(solution, settings);
        }
    }
}
