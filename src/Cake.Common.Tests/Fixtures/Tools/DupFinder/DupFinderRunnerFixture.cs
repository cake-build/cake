using System.Collections.Generic;
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.DupFinder;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.DupFinder
{
    internal sealed class DupFinderRunnerFixture : ToolFixture<DupFinderSettings>
    {
        private readonly ICakeLog Log;

        public List<FilePath> FilePaths { get; set; }

        public DupFinderRunnerFixture()
            : base("dupfinder.exe")
        {
            FilePaths = new List<FilePath>();
            FilePaths.Add(new FilePath("./Test.sln"));

            Log = Substitute.For<ICakeLog>();

            FileSystem.CreateFile("build/dupfinder.xml").SetContent(Resources.DupFinderReportNoDuplicates.NormalizeLineEndings());
            FileSystem.CreateFile("build/duplicates.xml").SetContent(Resources.DupFinderReportWithDuplicates.NormalizeLineEndings());
        }

        protected override void RunTool()
        {
            var tool = new DupFinderRunner(FileSystem, Environment, ProcessRunner, Globber, Log);
            tool.Run(FilePaths, Settings);
        }
    }
}