// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Microsoft.Win32;

namespace Cake.Core.IO
{
    internal sealed class WindowsRegistryKey : IRegistryKey
    {
        private const string RegistryNotSupportedMessage = "The Windows Registry is not supported on this platform.";

#pragma warning disable CA1416
        private readonly Lazy<RegistryKey> _key;
        private bool _disposed;

        public WindowsRegistryKey(Func<RegistryKey> keyFactory)
        {
            _key = new Lazy<RegistryKey>(
                keyFactory ?? throw new ArgumentNullException(nameof(keyFactory)),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public WindowsRegistryKey(RegistryKey key)
            : this(() => key)
        {
        }

        private RegistryKey Key
        {
            get
            {
                // Depending on the .NET version, the static Microsoft.Win32.Registry hive
                // properties either throw PlatformNotSupportedException on their own or return
                // null on platforms without registry support. Normalize the latter to the same
                // exception so that actual registry use always fails predictably.
                return _key.Value ?? throw new PlatformNotSupportedException(RegistryNotSupportedMessage);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                // Only dispose the underlying key if it was actually created.
                if (_key.IsValueCreated)
                {
                    _key.Value?.Dispose();
                }
            }
            _disposed = true;
        }

        public string[] GetSubKeyNames()
        {
            return Key.GetSubKeyNames();
        }

        public IRegistryKey OpenKey(string name)
        {
            var key = Key.OpenSubKey(name);
            return key != null ? new WindowsRegistryKey(key) : null;
        }

        public object GetValue(string name)
        {
            return Key.GetValue(name);
        }
#pragma warning restore CA1416
    }
}
