// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Cli;

namespace Cake.Frosting.Tests.Fakes
{
    internal sealed class FakeVersionResolver : IVersionResolver
    {
        public FakeVersionResolver()
        {
        }

        public string GetVersion()
        {
            return "FakeVersion";
        }

        public string GetProductVersion()
        {
            return "ProductVersion";
        }
    }
}
