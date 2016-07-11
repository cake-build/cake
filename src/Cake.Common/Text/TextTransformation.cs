// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Text;

namespace Cake.Common.Text
{
    /// <summary>
    /// Provides functionality to perform simple text transformations
    /// from a Cake build script and save them to disc.
    /// </summary>
    /// <typeparam name="TTemplate">The text transformation template.</typeparam>
    public sealed class TextTransformation<TTemplate>
        where TTemplate : class, ITextTransformationTemplate
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly TTemplate _template;

        /// <summary>
        /// Gets the text transformation template.
        /// </summary>
        /// <value>The text transformation template.</value>
        public TTemplate Template
        {
            get { return _template; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTransformation{TTemplate}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="template">The text template.</param>
        public TextTransformation(IFileSystem fileSystem,
            ICakeEnvironment environment, TTemplate template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _template = template;
        }

        /// <summary>
        /// Saves the text transformation to a file.
        /// </summary>
        /// <param name="path">The <see cref="FilePath"/> to save the test transformation to.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Stream writer leaves stream open.")]
        public void Save(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // Make the path absolute if necessary.
            path = path.IsRelative ? path.MakeAbsolute(_environment) : path;

            // Render the content to the file.
            var file = _fileSystem.GetFile(path);
            using (var stream = file.OpenWrite())
            using (var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            {
                writer.Write(_template.Render());
            }
        }

        /// <summary>
        /// Returns a string containing the rendered template.
        /// </summary>
        /// <returns>A string containing the rendered template.</returns>
        public override string ToString()
        {
            return _template.Render();
        }
    }
}
