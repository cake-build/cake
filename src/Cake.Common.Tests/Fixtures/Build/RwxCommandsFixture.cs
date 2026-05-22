// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Rwx.Commands;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class RwxCommandsFixture
    {
        public RwxInfoFixture InfoFixture { get; }

        public ICakeEnvironment Environment => InfoFixture.Environment;

        public FakeFileSystem FileSystem { get; }

        public RwxCommandsFixture()
        {
            InfoFixture = new RwxInfoFixture();
            Environment.WorkingDirectory.Returns("/work");
            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateDirectory("/rwx/values");
            FileSystem.CreateDirectory("/rwx/artifacts");
            FileSystem.CreateDirectory("/rwx/env");
        }

        public RwxCommands CreateRwxCommands()
        {
            return new RwxCommands(Environment, FileSystem, InfoFixture.CreateEnvironmentInfo());
        }

        public RwxCommandsFixture WithNoRwxValues()
        {
            Environment.GetEnvironmentVariable("RWX_VALUES").Returns(null as string);
            return this;
        }

        public RwxCommandsFixture WithNoRwxArtifacts()
        {
            Environment.GetEnvironmentVariable("RWX_ARTIFACTS").Returns(null as string);
            return this;
        }

        public RwxCommandsFixture WithNoRwxEnv()
        {
            Environment.GetEnvironmentVariable("RWX_ENV").Returns(null as string);
            return this;
        }
    }
}
