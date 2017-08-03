// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Solution.Project;
using Cake.Common.Tests.Properties;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Solution.Project
{
    internal sealed class ProjectParserFixture
    {
        public ProjectParserFixture()
        {
            ProjFilePath = "/Working/Cake.Sample.csproj";
            Pattern = "/Working/Cake.*.csproj";

            var environment = FakeEnvironment.CreateUnixEnvironment();
            Environment = environment;
            var fileSystem = new FakeFileSystem(environment);
            fileSystem.CreateFile(ProjFilePath.FullPath).SetContent(Resources.Csproj_ProjectFile);
            fileSystem.CreateFile("/Working/Cake.Incomplete.csproj").SetContent(Resources.Csproj_IncompleteFile);
            fileSystem.CreateFile("/Working/Cake.ContainsWildcard.csproj").SetContent(Resources.Csproj_With_WildCard_FilePaths);
            FileSystem = fileSystem;

            Globber = Substitute.For<IGlobber>();
            Globber.GetFiles(Pattern).Returns(new FilePath[] { "/Working/Cake.Sample.csproj", "/Working/Cake.Incomplete.csproj" });
            Globber.Match("**\\*.cs", Arg.Any<Func<IDirectory, bool>>()).Returns(new FilePath[] { "/Working/Program.cs", "/Working/Client.cs" });

            Log = Substitute.For<ICakeLog>();
        }

        public ICakeEnvironment Environment { get; set; }

        public ProjectParserResult Parse()
        {
            var parser = new ProjectParser(this.FileSystem, this.Environment, this.Globber);
            return parser.Parse(ProjFilePath);
        }

        public ProjectParserResult ParseIncomplete()
        {
            var parser = new ProjectParser(this.FileSystem, this.Environment, this.Globber);
            return parser.Parse("/Working/Cake.Incomplete.csproj");
        }

        public ProjectParserResult ParseWithWildcardFilePaths()
        {
            var parser = new ProjectParser(this.FileSystem, this.Environment, this.Globber);
            return parser.Parse("/Working/Cake.ContainsWildcard.csproj");
        }

        public string Pattern { get; set; }
        public IFileSystem FileSystem { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeLog Log { get; set; }
        public FilePath ProjFilePath { get; set; }
    }
}