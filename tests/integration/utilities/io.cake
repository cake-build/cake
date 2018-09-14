public DirectoryPath EnsureDirectoryExist(DirectoryPath path)
{
    if(!System.IO.Directory.Exists(path.FullPath))
    {
        System.IO.Directory.CreateDirectory(path.FullPath);
    }
    return path;
}

public void EnsureDirectoriesExist(IEnumerable<DirectoryPath> paths)
{
    foreach(var path in paths)
    {
        EnsureDirectoryExist(path);
    }
}

public FilePath EnsureFileExist(FilePath path, string message = "Hello World!")
{
    if(!System.IO.File.Exists(path.FullPath))
    {
        EnsureDirectoryExist(path.GetDirectory());
        using (var writer = System.IO.File.CreateText(path.FullPath))
        {
            writer.WriteLine(message);
        }
    }
    return path;
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

public bool AnyExist(IEnumerable<Cake.Core.IO.Path> paths)
{
    return paths.Any(PathExists);
}

public bool AllExist(IEnumerable<Cake.Core.IO.Path> paths)
{
    return paths.All(PathExists);
}

public bool PathExists(Cake.Core.IO.Path path)
{
    if (path is DirectoryPath)
    {
        return System.IO.Directory.Exists(path.FullPath);
    }

    return System.IO.File.Exists(path.FullPath);
}

public bool FileHashEquals(FilePath y, FilePath x)
{
    using (var sha512 = System.Security.Cryptography.SHA512.Create())
    using (System.IO.Stream
        yStream = System.IO.File.OpenRead(y.FullPath),
        xStream = System.IO.File.OpenRead(x.FullPath))
    {
        var yHash = sha512.ComputeHash(yStream);
        var xHash = sha512.ComputeHash(xStream);
        return Enumerable.SequenceEqual(yHash, xHash);
    }
}