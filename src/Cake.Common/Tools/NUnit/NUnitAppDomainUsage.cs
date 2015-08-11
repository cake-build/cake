namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    ///     The /domain option controls of the creation of AppDomains for running tests.
    /// </summary>
    public enum NUnitAppDomainUsage
    {
        /// <summary>
        ///     Create a separate AppDomain for each assembly listed on the command line.
        /// </summary>
        Default,

        /// <summary>
        ///     No domain is created - the tests are run in the primary domain. 
        ///     This normally requires copying the NUnit assemblies into the same directory as your tests.
        /// </summary>
        None,

        /// <summary>
        ///     A test domain is created - this is how NUnit worked prior to version 2.4
        /// </summary>
        Single
    }
}