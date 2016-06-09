// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal abstract class GitReleaseManagerFixture<TSettings> : ToolFixture<TSettings>
        where TSettings : ToolSettings, new()
    {
        public ICakeLog Log { get; set; }

        protected GitReleaseManagerFixture()
            : base("GitReleaseManager.exe")
        {
            Log = Substitute.For<ICakeLog>();
        }
    }
}
