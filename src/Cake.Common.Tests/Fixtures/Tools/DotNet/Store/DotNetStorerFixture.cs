// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Store;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Store
{
    internal sealed class DotNetStorerFixture : DotNetFixture<DotNetStoreSettings>
    {
        public ICollection<FilePath> ManifestFiles { get; set; }
        public string Framework { get; set; }
        public string Runtime { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetStorer(FileSystem, Environment, ProcessRunner, Tools);
            tool.Store(ManifestFiles, Framework, Runtime, Settings);
        }
    }
}