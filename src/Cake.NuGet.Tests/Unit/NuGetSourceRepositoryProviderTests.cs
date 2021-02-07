// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.Packaging;
using Cake.NuGet.Tests.Stubs;
using NSubstitute;
using NuGet.Configuration;
using Xunit;

namespace Cake.NuGet.Tests.Unit
{
    public sealed partial class NuGetSourceRepositoryProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                var configuration = new CakeConfiguration(new Dictionary<string, string>());
                var package = new PackageReference("nuget:?package=First.Package");
                var result = Record.Exception(() => new NuGetSourceRepositoryProvider(null, configuration, package, "/Work/packages/"));

                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_CakeConfiguration_Is_Null()
            {
                var package = new PackageReference("nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var result = Record.Exception(() => new NuGetSourceRepositoryProvider(settings, null, package, "/Work/packages/"));

                AssertEx.IsArgumentNullException(result, "config");
            }

            [Fact]
            public void Should_Throw_If_PackageReference_Is_Null()
            {
                var configuration = new CakeConfiguration(new Dictionary<string, string>());
                var settings = Substitute.For<ISettings>();
                var result = Record.Exception(() => new NuGetSourceRepositoryProvider(settings, configuration, null, "/Work/packages/"));

                AssertEx.IsArgumentNullException(result, "package");
            }

            [Fact]
            public void Should_Split_Multiple_NuGet_Sources_Into_Multiple_Repositories()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var package = new PackageReference("nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = nugetV2Api + ";" + nugetV3Api,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Equal(2, provider.Repositories.Count());
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Ignore_Trailing_Separator_For_NuGet_Source_Argument()
            {
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var package = new PackageReference("nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = nugetV2Api + ";",
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Single(provider.Repositories);
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV2Api);
            }

            [Fact]
            public void Should_Not_Throw_For_Null_NuGet_Source_Value_In_CakeConfiguration()
            {
                var package = new PackageReference("nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = null,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Empty(provider.Repositories);
            }

            [Fact]
            public void Should_Set_Source_Specified_In_Directive_As_Primary_But_Not_For_Config()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var primaryApi = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:{primaryApi}?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = $"{nugetV3Api};{nugetV2Api}",
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Single(provider.PrimaryRepositories);
                Assert.Contains(provider.PrimaryRepositories, p => p.PackageSource.Source == primaryApi);
                Assert.Equal(3, provider.Repositories.Count());
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == primaryApi);
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Set_Source_Specified_In_Directive_As_Primary_But_Not_For_Settings()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var primaryApi = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:{primaryApi}?package=First.Package");
                var settings = new FakeNuGetSettings();
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, new SourceItem("V3", nugetV3Api));
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, new SourceItem("V2", nugetV2Api));

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = string.Empty,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Single(provider.PrimaryRepositories);
                Assert.Contains(provider.PrimaryRepositories, p => p.PackageSource.Source == primaryApi);
                Assert.Equal(3, provider.Repositories.Count());
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == primaryApi);
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Set_Configuration_Source_As_Primary_If_Not_Specified_In_Directive()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var package = new PackageReference($"nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = $"{nugetV3Api};{nugetV2Api}",
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Equal(2, provider.PrimaryRepositories.Count());
                Assert.Contains(provider.PrimaryRepositories, p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.PrimaryRepositories, p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Set_Settings_Source_As_Primary_If_Not_Specified_In_Directive()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var package = new PackageReference($"nuget:?package=First.Package");
                var settings = new FakeNuGetSettings();
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, new SourceItem("V3", nugetV3Api));
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, new SourceItem("V2", nugetV2Api));

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = string.Empty,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Equal(2, provider.PrimaryRepositories.Count());
                Assert.Contains(provider.PrimaryRepositories, p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.PrimaryRepositories, p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Not_Set_Source_From_Settings_If_Cake_Config_Source_Is_Provided()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var settingsApi = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:?package=First.Package");
                var settings = new FakeNuGetSettings();
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, new SourceItem("foobar", settingsApi));

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = $"{nugetV3Api};{nugetV2Api}",
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Equal(2, provider.Repositories.Count());
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Set_Source_From_Settings_If_Cake_Config_Source_Is_Not_Provided()
            {
                var settingsApi = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:?package=First.Package");
                var settings = new FakeNuGetSettings();
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, new SourceItem("foobar", settingsApi));

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = string.Empty,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Single(provider.Repositories);
                Assert.Contains(provider.Repositories, p => p.PackageSource.Source == settingsApi);
            }

            [Fact]
            public void Should_Use_Feed_Specified_In_NuGet_Config_If_Available()
            {
                var feed = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:{feed}?package=First.Package");
                var settings = new FakeNuGetSettings();
                settings.AddOrUpdate(ConfigurationConstants.PackageSources, new SourceItem("foobar", feed));
                settings.AddOrUpdate(ConfigurationConstants.CredentialsSectionName, new CredentialsItem("foobar", "foo@bar.baz", "p455w0rdz", true, "foo"));

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = string.Empty,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package, "/Work/packages/");

                Assert.Single(provider.Repositories);
                Assert.Contains(provider.Repositories, p =>
                    p.PackageSource.Source == feed &&
                    p.PackageSource.Credentials.Username == "foo@bar.baz" &&
                    p.PackageSource.Credentials.Password == "p455w0rdz");
            }
        }
    }
}
