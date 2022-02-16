// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet;
using Cake.Common.Tools.DotNet;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore
{
    internal abstract class DotNetCoreFixture<TSettings> : DotNetFixture<TSettings>
        where TSettings : DotNetSettings, new()
    {
    }
}