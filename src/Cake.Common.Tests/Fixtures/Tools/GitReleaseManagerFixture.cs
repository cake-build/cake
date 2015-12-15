using Cake.Common.Tools.GitReleaseManager;
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;
using Cake.Testing.Shared;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal abstract class GitReleaseManagerFixture<TSettings> : ToolFixture<TSettings>
        where TSettings : ToolSettings, new()
    {
        public ICakeLog Log { get; set; }
        public IGitReleaseManagerToolResolver Resolver { get; set; }

        protected GitReleaseManagerFixture()
            : base("GitReleaseManager.exe")
        {
            Log = Substitute.For<ICakeLog>();
            Resolver = Substitute.For<IGitReleaseManagerToolResolver>();
        }
    }
}