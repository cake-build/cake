// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Scripting;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting
{
    public sealed class ScriptAliasTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Method_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ScriptAlias(null, ScriptAliasType.Method, new HashSet<string>()));

                // Then
                AssertEx.IsArgumentNullException(result, "method");
            }

            [Fact]
            public void Should_Not_Throw_If_Method_Is_Null()
            {
                // Given
                var method = typeof(TheConstructor).GetTypeInfo().GetMethods().First();

                // When
                var result = new ScriptAlias(method, ScriptAliasType.Method, null);

                // Then
                Assert.Equal(0, result.Namespaces.Count);
            }
        }
    }
}