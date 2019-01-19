// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.TeamCity
{
    /// <summary>
    /// A set of extensions for allowing "using" with TeamCity "blocks".
    /// </summary>
    public static class TeamCityDisposableExtensions
    {
        /// <summary>
        /// Writes the start of a TeamCity message block, then returns a disposable that writes the report block end on dispose.
        /// </summary>
        /// <param name="teamCityProvider">TeamCity provider.</param>
        /// <param name="blockName">The name of the report block.</param>
        /// <returns>A disposable that writes the report block end.</returns>
        public static IDisposable Block(this ITeamCityProvider teamCityProvider, string blockName)
        {
            if (teamCityProvider == null)
            {
                throw new ArgumentNullException(nameof(teamCityProvider));
            }
            teamCityProvider.WriteStartBlock(blockName);
            return Disposable.Create(() => teamCityProvider.WriteEndBlock(blockName));
        }

        /// <summary>
        /// Writes the start of a TeamCity build block, then returns a disposable that writes the build block end on dispose.
        /// </summary>
        /// <param name="teamCityProvider">TeamCity provider.</param>
        /// <param name="compilerName">The name of the build block.</param>
        /// <returns>A disposable that writes the build block end.</returns>
        public static IDisposable BuildBlock(this ITeamCityProvider teamCityProvider, string compilerName)
        {
            if (teamCityProvider == null)
            {
                throw new ArgumentNullException(nameof(teamCityProvider));
            }
            teamCityProvider.WriteStartBuildBlock(compilerName);
            return Disposable.Create(() => teamCityProvider.WriteEndBuildBlock(compilerName));
        }
    }
}