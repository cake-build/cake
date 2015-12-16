using Cake.Common.Tools.DNVM.Use;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Common.Tests.Fixtures.Tools.DNVM.Use
{
    internal sealed class DNVMUserFixture : DNVMFixture<DNVMSettings>
    {
        public string Version { get; set; }

        protected override void RunTool()
        {
            var tool = new DNVMUser(FileSystem, Environment, ProcessRunner, Globber);
            tool.Use(Version, Settings);
        }
    }
}
