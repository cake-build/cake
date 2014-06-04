using System.Collections.Generic;
using Cake.Core.Diagnostics;

namespace Cake.Tests.Fakes
{
    public sealed class FakeLog : ICakeLog
    {
        private readonly List<string> _messages;

        public List<string> Messages
        {
            get { return _messages; }
        }

        public FakeLog()
        {
            _messages = new List<string>();
        }

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            _messages.Add(string.Format(format,args));
        }
    }
}
