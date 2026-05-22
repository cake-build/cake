// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Rwx;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class RwxFixture
    {
        public ICakeEnvironment Environment { get; set; }

        public IFileSystem FileSystem { get; set; }

        public RwxFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            FileSystem = Substitute.For<IFileSystem>();
        }

        public void IsRunningOnRwx()
        {
            Environment.GetEnvironmentVariable("RWX").Returns("true");
        }

        public RwxProvider CreateRwxProvider()
        {
            return new RwxProvider(Environment, FileSystem);
        }
    }
}
