﻿using Cake.Core.IO;
using Cake.Core.Tests.Fakes;
using NSubstitute;

namespace Cake.Tests.Fixtures
{
    public class ArgumentParserFixture
    {
        public FakeLog Log { get; set; }

        public IFileSystem FileSystem { get; set; }

        public ArgumentParserFixture()
        {
            Log = new FakeLog();

            FileSystem = Substitute.For<IFileSystem>();
        }
    }
}
