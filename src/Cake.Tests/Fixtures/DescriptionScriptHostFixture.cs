using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Scripting;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    public sealed class DescriptionScriptHostFixture
    {
        public ICakeEngine Engine { get; set; }
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeLog Log { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeArguments Arguments { get; set; }

        public DescriptionScriptHostFixture()
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

        public DescriptionScriptHost CreateHost()
        {
            return new DescriptionScriptHost(Engine);
        }
    }
}