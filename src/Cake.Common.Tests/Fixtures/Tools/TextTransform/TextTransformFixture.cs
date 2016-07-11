// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.TextTransform;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.TextTransform
{
    internal sealed class TextTransformFixture : ToolFixture<TextTransformSettings>
    {
        public FilePath SourceFile { get; set; }

        public TextTransformFixture() : base("TextTransform.exe")
        {
            SourceFile = new FilePath("./Test.tt");
        }

        protected override void RunTool()
        {
            var tool = new TextTransformRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(SourceFile, Settings);
        }
    }
}
