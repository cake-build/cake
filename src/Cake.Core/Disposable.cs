// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Disposable helper.
    /// </summary>
    public static class Disposable
    {
        static Disposable()
        {
            Empty = Create(() => { });
        }

        /// <summary>
        /// Creates an anonymous disposable with the specified disposer action.
        /// </summary>
        /// <param name="disposer">The disposer action.</param>
        /// <returns>The anonymous disposable.</returns>
        public static IDisposable Create(Action disposer) => new AnonymousDisposable(disposer);

        /// <summary>
        /// An empty disposable; does nothing.
        /// </summary>
        public static readonly IDisposable Empty;

        private sealed class AnonymousDisposable : IDisposable
        {
            public AnonymousDisposable(Action disposer)
            {
                _disposer = disposer ?? throw new ArgumentNullException(nameof(disposer));
            }

            public void Dispose()
            {
                _disposer?.Invoke();
                _disposer = null;
            }

            private Action _disposer;
        }
    }
}