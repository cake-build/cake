// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// The /domain option controls of the creation of AppDomains for running tests.
    /// </summary>
    public enum NUnit3AppDomainUsage
    {
        /// <summary>
        /// Create a separate AppDomain for each assembly if more than one is listed on the command
        /// line, otherwise creates a single AppDomain.
        /// </summary>
        Default = 0,

        /// <summary>
        /// No AppDomain is created - the tests are run in the primary domain.
        /// This normally requires copying the NUnit assemblies into the same directory as your tests.
        /// </summary>
        None,

        /// <summary>
        /// A single test AppDomain is created for all test assemblies
        /// This is how NUnit worked prior to version 2.4.
        /// </summary>
        Single,

        /// <summary>
        /// An AppDomain is created for each assembly specified on the command line.
        /// </summary>
        Multiple
    }
}
