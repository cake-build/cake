// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.TeamCity;
using Cake.Core;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class TeamCityFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public FakeLog Log { get; set; }

        public TeamCityFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("C:\\build\\CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("TEAMCITY_VERSION").Returns((string)null);
            Log = new FakeLog();
        }

        public void IsRunningOnTeamCity()
        {
            Environment.GetEnvironmentVariable("TEAMCITY_VERSION").Returns("9.1.6");
        }

        public TeamCityProvider CreateTeamCityService()
        {
            return new TeamCityProvider(Environment, Log);
        }
    }
}
