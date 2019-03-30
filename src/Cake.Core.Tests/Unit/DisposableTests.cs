// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class DisposableTests
    {
        public sealed class TheCreateMethod
        {
            [Fact]
            public void Should_Throw_If_Disposer_Null()
            {
                // When
                var result = Record.Exception(() => Disposable.Create(null));

                // Then
                AssertEx.IsArgumentNullException(result, "disposer");
            }

            [Fact]
            public void Should_Return_Disposable_That_Invokes_Disposer_Once_Only()
            {
                // When
                var disposed = 0;
                var disposable = Disposable.Create(() => disposed++);
                using (disposable)
                {
                }
                disposable.Dispose();

                // Then
                Assert.Equal(1, disposed);
            }
        }
    }
}