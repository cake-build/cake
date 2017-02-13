// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.NuGet.Add;
using Cake.Common.Tools.NuGet.Init;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Init
{
    internal sealed class NuGetIniterFixture : NuGetFixture<NuGetInitSettings>
    {
        public string Source { get; set; }
        public string Destination { get; set; }

        public NuGetIniterFixture()
        {
            Source = "/Working/NuGet/localfeed";
            Destination = "/Working/NuGet/localfeed-destination";
        }

        protected override void RunTool()
        {
            var tool = new NuGetIniter(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Init(Source, Destination, Settings);
        }
    }
}
