// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Diagnostics
{
    internal sealed class LogActionFixture
    {
        public string Format { get; private set; }
        public object[] Args { get; private set; }
        public ICakeContext Context { get; set; }
        public bool Evaluated { get; private set; }

        public void Log(LogActionEntry actionEntry)
        {
            if (Evaluated)
            {
                throw new Exception("Message allready evaluated");
            }
            Evaluated = true;
            actionEntry(Format, Args);
        }

        public LogActionFixture(string format = "Hello {0}!", object[] args = null, Verbosity verbosity = Verbosity.Quiet)
        {
            Format = format;
            Args = args ?? new object[] { "World" };
            var context = Substitute.For<ICakeContext>();
            var log = Substitute.For<ICakeLog>();
            log.Verbosity.Returns(verbosity);
            context.Log.Returns(log);
            Context = context;
        }
    }
}
