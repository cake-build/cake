using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.Packaging;
using Cake.NuGet.Install;
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
                var settings = NSubstitute.Substitute.For<ISettings>();
                var result = Record.Exception(() => new NuGetSourceRepositoryProvider(settings, null, package));

                AssertEx.IsArgumentNullException(result, "config");
            }

            [Fact]
            public void Should_Throw_If_PackageReference_Is_Null()
            {
                var configuration = new CakeConfiguration(new Dictionary<string, string>());
                var settings = NSubstitute.Substitute.For<ISettings>();
                var result = Record.Exception(() => new NuGetSourceRepositoryProvider(settings, configuration, null));

                AssertEx.IsArgumentNullException(result, "package");
            }

            [Fact]
            public void Should_Split_Multiple_NugetSources_Into_Multple_Repositories()
            {
                var nugetV3Api = "https://api.nuget.org/v3/index.json";
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var package = new PackageReference("nuget:?package=First.Package");
                var settings = NSubstitute.Substitute.For<ISettings>();
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
            public void Should_Ignore_Trailing_Separator_For_NugetSource_Argument()
            {
                var nugetV2Api = "https://packages.nuget.org/api/v2";
                var package = new PackageReference("nuget:?package=First.Package");
                var settings = NSubstitute.Substitute.For<ISettings>();
                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = nugetV2Api + ";",
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Single(provider.GetRepositories());
                Assert.Contains(provider.GetRepositories(), p => p.PackageSource.Source == nugetV2Api);
            }

            [Fact]
            public void Should_Not_Throw_For_Null_NugetSource_Value_In_CakeConfiguration()
            {
                var package = new PackageReference("nuget:?package=First.Package");
                var settings = NSubstitute.Substitute.For<ISettings>();
                var configuration = new CakeConfiguration(new Dictionary<string, string>()
                {
                    [Constants.NuGet.Source] = null,
                });

                var provider = new NuGetSourceRepositoryProvider(settings, configuration, package);

                Assert.Empty(provider.GetRepositories());
            }
        }
    }
}
