﻿using Cake.Common.Tools.NSIS;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NSIS
{
    // ReSharper disable once InconsistentNaming
    public sealed class NSISAliasesTests
    {
        // ReSharper disable once InconsistentNaming
        public sealed class TheMakeNSISMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => NSISAliases.MakeNSIS(null, "some file.nsi"));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Script_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => NSISAliases.MakeNSIS(context, null));

                // Then
                Assert.IsArgumentNullException(result, "scriptFile");
            }
        }
    }
}