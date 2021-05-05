// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ArgumentAliases
    {
        public sealed class TheArgumentOfDirectoryPathMethod
        {
            [Fact]
            public void Should_Convert_A_String_Value_To_A_DirectoryPath_If_Argument_Exist()
            {
                var context = Substitute.For<ICakeContext>();
                context.Arguments.GetArguments("workdir").Returns(new[] { "c:/data/work" });

                var result = context.Argument<DirectoryPath>("workdir");

                Assert.Equal("c:/data/work", result.FullPath);
            }
        }

        public sealed class TheArgumentOfFilePathMethod
        {
            [Fact]
            public void Should_Convert_A_String_Value_To_A_FilePath_If_Argument_Exist()
            {
                var context = Substitute.For<ICakeContext>();
                context.Arguments.GetArguments("outputFile").Returns(new[] { "c:/data/work/output.txt" });

                var result = context.Argument<FilePath>("outputFile");

                Assert.Equal("c:/data/work/output.txt", result.FullPath);
            }
        }
    }
}
