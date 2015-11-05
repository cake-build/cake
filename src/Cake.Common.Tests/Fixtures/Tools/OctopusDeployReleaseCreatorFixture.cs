using Cake.Common.Tools.OctopusDeploy;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class OctopusDeployReleaseCreatorFixture : ToolFixture<CreateReleaseSettings>
    {
        public string ProjectName { get; set; }

        public OctopusDeployReleaseCreatorFixture()
            : base("Octo.exe")
        {
            ProjectName = "testProject";

            Settings.Server = "http://octopus";
            Settings.ApiKey = "API-12345";
        }

        public string GetDefaultArguments()
        {
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "create-release --project \"{0}\" --server {1} --apiKey {2}", ProjectName, Settings.Server, Settings.ApiKey);
        }

        protected override void RunTool()
        {
            var tool = new OctopusDeployReleaseCreator(FileSystem, Environment, Globber, ProcessRunner);
            tool.CreateRelease(ProjectName, Settings);
        }
    }
}