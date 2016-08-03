﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Annotations;
using Xunit;

namespace Cake.Core.Tests.Unit.Annotations
{
    public sealed class CakeNamespaceImportAttributeTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Namespace_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new CakeNamespaceImportAttribute(null));

                // Then
                Assert.IsArgumentNullException(result, "namespace");
            }
        }
    }
}