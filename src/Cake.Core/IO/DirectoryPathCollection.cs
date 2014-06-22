using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO
{
    public sealed class DirectoryPathCollection : IEnumerable<DirectoryPath>
    {
        private readonly PathComparer _comparer;
        private readonly HashSet<DirectoryPath> _paths;

        public int Count
        {
            get { return _paths.Count; }
        }

        internal PathComparer Comparer
        {
            get { return _comparer; }
        }

        public DirectoryPathCollection(PathComparer comparer)
            : this(Enumerable.Empty<DirectoryPath>(), comparer)
        {            
        }

        public DirectoryPathCollection(IEnumerable<DirectoryPath> paths, PathComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            _comparer = comparer;
            _paths = new HashSet<DirectoryPath>(paths, comparer);
        }

        public bool Add(DirectoryPath path)
        {
            return _paths.Add(path);
        }

        public void Add(IEnumerable<DirectoryPath> paths)
        {
            foreach (var path in paths)
            {
                _paths.Add(path);
            }
        }

        public bool Remove(DirectoryPath path)
        {
            return _paths.Remove(path);
        }

        public void Remove(IEnumerable<DirectoryPath> paths)
        {
            foreach (var path in paths)
            {
                _paths.Remove(path);
            }
        }

        public static DirectoryPathCollection operator +(DirectoryPathCollection collection, DirectoryPath path)
        {
            var result = new DirectoryPathCollection(collection, collection.Comparer);
            result.Add(path);
            return result;
        }

        public static DirectoryPathCollection operator +(DirectoryPathCollection collection, IEnumerable<DirectoryPath> paths)
        {
            var result = new DirectoryPathCollection(collection, collection.Comparer);
            result.Add(paths);
            return result;
        }

        public static DirectoryPathCollection operator -(DirectoryPathCollection collection, DirectoryPath path)
        {
            var result = new DirectoryPathCollection(collection, collection.Comparer);
            result.Remove(path);
            return result;
        }

        public static DirectoryPathCollection operator -(DirectoryPathCollection collection, IEnumerable<DirectoryPath> paths)
        {
            var result = new DirectoryPathCollection(collection, collection.Comparer);
            result.Remove(paths);
            return result;
        }

        public IEnumerator<DirectoryPath> GetEnumerator()
        {
            return _paths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
