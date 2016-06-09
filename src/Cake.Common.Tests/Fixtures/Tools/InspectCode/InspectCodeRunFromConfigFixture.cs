// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.InspectCode;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.InspectCode
{
    internal sealed class InspectCodeRunFromConfigFixture : InspectCodeFixture
    {
        public ICakeLog Log { get; set; }
        public FilePath Config { get; set; }

        public InspectCodeRunFromConfigFixture()
        {
            Log = Substitute.For<ICakeLog>();
        }

        protected override void RunTool()
        {
            var tool = new InspectCodeRunner(FileSystem, Environment, ProcessRunner, Tools, Log);
            tool.RunFromConfig(Config);
        }
    }
}
