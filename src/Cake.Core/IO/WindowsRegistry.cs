using System.Runtime.CompilerServices;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents an Windows implementation of <see cref="IRegistry"/>.
    /// </summary>
    public sealed class WindowsRegistry : IRegistry
    {
        /// <summary>
        /// Gets the LocalMachine <see cref="IRegistryKey"/>.
        /// </summary>
        public IRegistryKey LocalMachine
        {
            get { return new WindowsRegistryKey(Microsoft.Win32.Registry.LocalMachine); }
        }
    }
}
