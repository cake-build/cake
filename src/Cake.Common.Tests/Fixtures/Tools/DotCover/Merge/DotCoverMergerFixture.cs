// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotCover.Merge;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotCover.Merge
{
    internal sealed class DotCoverMergerFixture : DotCoverFixture<DotCoverMergeSettings>
    {
        public IEnumerable<FilePath> SourceFiles { get; set; }
        public FilePath OutputFile { get; set; }

        public DotCoverMergerFixture()
        {
            // Set the source files.
            SourceFiles = new[]
            {
                new FilePath("./result1.dcvr"),
                new FilePath("./result2.dcvr"),
            };

            // Setup the output file.
            OutputFile = new FilePath("./result.dcvr");
        }

        protected override void RunTool()
        {
            var tool = new DotCoverMerger(FileSystem, Environment, ProcessRunner, Tools);
            tool.Merge(SourceFiles, OutputFile, Settings);
        }
    }
}