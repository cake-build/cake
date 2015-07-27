﻿using Cake.Common.Solution.Project.Properties;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class AssemblyInfoParserFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }

        public AssemblyInfoParserFixture(string version = "1.2.3.4", 
            string fileVersion = "4.3.2.1", 
            string informationalVersion = "4.2.3.1", 
            bool createAssemblyInfo = true)
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateDirectory(Environment.WorkingDirectory);

            if (createAssemblyInfo)
            {
                // Set the versions.
                var settings = new AssemblyInfoSettings();
                if (version != null)
                {
                    settings.Version = version;
                }
                if (fileVersion != null)
                {
                    settings.FileVersion = fileVersion;
                }
                if (informationalVersion != null)
                {
                    settings.InformationalVersion = informationalVersion;
                }

                // Create the assembly info.
                var creator = new AssemblyInfoCreator(FileSystem, Environment, Substitute.For<ICakeLog>());
                creator.Create("./output.cs", settings);
            }
        }

        public AssemblyInfoParseResult Parse()
        {
            return Parse("./output.cs");
        }

        public AssemblyInfoParseResult Parse(FilePath filePath)
        {
            var parser = new AssemblyInfoParser(FileSystem, Environment);
            return parser.Parse(filePath);
        }
    }
}
