// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.Chocolatey.Features;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Features
{
    internal abstract class ChocolateyFeatureTogglerFixture : ChocolateyFixture<ChocolateyFeatureSettings>
    {
        public string Name { get; set; }

        protected ChocolateyFeatureTogglerFixture()
        {
            Name = "checkSumFiles";
        }
    }
}
