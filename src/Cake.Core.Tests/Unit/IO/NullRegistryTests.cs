// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class NullRegistryTests
    {
        [Fact]
        public void Registry_Properties_Should_Not_Throw()
        {
            var registry = new NullRegistry();

            using (var currentUser = registry.CurrentUser)
            using (var localMachine = registry.LocalMachine)
            using (var classesRoot = registry.ClassesRoot)
            using (var users = registry.Users)
            using (var performanceData = registry.PerformanceData)
            using (var currentConfig = registry.CurrentConfig)
            {
                Assert.Empty(currentUser.GetSubKeyNames());
                Assert.Null(currentUser.OpenKey("test"));
                Assert.Null(currentUser.GetValue("test"));
            }
        }
    }
}
