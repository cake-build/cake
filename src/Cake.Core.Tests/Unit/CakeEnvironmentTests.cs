// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    /// <summary>
    /// Tests for <see cref="CakeEnvironment"/>.
    /// </summary>
    public sealed class CakeEnvironmentTests
    {
        /// <summary>
        /// Tests that ApplicationRoot is set to a valid rooted path when the environment is created.
        /// When Assembly.Location is empty (e.g. single-file publish, AOT), ApplicationRoot falls back to AppContext.BaseDirectory.
        /// </summary>
        [Fact]
        public void ApplicationRoot_Should_Be_Valid_Rooted_Path()
        {
            // Given
            var platform = new CakePlatform();
            var runtime = new CakeRuntime();

            // When
            var environment = new CakeEnvironment(platform, runtime);

            // Then - ApplicationRoot must be set and rooted so script loading and tool resolution work
            Assert.NotNull(environment.ApplicationRoot);
            Assert.NotNull(environment.ApplicationRoot.FullPath);
            Assert.False(string.IsNullOrWhiteSpace(environment.ApplicationRoot.FullPath));
            Assert.True(environment.ApplicationRoot.IsRelative == false, "ApplicationRoot should be an absolute path.");
        }
    }
}
