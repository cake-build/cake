using System;
using System.Collections.Generic;
using System.IO;
using Cake.Common.Build;

namespace Cake.Common.Tests.Fakes
{
    public sealed class FakeBuildSystemServiceMessageWriter : IBuildSystemServiceMessageWriter
    {
        private readonly StringWriter _writer;

        public List<string> Entries { get; }

        public FakeBuildSystemServiceMessageWriter()
        {
            _writer = new StringWriter();
            Entries = new List<string>();
        }

        public void Write(string format, params object[] args)
        {
            _writer.WriteLine(format, args);
            Entries.Add(string.Format(format, args));
        }

        public string GetOutput()
        {
            return _writer.ToString();
        }
    }
}
