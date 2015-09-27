using Cake.Common.Text;
using Cake.Core;
using Cake.Core.Text;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class TextTransformationFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Enviroment { get; set; }
        public ITextTransformationTemplate TransformationTemplate { get; set; }

        public TextTransformationFixture()
        {
            Enviroment = Substitute.For<ICakeEnvironment>();
            Enviroment.WorkingDirectory.Returns("/Working");

            FileSystem = new FakeFileSystem(Enviroment);
            FileSystem.CreateDirectory(Enviroment.WorkingDirectory);

            TransformationTemplate = Substitute.For<ITextTransformationTemplate>();
        }

        public TextTransformation<ITextTransformationTemplate> CreateTextTransformation()
        {
            return new TextTransformation<ITextTransformationTemplate>(
                FileSystem, Enviroment, TransformationTemplate);
        }
    }
}
