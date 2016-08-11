// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.NuGet.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Sources
{
    internal sealed class NuGetListFixture : NuGetSourcesFixture
    {
        protected override void RunTool()
        {
            var tool = new NuGetList(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.List(Source, Settings);
        }
    }
}