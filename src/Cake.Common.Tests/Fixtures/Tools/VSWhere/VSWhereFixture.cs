// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.VSWhere
{
    internal abstract class VSWhereFixture<TSettings> : VSWhereFixture<TSettings, ToolFixtureResult>
        where TSettings : ToolSettings, new()
    {
        protected VSWhereFixture()
            : base()
        {
        }

        protected VSWhereFixture(bool is64BitOperativeSystem)
            : base(is64BitOperativeSystem)
        {
        }

        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }

    internal abstract class VSWhereFixture<TSettings, TFixtureResult> : ToolFixture<TSettings, TFixtureResult>
        where TSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        public ICakeLog Log { get; set; }

        protected VSWhereFixture()
            : this(true)
        {
        }

        protected VSWhereFixture(bool is64BitOperativeSystem)
            : base("vswhere.exe")
        {
            ProcessRunner.Process.SetStandardOutput(new string[] { });
            Log = Substitute.For<ICakeLog>();

            // Prepare the environment.
            Environment.SetSpecialPath(SpecialPath.ProgramFilesX86, "/Program86");
            Environment.SetSpecialPath(SpecialPath.ProgramFiles, "/Program");
            Environment.ChangeOperativeSystemBitness(is64BitOperativeSystem);
        }

        protected override FilePath GetDefaultToolPath(string toolFilename)
        {
            return Environment.Platform.Is64Bit
                ? new FilePath("/Program86/Microsoft Visual Studio/Installer/vswhere.exe")
                : new FilePath("/Program/Microsoft Visual Studio/Installer/vswhere.exe");
        }
    }
}
