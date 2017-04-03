// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.VSWhere;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.VSWhere
{
    public sealed class VSWhereAliasesTests
    {
        public sealed class TheLegacyMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // When
                var result = Record.Exception(() => VSWhereAliases.VSWhereLegacy(null, true));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => VSWhereAliases.VSWhereLegacy(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }
        }

        public sealed class TheLatestMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // When
                var result = Record.Exception(() => VSWhereAliases.VSWhereLatest(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => VSWhereAliases.VSWhereLatest(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }
        }

        public sealed class TheAllMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // When
                var result = Record.Exception(() => VSWhereAliases.VSWhereAll(null));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => VSWhereAliases.VSWhereAll(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }
        }

        public sealed class TheProductMethod
        {
            [Fact]
            public void Should_Throw_If_Products_Are_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => VSWhereAliases.VSWhereProducts(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "products");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => VSWhereAliases.VSWhereProducts(context, "Community", null));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }
        }
    }
}
