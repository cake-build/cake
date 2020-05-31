// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.TFBuild.Data
{
    /// <summary>
    /// Providers known values for the TF Test Runner types.
    /// </summary>
    public enum TFTestRunnerType
    {
        /// <summary>
        /// JUnit Test Result Format
        /// </summary>
        JUnit,

        /// <summary>
        /// NUnit (v2) Test Result Format
        /// </summary>
        NUnit,

        /// <summary>
        /// Visual Studio (MSTest) Test Result Format
        /// </summary>
        VSTest,

        /// <summary>
        /// XUnit Test Result Format
        /// </summary>
        XUnit
    }
}
