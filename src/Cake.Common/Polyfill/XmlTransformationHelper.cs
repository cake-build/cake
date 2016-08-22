// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml;

#if !NETCORE
using System.Xml.Xsl;
#else
using System;
#endif

namespace Cake.Common.Polyfill
{
    internal static class XmlTransformationHelper
    {
        public static void Transform(XmlReader xsl, XmlReader xml, XmlWriter result)
        {
#if NETCORE
            throw new NotSupportedException("Not supported on .NET Core.");
#else
            var xslTransform = new XslCompiledTransform();
            xslTransform.Load(xsl);
            xslTransform.Transform(xml, result);
#endif
        }
    }
}
