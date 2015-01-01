using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class ScriptHostFixture
    {
        public sealed class TestingScriptHost : ScriptHost
        {
            public TestingScriptHost(ICakeEngine engine)
                : base(engine)
            {
            }

            public override CakeReport RunTarget(string target)
            {
                return new CakeReport();
            }
        }

        public ICakeEngine Engine { get; set; }
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeLog Log { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeArguments Arguments { get; set; }

        public ScriptHostFixture()
        {
            FileSystem = Substitute.For<IFileSystem>();
            Environment = Substitute.For<ICakeEnvironment>();
            Log = Substitute.For<ICakeLog>();
            Globber = Substitute.For<IGlobber>();
            Arguments = Substitute.For<ICakeArguments>();

            Engine = Substitute.For<ICakeEngine>();
            Engine.FileSystem.Returns(FileSystem);
            Engine.Environment.Returns(Environment);
            Engine.Log.Returns(Log);
            Engine.Globber.Returns(Globber);
            Engine.Arguments.Returns(Arguments);
            Engine.RunTarget(Arg.Any<string>()).Returns(new CakeReport());
        }

        public ScriptHost CreateHost()
        {
            return new TestingScriptHost(Engine);
        }
    }
}
