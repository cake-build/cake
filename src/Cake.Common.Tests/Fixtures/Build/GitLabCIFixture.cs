// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI;
using Cake.Core;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GitLabCIFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public GitLabCIFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.GetEnvironmentVariable("CI_SERVER").Returns((string)null);
        }

        public void IsRunningOnGitLabCI()
        {
            Environment.GetEnvironmentVariable("CI_SERVER").Returns("yes");
        }

        public GitLabCIProvider CreateGitLabCIService()
        {
            return new GitLabCIProvider(Environment);
        }
    }
}