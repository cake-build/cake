// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public bool? ClsCompliant { get; set; }
        public bool? ComVisible { get; set; }
        public string Company { get; set; }
        public string Configuration { get; set; }
        public string Copyright { get; set; }
        public string Description { get; set; }
        public string FileVersion { get; set; }
        public string Guid { get; set; }
        public string InformationalVersion { get; set; }
        public List<string> InternalsVisibleTo { get; set; }
        public string Product { get; set; }
        public string Title { get; set; }
        public string Trademark { get; set; }
        public string Version { get; set; }

        public FilePath Path { get; set; }
        public bool CreateAssemblyInfo { get; set; }
        public bool ExtraWhiteSpaces { get; set; }

        public AssemblyInfoParserFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");
            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateDirectory(Environment.WorkingDirectory);

            // Set fixture values.
            Path = new FilePath("./output.cs");
            CreateAssemblyInfo = true;
        }

        public void WithAssemblyInfoContents(string assemblyInfoContents)
        {
            FileSystem.CreateFile("/Working/output.cs").SetContent(assemblyInfoContents);
        }

        public AssemblyInfoParseResult Parse()
        {
            if (CreateAssemblyInfo && Path != null)
            {
                CreateAssemblyInfoOnDisk(Path);
            }
            var parser = new AssemblyInfoParser(FileSystem, Environment);
            return parser.Parse(Path);
        }

        private void CreateAssemblyInfoOnDisk(FilePath path)
        {
            var settings = new AssemblyInfoSettings();
            settings.CLSCompliant = ClsCompliant;
            settings.ComVisible = ComVisible;

            if (Company != null)
            {
                settings.Company = Company;
            }
            if (Configuration != null)
            {
                settings.Configuration = Configuration;
            }
            if (Copyright != null)
            {
                settings.Copyright = Copyright;
            }
            if (Description != null)
            {
                settings.Description = Description;
            }
            if (FileVersion != null)
            {
                settings.FileVersion = FileVersion;
            }
            if (Guid != null)
            {
                settings.Guid = Guid;
            }
            if (InformationalVersion != null)
            {
                settings.InformationalVersion = InformationalVersion;
            }
            if (InternalsVisibleTo != null)
            {
                settings.InternalsVisibleTo = InternalsVisibleTo;
            }
            if (Product != null)
            {
                settings.Product = Product;
            }
            if (Title != null)
            {
                settings.Title = Title;
            }
            if (Trademark != null)
            {
                settings.Trademark = Trademark;
            }
            if (Version != null)
            {
                settings.Version = Version;
            }

            var creator = new AssemblyInfoCreator(FileSystem, Environment, Substitute.For<ICakeLog>());
            if (ExtraWhiteSpaces)
            {
                creator.Create(path, settings, "[assembly: {0} ]",
                    "[assembly: {0}( {1} )]",
                    "[assembly: {0}( {1}, {2} )]");
            }
            else
            {
                creator.Create(path, settings);
            }
        }
    }
}