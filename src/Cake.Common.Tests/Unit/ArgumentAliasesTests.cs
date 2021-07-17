// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ArgumentAliases
    {
        private const string WorkingDirectoryArgumentName = "workdir";
        private const string WorkingDirectoryArgumentValue = "c:/data/work";
        private const string OutputFileArgumentName = "outputFile";
        private const string OutputFileArgumentValue = "c:/data/work/output.txt";
        private const string VerboseArgumentName = "verbose";
        private const string VerboseArgumentValueOne = "enabled";
        private const string VerboseArgumentValueTwo = "full";

        public sealed class TheArgumentOfDirectoryPathMethod
        {
            [Fact]
            public void Should_Convert_A_String_Value_To_A_DirectoryPath_If_Argument_Exist()
            {
                var context = Substitute.For<ICakeContext>();
                context.Arguments.GetArguments(WorkingDirectoryArgumentName)
                    .Returns(new[] { WorkingDirectoryArgumentValue });

                var result = context.Argument<DirectoryPath>(WorkingDirectoryArgumentName);

                Assert.Equal(WorkingDirectoryArgumentValue, result.FullPath);
            }
        }

        public sealed class TheArgumentOfFilePathMethod
        {
            [Fact]
            public void Should_Convert_A_String_Value_To_A_FilePath_If_Argument_Exist()
            {
                var context = Substitute.For<ICakeContext>();
                context.Arguments.GetArguments(OutputFileArgumentName)
                    .Returns(new[] { OutputFileArgumentValue });

                var result = context.Argument<FilePath>(OutputFileArgumentName);

                Assert.Equal(OutputFileArgumentValue, result.FullPath);
            }
        }

        public sealed class TheArgumentCollectionMethod
        {
            [Fact]
            public void Should_Return_An_Arguments_Dictionary()
            {
                var context = Substitute.For<ICakeContext>();
                context.Arguments.GetArguments()
                    .Returns(new Dictionary<string, ICollection<string>>
                    {
                        { WorkingDirectoryArgumentName, new[] { WorkingDirectoryArgumentValue } },
                        { VerboseArgumentName, new[] { VerboseArgumentValueOne, VerboseArgumentValueTwo } },
                    });

                var result = context.Arguments();
                var wdValues = result[WorkingDirectoryArgumentName];
                var vValues = result[VerboseArgumentName];

                Assert.Equal(WorkingDirectoryArgumentValue, wdValues.First());
                Assert.Equal(2, vValues.Count);
                Assert.Equal(VerboseArgumentValueOne, vValues.ElementAt(0));
                Assert.Equal(VerboseArgumentValueTwo, vValues.ElementAt(1));
            }
        }
    }
}
