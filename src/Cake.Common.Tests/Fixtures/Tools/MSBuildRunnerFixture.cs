// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Common.Tools.MSBuild;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class MSBuildRunnerFixture : ToolFixture<MSBuildSettings>
    {
        public HashSet<FilePath> KnownMSBuildPaths { get; private set; }
        public FilePath Solution { get; set; }

        public MSBuildRunnerFixture(bool is64BitOperativeSystem)
            : base("MSBuild.exe")
        {
            // Create the list of all known MSBuild paths.
            KnownMSBuildPaths = new HashSet<FilePath>(new PathComparer(false));
            KnownMSBuildPaths.Add("/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe");
            KnownMSBuildPaths.Add("/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe");
            KnownMSBuildPaths.Add("/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe");
            KnownMSBuildPaths.Add("/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe");
            KnownMSBuildPaths.Add("/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe");
            KnownMSBuildPaths.Add("/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe");
            KnownMSBuildPaths.Add("/Program86/MSBuild/12.0/Bin/MSBuild.exe");
            KnownMSBuildPaths.Add("/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe");
            KnownMSBuildPaths.Add("/Program86/MSBuild/14.0/Bin/MSBuild.exe");
            KnownMSBuildPaths.Add("/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe");

            // Install all known MSBuild versions.
            foreach (var msBuildPath in KnownMSBuildPaths)
            {
                FileSystem.CreateFile(msBuildPath);
            }

            // Prepare the environment.
            Environment.SetSpecialPath(SpecialPath.ProgramFilesX86, "/Program86");
            Environment.SetSpecialPath(SpecialPath.Windows, "/Windows");
            Environment.ChangeOperativeSystemBitness(is64BitOperativeSystem);

            // Prepare the tool parameters.
            Solution = new FilePath("./src/Solution.sln");
            Settings.ToolVersion = MSBuildToolVersion.VS2013;
        }

        public void GivenMSBuildIsNotInstalled()
        {
            foreach (var msBuildPath in KnownMSBuildPaths)
            {
                var msBuildFile = FileSystem.GetFile(msBuildPath);
                if (msBuildFile.Exists)
                {
                    msBuildFile.Delete();
                }
            }
            FileSystem.GetDirectory("/Windows").Delete(true);
            FileSystem.GetDirectory("/Program86").Delete(true);
        }

        protected override void RunTool()
        {
            var runner = new MSBuildRunner(FileSystem, Environment, ProcessRunner, Tools);
            runner.Run(Solution, Settings);
        }
    }
}
