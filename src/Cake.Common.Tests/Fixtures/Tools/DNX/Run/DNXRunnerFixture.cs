using Cake.Common.Tools.DNX.Run;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Common.Tests.Fixtures.Tools.DNX.Run
{
    internal sealed class DNXRunnerFixture : DNXFixture<DNXRunSettings>
    {
        public DirectoryPath DirectoryPath { get; set; }

        public string Command { get; set; }

        protected override void RunTool()
        {
            var tool = new DNXRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(DirectoryPath, Command, Settings);
        }
    }
}
