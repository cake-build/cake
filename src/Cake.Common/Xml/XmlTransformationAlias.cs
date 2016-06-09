// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Xml
{
    /// <summary>
    /// Contains functionality related to XML XSL transformation.
    /// </summary>
    [CakeAliasCategory("XML")]
    public static class XmlTransformationAlias
    {
        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xsl">XML style sheet.</param>
        /// <param name="xml">XML data.</param>
        /// <returns>Transformed XML string.</returns>
        /// <example>
        /// <code>
        /// <para>This example code will convert xml to a new xml strucure using XmlTransform alias.</para>
        /// <![CDATA[
        /// string xsl = @"<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
        ///   <xsl:output method=""xml"" omit-xml-declaration=""yes"" />
        ///   <xsl:template match=""/"">
        ///     <xsl:for-each select=""pastery/cake"" >
        ///         <price><xsl:value-of select=""@price""/></price>
        ///       </xsl:for-each>
        ///   </xsl:template>
        /// </xsl:stylesheet>";
        ///
        /// string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
        /// <pastery>
        ///     <cake price=""1.62"" />
        /// </pastery>";
        ///
        /// var priceTag = XmlTransform(xsl, xml);
        /// ]]>
        /// </code>
        /// <para>Result:</para>
        /// <code>
        /// <![CDATA[<price>1.62</price>]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static string XmlTransform(this ICakeContext context, string xsl, string xml)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return XmlTransformation.Transform(xsl, xml);
        }

        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xsl">XML style sheet.</param>
        /// <param name="xml">XML data.</param>
        /// <param name="settings">Optional settings for result file xml writer</param>
        /// <returns>Transformed XML string.</returns>
        /// <example>
        /// <code>
        /// <para>This example code will convert specific part of xml to plaintext using XmlTransform alias.</para>
        /// <![CDATA[string xsl = @"<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
        ///   <xsl:output method=""text"" omit-xml-declaration=""yes"" indent=""no""/>
        ///   <xsl:strip-space elements=""*""/>
        ///   <xsl:template match=""pastery/cake""><xsl:value-of select=""@price""/></xsl:template>
        /// </xsl:stylesheet>";
        ///
        /// string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
        /// <pastery>
        ///     <cake price=""1.62"" />
        /// </pastery>";
        ///
        /// var text = XmlTransform(xsl, xml, new XmlTransformationSettings {
        ///     ConformanceLevel = System.Xml.ConformanceLevel.Fragment, Encoding = Encoding.ASCII });
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static string XmlTransform(this ICakeContext context, string xsl, string xml, XmlTransformationSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return XmlTransformation.Transform(xsl, xml, settings);
        }

        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xslPath">Path to xml style sheet.</param>
        /// <param name="xmlPath">Path xml data.</param>
        /// <param name="resultPath">Transformation result path, will overwrite if exists.</param>
        /// <example>
        /// <code>
        /// <para>This example code will convert the Cake nuspec into html using the XmlTransform alias.</para>
        /// <para>XML stylesheet:</para>
        /// <![CDATA[
        /// <?xml version="1.0" ?>
        /// <xsl:stylesheet
        ///   version="1.0"
        ///   xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        ///   xmlns:p="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"
        ///   exclude-result-prefixes="p"
        ///   >
        ///   <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
        ///   <xsl:template match="/">
        ///     <html lang="en" class="static">
        ///       <head>
        ///         <title>
        ///           <xsl:for-each select="package/p:metadata">
        ///               <xsl:value-of select="p:id"/>
        ///           </xsl:for-each>
        ///         </title>
        ///       </head>
        ///       <body>
        ///           <xsl:for-each select="package/p:metadata">
        ///             <h1>
        ///               <xsl:value-of select="p:id"/>
        ///             </h1>
        ///             <h2>Description</h2>
        ///             <i><xsl:value-of select="p:description"/></i>
        ///           </xsl:for-each>
        ///         <h3>Files</h3>
        ///         <ul>
        ///           <xsl:for-each select="package/files/file" >
        ///             <li><xsl:value-of select="@src"/></li>
        ///           </xsl:for-each>
        ///         </ul>
        ///       </body>
        ///     </html>
        ///   </xsl:template>
        /// </xsl:stylesheet>
        /// ]]>
        /// </code>
        /// <para>XmlTransform usage:</para>
        /// <code>
        /// XmlTransform("./nuspec.xsl", "./nuspec/Cake.nuspec", "./Cake.htm");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XmlTransform(this ICakeContext context, FilePath xslPath, FilePath xmlPath, FilePath resultPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var settings = new XmlTransformationSettings();
            XmlTransformation.Transform(context.FileSystem, xslPath, xmlPath, resultPath, settings);
        }

        /// <summary>
        /// Performs XML XSL transformation
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xslPath">Path to xml style sheet.</param>
        /// <param name="xmlPath">Path xml data.</param>
        /// <param name="resultPath">Transformation result path.</param>
        /// <param name="settings">Optional settings for result file xml writer</param>
        /// <example>
        /// <code>
        /// <para>This example code will convert the Cake nuspec into html using the XmlTransform alias,
        /// specifying that result should be indented and using Unicode encoding.</para>
        /// <para>XML stylesheet:</para>
        /// <![CDATA[
        /// <?xml version="1.0" ?>
        /// <xsl:stylesheet
        ///   version="1.0"
        ///   xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        ///   xmlns:p="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"
        ///   exclude-result-prefixes="p"
        ///   >
        ///   <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />
        ///   <xsl:template match="/">
        ///     <html lang="en" class="static">
        ///       <head>
        ///         <title>
        ///           <xsl:for-each select="package/p:metadata">
        ///               <xsl:value-of select="p:id"/>
        ///           </xsl:for-each>
        ///         </title>
        ///       </head>
        ///       <body>
        ///           <xsl:for-each select="package/p:metadata">
        ///             <h1>
        ///               <xsl:value-of select="p:id"/>
        ///             </h1>
        ///             <h2>Description</h2>
        ///             <i><xsl:value-of select="p:description"/></i>
        ///           </xsl:for-each>
        ///         <h3>Files</h3>
        ///         <ul>
        ///           <xsl:for-each select="package/files/file" >
        ///             <li><xsl:value-of select="@src"/></li>
        ///           </xsl:for-each>
        ///         </ul>
        ///       </body>
        ///     </html>
        ///   </xsl:template>
        /// </xsl:stylesheet>
        /// ]]>
        /// </code>
        /// <para>XmlTransform usage:</para>
        /// <code>
        /// XmlTransform("./nuspec.xsl", "./nuspec/Cake.nuspec", "./Cake.htm",
        ///     new XmlTransformationSettings { Indent = true, Encoding = Encoding.Unicode});
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void XmlTransform(this ICakeContext context, FilePath xslPath, FilePath xmlPath, FilePath resultPath, XmlTransformationSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            XmlTransformation.Transform(context.FileSystem, xslPath, xmlPath, resultPath, settings);
        }
    }
}
