// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Testing;
using NSubstitute;

namespace Cake.Cli.Tests.Unit;

public sealed class TreeScriptHostTests
{
    public sealed class TheRunTargetAsyncMethod
    {
        [Fact]
        public async Task Should_Show_Dependee_Tasks_In_Dependency_Tree()
        {
            // Given
            var engine = new CakeEngine(Substitute.For<ICakeDataService>(), new FakeLog());
            var console = new FakeConsole();
            var host = new TreeScriptHost(engine, Substitute.For<ICakeContext>(), console);

            host.Task("A");
            host.Task("B").IsDependeeOf("A");

            // When
            await host.RunTargetAsync("A");

            // Then
            Assert.Contains("A", console.Messages);
            Assert.Contains("\u2514\u2500B", console.Messages);
        }
    }
}
