// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.WoodpeckerCI.Commands;
using Cake.Common.Build.WoodpeckerCI.Data;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class WoodpeckerCICommandsFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IFileSystem FileSystem { get; set; }
        public WoodpeckerCIEnvironmentInfo WoodpeckerEnvironment { get; set; }

        public WoodpeckerCICommandsFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);

            // Set up the WoodpeckerCI environment variables
            ((FakeEnvironment)Environment).SetEnvironmentVariable("CI", "woodpecker");
            ((FakeEnvironment)Environment).SetEnvironmentVariable("CI_WORKSPACE", "/woodpecker/src/git.example.com/john-doe/my-repo");

            WoodpeckerEnvironment = new WoodpeckerCIEnvironmentInfo(Environment);

            // Create the .woodpecker/env file for testing
            var envFile = WoodpeckerEnvironment.Workspace?.CombineWithFilePath(".woodpecker/env");
            if (envFile != null)
            {
                // Create the directory structure first
                var directory = envFile.GetDirectory();
                ((FakeFileSystem)FileSystem).CreateDirectory(directory);
                ((FakeFileSystem)FileSystem).CreateFile(envFile);
            }
        }

        public WoodpeckerCICommands CreateWoodpeckerCICommands()
        {
            return new WoodpeckerCICommands(Environment, FileSystem, WoodpeckerEnvironment);
        }

        public WoodpeckerCICommandsFixture WithNoWoodpeckerEnv()
        {
            var envFile = Environment.WorkingDirectory.CombineWithFilePath(".woodpecker/env");
            if (FileSystem.Exist(envFile))
            {
                FileSystem.GetFile(envFile).Delete();
            }
            return this;
        }
    }
}
