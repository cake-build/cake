﻿using Cake.Common.Tests.Fixtures.Tools.TextTransform;
using Cake.Common.Tools.TextTransform;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.TextTransform
{
    public sealed class TextTransformAliasTests
    {
        public sealed class TheTransformTemplateMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var fixture = new TextTransformFixture();

                // When
                var result = Record.Exception(() => TextTransformAliases.TransformTemplate(null, fixture.SourceFile));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Source_File_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => TextTransformAliases.TransformTemplate(context, null));

                // Then
                Assert.IsArgumentNullException(result, "sourceFile");
            }
        }
    }
}