// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Polyfill;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class DirectoryCleanerSymlinkTests
    {
        [Fact]
        public void Should_Clean_Directory_Containing_Broken_Symlink_On_Unix()
        {
            if (OperatingSystem.IsWindows())
            {
                return;
            }

            var tempPath = Path.Combine(Path.GetTempPath(), "cake-clean-symlink-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempPath);

            try
            {
                var versions = Path.Combine(tempPath, "Versions");
                Directory.CreateDirectory(versions);
                File.CreateSymbolicLink(
                    Path.Combine(versions, "Current"),
                    Path.Combine(versions, "missing-target"));
                File.WriteAllText(Path.Combine(tempPath, "file.txt"), "x");

                var environment = new CakeEnvironment(new CakePlatform(), new CakeRuntime());
                var fileSystem = new FileSystem();
                var globber = new Globber(fileSystem, environment);
                var context = new CakeContextFixture
                {
                    FileSystem = fileSystem,
                    Environment = environment,
                    Globber = globber
                }.CreateContext();

                var exception = Record.Exception(() =>
                    DirectoryAliases.CleanDirectory(context, new DirectoryPath(tempPath)));

                Assert.Null(exception);
                Assert.False(File.Exists(Path.Combine(tempPath, "file.txt")));
                Assert.False(Directory.Exists(Path.Combine(versions, "Current")));
            }
            finally
            {
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
            }
        }
    }
}
