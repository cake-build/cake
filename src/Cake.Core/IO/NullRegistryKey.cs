// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.IO
{
    internal sealed class NullRegistryKey : IRegistryKey
    {
        public static readonly NullRegistryKey Instance = new NullRegistryKey();

        private NullRegistryKey()
        {
        }

        public void Dispose()
        {
        }

        public string[] GetSubKeyNames()
        {
            return Array.Empty<string>();
        }

        public IRegistryKey OpenKey(string name)
        {
            return null;
        }

        public object GetValue(string name)
        {
            return null;
        }
    }
}
