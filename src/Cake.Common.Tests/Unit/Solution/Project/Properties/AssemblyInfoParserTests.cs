// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Common.Solution.Project.Properties;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Properties;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution.Project.Properties
{
    public sealed class AssemblyInfoParserTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new AssemblyInfoParser(null, environment));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new AssemblyInfoParser(fileSystem, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheParseMethod
        {
            [Fact]
            public void Should_Throw_If_AssemblyInfo_Path_Is_Null()
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Path = null;

                // When
                var result = Record.Exception(() => fixture.Parse());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyInfoPath");
            }

            [Fact]
            public void Should_Throw_If_AssemblyInfo_File_Do_Not_Exist()
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.CreateAssemblyInfo = false;

                // When
                var result = Record.Exception(() => fixture.Parse());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Assembly info file '/Working/output.cs' does not exist.", result?.Message);
            }

            [Theory]
            [InlineData(true, true)]
            [InlineData(false, false)]
            [InlineData(null, false)]
            public void Should_Read_ClsCompliance(bool value, bool expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.ClsCompliant = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.ClsCompliant);
            }

            [Theory]
            [InlineData("CompanyA", "CompanyA")]
            [InlineData(null, "")]
            public void Should_Read_Company(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Company = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Company);
            }

            [Theory]
            [InlineData(true, true)]
            [InlineData(false, false)]
            [InlineData(null, false)]
            public void Should_Read_ComVisible(bool value, bool expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.ComVisible = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.ComVisible);
            }

            [Theory]
            [InlineData("Debug", "Debug")]
            [InlineData("Release", "Release")]
            [InlineData(null, "")]
            public void Should_Read_Configuration(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Configuration = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Configuration);
            }

            [Theory]
            [InlineData("Copyright (c) Patrik Svensson, Mattias Karlsson, Gary Ewan Park, Alistair Chapman, Martin Björkström and contributors", "Copyright (c) Patrik Svensson, Mattias Karlsson, Gary Ewan Park, Alistair Chapman, Martin Björkström and contributors")]
            [InlineData(null, "")]
            public void Should_Read_Copyright(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Copyright = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Copyright);
            }

            [Theory]
            [InlineData("Assembly Description", "Assembly Description")]
            [InlineData(null, "")]
            public void Should_Read_Description(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Description = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Description);
            }

            [Theory]
            [InlineData("1.2.3.4", "1.2.3.4")]
            [InlineData("1.2.*.*", "1.2.*.*")]
            [InlineData(null, "1.0.0.0")]
            public void Should_Read_AssemblyFileVersion(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.FileVersion = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.AssemblyFileVersion);
            }

            [Theory]
            [InlineData("D394B7DB-0DDC-4D11-AD69-C408212E1E80", "D394B7DB-0DDC-4D11-AD69-C408212E1E80")]
            [InlineData(null, "")]
            public void Should_Read_Guid(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Guid = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Guid);
            }

            [Theory]
            [InlineData("1.2.3.4", "1.2.3.4")]
            [InlineData("1.2.*.*", "1.2.*.*")]
            [InlineData("1.2.3-rc1", "1.2.3-rc1")]
            [InlineData(null, "1.0.0.0")]
            public void Should_Read_AssemblyInformationalVersion(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.InformationalVersion = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.AssemblyInformationalVersion);
            }

            [Fact]
            public void Should_Read_InternalsVisibleTo()
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.InternalsVisibleTo = new List<string>
                {
                    "Cake.Core.Tests",
                    "Cake.Common.Tests"
                };

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(2, result.InternalsVisibleTo.Count);
                Assert.Equal("Cake.Core.Tests", result.InternalsVisibleTo.ElementAt(0));
                Assert.Equal("Cake.Common.Tests", result.InternalsVisibleTo.ElementAt(1));
            }

            [Theory]
            [InlineData("Cake", "Cake")]
            [InlineData(null, "")]
            public void Should_Read_Product(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Product = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Product);
            }

            [Theory]
            [InlineData("Cake.Common", "Cake.Common")]
            [InlineData(null, "")]
            public void Should_Read_Title(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Title = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Title);
            }

            [Theory]
            [InlineData("Trademark Cake", "Trademark Cake")]
            [InlineData(null, "")]
            public void Should_Read_Trademark(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Trademark = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.Trademark);
            }

            [Theory]
            [InlineData("1.2.3.4", "1.2.3.4")]
            [InlineData("1.2.*.*", "1.2.*.*")]
            [InlineData(null, "1.0.0.0")]
            public void Should_Read_AssemblyVersion(string value, string expected)
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.Version = value;

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(expected, result.AssemblyVersion);
            }

            [Fact]
            public void Should_Correctly_Parse_VisualStudio_AssemblyInfo_File()
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.CreateAssemblyInfo = false;
                fixture.WithAssemblyInfoContents(Resources.VisualStudioAssemblyInfo.NormalizeLineEndings());

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.Title, "VisualStudioAssemblyTitle");
                Assert.Equal(result.Description, "VisualStudioAssemblyDescription");
                Assert.Equal(result.Configuration, "VisualStudioConfiguration");
                Assert.Equal(result.Company, "VisualStudioCompany");
                Assert.Equal(result.Product, "VisualStudioProduct");
                Assert.Equal(result.Copyright, "VisualStudioCopyright");
                Assert.Equal(result.Trademark, "VisualStudioTrademark");
            }

            [Fact]
            public void Should_Correctly_Parse_VisualStudio_VB_AssemblyInfo_File()
            {
                // Given
                var fixture = new AssemblyInfoParserFixture_VB();
                fixture.CreateAssemblyInfo = false;
                fixture.WithAssemblyInfoContents(Resources.VisualStudioAssemblyInfo_VB.NormalizeLineEndings());

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.Title, "VisualStudioAssemblyTitle");
                Assert.Equal(result.Description, "VisualStudioAssemblyDescription");
                Assert.Equal(result.Configuration, "VisualStudioConfiguration");
                Assert.Equal(result.Company, "VisualStudioCompany");
                Assert.Equal(result.Product, "VisualStudioProduct");
                Assert.Equal(result.Copyright, "VisualStudioCopyright");
                Assert.Equal(result.Trademark, "VisualStudioTrademark");
            }

            [Fact]
            public void Should_Correctly_Parse_MonoDevelop_AssemblyInfo_File()
            {
                // Given
                var fixture = new AssemblyInfoParserFixture();
                fixture.CreateAssemblyInfo = false;
                fixture.WithAssemblyInfoContents(Resources.MonoDevelopAssemblyInfo.NormalizeLineEndings());

                // When
                var result = fixture.Parse();

                // Then
                Assert.Equal(result.Title, "MonoDevelopAssemblyTitle");
                Assert.Equal(result.Description, "MonoDevelopAssemblyDescription");
                Assert.Equal(result.Configuration, "MonoDevelopConfiguration");
                Assert.Equal(result.Company, "MonoDevelopCompany");
                Assert.Equal(result.Product, "MonoDevelopProduct");
                Assert.Equal(result.Copyright, "MonoDevelopCopyright");
                Assert.Equal(result.Trademark, "MonoDevelopTrademark");
            }
        }
    }
}