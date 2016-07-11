// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.InspectCode;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.InspectCode
{
    internal abstract class InspectCodeFixture : ToolFixture<InspectCodeSettings>
    {
        protected InspectCodeFixture()
            : base("inspectcode.exe")
        {
        }
    }
}
