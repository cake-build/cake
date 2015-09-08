﻿using System;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting
{
    public sealed class ScriptHostTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Engine_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => new ScriptHostFixture.TestingScriptHost(null, context));

                // Then
                Assert.IsArgumentNullException(result, "engine");
            }
        }

        public sealed class TheContextProperty
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                var result = host.Context;

                // Then
                Assert.Equal(fixture.Context, result);
            }
        }

        public sealed class TheTaskMethod
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();

                // When
                host.Task("Task");

                // Then
                fixture.Engine.Received(1).RegisterTask("Task");
            }
        }

        public sealed class TheTaskSetupMethod
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();
                Action<ICakeContext, ITaskSetupContext> action = (context, setupContext) => { };

                // When
                host.TaskSetup(action);

                // Then
                fixture.Engine.Received().RegisterTaskSetupAction(action);
            }
        }

        public sealed class TheTaskTeardownMethod
        {
            [Fact]
            public void Should_Proxy_Call_To_Engine()
            {
                // Given
                var fixture = new ScriptHostFixture();
                var host = fixture.CreateHost();
                Action<ICakeContext, ITaskTeardownContext> action = (context, setupContext) => { };

                // When
                host.TaskTeardown(action);

                // Then
                fixture.Engine.Received().RegisterTaskTeardownAction(action);
            }
        }
    }
}
