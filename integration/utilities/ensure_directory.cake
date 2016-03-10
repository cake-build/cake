public partial class EnsureImplementation
{
	// Ensures that a directory exist.
	public void That_Directory_Exist(DirectoryPath path)
	{
		path = path.IsRelative ? path.MakeAbsolute(_context.Environment) : path;
		var directory = new System.IO.DirectoryInfo(path.FullPath);
		if(!directory.Exists)
		{
			directory.Create();
		}
	}	

	// Ensures that a directory do not exist.
	public void That_Directory_Do_Not_Exist(DirectoryPath path)
	{		
		path = path.IsRelative ? path.MakeAbsolute(_context.Environment) : path;
		var directory = new System.IO.DirectoryInfo(path.FullPath);
		if(directory.Exists)
		{
			directory.Delete(true);
		}
	}
}