using Cake.Common.Text;
using Cake.Core;
using Cake.Core.Tests.Fakes;
using Cake.Core.Text;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public sealed class TextTransformationFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Enviroment { get; set; }
        public ITextTransformationTemplate TransformationTemplate { get; set; }

        public TextTransformationFixture()
        {
            FileSystem = new FakeFileSystem(true);

            Enviroment = Substitute.For<ICakeEnvironment>();
            Enviroment.WorkingDirectory.Returns("/Working");

            TransformationTemplate = Substitute.For<ITextTransformationTemplate>();
        }

        public TextTransformation<ITextTransformationTemplate> CreateTextTransformation()
        {
            return new TextTransformation<ITextTransformationTemplate>(
                FileSystem, Enviroment, TransformationTemplate);
        }
    }
}
