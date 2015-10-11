using Cake.Common.Tools.WiX;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class WiXFixture
    {
        public IFileSystem FileSystem { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }

        public WiXFixture(FilePath toolPath = null)
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/candle.exe").Returns(new[] { (FilePath)"/Working/tools/candle.exe" });
            Globber.Match("./tools/**/light.exe").Returns(new[] { (FilePath)"/Working/tools/light.exe" });

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/candle.exe")).Returns(true);
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/light.exe")).Returns(true);

            if (toolPath != null)
            {
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == toolPath.FullPath)).Returns(true);
            }
        }

        public CandleRunner CreateCandleRunner()
        {
            return new CandleRunner(FileSystem, Environment, Globber, ProcessRunner);
        }

        public LightRunner CreateLightRunner()
        {
            return new LightRunner(FileSystem, Environment, Globber, ProcessRunner);
        }
    }
}