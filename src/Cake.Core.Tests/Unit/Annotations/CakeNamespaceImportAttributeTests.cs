// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
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
                AssertEx.IsArgumentNullException(result, "namespace");
            }
        }

        public sealed class TheType
        {
            private readonly AttributeUsageAttribute _attributeUsage;

            public TheType()
            {
                // Given, When
                _attributeUsage = (AttributeUsageAttribute)typeof(CakeNamespaceImportAttribute).GetTypeInfo().GetCustomAttribute(typeof(AttributeUsageAttribute));
            }

            [Fact]
            public void Should_Be_Able_To_Target_Method()
            {
                // Then
                Assert.True(_attributeUsage.ValidOn.HasFlag(AttributeTargets.Method));
            }

            [Fact]
            public void Should_Be_Able_To_Target_Class()
            {
                // Then
                Assert.True(_attributeUsage.ValidOn.HasFlag(AttributeTargets.Class));
            }

            [Fact]
            public void Should_Be_Able_To_Target_Assembly()
            {
                // Then
                Assert.True(_attributeUsage.ValidOn.HasFlag(AttributeTargets.Assembly));
            }
        }
    }
}