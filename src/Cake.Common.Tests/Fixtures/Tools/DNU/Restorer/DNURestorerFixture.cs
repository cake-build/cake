// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.DNU.Restore;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DNU.Restorer
{
    internal sealed class DNURestorerFixture : DNUFixture<DNURestoreSettings>
    {
        public FilePath FilePath { get; set; }

        protected override void RunTool()
        {
            var tool = new DNURestorer(FileSystem, Environment, ProcessRunner, Tools);
            tool.Restore(FilePath, Settings);
        }
    }
}
