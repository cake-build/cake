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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyTitle(\"TheTitle\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyDescription(\"TheDescription\")>", result);
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
                Assert.Contains("Imports System.Runtime.InteropServices", result);
                Assert.Contains("<Assembly: Guid(\"TheGuid\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyCompany(\"TheCompany\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyProduct(\"TheProduct\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyCopyright(\"TheCopyright\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyTrademark(\"TheTrademark\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyVersion(\"TheVersion\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyFileVersion(\"TheFileVersion\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyInformationalVersion(\"TheInformationalVersion\")>", result);
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
                Assert.Contains("Imports System.Runtime.InteropServices", result);
                Assert.Contains("<Assembly: ComVisible(True)>", result);
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
                Assert.Contains("Imports System", result);
                Assert.Contains("<Assembly: CLSCompliant(True)>", result);
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
                Assert.Contains("Imports System.Runtime.CompilerServices", result);
                Assert.Contains("<Assembly: InternalsVisibleTo(\"Assembly1.Tests\")>", result);
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
                Assert.Contains("Imports System.Runtime.CompilerServices", result);
                Assert.Contains("<Assembly: InternalsVisibleTo(\"Assembly1.Tests\")>", result);
                Assert.Contains("<Assembly: InternalsVisibleTo(\"Assembly2.Tests\")>", result);
                Assert.Contains("<Assembly: InternalsVisibleTo(\"Assembly3.Tests\")>", result);
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
                Assert.Contains("Imports System.Reflection", result);
                Assert.Contains("<Assembly: AssemblyConfiguration(\"TheConfiguration\")>", result);
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
                Assert.Contains("Imports Test.NameSpace", result);
                Assert.Contains("<Assembly: TestAttribute(\"TestValue\")>", result);
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set_With_Boolean_Value()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = true } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("Imports Test.NameSpace", result);
                Assert.Contains("<Assembly: TestAttribute(True)>", result);
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set_With_Null_Value()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = null } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("Imports Test.NameSpace", result);
                Assert.Contains("<Assembly: TestAttribute()>", result);
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set_With_Empty_Value()
            {
                // Given
                var fixture = new AssemblyInfoFixture_VB();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = string.Empty } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("Imports Test.NameSpace", result);
                Assert.Contains("<Assembly: TestAttribute()>", result);
            }
        }
    }
}