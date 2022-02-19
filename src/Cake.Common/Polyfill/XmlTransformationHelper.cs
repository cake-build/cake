// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml;
using System.Xml.Xsl;

namespace Cake.Common.Polyfill
{
    internal static class XmlTransformationHelper
    {
        public static void Transform(XmlReader xsl, XsltArgumentList arguments, XmlReader xml, XmlWriter result)
        {
            var xslTransform = new XslCompiledTransform();
            xslTransform.Load(xsl);
            xslTransform.Transform(xml, arguments, result);
        }
    }
}
