using System.Collections.Generic;
using Cake.Common.Solution.Project.Properties;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class AssemblyInfoParserFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }

        public AssemblyInfoParserFixture(bool clsCompliant = false,
            string company = "Company",
            bool comVisible = false,
            string configuration = "Debug",
            string copyright = "Copyright 2015",
            string description = "Description",
            string fileVersion = "4.3.2.1",
            string guid = "ABCEDF",
            string informationalVersion = "4.2.3.1",
            string internalsVisibleTo = "Cake.Common.Test",
            string product = "Cake",
            string title = "Cake",
            string trademark = "Trademark",
            string version = "1.2.3.4",
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
                settings.CLSCompliant = clsCompliant;

                if (company != null)
                {
                    settings.Company = company;
                }

                settings.ComVisible = comVisible;

                if (configuration != null)
                {
                    settings.Configuration = configuration;
                }
                if (copyright != null)
                {
                    settings.Copyright = copyright;
                }
                if (description != null)
                {
                    settings.Description = description;
                }
                if (fileVersion != null)
                {
                    settings.FileVersion = fileVersion;
                }
                if (guid != null)
                {
                    settings.Guid = guid;
                }
                if (informationalVersion != null)
                {
                    settings.InformationalVersion = informationalVersion;
                }
                if (internalsVisibleTo != null)
                {
                    settings.InternalsVisibleTo = new List<string>() { internalsVisibleTo };
                }
                if (product != null)
                {
                    settings.Product = product;
                }
                if (title != null)
                {
                    settings.Title = title;
                }
                if (trademark != null)
                {
                    settings.Trademark = trademark;
                }
                if (version != null)
                {
                    settings.Version = version;
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