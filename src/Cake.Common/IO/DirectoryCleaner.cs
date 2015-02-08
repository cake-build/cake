using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
	internal static class DirectoryCleaner
	{
		public static void Clean(ICakeContext context, DirectoryPath path)
		{
			Clean(context, path, null, null);
		}

		public static void Clean(ICakeContext context, DirectoryPath path, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
		   
			if (path.IsRelative)
			{
				path = path.MakeAbsolute(context.Environment);
			}

			// Get the root directory.
			var root = context.FileSystem.GetDirectory(path);
			if (!root.Exists)
			{
				context.Log.Verbose("Creating directory {0}", path);
				root.Create();
				return;
			}

			context.Log.Verbose("Deleting contents of {0}", path);

			var hasFilteredDirs = false;
			// Delete all directories.
			Action<IFileSystemInfo> directoryFiltered = filteredEntry =>
			{
				hasFilteredDirs = true;
			    if (predicateFiltered != null)
			    {
			        predicateFiltered(filteredEntry);
			    }
			    else
			    {
			        context.Log.Verbose("Skipping {0} because specified predicate", filteredEntry.Path.FullPath);
			    }
			};
			foreach (var directory in root.GetDirectories("*", SearchScope.Current, wherePredicate, directoryFiltered))
			{
				Clean(context, directory.Path.FullPath, wherePredicate, directoryFiltered);
				if (!hasFilteredDirs)
				{
					directory.Delete(false);
				}
			}

			// Delete all files.
			foreach (var file in root.GetFiles("*", SearchScope.Current, wherePredicate, predicateFiltered))
			{
				file.Delete();
			}
		}
	}
}
