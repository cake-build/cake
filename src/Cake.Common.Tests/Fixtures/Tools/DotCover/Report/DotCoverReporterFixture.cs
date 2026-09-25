// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotCover.Report;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotCover.Report;

internal sealed class DotCoverReporterFixture : DotCoverFixture<DotCoverReportSettings>
{
    public FilePath SourceFile { get; set; }
    public FilePath OutputFile { get; set; }

    public DotCoverReporterFixture()
    {
        // Set the source file.
        SourceFile = new FilePath("./result.dcvr");

        // Setup the output file.
        OutputFile = new FilePath("./result.xml");
    }

    protected override void RunTool()
    {
        var tool = new DotCoverReporter(FileSystem, Environment, ProcessRunner, Tools);
        tool.Report(SourceFile, OutputFile, Settings);
    }
}
