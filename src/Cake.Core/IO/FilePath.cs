namespace Cake.Core.IO
{
    public sealed class FilePath : Path
    {
        public bool HasExtension
        {
            get { return System.IO.Path.HasExtension(FullPath); }
        }

        public FilePath(string path)
            : base(path)
        {
        }

        public DirectoryPath GetDirectory()
        {
            var directory = System.IO.Path.GetDirectoryName(FullPath);
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = "./";
            }
            return new DirectoryPath(directory);
        }

        public FilePath GetFilename()
        {
            var filename = System.IO.Path.GetFileName(FullPath);
            return new FilePath(filename);
        }

        public string GetExtension()
        {
            var extension = System.IO.Path.GetExtension(FullPath);
            return string.IsNullOrWhiteSpace(extension) ? null : extension;
        }

        public FilePath ChangeExtension(string extension)
        {
            return new FilePath(System.IO.Path.ChangeExtension(FullPath, extension));
        }

        public FilePath MakeAbsolute(ICakeEnvironment environment)
        {
            return IsRelative
                ? environment.WorkingDirectory.CombineWithFilePath(this)
                : new FilePath(FullPath);
        }

        public static implicit operator FilePath(string path)
        {
            return FromString(path);
        }

        public static FilePath FromString(string path)
        {
            return new FilePath(path);
        }
    }
}