using System.Collections.Generic;
using System.Linq;
using Cake.Arguments;
using Cake.Commands;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Diagnostics;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    internal sealed class CakeApplicationFixture
    {
        public IVerbosityAwareLog Log { get; set; }
        public ICommandFactory CommandFactory { get; set; }
        public IArgumentParser ArgumentParser { get; set; }
        public IConsole Console { get; set; }

        public CakeOptions Options { get; set; }

        public CakeApplicationFixture()
        {
            Options = new CakeOptions();
            Options.Verbosity = Verbosity.Diagnostic;

            Log = Substitute.For<IVerbosityAwareLog>();
            CommandFactory = Substitute.For<ICommandFactory>();

            ArgumentParser = Substitute.For<IArgumentParser>();
            ArgumentParser.Parse(Arg.Any<IEnumerable<string>>()).Returns(c => Options);

            Console = Substitute.For<IConsole>();
        }

        public CakeApplication CreateApplication()
        {
            return new CakeApplication(Log, CommandFactory, ArgumentParser, Console);
        }

        public int RunApplication()
        {
            return CreateApplication().Run(Enumerable.Empty<string>());
        }
    }
}