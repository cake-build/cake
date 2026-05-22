// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Rwx.Data
{
    /// <summary>
    /// Provides RWX actor information for the current build.
    /// </summary>
    public sealed class RwxActorInfo : RwxInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RwxActorInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public RwxActorInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the identifier of the RWX actor that started the run.
        /// </summary>
        /// <value>
        /// The actor identifier.
        /// </value>
        public string Id => GetEnvironmentString("RWX_ACTOR_ID");

        /// <summary>
        /// Gets the name of the RWX actor that started the run.
        /// </summary>
        /// <value>
        /// The actor name.
        /// </value>
        public string Name => GetEnvironmentString("RWX_ACTOR");
    }
}
