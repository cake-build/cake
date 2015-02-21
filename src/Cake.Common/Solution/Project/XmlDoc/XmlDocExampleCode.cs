namespace Cake.Common.Solution.Project.XmlDoc
{
    /// <summary>
    /// Parsed Xml documentation example code
    /// </summary>
    public sealed class XmlDocExampleCode
    {
        private readonly string _name;
        private readonly string _code;

        /// <summary>
        /// Gets Example code parent name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets Example code
        /// </summary>
        public string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocExampleCode"/> class.
        /// </summary>
        /// <param name="name">The name of code parent.</param>
        /// <param name="code">The example code.</param>
        public XmlDocExampleCode(string name, string code)
        {
            _name = name;
            _code = code;
        }
    }
}