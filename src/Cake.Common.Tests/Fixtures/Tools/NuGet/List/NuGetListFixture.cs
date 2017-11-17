using System.Collections.Generic;
using Cake.Common.Tools.NuGet.List;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.List
{
    internal sealed class NuGetListFixture : NuGetFixture<NuGetListSettings>
    {
        public string PackageId { get; set; }

        public IEnumerable<NuGetListItem> Result { get; set; }

        public NuGetListFixture()
        {
            PackageId = "Cake";
        }

        public void GivenNormalPackageResult()
        {
            ProcessRunner.Process.SetStandardOutput(new string[]
            {
                "Cake 0.22.2",
                "Cake.Core 0.22.2",
                "Cake.CoreCLR 0.22.2",
            });
        }

        protected override void RunTool()
        {
            var tool = new NuGetList(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            Result = tool.List(PackageId, Settings);
        }
    }
}
