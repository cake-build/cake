using Cake.Common.NuSpec;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.NuSpec
{
    internal class NuSpecFixtureBase
    {
        protected FakeFileSystem FileSystem { get; }
        protected ICakeLog Log { get; }
        protected ICakeEnvironment Environment { get; }
        public NuSpecSettings Settings { get; }

        protected NuSpecFixtureBase()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns(new DirectoryPath("/Working"));

            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateDirectory(Environment.WorkingDirectory);

            Log = Substitute.For<ICakeLog>();
            Settings = new NuSpecSettings();
        }
    }
}