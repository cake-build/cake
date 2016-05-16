public void EnsureDirectoryExist(DirectoryPath path)
{
    if(!System.IO.Directory.Exists(path.FullPath))
    {
        System.IO.Directory.CreateDirectory(path.FullPath);
    }
}

public void EnsureDirectoriesExist(IEnumerable<DirectoryPath> paths)
{
    foreach(var path in paths)
    {   
        EnsureDirectoryExist(path);
    }
}

public void EnsureFileExist(FilePath path)
{
    if(!System.IO.File.Exists(path.FullPath))
    {
        using (var writer = System.IO.File.CreateText(path.FullPath)) 
        {
            writer.WriteLine("Hello World!");
        }	
    }
}

public void EnsureFilesExist(IEnumerable<FilePath> paths)
{
    foreach(var path in paths)
    {  
        EnsureFileExist(path);
    }
}

public void EnsureDirectoryDoNotExist(DirectoryPath path)
{
    if(System.IO.Directory.Exists(path.FullPath))
    {
        System.IO.Directory.Delete(path.FullPath);
    }
}