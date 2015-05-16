using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class ScriptRunnerFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeArguments Arguments { get; set; }
        public IScriptEngine Engine { get; set; }
        public IScriptSession Session { get; set; }
        public IScriptProcessor ScriptProcessor { get; set; }
        public IScriptAliasFinder AliasFinder { get; set; }
        public ICakeLog Log { get; set; }

        public IScriptHost Host { get; set; }
        public FilePath Script { get; set; }
        public IDictionary<string, string> ArgumentDictionary { get; set; }
        public string Source { get; private set; }
        public IGlobber Globber{ get; set; }
        public INuGetToolResolver NuGetToolResolver{ get; private set; }

        public ScriptRunnerFixture(string fileName = "./build.cake")
        {
            Script = fileName;
            Source = "Hello World";

            ArgumentDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            FileSystem = new FakeFileSystem(true);
            FileSystem.GetCreatedFile(Script.MakeAbsolute(Environment), Source);

            Globber = Substitute.For<IGlobber>();

            Session = Substitute.For<IScriptSession>();
            Engine = Substitute.For<IScriptEngine>();
            Engine.CreateSession(Arg.Any<IScriptHost>(), ArgumentDictionary).Returns(Session);

            Arguments = Substitute.For<ICakeArguments>();
            AliasFinder = Substitute.For<IScriptAliasFinder>();
            Log = Substitute.For<ICakeLog>();
            NuGetToolResolver = new NuGetToolResolver(FileSystem, Environment, Globber);
            ScriptProcessor = new ScriptProcessor(FileSystem, Environment, Log, NuGetToolResolver);

            var context = Substitute.For<ICakeContext>();
            context.Arguments.Returns(c => Arguments);
            context.Environment.Returns(c => Environment);
            context.FileSystem.Returns(c => FileSystem);

            Host = Substitute.For<IScriptHost>();
            Host.Context.Returns(context);
        }

        public ScriptRunner CreateScriptRunner()
        {
            return new ScriptRunner(Engine, AliasFinder, ScriptProcessor);
        }

        public string GetExpectedSource()
        {
            return string.Concat("#line 1 \"build.cake\"", "\r\n", Source);
        }
    }
}
