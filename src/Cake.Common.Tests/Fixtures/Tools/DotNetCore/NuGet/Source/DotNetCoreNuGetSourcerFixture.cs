﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.NuGet.Source;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.NuGet.Source
{
    internal abstract class DotNetCoreNuGetSourcerFixture : DotNetCoreFixture<DotNetCoreNuGetSourceSettings>
    {
        public string Name { get; set; } = "name";
    }
}