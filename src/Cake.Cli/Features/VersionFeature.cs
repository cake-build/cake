// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Cli
{
    /// <summary>
    /// Represents a feature that writes the Cake version to the console.
    /// </summary>
    public interface ICakeVersionFeature
    {
        /// <summary>
        /// Writes the Cake version to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        void Run(IConsole console);
    }

    /// <summary>
    /// Writes the Cake version to the console.
    /// </summary>
    public sealed class VersionFeature : ICakeVersionFeature
    {
        private readonly IVersionResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionFeature"/> class.
        /// </summary>
        /// <param name="resolver">The version resolver.</param>
        public VersionFeature(IVersionResolver resolver)
        {
            _resolver = resolver;
        }

        /// <inheritdoc/>
        public void Run(IConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.WriteLine(_resolver.GetVersion());
        }
    }
}
