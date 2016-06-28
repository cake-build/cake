// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCORE
using System;
#endif
using System.Xml;

namespace Cake.Common.Polyfill
{
    internal static class XmlWriterSettingsHelper
    {
        public static bool GetDoNotEscapeUriAttributes(XmlWriterSettings settings)
        {
#if NETCORE
            throw new NotSupportedException("Not supported on .NET Core.");
#elif __MonoCS__
            throw new NotSupportedException("Not supported on Mono.");
#else
            return settings.DoNotEscapeUriAttributes;
#endif
        }

        public static void SetDoNotEscapeUriAttributes(XmlWriterSettings settings, bool value)
        {
#if NETCORE
            throw new NotSupportedException("Not supported on .NET Core.");
#elif __MonoCS__
            throw new NotSupportedException("Not supported on Mono.");
#else
            settings.DoNotEscapeUriAttributes = value;
#endif
        }
    }
}
