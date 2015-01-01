using System;
using Cake.Common.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Text
{
    public sealed class TextTransformationTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Template_Is_Null()
            {
                // Given
                var fixture = new TextTransformationFixture();
                fixture.TransformationTemplate = null;

                // When
                var result = Record.Exception(() => fixture.CreateTextTransformation());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("template", ((ArgumentNullException)result).ParamName);
            }
        }

        public sealed class TheSaveMethod
        {
            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var fixture = new TextTransformationFixture();
                var transformation = fixture.CreateTextTransformation();

                // When
                var result = Record.Exception(() => transformation.Save(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Render_Content_To_File()
            {
                // Given
                var fixture = new TextTransformationFixture();
                fixture.TransformationTemplate.Render().Returns("Hello World");
                var transformation = fixture.CreateTextTransformation();

                // When
                transformation.Save("./output.txt");

                // Then
                Assert.Equal("Hello World", fixture.FileSystem.GetTextContent("/Working/output.txt"));
            }
        }

        public sealed class TheToStringMethod
        {
            [Fact]
            public void Should_Render_The_Provided_Template()
            {
                // Given
                var fixture = new TextTransformationFixture();
                fixture.TransformationTemplate.Render().Returns("Hello World");
                var transformation = fixture.CreateTextTransformation();

                // When
                var result = transformation.ToString();

                // Then
                Assert.Equal("Hello World", result);
            }
        }
    }
}
