using Cake.Common.Tools.NuGet.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Sources
{
    internal abstract class NuGetSourcesFixture : NuGetFixture<NuGetSourcesSettings>
    {
        public string Name { get; set; }
        public string Source { get; set; }

        protected NuGetSourcesFixture()
        {
            Name = "name";
            Source = "source";
        }

        public void GivenExistingSource()
        {
            ProcessRunner.Process.SetStandardOutput(new[] {
                "  1.  https://www.nuget.org/api/v2/ [Enabled]",
                "      https://www.nuget.org/api/v2/",
                string.Format("  2.  {0} [Enabled]", Name),
                string.Format("      {0}", Source)});
        }

        public void GivenSourceAlreadyHasBeenAdded()
        {
            ProcessRunner.Process.SetStandardOutput(new[] { Source });
        }
    }
}