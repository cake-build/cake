namespace Cake.Core.IO
{
    /// <summary>
    /// Represents the Windows registry.
    /// </summary>
    public interface IRegistry
    {
        /// <summary>
        /// Gets a registry key representing HKEY_LOCAL_MACHINE.
        /// </summary>
        /// <value>
        /// A registry key representing HKEY_LOCAL_MACHINE.
        /// </value>
        IRegistryKey LocalMachine { get; }
    }
}