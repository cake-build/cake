using System;
using Cake.Common.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tests.Fakes;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Text
{
    public sealed class TextTransformationAliasesTests
    {
        public sealed class TheTransformTextMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => 
                    TextTransformationAliases.TransformText(null, "Hello World"));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Template_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    TextTransformationAliases.TransformText(context, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("template", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Create_Text_Transformation()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
               
                // When
                var result = TextTransformationAliases.TransformText(context, "Hello World");

                // Then
                Assert.Equal("Hello World", result.ToString());
           }
        }

        public sealed class TheTransformTextFileMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    TextTransformationAliases.TransformTextFile(
                        null, new FilePath("./template.txt")));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Template_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    TextTransformationAliases.TransformTextFile(
                        context, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Create_Text_Transformation_From_Disc_Template()
            {
                // Given
                var fileSystem = new FakeFileSystem(false);
                fileSystem.GetCreatedFile("/Working/template.txt", "Hello World");
                
                var environment = Substitute.For<ICakeEnvironment>();
                environment.WorkingDirectory.Returns("/Working");

                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                var result = TextTransformationAliases.TransformTextFile(
                    context, "./template.txt");

                // Then
                Assert.Equal("Hello World", result.ToString());
            }
        }
    }
}
