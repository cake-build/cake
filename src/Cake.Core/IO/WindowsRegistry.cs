namespace Cake.Core.IO
{
    internal sealed class WindowsRegistry : IRegistry
    {
        public IRegistryKey LocalMachine
        {
            get { return new WindowsRegistryKey(Microsoft.Win32.Registry.LocalMachine); }
        }
    }
}
