// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Common.Build.AzurePipelines
{
    /// <summary>
    /// A set of extensions for allowing "using" with Azure Pipelines "blocks".
    /// </summary>
    public static class AzurePipelinesDisposableExtensions
    {
        /// <summary>
        /// Groups Azure Pipelines output.
        /// </summary>
        /// <param name="azurePipelinesCommands">The Azure Pipelines Commands.</param>
        /// <param name="name">The name.</param>
        /// <returns>An <see cref="IDisposable"/>.</returns>
        public static IDisposable Group(this IAzurePipelinesCommands azurePipelinesCommands, string name)
        {
            ArgumentNullException.ThrowIfNull(name);

            azurePipelinesCommands.BeginGroup(name);
            return new AzurePipelinesActionDisposable<IAzurePipelinesCommands>(azurePipelinesCommands, apc => apc.EndGroup());
        }

        /// <summary>
        /// Disposable helper for writing Azure Pipelines message blocks.
        /// </summary>
        internal sealed class AzurePipelinesActionDisposable<T> : IDisposable
        {
            private readonly Action<T> _disposeAction;
            private readonly T _instance;

            /// <summary>
            /// Initializes a new instance of the <see cref="AzurePipelinesActionDisposable{T}"/> class.
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <param name="disposeAction">The dispose action.</param>
            public AzurePipelinesActionDisposable(T instance, Action<T> disposeAction)
            {
                _instance = instance;
                _disposeAction = disposeAction;
            }

            /// <summary>
            /// Calls dispose action.
            /// </summary>
            public void Dispose()
            {
                _disposeAction(_instance);
            }
        }
    }
}
