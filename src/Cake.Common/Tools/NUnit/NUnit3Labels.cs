// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Represents the possible values for the Labels option.
    /// </summary>
    public enum NUnit3Labels
    {
        /// <summary>
        /// Does not output labels. This is the default.
        /// </summary>
        Off = 0,

        /// <summary>
        /// Outputs labels for tests that are run.
        /// </summary>
        On,

        /// <summary>
        /// Outputs labels for all tests.
        /// </summary>
        All
    }
}
