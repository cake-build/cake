﻿using System.Collections.Generic;
using System.Linq;
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
                Assert.IsArgumentNullException(result, "method");
            }

            [Fact]
            public void Should_Not_Throw_If_Method_Is_Null()
            {
                // Given
                var method = typeof(TheConstructor).GetMethods().First();

                // When
                var result = new ScriptAlias(method, ScriptAliasType.Method, null);

                // Then
                Assert.Equal(0, result.Namespaces.Count);
            }
        }
    }
}