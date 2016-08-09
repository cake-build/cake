// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Loggers available for outputting test results.
    /// </summary>
    [Obsolete]
    public enum VSTestLogger
    {
        /// <summary>
        /// No logging of test results.
        /// </summary>
        None,

        /// <summary>
        /// Log results to a Visual Studio test results file.
        /// </summary>
        Trx,

        /// <summary>
        /// Log results to ApVeyor's custom logger. Please note, this logger is only available when building your solution on AppVeyor.
        /// </summary>
        AppVeyor,

        /// <summary>
        /// Log results to a custom logger. Please specify the name of your custom logger in the <see cref="VSTestSettings.LoggerName"/> property
        /// </summary>
        Custom
    }
}