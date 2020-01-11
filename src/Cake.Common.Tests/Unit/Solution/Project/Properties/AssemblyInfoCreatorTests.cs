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
    public sealed class AssemblyInfoCreatorTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var log = Substitute.For<ICakeLog>();

                // When
                var result = Record.Exception(() => new AssemblyInfoCreator(null, environment, log));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var log = Substitute.For<ICakeLog>();

                // When
                var result = Record.Exception(() => new AssemblyInfoCreator(fileSystem, null, log));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new AssemblyInfoCreator(fileSystem, environment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheCreateMethod
        {
            [Fact]
            public void Should_Throw_If_Output_Path_Is_Null()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
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
                var fixture = new AssemblyInfoFixture();
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
                var fixture = new AssemblyInfoFixture();
                var creator = new AssemblyInfoCreator(fixture.FileSystem, fixture.Environment, fixture.Log);

                // When
                creator.Create("AssemblyInfo.cs", new AssemblyInfoSettings());

                // Then
                Assert.True(fixture.FileSystem.Exist((FilePath)"/Working/AssemblyInfo.cs"));
            }

            [Fact]
            public void Should_Add_Title_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Title = "TheTitle";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyTitle(\"TheTitle\")]", result);
            }

            [Fact]
            public void Should_Add_Description_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Description = "TheDescription";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyDescription(\"TheDescription\")]", result);
            }

            [Fact]
            public void Should_Add_Guid_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Guid = "TheGuid";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Runtime.InteropServices;", result);
                Assert.Contains("[assembly: Guid(\"TheGuid\")]", result);
            }

            [Fact]
            public void Should_Add_Company_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Company = "TheCompany";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyCompany(\"TheCompany\")]", result);
            }

            [Fact]
            public void Should_Add_Product_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Product = "TheProduct";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyProduct(\"TheProduct\")]", result);
            }

            [Fact]
            public void Should_Add_Copyright_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Copyright = "TheCopyright";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyCopyright(\"TheCopyright\")]", result);
            }

            [Fact]
            public void Should_Add_Trademark_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Trademark = "TheTrademark";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyTrademark(\"TheTrademark\")]", result);
            }

            [Fact]
            public void Should_Add_Version_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Version = "TheVersion";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyVersion(\"TheVersion\")]", result);
            }

            [Fact]
            public void Should_Add_FileVersion_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.FileVersion = "TheFileVersion";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyFileVersion(\"TheFileVersion\")]", result);
            }

            [Fact]
            public void Should_Add_InformationalVersion_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.InformationalVersion = "TheInformationalVersion";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyInformationalVersion(\"TheInformationalVersion\")]", result);
            }

            [Fact]
            public void Should_Add_ComVisible_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.ComVisible = true;

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Runtime.InteropServices;", result);
                Assert.Contains("[assembly: ComVisible(true)]", result);
            }

            [Fact]
            public void Should_Add_CLSCompliant_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.CLSCompliant = true;

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System;", result);
                Assert.Contains("[assembly: CLSCompliant(true)]", result);
            }

            [Fact]
            public void Should_Add_InternalsVisibleTo_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.InternalsVisibleTo = new List<string> { "Assembly1.Tests" };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Runtime.CompilerServices;", result);
                Assert.Contains("[assembly: InternalsVisibleTo(\"Assembly1.Tests\")]", result);
            }

            [Fact]
            public void Should_Add_Multiple_InternalsVisibleTo_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.InternalsVisibleTo = new Collection<string> { "Assembly1.Tests", "Assembly2.Tests", "Assembly3.Tests" };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Runtime.CompilerServices;", result);
                Assert.Contains("[assembly: InternalsVisibleTo(\"Assembly1.Tests\")]", result);
                Assert.Contains("[assembly: InternalsVisibleTo(\"Assembly2.Tests\")]", result);
                Assert.Contains("[assembly: InternalsVisibleTo(\"Assembly3.Tests\")]", result);
            }

            [Fact]
            public void Should_Add_Configuration_Attribute_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.Configuration = "TheConfiguration";

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyConfiguration(\"TheConfiguration\")]", result);
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set_With_Raw_Value()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = "RawTestValue", UseRawValue = true } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using Test.NameSpace;", result);
                Assert.Contains("[assembly: TestAttribute(RawTestValue)]", result);
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = "TestValue" } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using Test.NameSpace;", result);
                Assert.Contains("[assembly: TestAttribute(\"TestValue\")]", result);
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set_With_Boolean_Value()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = true } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using Test.NameSpace;", result);
                Assert.Contains("[assembly: TestAttribute(true)]", result);
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set_With_Null_Value()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = null } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using Test.NameSpace;", result);
                Assert.Contains("[assembly: TestAttribute()]", result);
            }

            [Fact]
            public void Should_Add_CustomAttributes_If_Set_With_Empty_Value()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.CustomAttributes = new Collection<AssemblyInfoCustomAttribute> { new AssemblyInfoCustomAttribute { Name = "TestAttribute", NameSpace = "Test.NameSpace", Value = string.Empty } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using Test.NameSpace;", result);
                Assert.Contains("[assembly: TestAttribute()]", result);
            }

            [Fact]
            public void Should_Add_MetadataAttributes_If_Set()
            {
                // Given
                var fixture = new AssemblyInfoFixture();
                fixture.Settings.MetaDataAttributes = new Collection<AssemblyInfoMetadataAttribute> { new AssemblyInfoMetadataAttribute { Key = "Key1", Value = "TestValue1" }, new AssemblyInfoMetadataAttribute { Key = "Key2", Value = "TestValue2" }, new AssemblyInfoMetadataAttribute { Key = "Key1", Value = "TestValue3" } };

                // When
                var result = fixture.CreateAndReturnContent();

                // Then
                Assert.Contains("using System.Reflection;", result);
                Assert.Contains("[assembly: AssemblyMetadata(\"Key1\", \"TestValue3\")]", result);
                Assert.Contains("[assembly: AssemblyMetadata(\"Key2\", \"TestValue2\")]", result);
                Assert.DoesNotContain("[assembly: AssemblyMetadata(\"Key1\", \"TestValue1\")]", result);
            }
        }
    }
}