// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.IO;
using Cake.Common.Tools.GitVersion;
using Cake.Core.Diagnostics;
using NSubstitute;
using System.Runtime.Serialization.Json;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class GitVersionRunnerFixture : ToolFixture<GitVersionSettings>
    {
        public ICakeLog Log { get; set; }

        public GitVersionRunnerFixture()
             : base("GitVersion.exe")
        {
            var resultJson = new GitVersion
            {
                Major = 1,
                Minor = 0,
                Patch = 0
            };

            var serializer = new DataContractJsonSerializer(typeof(GitVersion));
            using (var memoryStream = new MemoryStream())
            using (var reader = new StreamReader(memoryStream))
            {
                serializer.WriteObject(memoryStream, resultJson);
                memoryStream.Position = 0;
                var output = new List<string>();
                while (!reader.EndOfStream)
                {
                    output.Add(reader.ReadLine());
                }

                ProcessRunner.Process.SetStandardOutput(output);
            }

            Log = Substitute.For<ICakeLog>();
        }

        protected override void RunTool()
        {
            var tool = new GitVersionRunner(FileSystem, Environment, ProcessRunner, Tools, Log);
            tool.Run(Settings);
        }
    }
}
