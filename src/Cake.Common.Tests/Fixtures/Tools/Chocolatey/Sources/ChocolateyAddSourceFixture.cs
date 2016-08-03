﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.Chocolatey.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Sources
{
    internal sealed class ChocolateyAddSourceFixture : ChocolateySourcesFixture
    {
        public string Source { get; set; }

        public ChocolateyAddSourceFixture()
        {
            Source = "source";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateySources(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.AddSource(Name, Source, Settings);
        }
    }
}