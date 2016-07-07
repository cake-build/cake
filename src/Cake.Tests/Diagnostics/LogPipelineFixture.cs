using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cake.Commands;
using Cake.Diagnostics;
using Cake.Testing;

using NSubstitute;

namespace Cake.Tests.Diagnostics
{
    internal sealed class LogPipelineFixture
    {
        public FakeLog Log { get; set; }

        public LogPipelineFixture()
        {
            Log = new FakeLog();
        }

        public CakeLogPipeline CreateLogPipeline()
        {
            return new CakeLogPipeline(Log);
        }
    }
}
