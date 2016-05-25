using System.Collections.Generic;
using Cake.Common.Tools.WiX;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class CandleFixture : ToolFixture<CandleSettings>
    {
        public List<FilePath> SourceFiles { get; set; }

        public CandleFixture()
            : base("candle.exe")
        {
            SourceFiles = new List<FilePath>();
            SourceFiles.Add(new FilePath("./Test.wxs"));
        }

        protected override void RunTool()
        {
            var tool = new CandleRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(SourceFiles, Settings);
        }
    }
}