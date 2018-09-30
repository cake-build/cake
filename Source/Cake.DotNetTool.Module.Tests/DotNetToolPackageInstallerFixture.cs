using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Testing;
using NSubstitute;
using System.Collections.Generic;

namespace Cake.DotNetTool.Module.Tests
{
    /// <summary>
    /// Fixture used for testing DotNetToolPackageInstaller
    /// </summary>
    internal sealed class DotNetToolPackageInstallerFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IFileSystem FileSystem { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IDotNetToolContentResolver ContentResolver { get; set; }
        public ICakeLog Log { get; set; }

        public PackageReference Package { get; set; }
        public PackageType PackageType { get; set; }
        public DirectoryPath InstallPath { get; set; }

        public ICakeConfiguration Config { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetToolPackageInstallerFixture"/> class.
        /// </summary>
        internal DotNetToolPackageInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ContentResolver = Substitute.For<IDotNetToolContentResolver>();
            Log = new FakeLog();
            Config = Substitute.For<ICakeConfiguration>();
            Package = new PackageReference("dotnet:?package=windirstat");
            PackageType = PackageType.Addin;
            InstallPath = new DirectoryPath("./dotnet");
        }

        /// <summary>
        /// Create the installer.
        /// </summary>
        /// <returns>The dotnet Tool package installer.</returns>
        internal DotNetToolPackageInstaller CreateInstaller()
        {
            return new DotNetToolPackageInstaller(Environment, ProcessRunner, Log, ContentResolver, Config, FileSystem);
        }

        /// <summary>DotNetPackageInstallerFixture
        /// Installs the specified resource at the given location.
        /// </summary>
        /// <returns>The installed files.</returns>
        internal IReadOnlyCollection<IFile> Install()
        {
            var installer = CreateInstaller();
            return installer.Install(Package, PackageType, InstallPath);
        }

        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <returns><c>true</c> if this installer can install the specified resource; otherwise <c>false</c>.</returns>
        internal bool CanInstall()
        {
            var installer = CreateInstaller();
            return installer.CanInstall(Package, PackageType);
        }
    }
}
