using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Unique Script Reference
    /// </summary>
    internal sealed class ScriptReference
    {
        private readonly string _origin;
        private readonly FilePath _path;
        private readonly string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptReference" /> class.
        /// </summary>
        /// <param name="localPath">The local path of the </param>
        /// <param name="origin">The original file path or URI of the script.</param>
        public ScriptReference(FilePath localPath, string origin = null)
        {
            if (localPath == null)
            {
                throw new ArgumentNullException("localPath");
            }

            _path = localPath;

            if (origin == null)
            {
                origin = localPath.ToString();
            }

            _origin = origin;
            
            _id = (localPath + origin).ToString();
        }

        /// <summary>
        /// Gets the original file path or URI of the script.
        /// </summary>
        public string Origin
        {
            get { return _origin; }
        }

        /// <summary>
        /// Gets the local script path.
        /// </summary>
        public FilePath Path
        {
            get { return _path; }
        }


        /// <summary>
        /// Gets the script's unique identifier;
        /// </summary>
        public string Id
        {
            get { return _id; }
        }

    }
}
