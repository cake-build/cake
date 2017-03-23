// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.VSWhere.Latest;

namespace Cake.Common.Tests.Fixtures.Tools.VSWhere.Latest
{
    internal sealed class VSWhereLatestFixture : VSWhereFixture<VSWhereLatestSettings>
    {
        protected override void RunTool()
        {
            var tool = new VSWhereLatest(FileSystem, Environment, ProcessRunner, Tools);
            tool.Latest(Settings);
        }
    }
}
