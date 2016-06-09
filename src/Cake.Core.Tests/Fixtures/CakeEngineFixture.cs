// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class CakeEngineFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public FakeLog Log { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeArguments Arguments { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public ICakeContext Context { get; set; }
        public IExecutionStrategy ExecutionStrategy { get; set; }

        public CakeEngineFixture()
        {
            FileSystem = Substitute.For<IFileSystem>();
            Environment = Substitute.For<ICakeEnvironment>();
            Log = new FakeLog();
            Globber = Substitute.For<IGlobber>();
            Arguments = Substitute.For<ICakeArguments>();
            ProcessRunner = Substitute.For<IProcessRunner>();
            ExecutionStrategy = new DefaultExecutionStrategy(Log);

            Context = Substitute.For<ICakeContext>();
            Context.Arguments.Returns(Arguments);
            Context.Environment.Returns(Environment);
            Context.FileSystem.Returns(FileSystem);
            Context.Globber.Returns(Globber);
            Context.Log.Returns(Log);
            Context.ProcessRunner.Returns(ProcessRunner);
        }

        public CakeEngine CreateEngine()
        {
            return new CakeEngine(Log);
        }
    }
}
