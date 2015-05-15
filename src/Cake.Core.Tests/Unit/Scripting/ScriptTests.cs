using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var script = new Script(null, new string[] { }, new ScriptAlias[] { });

                // Then
                Assert.Equal(0, script.Namespaces.Count);
            }

            [Fact]
            public void Should_Not_Throw_If_Lines_Are_Null()
            {
                // Given, When
                var script = new Script(new string[] { }, null, new ScriptAlias[] { });

                // Then
                Assert.Equal(0, script.Lines.Count);
            }

            [Fact]
            public void Should_Not_Throw_If_Aliases_Are_Null()
            {
                // Given, When
                var script = new Script(new string[] { }, new string[] { }, null);

                // Then
                Assert.Equal(0, script.Aliases.Count);
            }
        }
    }
}
