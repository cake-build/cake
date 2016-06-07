namespace Cake.Common.Xml
{
    /// <summary>
    /// Speficies how will an XmlReader handle DTDs in the XML document.
    /// </summary>
    public enum XmlDtdProcessing 
    {
        /// <summary>
        /// The XmlReader will throw an exception when it finds a 'DOCTYPE' markup.
        /// </summary>
        Prohibit,

        /// <summary>
        /// The DTD will be ignored. Any reference to a general entity in the XML document 
        /// will cause an exception (except for the predefined entities &lt; &gt; &amp; &quot; and &apos;).
        /// The DocumentType node will not be reported.
        /// </summary>
        Ignore,

        /// <summary>
        /// The DTD will be parsed and fully processed (entities expanded, default attributes added etc.)
        /// </summary>
        Parse,
    }
}