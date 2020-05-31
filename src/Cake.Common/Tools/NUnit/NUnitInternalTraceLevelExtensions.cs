// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.NUnit;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Contains extension methods for <see cref="NUnitInternalTraceLevel"/>.
    /// </summary>
    public static class NUnitInternalTraceLevelExtensions
    {
        /// <summary>
        /// Gets the LEVEL value for the --trace command line argument for the given <see cref="NUnitInternalTraceLevel"/>.
        /// </summary>
        /// <param name="level">The <see cref="NUnitInternalTraceLevel"/> value for which to get the <see cref="string"/> representation.</param>
        /// <returns>Returns the appropriate <see cref="string"/> representation for the given <see cref="NUnitInternalTraceLevel"/> value.</returns>
        public static string GetArgumentValue(this NUnitInternalTraceLevel level)
        {
            string result;
            switch (level)
            {
                case NUnitInternalTraceLevel.Debug:
                    result = "verbose";
                    break;
                default:
                    result = Enum.GetName(typeof(NUnitInternalTraceLevel), level)?.ToLowerInvariant();
                    break;
            }

            return result ?? throw new ArgumentOutOfRangeException(nameof(level), level, "Unexpected value was encountered.");
        }
    }
}
