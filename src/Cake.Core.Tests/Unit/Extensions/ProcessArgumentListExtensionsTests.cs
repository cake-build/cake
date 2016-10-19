// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Xunit;

namespace Cake.Core.Tests.Unit.Extensions
{
    public class ProcessArgumentListExtensionsTests
    {
        public class TheAppendMethods
        {
            [Fact]
            public void ShouldAppendTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("string arg")
                    .RenderSafe();

                Assert.Equal("string arg", result);
            }

            [Fact]
            public void ShouldAppendFormattedTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("/arg1:{0} /arg2:{1}", "Value1", "Value2")
                    .RenderSafe();

                Assert.Equal("/arg1:Value1 /arg2:Value2", result);
            }
        }
        public class ThePrependMethods
        {
            [Fact]
            public void ShouldPrependTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("string arg")
                    .Prepend("first")
                    .RenderSafe();

                Assert.Equal("first string arg", result);
            }

            [Fact]
            public void ShouldPrependFormattedTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("string arg")
                    .Prepend("/arg1:{0} /arg2:{1}", "Value1", "Value2")
                    .RenderSafe();

                Assert.Equal("/arg1:Value1 /arg2:Value2 string arg", result);
            }
        }
        public class TheAppendQuotedMethods
        {
            [Fact]
            public void ShouldAppendTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendQuoted("string arg")
                    .RenderSafe();

                Assert.Equal("\"string arg\"", result);
            }

            [Fact]
            public void ShouldAppendProcessArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendQuoted(new TextArgument("text arg"))
                    .RenderSafe();

                Assert.Equal("\"text arg\"", result);
            }

            [Fact]
            public void ShouldAppendFormattedTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendQuoted("/arg1:{0}", "Value1")
                    .RenderSafe();

                Assert.Equal("\"/arg1:Value1\"", result);
            }
        }

        public class ThePrependQuotedMethods
        {
            [Fact]
            public void ShouldPrependTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependQuoted("string arg")
                    .RenderSafe();

                Assert.Equal("\"string arg\" last", result);
            }

            [Fact]
            public void ShouldPrependProcessArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependQuoted(new TextArgument("text arg"))
                    .RenderSafe();

                Assert.Equal("\"text arg\" last", result);
            }

            [Fact]
            public void ShouldPrependFormattedTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependQuoted("/arg1:{0}", "Value1")
                    .RenderSafe();

                Assert.Equal("\"/arg1:Value1\" last", result);
            }
        }

        public class TheAppendSecretMethods
        {
            [Fact]
            public void ShouldAppendTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendSecret("string arg")
                    .Render();

                Assert.Equal("string arg", result);
            }

            [Fact]
            public void ShouldAppendProcessArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendSecret(new TextArgument("text arg"))
                    .Render();

                Assert.Equal("text arg", result);
            }

            [Fact]
            public void ShouldAppendFormattedTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendSecret("/arg1:{0}", "Value1")
                    .Render();

                Assert.Equal("/arg1:Value1", result);
            }
        }

        public class ThePrependSecretMethods
        {
            [Fact]
            public void ShouldPrependTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependSecret("string arg")
                    .Render();

                Assert.Equal("string arg last", result);
            }

            [Fact]
            public void ShouldPrependProcessArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependSecret(new TextArgument("text arg"))
                    .Render();

                Assert.Equal("text arg last", result);
            }

            [Fact]
            public void ShouldPrependFormattedTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependSecret("/arg1:{0}", "Value1")
                    .Render();

                Assert.Equal("/arg1:Value1 last", result);
            }
        }

        public class TheAppendQuotedSecretMethods
        {
            [Fact]
            public void ShouldAppendTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendQuotedSecret("string arg")
                    .Render();

                Assert.Equal("\"string arg\"", result);
            }

            [Fact]
            public void ShouldAppendProcessArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendQuotedSecret(new TextArgument("text arg"))
                    .Render();

                Assert.Equal("\"text arg\"", result);
            }

            [Fact]
            public void ShouldAppendFormattedTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .AppendQuotedSecret("/arg1:{0} /arg2:{1}", "Value1", "Value2")
                    .Render();

                Assert.Equal("\"/arg1:Value1 /arg2:Value2\"", result);
            }
        }

        public class ThePrependQuotedSecretMethods
        {
            [Fact]
            public void ShouldPrependTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependQuotedSecret("string arg")
                    .Render();

                Assert.Equal("\"string arg\" last", result);
            }

            [Fact]
            public void ShouldPrependProcessArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependQuotedSecret(new TextArgument("text arg"))
                    .Render();

                Assert.Equal("\"text arg\" last", result);
            }

            [Fact]
            public void ShouldPrependFormattedTextArgument()
            {
                var result = new ProcessArgumentBuilder()
                    .Append("last")
                    .PrependQuotedSecret("/arg1:{0} /arg2:{1}", "Value1", "Value2")
                    .Render();

                Assert.Equal("\"/arg1:Value1 /arg2:Value2\" last", result);
            }
        }
    }
}