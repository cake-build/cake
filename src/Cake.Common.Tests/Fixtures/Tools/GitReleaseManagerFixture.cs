using Cake.Core.Diagnostics;
using Cake.Core.Tooling;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal abstract class GitReleaseManagerFixture<TSettings> : ToolFixture<TSettings>
        where TSettings : ToolSettings, new()
    {
        public ICakeLog Log { get; set; }

        protected GitReleaseManagerFixture()
            : base("GitReleaseManager.exe")
        {
            Log = Substitute.For<ICakeLog>();
        }
    }
}