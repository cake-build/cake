// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Microsoft.Win32;

namespace Cake.Core.IO
{
    internal sealed class WindowsRegistryKey : IRegistryKey
    {
        private readonly RegistryKey _key;
        private bool _disposed;

        public WindowsRegistryKey(RegistryKey key)
        {
            _key = key;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_key != null)
                {
                    _key.Dispose();
                }
            }
            _disposed = true;
        }

        public string[] GetSubKeyNames()
        {
            return _key.GetSubKeyNames();
        }

        public IRegistryKey OpenKey(string name)
        {
            var key = _key.OpenSubKey(name);
            return key != null ? new WindowsRegistryKey(key) : null;
        }

        public object GetValue(string name)
        {
            return _key.GetValue(name);
        }
    }
}
