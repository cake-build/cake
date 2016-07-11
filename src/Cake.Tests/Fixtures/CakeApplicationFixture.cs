// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Commands;
using Cake.Core.Diagnostics;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    internal sealed class CakeApplicationFixture
    {
        public ICommandFactory CommandFactory { get; set; }

        public CakeOptions Options { get; set; }

        public CakeApplicationFixture()
        {
            Options = new CakeOptions();
            Options.Verbosity = Verbosity.Diagnostic;

            CommandFactory = Substitute.For<ICommandFactory>();
        }

        public CakeApplication CreateApplication()
        {
            return new CakeApplication(CommandFactory);
        }

        public int RunApplication()
        {
            return CreateApplication().Run(Options);
        }
    }
}
