// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Xml;

namespace Cake.Common.Polyfill
{
    internal static class XmlTransformationHelper
    {
        public static void Transform(XmlReader xsl, XmlReader xml, XmlWriter result)
        {
            // TODO: NO?!
            throw new NotSupportedException("Not supported on .NET Core.");
        }
    }
}
