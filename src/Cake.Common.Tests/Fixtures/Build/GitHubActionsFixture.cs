// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitHubActions;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class GitHubActionsFixture
    {
        public ICakeEnvironment Environment { get; }
        public IFileSystem FileSystem { get; }

        public GitHubActionsFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS").Returns((string)null);
            Environment.WorkingDirectory.Returns("/home/cake");
            FileSystem = new FakeFileSystem(Environment);
        }

        public void IsRunningOnGitHubActions()
        {
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS").Returns("true");
        }

        public GitHubActionsProvider CreateGitHubActionsService()
        {
            return new GitHubActionsProvider(Environment, FileSystem);
        }
    }
}
