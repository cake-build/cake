// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class ProcessFixture
    {
        public ICakeContext Context { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IProcess Process { get; set; }

        public ProcessFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            Process = Substitute.For<IProcess>();

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(p => Process);

            Context = Substitute.For<ICakeContext>();
            Context.ProcessRunner.Returns(ProcessRunner);
            Context.Environment.Returns(Environment);
        }

        public int Start(string filename)
        {
            return Context.StartProcess(filename);
        }

        public IProcess StartNewProcess(string filename)
        {
            return Context.StartAndReturnProcess(filename);
        }

        public int Start(string filename, string processArguments)
        {
            return Start(filename, new ProcessSettings { Arguments = processArguments });
        }

        public int Start(string filename, ProcessSettings settings)
        {
            return Context.StartProcess(filename, settings);
        }

        public IProcess StartNewProcess(string filename, ProcessSettings settings)
        {
            return Context.StartAndReturnProcess(filename, settings);
        }
    }
}
