using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class CakeContextFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeLog Log { get; set; }
        public ICakeArguments Arguments { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public List<IToolResolver> ToolResolvers { get; set; }
        public IRegistry Registry { get; set; }

        public CakeContextFixture()
        {
            FileSystem = Substitute.For<IFileSystem>();
            Environment = Substitute.For<ICakeEnvironment>();
            Globber = Substitute.For<IGlobber>();
            Log = Substitute.For<ICakeLog>();
            Arguments = Substitute.For<ICakeArguments>();
            ProcessRunner = Substitute.For<IProcessRunner>();
            ToolResolvers = new List<IToolResolver>();
            Registry = Substitute.For<IRegistry>();
        }

        public CakeContext CreateContext()
        {
            return new CakeContext(FileSystem, Environment, Globber, 
                Log, Arguments, ProcessRunner, ToolResolvers, Registry);
        }
    }
}
