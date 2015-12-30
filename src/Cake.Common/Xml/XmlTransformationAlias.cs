#if NET45
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
#endif