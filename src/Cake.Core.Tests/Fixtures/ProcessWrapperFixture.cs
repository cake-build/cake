using System;
using System.Diagnostics;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    internal sealed class ProcessWrapperFixture
    {
        public Process Process { get; set; }
        public ICakeLog Log { get; set; }
        public Func<string, string> FilterOutput { get; set; }
        public Func<string, string> FilterError { get; set; }
        public Func<string, string> StandartOutputHandler { get; set; }
        public Func<string, string> StandardErrorHandler { get; set; }

        public ProcessWrapperFixture()
        {
            Log = Substitute.For<ICakeLog>();
        }

        public ProcessWrapper CreateProcessWrapper()
        {
            return new ProcessWrapper(Process, Log, FilterOutput, StandartOutputHandler, FilterError, StandardErrorHandler);
        }
    }
}
