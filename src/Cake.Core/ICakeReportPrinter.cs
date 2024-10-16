// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;

namespace Cake.Core
{
    /// <summary>
    /// Represents a report printer.
    /// </summary>
    public interface ICakeReportPrinter
    {
        /// <summary>
        /// Writes the specified report to a target.
        /// </summary>
        /// <param name="report">The report to write.</param>
        /// <param name="verbosity">The <see cref="Verbosity"/> at which the report should be written.</param>
        void Write(CakeReport report, Verbosity verbosity);

        /// <summary>
        /// Writes the specified lifecyle steps (i.e. Setup/TearDown) to a target.
        /// </summary>
        /// <param name="name">The name of the lifecycyle step.</param>
        /// <param name="verbosity">The <see cref="Verbosity"/> at which the step should be written.</param>
        void WriteLifeCycleStep(string name, Verbosity verbosity);

        /// <summary>
        /// Writes the specified step to a target.
        /// </summary>
        /// <param name="name">The name of the step.</param>
        /// <param name="verbosity">The <see cref="Verbosity"/> at which the step should be written.</param>
        void WriteStep(string name, Verbosity verbosity);

        /// <summary>
        /// Writes the skipped step to a target.
        /// </summary>
        /// <param name="name">The name of the step.</param>
        /// <param name="verbosity">The <see cref="Verbosity"/> at which the step should be written.</param>
        void WriteSkippedStep(string name, Verbosity verbosity);
    }
}