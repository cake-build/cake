using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Core.Tests.Fakes;
using Cake.Scripting;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    internal sealed class ScriptRunnerFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public CakeArguments Arguments { get; set; }
        public IScriptSessionFactory SessionFactory { get; set; }
        public IScriptSession Session { get; set; }
        public IScriptProcessor ScriptProcessor { get; set; }
        public IScriptAliasGenerator AliasGenerator { get; set; }
        public ICakeLog Log { get; set; }

        public IScriptHost Host { get; set; }
        public CakeOptions Options { get; set; }
        public string Source { get; private set; }

        public ScriptRunnerFixture(string fileName = "./build.cake")
        {
            Source = "Hello World";

            Options = new CakeOptions();
            Options.Verbosity = Verbosity.Diagnostic;
            Options.Script = fileName;

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            FileSystem = new FakeFileSystem(true);
            FileSystem.GetCreatedFile(Options.Script.MakeAbsolute(Environment), Source);

            Session = Substitute.For<IScriptSession>();
            SessionFactory = Substitute.For<IScriptSessionFactory>();
            SessionFactory.CreateSession(Arg.Any<IScriptHost>()).Returns(Session);

            Arguments = new CakeArguments();
            AliasGenerator = Substitute.For<IScriptAliasGenerator>();            
            Host = Substitute.For<IScriptHost>();
            Log = Substitute.For<ICakeLog>();

            ScriptProcessor = new ScriptProcessor(FileSystem, Environment, Log);
        }

        public ScriptRunner CreateScriptRunner()
        {
            return new ScriptRunner(FileSystem, Environment, Arguments,
                SessionFactory, AliasGenerator, ScriptProcessor, Host);
        }
    }
}
