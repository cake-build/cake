﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.Packaging;
using Cake.NuGet.Install;
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
                var result = Record.Exception(() => new NuGetSourceRepositoryProvider(null, configuration, package));

                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_CakeConfiguration_Is_Null()
            {
                var package = new PackageReference("nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var result = Record.Exception(() => new NuGetSourceRepositoryProvider(settings, null, package));

                AssertEx.IsArgumentNullException(result, "config");
            }

            [Fact]
            public void Should_Throw_If_PackageReference_Is_Null()
            {
                var configuration = new CakeConfiguration(new Dictionary<string, string>());
                var settings = Substitute.For<ISettings>();
                var result = Record.Exception(() => new NuGetSourceRepositoryProvider(settings, configuration, null));

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

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Equal(2, provider.GetRepositories().Count());
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV3Api);
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

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Single(provider.GetRepositories());
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV2Api);
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

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Empty(provider.GetRepositories());
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

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Single(provider.GetPrimaryRepositories());
                Assert.Contains(provider.GetPrimaryRepositories(), p => p.PackageSource.Source == primaryApi);
                Assert.Equal(3, provider.GetRepositories().Count());
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == primaryApi);
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Set_Source_Specified_In_Directive_As_Primary_But_Not_For_Settings()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var primaryApi = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:{primaryApi}?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var settingSection = Activator.CreateInstance(
                    type: typeof(VirtualSettingSection),
                    bindingAttr: System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    binder: null,
                    args: new object[]
                    {
                        ConfigurationConstants.PackageSources,
                        (IReadOnlyDictionary<string, string>)null,
                        new[]
                        {
                            new SourceItem("V3", nugetV3Api),
                            new SourceItem("V2", nugetV2Api),
                        }
                    },
                    culture: null);

                settings.GetSection(ConfigurationConstants.PackageSources).Returns(settingSection);

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = string.Empty,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Single(provider.GetPrimaryRepositories());
                Assert.Contains(provider.GetPrimaryRepositories(), p => p.PackageSource.Source == primaryApi);
                Assert.Equal(3, provider.GetRepositories().Count());
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == primaryApi);
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV3Api);
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

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Equal(2, provider.GetPrimaryRepositories().Count());
                Assert.Contains(provider.GetPrimaryRepositories(), p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.GetPrimaryRepositories(), p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Set_Settings_Source_As_Primary_If_Not_Specified_In_Directive()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var package = new PackageReference($"nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var settingSection = Activator.CreateInstance(
                    type: typeof(VirtualSettingSection),
                    bindingAttr: System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    binder: null,
                    args: new object[]
                    {
                        ConfigurationConstants.PackageSources,
                        (IReadOnlyDictionary<string, string>)null,
                        new[]
                        {
                            new SourceItem("V3", nugetV3Api),
                            new SourceItem("V2", nugetV2Api)
                        }
                    },
                    culture: null);

                settings.GetSection(ConfigurationConstants.PackageSources).Returns(settingSection);

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = string.Empty,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Equal(2, provider.GetPrimaryRepositories().Count());
                Assert.Contains(provider.GetPrimaryRepositories(), p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.GetPrimaryRepositories(), p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Not_Set_Source_From_Settings_If_Cake_Config_Source_Is_Provided()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var settingsApi = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var settingSection = Activator.CreateInstance(
                    type: typeof(VirtualSettingSection),
                    bindingAttr: System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    binder: null,
                    args: new object[]
                    {
                        ConfigurationConstants.PackageSources,
                        (IReadOnlyDictionary<string, string>)null,
                        new[]
                        {
                            new SourceItem("foobar", settingsApi)
                        }
                    },
                    culture: null);

                settings.GetSection(ConfigurationConstants.PackageSources).Returns(settingSection);

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = $"{nugetV3Api};{nugetV2Api}",
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Equal(2, provider.GetRepositories().Count());
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV2Api);
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV3Api);
            }

            [Fact]
            public void Should_Set_Source_From_Settings_If_Cake_Config_Source_Is_Not_Provided()
            {
                var settingsApi = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var settingSection = Activator.CreateInstance(
                    type: typeof(VirtualSettingSection),
                    bindingAttr: System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    binder: null,
                    args: new object[]
                    {
                        ConfigurationConstants.PackageSources,
                        (IReadOnlyDictionary<string, string>)null,
                        new[]
                        {
                            new SourceItem("foobar", settingsApi)
                        }
                    },
                    culture: null);

                settings.GetSection(ConfigurationConstants.PackageSources).Returns(settingSection);

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = string.Empty,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Single(provider.GetRepositories());
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == settingsApi);
            }

            [Fact]
            public void Should_Use_Feed_Specified_In_NuGet_Config_If_Available()
            {
                var feed = "https://foo.bar/api.json";
                var package = new PackageReference($"nuget:{feed}?package=First.Package");
                var settings = Substitute.For<ISettings>();
                var packageSourceSection = Activator.CreateInstance(
                    type: typeof(VirtualSettingSection),
                    bindingAttr: System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    binder: null,
                    args: new object[]
                    {
                        ConfigurationConstants.PackageSources,
                        (IReadOnlyDictionary<string, string>)null,
                        new[]
                        {
                            new SourceItem("foobar", feed)
                        }
                    },
                    culture: null);
                var credentialSection = Activator.CreateInstance(
                    type: typeof(VirtualSettingSection),
                    bindingAttr: System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                    binder: null,
                    args: new object[]
                    {
                        ConfigurationConstants.CredentialsSectionName,
                        (IReadOnlyDictionary<string, string>)null,
                        new[]
                        {
                            new CredentialsItem("foobar", "foo@bar.baz", "p455w0rdz", true)
                        }
                    },
                    culture: null);

                settings.GetSection(ConfigurationConstants.PackageSources).Returns(packageSourceSection);
                settings.GetSection(ConfigurationConstants.CredentialsSectionName).Returns(credentialSection);

                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = string.Empty,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Single(provider.GetRepositories());
                Assert.Contains(provider.GetRepositories(), p =>
                    p.PackageSource.Source == feed &&
                    p.PackageSource.Credentials.Username == "foo@bar.baz" &&
                    p.PackageSource.Credentials.Password == "p455w0rdz");
            }
        }
    }
}
