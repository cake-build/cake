using System;
using Cake.Common.Tools.SignTool;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class SignToolSignRunnerFixture : ToolFixture<SignToolSignSettings>
    {
        public ISignToolResolver Resolver { get; set; }
        public IFile AssemblyFile { get; set; }
        public IFile CertificateFile { get; set; }

        public FilePath AssemblyPath { get; set; }

        public SignToolSignRunnerFixture()
            : base("signtool.exe")
        {
            Settings.CertPath = "./cert.pfx";
            Settings.Password = "secret";
            Settings.TimeStampUri = new Uri("https://t.com");

            AssemblyPath = new FilePath("./a.dll");
            FileSystem.CreateFile("/Working/a.dll");
            FileSystem.CreateFile("/Working/cert.pfx");

            Resolver = Substitute.For<ISignToolResolver>();
        }

        protected override void RunTool()
        {
            var tool = new SignToolSignRunner(FileSystem, Environment, ProcessRunner, Globber, null, Resolver);
            tool.Run(AssemblyPath, Settings);
        }
    }
}