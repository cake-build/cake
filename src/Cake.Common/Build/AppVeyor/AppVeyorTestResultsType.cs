// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// Provides the known values for the AppVeyor test results types.
    /// </summary>
    public enum AppVeyorTestResultsType
    {
        /// <summary>
        /// MSTest test results.
        /// </summary>
        MSTest,

        /// <summary>
        /// XUnit test results.
        /// </summary>
        XUnit,

        /// <summary>
        /// NUnit test results.
        /// </summary>
        NUnit,

        /// <summary>
        /// NUnit v3 test results.
        /// </summary>
        NUnit3,

        /// <summary>
        /// JUnit test results.
        /// </summary>
        JUnit
    }
}
