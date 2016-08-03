﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DNU.Build;

namespace Cake.Common.Tests.Fixtures.Tools.DNU.Build
{
    internal sealed class DNUBuilderFixture : DNUFixture<DNUBuildSettings>
    {
        public string Path { get; set; }

        protected override void RunTool()
        {
            var tool = new DNUBuilder(FileSystem, Environment, ProcessRunner, Tools);
            tool.Build(Path, Settings);
        }
    }
}