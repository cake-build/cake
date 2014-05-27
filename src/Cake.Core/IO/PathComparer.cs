using System;
using System.Collections.Generic;

namespace Cake.Core.IO
{
    public sealed class PathComparer : IEqualityComparer<Path>
    {
        private readonly bool _isCaseSensitive;
      
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly PathComparer Default = new PathComparer(Machine.IsUnix());

        public bool IsCaseSensitive
        {
            get { return _isCaseSensitive; }
        }

        public PathComparer(bool isCaseSensitive)
        {
            _isCaseSensitive = isCaseSensitive;
        }

        public bool Equals(Path x, Path y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            if (IsCaseSensitive)
            {
                return x.FullPath.Equals(y.FullPath);
            }
            return x.FullPath.Equals(y.FullPath, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Path obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (IsCaseSensitive)
            {
                return obj.FullPath.GetHashCode();
            }
            return obj.FullPath.ToUpperInvariant().GetHashCode();
        }
    }
}