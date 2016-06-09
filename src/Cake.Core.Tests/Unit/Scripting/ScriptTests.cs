// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Scripting;
using Xunit;

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
                var script = new Script(null, new string[] { }, new ScriptAlias[] { }, new string[] { });

                // Then
                Assert.Equal(0, script.Namespaces.Count);
            }

            [Fact]
            public void Should_Not_Throw_If_Lines_Are_Null()
            {
                // Given, When
                var script = new Script(new string[] { }, null, new ScriptAlias[] { }, new string[] { });

                // Then
                Assert.Equal(0, script.Lines.Count);
            }

            [Fact]
            public void Should_Not_Throw_If_Aliases_Are_Null()
            {
                // Given, When
                var script = new Script(new string[] { }, new string[] { }, null, new string[] { });

                // Then
                Assert.Equal(0, script.Aliases.Count);
            }

            [Fact]
            public void Should_Not_Throw_If_Using_Alias_Directives_Are_Null()
            {
                // Given, When
                var script = new Script(new string[] { }, new string[] { }, new ScriptAlias[] { }, null);

                // Then
                Assert.Equal(0, script.UsingAliasDirectives.Count);
            }
        }
    }
}
