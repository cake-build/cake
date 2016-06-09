// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Common.Build.TravisCI
{
    /// <summary>
    /// A set of extensions for allowing "using" with Travis CI "blocks".
    /// </summary>
    public static class TravisCIDisposableExtensions
    {
        /// <summary>
        /// Folds travis log output.
        /// </summary>
        /// <param name="travisCIProvider">The Travis CI provider.</param>
        /// <param name="name">The name.</param>
        /// <returns>An <see cref="IDisposable"/>.</returns>
        public static IDisposable Fold(this ITravisCIProvider travisCIProvider, string name)
        {
            if (travisCIProvider == null)
            {
                throw new ArgumentNullException("travisCIProvider");
            }
            travisCIProvider.WriteStartFold(name);
            return new TravisCIActionDisposable(travisCIProvider, tci => tci.WriteEndFold(name));
        }

        /// <summary>
        /// Disposable helper for writing Travis CI message blocks.
        /// </summary>
        internal sealed class TravisCIActionDisposable : IDisposable
        {
            private readonly Action<ITravisCIProvider> _disposeAction;
            private readonly ITravisCIProvider _travisCiProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="TravisCIActionDisposable"/> class.
            /// </summary>
            /// <param name="travisCiProvider">The Travis CI provider.</param>
            /// <param name="disposeAction">The dispose action.</param>
            public TravisCIActionDisposable(ITravisCIProvider travisCiProvider, Action<ITravisCIProvider> disposeAction)
            {
                _travisCiProvider = travisCiProvider;
                _disposeAction = disposeAction;
            }

            /// <summary>
            /// Writes the end block for this message block.
            /// </summary>
            public void Dispose()
            {
                _disposeAction(_travisCiProvider);
            }
        }
    }
}
