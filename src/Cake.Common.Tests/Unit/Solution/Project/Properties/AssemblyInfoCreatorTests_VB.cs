// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cake.Common.Solution.Project.Properties;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Solution.Project.Properties
{
    public sealed class AssemblyInfoCreatorTests_VB
    {
        public sealed class TheCreateMethod
        {
            [Fact]
            public void Should_Throw_If_Output_Path_Is_Null()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                var creator = new AssemblyInfoCreator(fixture.FileSystem, fixture.Environment, fixture.Log);

                // When
                var result = Record.Exception(() => creator.Create(null, new AssemblyInfoSettings()));

                // Then
                AssertEx.IsArgumentNullException(result, "outputPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                var creator = new AssemblyInfoCreator(fixture.FileSystem, fixture.Environment, fixture.Log);

                // When
                var result = Record.Exception(() => creator.Create("A.cs", null));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Make_Relative_Output_Path_Absolute()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                var creator = new AssemblyInfoCreator(fixture.FileSystem, fixture.Environment, fixture.Log);

                // When
                creator.Create("AssemblyInfo.vb", new AssemblyInfoSettings());

                // Then
                Assert.True(fixture.FileSystem.Exist((FilePath)"/Working/AssemblyInfo.vb"));
            }

            [Fact]
            public void Should_Add_Title_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Title = "TheTitle";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyTitle(\"TheTitle\")>"));
            }

            [Fact]
            public void Should_Add_Description_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Description = "TheDescription";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyDescription(\"TheDescription\")>"));
            }

            [Fact]
            public void Should_Add_Guid_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Guid = "TheGuid";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Runtime.InteropServices"));
                Assert.True(result.Contains("<Assembly: Guid(\"TheGuid\")>"));
            }

            [Fact]
            public void Should_Add_Company_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Company = "TheCompany";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyCompany(\"TheCompany\")>"));
            }

            [Fact]
            public void Should_Add_Product_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Product = "TheProduct";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyProduct(\"TheProduct\")>"));
            }

            [Fact]
            public void Should_Add_Copyright_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Copyright = "TheCopyright";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyCopyright(\"TheCopyright\")>"));
            }

            [Fact]
            public void Should_Add_Trademark_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Trademark = "TheTrademark";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyTrademark(\"TheTrademark\")>"));
            }

            [Fact]
            public void Should_Add_Version_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Version = "TheVersion";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyVersion(\"TheVersion\")>"));
            }

            [Fact]
            public void Should_Add_FileVersion_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.FileVersion = "TheFileVersion";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyFileVersion(\"TheFileVersion\")>"));
            }

            [Fact]
            public void Should_Add_InformationalVersion_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.InformationalVersion = "TheInformationalVersion";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyInformationalVersion(\"TheInformationalVersion\")>"));
            }

            [Fact]
            public void Should_Add_ComVisible_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.ComVisible = true;

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Runtime.InteropServices"));
                Assert.True(result.Contains("<Assembly: ComVisible(true)>"));
            }

            [Fact]
            public void Should_Add_CLSCompliant_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.CLSCompliant = true;

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System"));
                Assert.True(result.Contains("<Assembly: CLSCompliant(true)>"));
            }

            [Fact]
            public void Should_Add_InternalsVisibleTo_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.InternalsVisibleTo = new List<string> { "Assembly1.Tests" };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Runtime.CompilerServices"));
                Assert.True(result.Contains("<Assembly: InternalsVisibleTo(\"Assembly1.Tests\")>"));
            }

            [Fact]
            public void Should_Add_Multiple_InternalsVisibleTo_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.InternalsVisibleTo = new Collection<string> { "Assembly1.Tests", "Assembly2.Tests", "Assembly3.Tests" };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Runtime.CompilerServices"));
                Assert.True(result.Contains("<Assembly: InternalsVisibleTo(\"Assembly1.Tests\")>"));
                Assert.True(result.Contains("<Assembly: InternalsVisibleTo(\"Assembly2.Tests\")>"));
                Assert.True(result.Contains("<Assembly: InternalsVisibleTo(\"Assembly3.Tests\")>"));
            }

            [Fact]
            public void Should_Add_Configuration_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.Configuration = "TheConfiguration";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports System.Reflection"));
                Assert.True(result.Contains("<Assembly: AssemblyConfiguration(\"TheConfiguration\")>"));
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = "TestValue" } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.True(result.Contains("Imports Test.NameSpace"));
                Assert.True(result.Contains("<Assembly: TestAttribute(\"TestValue\")>"));
            }
        }
    }
}