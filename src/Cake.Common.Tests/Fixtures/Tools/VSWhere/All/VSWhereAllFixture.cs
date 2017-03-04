// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.VSWhere.All;

namespace Cake.Common.Tests.Fixtures.Tools.VSWhere.All
{
    internal sealed class VSWhereAllFixture : VSWhereFixture<VSWhereAllSettings>
    {
        protected override void RunTool()
        {
            var tool = new VSWhereAll(FileSystem, Environment, ProcessRunner, Tools);
            tool.All(Settings);
        }
    }
}
