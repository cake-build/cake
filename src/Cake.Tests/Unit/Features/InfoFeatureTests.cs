﻿using Cake.Cli;
using Cake.Testing;
using Cake.Tests.Fakes;
using Xunit;

namespace Cake.Tests.Unit.Features
{
    public sealed class InfoFeatureTests
    {
        [Fact]
        public void Should_Output_Version_To_Console()
        {
            // Given
            var console = new FakeConsole();
            var resolver = new FakeVersionResolver("1.2.3", "3.2.1");
            var feature = new InfoFeature(resolver);

            // When
            feature.Run(console);

            // Then
            Assert.Contains("Version: 1.2.3", console.Messages);
            Assert.Contains("Details: 3.2.1", console.Messages);
        }

        [Fact]
        public void Should_Throw_If_Console_Is_Null()
        {
            // Given
            var resolver = new FakeVersionResolver("1.2.3", "3.2.1");
            var feature = new InfoFeature(resolver);

            // When
            var result = Record.Exception(() =>
                     feature.Run(null));

            // Then
            AssertEx.IsArgumentNullException(result, "console");
        }
    }
}
