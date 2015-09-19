using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class ScriptProcessorFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeLog Log { get; set; }
        public INuGetPackageInstaller Installer { get; set; }

        public ScriptAnalyzerResult Result { get; set; }
        public DirectoryPath ApplicationRoot { get; set; }

        public ScriptProcessorFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);

            Log = Substitute.For<ICakeLog>();
            Installer = Substitute.For<INuGetPackageInstaller>();
            ApplicationRoot = new DirectoryPath("/Working/Bin");

            // Create the script analyzer result.
            var script1 = new ScriptInformation("/Working/build.cake");
            script1.Addins.Add(new NuGetPackage("Addin")
            {
                Source = "http://example.com"
            });
            script1.Tools.Add(new NuGetPackage("Tool")
            {
                Source = "http://example.com"
            });
            Result = new ScriptAnalyzerResult(script1, new List<string>());
        }

        public void GivenAddinFilesAreDownloaded()
        {
            // Create the addin file when the installer is invoked.
            Installer.When(i => i.InstallPackage(Arg.Any<NuGetPackage>(), Arg.Any<DirectoryPath>()))
                .Do(info => GivenAddinFilesAlreadyHaveBeenDownloaded());
        }

        public void GivenAddinFilesAlreadyHaveBeenDownloaded()
        {
            FileSystem.CreateFile("/Working/Bin/Addin/Temp.dll");
        }

        public void GivenToolFilesAreDownloaded()
        {
            // Create the tool file when the installer is invoked.
            Installer.When(i => i.InstallPackage(Arg.Any<NuGetPackage>(), Arg.Any<DirectoryPath>()))
                .Do(info => GivenToolFilesAlreadyHaveBeenDownloaded());
        }

        public void GivenToolFilesAlreadyHaveBeenDownloaded()
        {
            FileSystem.CreateFile("/Working/tools/Tool/Temp.exe");
        }

        public ScriptProcessor CreateProcessor()
        {
            return new ScriptProcessor(FileSystem, Environment, Log, Installer);
        }

        public void InstallAddins()
        {
            CreateProcessor().InstallAddins(Result, ApplicationRoot);
        }

        public void InstallTools()
        {
            CreateProcessor().InstallTools(
                Result, Result.Script.Path.GetDirectory().Combine("tools"));
        }
    }
}
