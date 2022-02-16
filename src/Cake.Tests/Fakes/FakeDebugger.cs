using System;
using Cake.Core.Diagnostics;

namespace Cake.Tests.Fakes
{
    public sealed class FakeDebugger : ICakeDebugger
    {
        public bool Attached { get; set; }

        public void WaitForAttach(TimeSpan timeout)
        {
            Attached = true;
        }
    }
}
