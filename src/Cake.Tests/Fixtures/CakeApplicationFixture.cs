using System.Collections.Generic;
using Cake.Arguments;
using Cake.Commands;
using Cake.Core.Diagnostics;
using Cake.Diagnostics;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    public sealed class CakeApplicationFixture
    {
        public IVerbosityAwareLog Log { get; set; }
        public ICommandFactory CommandFactory { get; set; }
        public IArgumentParser ArgumentParser { get; set; }
        public CakeOptions Options { get; set; }

        public CakeApplicationFixture()
        {
            Options = new CakeOptions();
            Options.Verbosity = Verbosity.Diagnostic;

            Log = Substitute.For<IVerbosityAwareLog>();
            CommandFactory = Substitute.For<ICommandFactory>();

            ArgumentParser = Substitute.For<IArgumentParser>();
            ArgumentParser.Parse(Arg.Any<IEnumerable<string>>()).Returns(Options);
        }

        public CakeApplication CreateApplication()
        {
            return new CakeApplication(Log, CommandFactory, ArgumentParser);
        }
    }
}
