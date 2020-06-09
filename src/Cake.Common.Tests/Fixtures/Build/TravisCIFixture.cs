// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TravisCI;
using Cake.Common.Tests.Fakes;
using Cake.Core;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class TravisCIFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public FakeBuildSystemServiceMessageWriter Writer { get; set; }

        public TravisCIFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/home/travis/.local");
            Writer = new FakeBuildSystemServiceMessageWriter();
        }

        public void IsRunningOnTravisCI()
        {
            Environment.GetEnvironmentVariable("TRAVIS").Returns("true");
        }

        public TravisCIProvider CreateTravisCIProvider()
        {
            return new TravisCIProvider(Environment, Writer);
        }
    }
}