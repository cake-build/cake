// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Scripting;
using Xunit;
using static System.Array;

namespace Cake.Core.Tests.Unit.Scripting
{
    public sealed class ScriptTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Not_Throw_If_Namespaces_Are_Null()
            {
                // Given, When
                var script = new Script(null, Empty<string>(), Empty<ScriptAlias>(), Empty<string>(), Empty<string>(), Empty<string>());

                // Then
                Assert.Empty(script.Namespaces);
            }

            [Fact]
            public void Should_Not_Throw_If_Lines_Are_Null()
            {
                // Given, When
                var script = new Script(Empty<string>(), null, new ScriptAlias[] { }, Empty<string>(), Empty<string>(), Empty<string>());

                // Then
                Assert.Empty(script.Lines);
            }

            [Fact]
            public void Should_Not_Throw_If_Aliases_Are_Null()
            {
                // Given, When
                var script = new Script(Empty<string>(), Empty<string>(), null, Empty<string>(), Empty<string>(), Empty<string>());

                // Then
                Assert.Empty(script.Aliases);
            }

            [Fact]
            public void Should_Not_Throw_If_Using_Alias_Directives_Are_Null()
            {
                // Given, When
                var script = new Script(Empty<string>(), Empty<string>(), Empty<ScriptAlias>(), null, Empty<string>(), Empty<string>());

                // Then
                Assert.Empty(script.UsingAliasDirectives);
            }

            [Fact]
            public void Should_Not_Throw_If_Using_Static_Directives_Are_Null()
            {
                // Given, When
                var script = new Script(Empty<string>(), Empty<string>(), Empty<ScriptAlias>(), Empty<string>(), null, Empty<string>());

                // Then
                Assert.Empty(script.UsingStaticDirectives);
            }

            [Fact]
            public void Should_Not_Throw_If_Defines_Are_Null()
            {
                // Given, When
                var script = new Script(Empty<string>(), Empty<string>(), Empty<ScriptAlias>(), Empty<string>(), Empty<string>(), null);

                // Then
                Assert.Empty(script.Defines);
            }
        }
    }
}