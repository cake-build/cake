using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Core.Tests.Fakes;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class ScriptRunnerFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeArguments Arguments { get; set; }
        public IScriptSessionFactory SessionFactory { get; set; }
        public IScriptSession Session { get; set; }
        public IScriptProcessor ScriptProcessor { get; set; }
        public IScriptAliasGenerator AliasGenerator { get; set; }
        public ICakeLog Log { get; set; }

        public IScriptHost Host { get; set; }
        public FilePath Script { get; set; }
        public IDictionary<string, string> ArgumentDictionary { get; set; }
        public string Source { get; private set; }

        public ScriptRunnerFixture(string fileName = "./build.cake")
        {
            Script = fileName;
            Source = "Hello World";

            ArgumentDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            FileSystem = new FakeFileSystem(true);
            FileSystem.GetCreatedFile(Script.MakeAbsolute(Environment), Source);

            Session = Substitute.For<IScriptSession>();
            SessionFactory = Substitute.For<IScriptSessionFactory>();
            SessionFactory.CreateSession(Arg.Any<IScriptHost>()).Returns(Session);

            Arguments = Substitute.For<ICakeArguments>();
            AliasGenerator = Substitute.For<IScriptAliasGenerator>();            
            Log = Substitute.For<ICakeLog>();
            ScriptProcessor = new ScriptProcessor(FileSystem, Environment, Log);

            Host = Substitute.For<IScriptHost>();
            Host.FileSystem.Returns(c => FileSystem);
            Host.Arguments.Returns(c => Arguments);
            Host.Environment.Returns(c => Environment);
        }

        public ScriptRunner CreateScriptRunner()
        {
            return new ScriptRunner(SessionFactory, AliasGenerator, ScriptProcessor);
        }

        public string GetExpectedSource()
        {
            return string.Concat("#line 1 \"build.cake\"", "\r\n", Source);
        }
    }
}
