using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class ScriptRunnerFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public FakeEnvironment Environment { get; set; }
        public ICakeArguments Arguments { get; set; }
        public IScriptEngine Engine { get; set; }
        public IScriptSession Session { get; set; }
        public IScriptAnalyzer ScriptAnalyzer { get; set; }
        public IScriptProcessor ScriptProcessor { get; set; }
        public IScriptConventions ScriptConventions { get; set; }
        public IScriptAliasFinder AliasFinder { get; set; }
        public ICakeLog Log { get; set; }

        public IScriptHost Host { get; set; }
        public FilePath Script { get; set; }
        public IDictionary<string, string> ArgumentDictionary { get; set; }
        public string Source { get; private set; }
        public IGlobber Globber{ get; set; }

        public ScriptRunnerFixture(string fileName = "/Working/build.cake")
        {
            Script = fileName;
            Source = "Hello World";

            ArgumentDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Environment = FakeEnvironment.CreateUnixEnvironment();

            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateFile(Script.MakeAbsolute(Environment)).SetContent(Source);

            Globber = Substitute.For<IGlobber>();

            Session = Substitute.For<IScriptSession>();
            Engine = Substitute.For<IScriptEngine>();
            Engine.CreateSession(Arg.Any<IScriptHost>(), ArgumentDictionary).Returns(Session);

            Arguments = Substitute.For<ICakeArguments>();
            AliasFinder = Substitute.For<IScriptAliasFinder>();
            Log = Substitute.For<ICakeLog>();

            ScriptAnalyzer = new ScriptAnalyzer(FileSystem, Environment, Log);
            ScriptProcessor = Substitute.For<IScriptProcessor>();
            ScriptConventions = new ScriptConventions(FileSystem);

            var context = Substitute.For<ICakeContext>();
            context.Arguments.Returns(c => Arguments);
            context.Environment.Returns(c => Environment);
            context.FileSystem.Returns(c => FileSystem);

            Host = Substitute.For<IScriptHost>();
            Host.Context.Returns(context);
        }

        public ScriptRunner CreateScriptRunner()
        {
            return new ScriptRunner(Environment, Log, Engine,
                AliasFinder, ScriptAnalyzer, ScriptProcessor, ScriptConventions);
        }

        public string GetExpectedSource()
        {
            return string.Concat("#line 1 \"build.cake\"", "\r\n", Source);
        }
    }
}