public void EnsureDirectoryExist(DirectoryPath path)
{
  path = path.MakeAbsolute(Context.Environment);
  if(!System.IO.Directory.Exists(path.FullPath))
  {
    System.IO.Directory.CreateDirectory(path.FullPath);
  }
}

public void EnsureDirectoryDoNotExist(DirectoryPath path)
{
  path = path.MakeAbsolute(Context.Environment);
  if(System.IO.Directory.Exists(path.FullPath))
  {
    System.IO.Directory.Delete(path.FullPath);
  }
}