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
    public sealed class TextTransformation<TTemplate>
        where TTemplate : class, ITextTransformationTemplate
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly TTemplate _template;

        /// <summary>
        /// The test transformation template.
        /// </summary>
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
        /// <param name="path"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "StreamWriter leaves stream open.")]
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
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 4096, true))
            {
                writer.Write(_template.Render());
            }
        }

        public override string ToString()
        {
            return _template.Render();
        }
    }
}
