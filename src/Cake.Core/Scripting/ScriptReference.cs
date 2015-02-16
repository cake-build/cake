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
    public class ScriptReference
    {
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

            Path = localPath;

            if (origin == null)
            {
                origin = localPath.ToString();
            }
            Origin = origin;

            var hasher = new SHA1CryptoServiceProvider();
            var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(origin));
            var sb = new StringBuilder();
            foreach (byte part in hash)
            {
                sb.Append(part.ToString("x2", CultureInfo.InvariantCulture));
            }
            TokenId = sb.ToString();
        }

        /// <summary>
        /// Gets the original file path or URI of the script.
        /// </summary>
        public string Origin { get; private set; }

        /// <summary>
        /// Gets the local script path.
        /// </summary>
        public FilePath Path { get; private set; }

        /// <summary>
        /// Gets the script's unique token identifier;
        /// </summary>
        public string TokenId { get; private set; } 
    }
}
