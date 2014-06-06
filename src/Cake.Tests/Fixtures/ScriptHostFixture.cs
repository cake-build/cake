using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Scripting;
using Cake.Scripting.Host;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    public sealed class ScriptHostFixture
    {
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
        }

        public ScriptHost CreateHost()
        {
            return new ScriptHost(Engine);
        }
    }
}
