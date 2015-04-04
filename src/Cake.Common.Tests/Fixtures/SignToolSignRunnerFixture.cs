using System;
using Cake.Common.Tools.SignTool;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class SignToolSignRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IRegistry Registry { get; set; }
        public ISignToolResolver Resolver { get; set; }

        public IFile AssemblyFile { get; set; }
        public IFile CertificateFile { get; set; }
        public IFile DefaultToolFile { get; set; }

        public SignToolSignSettings Settings { get; set; }

        public SignToolSignRunnerFixture()
        {
            Settings = new SignToolSignSettings();
            Settings.CertPath = "./cert.pfx";
            Settings.Password = "secret";
            Settings.TimeStampUri = new Uri("https://t.com");

            AssemblyFile = Substitute.For<IFile>();
            AssemblyFile.Exists.Returns(true);
            CertificateFile = Substitute.For<IFile>();
            CertificateFile.Exists.Returns(true);
            DefaultToolFile = Substitute.For<IFile>();
            DefaultToolFile.Exists.Returns(true);           

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == "/Working/a.dll")).Returns(AssemblyFile);
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == "/Working/cert.pfx")).Returns(CertificateFile);
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath == "/Working/Default/tool.exe")).Returns(DefaultToolFile);

            Resolver = Substitute.For<ISignToolResolver>();
            Resolver.GetPath().Returns("/Working/Default/tool.exe");

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            ProcessRunner = Substitute.For<IProcessRunner>();
        }

        public SignToolSignRunner CreateRunner()
        {
            return new SignToolSignRunner(FileSystem, Environment, ProcessRunner, Registry, Resolver);
        }

        public void RunTool()
        {
            CreateRunner().Run("./a.dll", Settings);
        }
    }
}
