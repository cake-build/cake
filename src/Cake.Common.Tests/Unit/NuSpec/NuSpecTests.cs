// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.NuSpec;
using Cake.Common.Tests.Fixtures.NuSpec;
using Cake.Common.Tests.Properties;
using Cake.Core;
using Xunit;

namespace Cake.Common.Tests.Unit.NuSpec
{
    public sealed class NuSpecTests
    {
        public sealed class ForNuget
        {
            [Fact]
            public void Should_Add_Metadata_Element_To_Nuspec_If_Missing()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataElement);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.DevelopmentDependency = true;
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Language = "en-us";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Add_Repository_Element_To_Nuspec_If_Missing()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataElement);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.DevelopmentDependency = true;
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Language = "en-us";
                fixture.Settings.Repository = new NuSpecRepository { Url = "https://test", Branch = "master", Commit = "0000000000000000000000000000000000000000", Type = "git" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Repository.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.DevelopmentDependency = true;
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Language = "en-us";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_Without_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.DevelopmentDependency = true;
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Language = "en-us";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithoutNamespaces.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_Files()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.DevelopmentDependency = true;
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Language = "en-us";
                fixture.Settings.Files = new[]
                {
                    new NuSpecContent { Source = "Cake.Core.dll", Target = "lib/net45" },
                    new NuSpecContent { Source = "Cake.Core.xml", Target = "lib/net45" },
                    new NuSpecContent { Source = "Cake.Core.pdb", Target = "lib/net45" },
                    new NuSpecContent { Source = "LICENSE" }
                };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Test1", Version = "1.0.0" },
                    new NuSpecDependency { Id = "Test2", Version = "[1.0.0]" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithDependencies.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_Files_Without_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.DevelopmentDependency = true;
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Language = "en-us";
                fixture.Settings.Files = new[]
                {
                    new NuSpecContent { Source = "Cake.Core.dll", Target = "lib/net45" },
                    new NuSpecContent { Source = "Cake.Core.xml", Target = "lib/net45" },
                    new NuSpecContent { Source = "Cake.Core.pdb", Target = "lib/net45" },
                    new NuSpecContent { Source = "LICENSE" }
                };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Test1", Version = "1.0.0" },
                    new NuSpecDependency { Id = "Test2", Version = "[1.0.0]" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithoutNamespaces_WithDependencies.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_Files_And_DependencyTargetFramework()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.DevelopmentDependency = true;
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Language = "en-us";
                fixture.Settings.Files = new[]
                {
                    new NuSpecContent { Source = "Cake.Core.dll", Target = "lib/net45" },
                    new NuSpecContent { Source = "Cake.Core.xml", Target = "lib/net45" },
                    new NuSpecContent { Source = "Cake.Core.pdb", Target = "lib/net45" },
                    new NuSpecContent { Source = "LICENSE" }
                };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Test1", Version = "1.0.0", TargetFramework = "net452" },
                    new NuSpecDependency { Id = "Test2", Version = "[1.0.0]", TargetFramework = "net46" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithTragetFramworkDependencies.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_Files_And_DependencyTargetFramework_Without_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Description = "The description";
                fixture.Settings.Summary = "The summary";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.DevelopmentDependency = true;
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Language = "en-us";
                fixture.Settings.Files = new[]
                {
                    new NuSpecContent { Source = "Cake.Core.dll", Target = "lib/net45" },
                    new NuSpecContent { Source = "Cake.Core.xml", Target = "lib/net45" },
                    new NuSpecContent { Source = "Cake.Core.pdb", Target = "lib/net45" },
                    new NuSpecContent { Source = "LICENSE" }
                };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Test1", Version = "1.0.0", TargetFramework = "net452" },
                    new NuSpecDependency { Id = "Test2", Version = "[1.0.0]", TargetFramework = "net46" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithoutNamespaces_WithTargetFramworkDependencies.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Pack_If_Sufficient_Settings_For_MetaPackage_With_TargetFrameWork_Specified()
            {
                // Given
                var fixture = new NuSpecWithoutFileFixture();
                fixture.Settings.Id = "nonexisting";
                fixture.Settings.Version = "1.0.0";
                fixture.Settings.Description = "The description";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Files = null;
                fixture.Settings.Dependencies = new List<NuSpecDependency>
                {
                    new NuSpecDependency { Id = "Test1", Version = "1.0.0", TargetFramework = "net452" },
                    new NuSpecDependency { Id = "Test1", Version = "1.0.0", TargetFramework = "net46" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithTargetFrameworkDependencies.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_References()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                fixture.Settings.References = new[]
                {
                    new NuSpecReference { File = "Cake.Core.dll", TargetFramework = "net452" },
                    new NuSpecReference { File = "Cake.Core.dll", TargetFramework = "net46" },
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithReferences.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_References_Without_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                fixture.Settings.References = new[]
                {
                    new NuSpecReference { File = "Cake.Core.dll", TargetFramework = "net452" },
                    new NuSpecReference { File = "Cake.Core.dll", TargetFramework = "net46" },
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithoutNamespaces_WithReferences.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_PackageTypes()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                fixture.Settings.PackageTypes = new[]
                {
                    new NuSpecPackageType { Name = "Cake.Core", Version = "1.0.0" },
                    new NuSpecPackageType { Name = "Cake.Core", Version = "2.0.0" },
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithPackageTypes.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_PackageTypes_Without_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                fixture.Settings.PackageTypes = new[]
                {
                    new NuSpecPackageType { Name = "Cake.Core", Version = "1.0.0" },
                    new NuSpecPackageType { Name = "Cake.Core", Version = "2.0.0" },
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithoutNamespaces_WithPackageTypes.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_FrameworkAssemblies()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                fixture.Settings.FrameworkAssemblies = new[]
                {
                    new NuSpecFrameworkAssembly { AssemblyName = "Cake.Core.dll", TargetFramework = "net452" },
                    new NuSpecFrameworkAssembly { AssemblyName = "Cake.Core.dll", TargetFramework = "net46" },
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithFrameworkAssemblies.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_FrameworkAssemblies_Without_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                fixture.Settings.FrameworkAssemblies = new[]
                {
                    new NuSpecFrameworkAssembly { AssemblyName = "Cake.Core.dll", TargetFramework = "net452" },
                    new NuSpecFrameworkAssembly { AssemblyName = "Cake.Core.dll", TargetFramework = "net46" },
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithoutNamespaces_WithFrameworkAssemblies.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_ContentFiles()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                fixture.Settings.ContentFiles = new[]
                {
                    new NuSpecContentFile { Include = "*.Core.dll" },
                    new NuSpecContentFile { Include = "*.Core.pdb.dll" },
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithContentFiles.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_ContentFilesWithout_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.Nuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Version = "The version";
                fixture.Settings.Title = "The title";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };

                fixture.Settings.ContentFiles = new[]
                {
                    new NuSpecContentFile { Include = "*.Core.dll" },
                    new NuSpecContentFile { Include = "*.Core.pdb.dll" },
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.Nuspec_Metadata_WithoutNamespaces_WithContentFiles.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }
        }

        public sealed class ForChocolatey
        {
            [Fact]
            public void Should_Add_Metadata_Element_To_Nuspec_If_Missing()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.ChocolateyNuspec_NoMetadataElement);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Title = "The title";
                fixture.Settings.Version = "The version";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Summary = "The summary";
                fixture.Settings.Description = "The description";
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
                fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
                fixture.Settings.DocsUrl = new Uri("https://docs.com");
                fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
                fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                    new NuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.ChocolateyNuspec_Metadata.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.ChocolateyNuspec_NoMetadataValues);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Title = "The title";
                fixture.Settings.Version = "The version";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Summary = "The summary";
                fixture.Settings.Description = "The description";
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
                fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
                fixture.Settings.DocsUrl = new Uri("https://docs.com");
                fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
                fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                    new NuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.ChocolateyNuspec_Metadata.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_Without_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.ChocolateyNuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Title = "The title";
                fixture.Settings.Version = "The version";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Summary = "The summary";
                fixture.Settings.Description = "The description";
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
                fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
                fixture.Settings.DocsUrl = new Uri("https://docs.com");
                fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
                fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                    new NuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.ChocolateyNuspec_Metadata_WithoutNamespaces.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_Files()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.ChocolateyNuspec_NoMetadataValues);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Title = "The title";
                fixture.Settings.Version = "The version";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Summary = "The summary";
                fixture.Settings.Description = "The description";
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
                fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
                fixture.Settings.DocsUrl = new Uri("https://docs.com");
                fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
                fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Files = new[]
                {
                    new NuSpecContent { Source = @"tools\**", Target = "tools" },
                };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                    new NuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.ChocolateyNuspec_Metadata.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }

            [Fact]
            public void Should_Replace_Template_Tokens_In_Nuspec_With_Files_Without_Namespaces()
            {
                // Given
                var fixture = new NuSpecWithFileFixture();
                fixture.WithNuSpecXml(Resources.ChocolateyNuspec_NoMetadataValues_WithoutNamespaces);

                fixture.Settings.Id = "The ID";
                fixture.Settings.Title = "The title";
                fixture.Settings.Version = "The version";
                fixture.Settings.Authors = new[] { "Author #1", "Author #2" };
                fixture.Settings.Owners = new[] { "Owner #1", "Owner #2" };
                fixture.Settings.Summary = "The summary";
                fixture.Settings.Description = "The description";
                fixture.Settings.ProjectUrl = new Uri("https://project.com");
                fixture.Settings.PackageSourceUrl = new Uri("https://packagesource.com");
                fixture.Settings.ProjectSourceUrl = new Uri("https://projectsource.com");
                fixture.Settings.DocsUrl = new Uri("https://docs.com");
                fixture.Settings.MailingListUrl = new Uri("https://mailing.com");
                fixture.Settings.BugTrackerUrl = new Uri("https://bug.com");
                fixture.Settings.Tags = new[] { "Tag1", "Tag2", "Tag3" };
                fixture.Settings.Copyright = "The copyright";
                fixture.Settings.LicenseUrl = new Uri("https://license.com");
                fixture.Settings.RequireLicenseAcceptance = true;
                fixture.Settings.IconUrl = new Uri("https://icon.com");
                fixture.Settings.ReleaseNotes = new[] { "Line #1", "Line #2", "Line #3" };
                fixture.Settings.Files = new[]
                {
                    new NuSpecContent { Source = @"tools\**", Target = "tools" },
                };
                fixture.Settings.Dependencies = new[]
                {
                    new NuSpecDependency { Id = "Dependency1", Version = "1.0.0" },
                    new NuSpecDependency { Id = "Dependency2", Version = "[2.0.0]" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    Resources.ChocolateyNuspec_Metadata_WithoutNamespaces.NormalizeLineEndings(),
                    result.NuspecContent.NormalizeLineEndings());
            }
        }
    }
}