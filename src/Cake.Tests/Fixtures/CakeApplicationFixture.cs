using Cake.Commands;
using Cake.Core.Diagnostics;
using Cake.Diagnostics;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    internal sealed class CakeApplicationFixture
    {
        public IVerbosityAwareLog Log { get; set; }
        public ICommandFactory CommandFactory { get; set; }

        public CakeOptions Options { get; set; }

        public CakeApplicationFixture()
        {
            Options = new CakeOptions();
            Options.Verbosity = Verbosity.Diagnostic;

            Log = Substitute.For<IVerbosityAwareLog>();
            CommandFactory = Substitute.For<ICommandFactory>();
        }

        public CakeApplication CreateApplication()
        {
            return new CakeApplication(Log, CommandFactory);
        }

        public int RunApplication()
        {
            return CreateApplication().Run(Options);
        }
    }
}