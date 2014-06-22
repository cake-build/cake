using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO
{
    public sealed class FilePathCollection : IEnumerable<FilePath>
    {
        private readonly PathComparer _comparer;
        private readonly HashSet<FilePath> _paths;

        public int Count
        {
            get { return _paths.Count; }
        }

        internal PathComparer Comparer
        {
            get { return _comparer; }
        }

        public FilePathCollection(PathComparer comparer)
            : this(Enumerable.Empty<FilePath>(), comparer)
        {            
        }

        public FilePathCollection(IEnumerable<FilePath> paths, PathComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            _comparer = comparer;
            _paths = new HashSet<FilePath>(paths, comparer);
        }

        public bool Add(FilePath path)
        {
            return _paths.Add(path);
        }

        public void Add(IEnumerable<FilePath> paths)
        {
            foreach (var path in paths)
            {
                _paths.Add(path);
            }
        }

        public bool Remove(FilePath path)
        {
            return _paths.Remove(path);
        }

        public void Remove(IEnumerable<FilePath> paths)
        {
            foreach (var path in paths)
            {
                _paths.Remove(path);
            }
        }

        public static FilePathCollection operator +(FilePathCollection collection, FilePath path)
        {
            var result = new FilePathCollection(collection, collection.Comparer);
            result.Add(path);
            return result;
        }

        public static FilePathCollection operator +(FilePathCollection collection, IEnumerable<FilePath> paths)
        {
            var result = new FilePathCollection(collection, collection.Comparer);
            result.Add(paths);
            return result;
        }

        public static FilePathCollection operator -(FilePathCollection collection, FilePath path)
        {
            var result = new FilePathCollection(collection, collection.Comparer);
            result.Remove(path);
            return result;
        }

        public static FilePathCollection operator -(FilePathCollection collection, IEnumerable<FilePath> paths)
        {
            var result = new FilePathCollection(collection, collection.Comparer);
            result.Remove(paths);
            return result;
        }

        public IEnumerator<FilePath> GetEnumerator()
        {
            return _paths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
