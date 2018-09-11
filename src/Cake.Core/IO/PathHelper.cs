using System;
using System.Linq;
using Cake.Core.Polyfill;

namespace Cake.Core.IO
{
    internal static class PathHelper
    {
        private static readonly bool _isWindows = EnvironmentHelper.GetPlatformFamily() == PlatformFamily.Windows;

        public static string Combine(params string[] paths)
        {
            if (paths.Length == 0)
            {
                return string.Empty;
            }

            var current = paths[0];
            for (int index = 1; index < paths.Length; index++)
            {
                current = PathHelper.Combine(current, paths[index]);
            }
            return current;
        }

        public static string Combine(string first, string second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            // Both empty?
            if (string.IsNullOrWhiteSpace(first) && string.IsNullOrWhiteSpace(second))
            {
                return string.Empty;
            }

            // First empty?
            if (string.IsNullOrWhiteSpace(first) && !string.IsNullOrWhiteSpace(second))
            {
                return second;
            }

            // Second empty?
            if (!string.IsNullOrWhiteSpace(first) && string.IsNullOrWhiteSpace(second))
            {
                return first;
            }

            first = first.TrimEnd('/', '\\');
            second = second.TrimEnd('/', '\\');

            return string.Concat(first, "/", second);
        }

        public static bool HasExtension(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            for (int index = path.FullPath.Length - 1; index >= 0; index--)
            {
                if (path.FullPath[index] == '.')
                {
                    return true;
                }

                if (path.FullPath[index] == '/')
                {
                    break;
                }
            }

            return false;
        }

        public static string GetDirectoryName(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (path.Segments.Length == 1)
            {
                if (path.Segments[0].Length >= 1 && path.Segments[0][0] == '/')
                {
                    return "/";
                }
            }
            if (path.Segments.Length <= 1)
            {
                return string.Empty;
            }

            return string.Join("/", path.Segments.Take(path.Segments.Length - 1));
        }

        public static string GetFileName(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (path.Segments.Length == 0)
            {
                return null;
            }

            var filename = path.Segments[path.Segments.Length - 1];
            return filename.Replace("/", string.Empty);
        }

        public static string GetFileNameWithoutExtension(FilePath path)
        {
            var filename = PathHelper.GetFileName(path);
            if (filename == null)
            {
                return null;
            }

            var index = filename.LastIndexOf('.');
            if (index != -1)
            {
                return filename.Substring(0, index);
            }

            return filename;
        }

        public static string ChangeExtension(FilePath path, string extension)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (extension == null)
            {
                // No extension means no change.
                return path.FullPath;
            }
            if (extension != null && string.IsNullOrWhiteSpace(extension))
            {
                // Empty extension is an extension consisting of only a period.
                extension = ".";
            }

            // Make sure that the extension has a dot.
            if (!extension.StartsWith("."))
            {
                extension = string.Concat(".", extension);
            }

            var filename = path.FullPath;
            if (string.IsNullOrWhiteSpace(filename))
            {
                return null;
            }

            for (int index = path.FullPath.Length - 1; index >= 0; index--)
            {
                if (filename[index] == '/')
                {
                    // No extension found.
                    break;
                }
                if (filename[index] == '.')
                {
                    // Replace the extension.
                    return string.Concat(filename.Substring(0, index), extension);
                }
            }

            return string.Concat(filename, extension);
        }

        public static bool IsPathRooted(string path)
        {
            var length = path.Length;
            if (length >= 1)
            {
                if (path[0] == '/')
                {
                    return true;
                }

                if (_isWindows)
                {
                    if (length >= 2 && path[1] == ':')
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
