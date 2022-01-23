// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.SDKCheck;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.SDKCheck
{
    internal sealed class DotNetSDKCheckerFixture : DotNetFixture<DotNetSDKCheckSettings>
    {
        protected override void RunTool()
        {
            var tool = new DotNetSDKChecker(FileSystem, Environment, ProcessRunner, Tools);
            tool.Check();
        }
    }
}
