// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
        public ICakeEnvironment Environment { get; set; }
        public ITextTransformationTemplate TransformationTemplate { get; set; }

        public TextTransformationFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            FileSystem = new FakeFileSystem(Environment);
            FileSystem.CreateDirectory(Environment.WorkingDirectory);

            TransformationTemplate = Substitute.For<ITextTransformationTemplate>();
        }

        public TextTransformation<ITextTransformationTemplate> CreateTextTransformation()
        {
            return new TextTransformation<ITextTransformationTemplate>(
                FileSystem, Environment, TransformationTemplate);
        }
    }
}
