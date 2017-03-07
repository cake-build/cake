// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors.Loading
{
    /// <summary>
    /// Represents a load directive provider.
    /// </summary>
    public interface ILoadDirectiveProvider
    {
        /// <summary>
        /// Indicates whether or not this provider can load the specified <see cref="LoadReference"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to the code to load.</param>
        /// <returns><c>true</c> if the provider can load the reference; otherwise <c>false</c>.</returns>
        bool CanLoad(IScriptAnalyzerContext context, LoadReference reference);

        /// <summary>
        /// Loads the specified <see cref="LoadReference"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reference">The reference to load.</param>
        void Load(IScriptAnalyzerContext context, LoadReference reference);
    }
}