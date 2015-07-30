using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO.Globbing;

namespace Cake.Core.IO
{
    /// <summary>
    /// The file system globber.
    /// </summary>
    public sealed class Globber : IGlobber
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Globber"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public Globber(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            _fileSystem = fileSystem;
            _environment = environment;
        }

        /// <summary>
        /// Returns <see cref="Path" /> instances matching the specified pattern.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="predicate">The predicate used to filter directories based on file system information.</param>
        /// <returns>
        ///   <see cref="Path" /> instances matching the specified pattern.
        /// </returns>
        public IEnumerable<Path> Match(string pattern, Func<IFileSystemInfo, bool> predicate)
        {
            // Create the visitor and context.
            var visitor = new GlobVisitor();
            var context = new GlobVisitorContext(_fileSystem, _environment);

            // Parse the pattern.
            var tree = new GlobParser(new GlobTokenizer(pattern), _environment).Parse();
            
            // Visit all nodes in the parsed patterns and filter the result.
            tree.Accept(visitor, context);

            // Get all unique items.
            var result = context.Results.Where(predicate ?? (info => true)).Select(x => x.Path);
            return new HashSet<Path>(result, new PathComparer(_environment.IsUnix()));
        }
    }
}
