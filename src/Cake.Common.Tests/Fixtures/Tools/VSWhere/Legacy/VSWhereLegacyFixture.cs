// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.VSWhere.Legacy;

namespace Cake.Common.Tests.Fixtures.Tools.VSWhere.Legacy
{
    internal sealed class VSWhereLegacyFixture : VSWhereFixture<VSWhereLegacySettings>
    {
        protected override void RunTool()
        {
            var tool = new VSWhereLegacy(FileSystem, Environment, ProcessRunner, Tools);
            tool.Legacy(Settings);
        }
    }
}
