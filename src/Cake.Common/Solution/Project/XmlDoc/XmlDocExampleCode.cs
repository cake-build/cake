// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Solution.Project.XmlDoc
{
    /// <summary>
    /// Parsed Xml documentation example code
    /// </summary>
    public sealed class XmlDocExampleCode
    {
        /// <summary>
        /// Gets Example code parent name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets Example code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocExampleCode"/> class.
        /// </summary>
        /// <param name="name">The name of code parent.</param>
        /// <param name="code">The example code.</param>
        public XmlDocExampleCode(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}