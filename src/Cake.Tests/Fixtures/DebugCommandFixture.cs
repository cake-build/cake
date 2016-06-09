// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Commands;
using Cake.Diagnostics;
using Cake.Scripting;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    internal sealed class DebugCommandFixture
    {
        public IScriptRunner ScriptRunner { get; set; }
        public ICakeLog Log { get; set; }
        public IDebugger Debugger { get; set; }
        public CakeOptions Options { get; set; }

        public DebugCommandFixture()
        {
            ScriptRunner = Substitute.For<IScriptRunner>();
            Log = Substitute.For<ICakeLog>();
            Debugger = Substitute.For<IDebugger>();
            Options = new CakeOptions
            {
                Script = "build.cake"
            };
        }

        public DebugCommand CreateCommand()
        {
            var context = Substitute.For<ICakeContext>();
            var engine = Substitute.For<ICakeEngine>();
            var printer = Substitute.For<ICakeReportPrinter>();

            context.Log.Returns(Log);

            var host = new BuildScriptHost(engine, context, printer, Log);

            return new DebugCommand(ScriptRunner, Debugger, host);
        }

        public bool Execute()
        {
            return CreateCommand().Execute(Options);
        }
    }
}
