// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
            public TestingScriptHost(ICakeEngine engine, ICakeContext context)
                : base(engine, context)
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
        public ICakeContext Context { get; set; }

        public ScriptHostFixture()
        {
            FileSystem = Substitute.For<IFileSystem>();
            Environment = Substitute.For<ICakeEnvironment>();
            Log = Substitute.For<ICakeLog>();
            Globber = Substitute.For<IGlobber>();
            Arguments = Substitute.For<ICakeArguments>();

            Context = Substitute.For<ICakeContext>();
            Context.Arguments.Returns(Arguments);
            Context.Environment.Returns(Environment);
            Context.FileSystem.Returns(FileSystem);
            Context.Globber.Returns(Globber);
            Context.Log.Returns(Log);

            Engine = Substitute.For<ICakeEngine>();
            Engine.RunTarget(Context, Arg.Any<IExecutionStrategy>(), Arg.Any<string>())
                .Returns(new CakeReport());
        }

        public ScriptHost CreateHost()
        {
            return new TestingScriptHost(Engine, Context);
        }
    }
}
