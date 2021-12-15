// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Format;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Format
{
    internal sealed class DotNetFormatterFixture : DotNetFixture<DotNetFormatSettings>
    {
        public string Root { get; set; }

        public string Subcommand { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetFormatter(FileSystem, Environment, ProcessRunner, Tools);
            tool.Format(Root, Subcommand, Settings);
        }
    }
}
