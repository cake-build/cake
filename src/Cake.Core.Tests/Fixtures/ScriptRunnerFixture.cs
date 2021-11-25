// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors.Loading;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class ScriptRunnerFixture
    {
        public IAssemblyLoader AssemblyLoader { get; set; }
        public FakeFileSystem FileSystem { get; set; }
        public FakeEnvironment Environment { get; set; }
        public ICakeConfiguration Configuration { get; set; }
        public IScriptEngine Engine { get; set; }
        public IScriptSession Session { get; set; }
        public IScriptAnalyzer ScriptAnalyzer { get; set; }
        public IScriptProcessor ScriptProcessor { get; set; }
        public IScriptConventions ScriptConventions { get; set; }
        public IScriptAliasFinder AliasFinder { get; set; }
        public FakeLog Log { get; set; }

        public IScriptHost Host { get; set; }
        public FilePath Script { get; set; }
        public IDictionary<string, string> ArgumentDictionary { get; set; }
        public string Source { get; }
        public IGlobber Globber { get; set; }

        public ScriptRunnerFixture(string fileName = "/Working/build.cake")
        {
            Script = fileName;
            Source = "Hello World";

            AssemblyLoader = Substitute.For<IAssemblyLoader>();

            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateFile(Script.MakeAbsolute(Environment)).SetContent(Source);
            Globber = new Globber(FileSystem, Environment);

            Configuration = Substitute.For<ICakeConfiguration>();
            AliasFinder = Substitute.For<IScriptAliasFinder>();
            Log = new FakeLog();

            Session = Substitute.For<IScriptSession>();
            ArgumentDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Engine = Substitute.For<IScriptEngine>();
            Engine.CreateSession(Arg.Any<IScriptHost>()).Returns(Session);

            var runtime = new CakeRuntime();
            var referenceAssemblyResolver = Substitute.For<IReferenceAssemblyResolver>();
            referenceAssemblyResolver.GetReferenceAssemblies().Returns(Array.Empty<System.Reflection.Assembly>());
            ScriptAnalyzer = new ScriptAnalyzer(FileSystem, Environment, Log, new[] { new FileLoadDirectiveProvider(Globber, Log) });
            ScriptProcessor = Substitute.For<IScriptProcessor>();
            ScriptConventions = new ScriptConventions(FileSystem, AssemblyLoader, runtime, referenceAssemblyResolver);

            var context = Substitute.For<ICakeContext>();
            context.Environment.Returns(c => Environment);
            context.FileSystem.Returns(c => FileSystem);
            Host = Substitute.For<IScriptHost>();
            Host.Context.Returns(context);
        }

        public ScriptRunner CreateScriptRunner()
        {
            return new ScriptRunner(Environment, Log, Configuration, Engine,
                AliasFinder, ScriptAnalyzer, ScriptProcessor,
                ScriptConventions, AssemblyLoader);
        }
    }
}