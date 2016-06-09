// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Loggers available for outputting test results.
    /// </summary>
    public enum VSTestLogger
    {
        /// <summary>
        /// No logging of test results.
        /// </summary>
        None,

        /// <summary>
        /// Log results to a Visual Studio test results file.
        /// </summary>
        Trx
    }
}
