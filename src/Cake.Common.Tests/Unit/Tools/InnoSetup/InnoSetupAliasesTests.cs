// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.InnoSetup;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.InnoSetup
{
    public class InnoSetupAliasesTests
    {
        public sealed class TheInnoSetupMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => InnoSetupAliases.InnoSetup(null, "some file.iss"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Script_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => InnoSetupAliases.InnoSetup(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "scriptFile");
            }
        }
    }
}
