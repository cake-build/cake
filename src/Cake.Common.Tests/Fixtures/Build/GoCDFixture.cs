// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GoCD;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GoCDFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public GoCDFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.GetEnvironmentVariable("https://127.0.0.1:8154/go").Returns((string)null);
        }

        public void IsRunningOnGoCD()
        {
            Environment.GetEnvironmentVariable("GO_SERVER_URL").Returns("https://127.0.0.1:8154/go");
        }

        public GoCDProvider CreateGoCDService()
        {
            return new GoCDProvider(Environment);
        }
    }
}