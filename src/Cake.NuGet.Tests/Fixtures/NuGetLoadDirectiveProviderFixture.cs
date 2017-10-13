// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors.Loading;
using Cake.Testing;
using NSubstitute;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class NuGetLoadDirectiveProviderFixture
    {
        public FakeEnvironment Environment { get; set; }
        public INuGetPackageInstaller Installer { get; set; }
        public IScriptAnalyzerContext Context { get; set; }
        public FakeConfiguration Configuration { get; set; }
        public FakeLog Log { get; set; }

        public LoadReference Reference { get; set; }
        public List<FilePath> InstallResult { get; set; }

        public NuGetLoadDirectiveProviderFixture()
            : this("nuget:?package=Cake.Recipe")
        {
        }

        public NuGetLoadDirectiveProviderFixture(string uri)
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            Installer = Substitute.For<INuGetPackageInstaller>();
            Configuration = new FakeConfiguration();
            Log = new FakeLog();
            Reference = new LoadReference(new Uri(uri));
            InstallResult = new List<FilePath>();

            Context = Substitute.For<IScriptAnalyzerContext>();
            Context.Root.Returns(new FilePath("/Working/build.cake"));
        }

        public bool CanLoad()
        {
            var provider = new NuGetLoadDirectiveProvider(Environment, Installer, Configuration, Log);
            return provider.CanLoad(Context, Reference);
        }

        public NuGetLoadDirectiveProviderFixtureResult Load()
        {
            var provider = new NuGetLoadDirectiveProvider(Environment, Installer, Configuration, Log);
            var result = new NuGetLoadDirectiveProviderFixtureResult();

            // Setup the files that should be installed.
            Installer.Install(Arg.Any<PackageReference>(), PackageType.Tool, Arg.Any<DirectoryPath>())
                .Returns(info =>
                {
                    var files = new List<IFile>();
                    foreach (var path in InstallResult)
                    {
                        var file = Substitute.For<IFile>();
                        file.Path.Returns(path);
                        files.Add(file);
                    }
                    return files;
                });

            // Capture install parameters.
            Installer.When(i => i.Install(Arg.Any<PackageReference>(), Arg.Any<PackageType>(), Arg.Any<DirectoryPath>()))
                .Do(info =>
                {
                    result.Package = info.Arg<PackageReference>();
                    result.PackageType = info.Arg<PackageType>();
                    result.InstallPath = info.Arg<DirectoryPath>();
                });

            // Capture analysis results.
            Context?.When(c => c.Analyze(Arg.Any<FilePath>()))
                .Do(info => result.AnalyzedFiles.Add(info.Arg<FilePath>()));

            // Load the reference.
            provider.Load(Context, Reference);

            // Return the result.
            return result;
        }
    }
}