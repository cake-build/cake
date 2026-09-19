// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Win32;

namespace Cake.Core.IO
{
internal sealed class WindowsRegistryKey : IRegistryKey
{
        private const string RegistryNotSupportedMessage = "The Windows Registry is not supported on this platform.";

#pragma warning disable CA1416
        private readonly Func<RegistryKey> _keyFactory;
        private readonly object _lock;
        private RegistryKey _key;
        private bool _keyCreated;
        private bool _disposed;

        public WindowsRegistryKey(Func<RegistryKey> keyFactory)
        {
            _keyFactory = keyFactory ?? throw new ArgumentNullException(nameof(keyFactory));
            _lock = new object();
        }

        public WindowsRegistryKey(RegistryKey key)
            : this(() => key)
        {
        }

        private RegistryKey Key
        {
            get
            {
                if (!_keyCreated)
                {
                    lock (_lock)
                    {
                        if (!_keyCreated)
                        {
                            _key = _keyFactory();
                            _keyCreated = true;
                        }
                    }
                }

                // Depending on the .NET version, the static Microsoft.Win32.Registry hive
                // properties either throw PlatformNotSupportedException on their own or return
                // null on platforms without registry support. Normalize the latter to the same
                // exception so that actual registry use always fails predictably.
                return _key ?? throw new PlatformNotSupportedException(RegistryNotSupportedMessage);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                // Only dispose the underlying key if it was actually created.
                _key?.Dispose();
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
